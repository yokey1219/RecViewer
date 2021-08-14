﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using RecordFileUtil;
using System.Threading;
using System.IO;

namespace RecViewer
{
    public partial class ReadData : Form
    {
        public ReadData()
        {
            InitializeComponent();
        }

        private void ReadData_Load(object sender, EventArgs e)
        {
            this.comboBox1.DataSource = SerialPort.GetPortNames();
            this.comboBox3.DataSource = RecordInfoFactory.Infos;
            this.comboBox3.DisplayMember = "Name";
            this.comboBox3.ValueMember = "InfoType";
            defaultNumRangeCheck();
            LoadHistory();

        }

        private void LoadHistory()
        {
            String history_no = (String)GerneralConfig.getUserData("test_no");
            if (history_no != null)
            {
                numUpDwn.Value = Convert.ToInt32(history_no);
            }
        }

        private AbstractRecordInfo arInfo = null;
        private Boolean bWaitread = false;
        public AbstractRecordInfo Info { get { return arInfo; } }
        public Boolean isReaded = false;
        private const int MAX_TIMEOUT_SECONDS = 5;
        private int time_out_seconds = 0;

        private void setTimeout()
        {
            //time_out_seconds = MAX_TIMEOUT_SECONDS;
            try
            {
                time_out_seconds = Int32.Parse(tbTimeout.Text);

            }
            catch
            {
                time_out_seconds = MAX_TIMEOUT_SECONDS;
                tbTimeout.Text = time_out_seconds.ToString();
            }

        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            setTimeout();
            
            AbstractRecordInfo info=RecordInfoFactory.CreateInfo((int)comboBox3.SelectedValue);
            int no = 0;// Convert.ToInt32(numericUpDown1.Value * 10 + numericUpDown2.Value);
            try
            {
                no = Convert.ToInt32(numUpDwn.Value);//no = Convert.ToInt32(tbreadno.Text);
                if(!checkNumRange(no))
                    return;
            }
            catch (FormatException fmtex)
            {
                MessageForm.Show(fmtex.Message, fmtex);
                return;
            }
            catch (OverflowException ovrex)
            {
                MessageForm.Show(ovrex.Message, ovrex);
                return;
            }

            GerneralConfig.setUserData("test_no", no.ToString());

            info.MakeSendBuffer();
            info.SetReadNo(no);
            arInfo = info;
            this.btnRead.Enabled = false;
            tbsend.Text = "";
            tbread.Clear();
            try
            {
                foreach (byte b in info.SendBuffer)
                    tbsend.Text += b.ToString("X2") + " ";
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
               
                serialPort1.Open();
                serialPort1.DiscardOutBuffer();
                serialPort1.DiscardInBuffer();
                serialPort1.Write(info.SendBuffer, 0, info.SendBuffer.Length);
                bWaitread = true;
                int count = 0;
                Boolean istimeout = false;
                DateTime begin = DateTime.Now;
                while (bWaitread)
                {
                    TimeSpan timespan=DateTime.Now.Subtract(begin);// int timespan = DateTime.Now.Subtract(begin).Seconds;
                    if (timespan.TotalSeconds > time_out_seconds)//if (count > 60)
                    {
                        istimeout = true;
                        break;
                    }
                    else
                    {
                        Thread.Sleep(10);
                        Application.DoEvents();
                    }
                    count++;
                }
                serialPort1.DiscardInBuffer();
                serialPort1.Close();
                int toread = 0;
                if(arInfo!=null&&arInfo.DataBuffer!=null&&arInfo.DataBuffer.Length>4)
                    toread=Convert.ToInt32((int)(arInfo.DataBuffer[2] << 8) + (int)arInfo.DataBuffer[3]);
                //if (arInfo is ModulusYuanInfo)
                //    toread = toread - 6;
                if (istimeout && (toread < 1 || arInfo.DataBuffer.Length< (toread + 6)))
                {
                    //MessageForm.Show("读取超时",new Exception());//MessageBox.Show("读取超时!");
                    //this.Close();
                    tbread.Clear();
                    if(arInfo.DataBuffer==null)
                        throw new Exception(String.Format("读取超时,未读取到数据"));
                    foreach (byte b in arInfo.DataBuffer)
                        tbread.AppendText(b.ToString("X2") + " ");
                    throw new Exception(String.Format("读取超时,需要读取{0},实际读取{1}",toread+6,arInfo.DataBuffer.Length));
                }
                else
                {
                    //int toread = Convert.ToInt32((int)(arInfo.DataBuffer[2] << 8) + (int)arInfo.DataBuffer[3]);
                    foreach (byte b in arInfo.DataBuffer)
                        tbread.AppendText(b.ToString("X2") + " ");
                    if (arInfo.DataBuffer.Length >= (toread + 6))
                    {
                        int len = arInfo.DataBuffer.Length;
                        if (len > toread + 6)
                            len = toread + 6;
                        byte[] tocrc = new byte[len - 2];
                        Array.Copy(arInfo.DataBuffer, tocrc, len - 2);
                        byte[] crc = UtilTools.CRCCalc(tocrc);
                        if (true)//crc[0] == arInfo.DataBuffer[len - 2] && crc[1] == arInfo.DataBuffer[len - 1])
                        {
                            info.ProcBufferWhenReadEnd(len);
                            info.ReadFinish();
                            isReaded = true;
                            MessageBox.Show("读取完成!");
                            this.Close();
                        }
                        else
                        {
                            isReaded = false;
                            MessageBox.Show("校验失败!");
                        }
                    }
                    else
                    {
                        isReaded = false;
                        MessageBox.Show("读取失败!");
                    }
                }
                this.btnRead.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageForm.Show(ex.Message,ex);//MessageBox.Show(ex.Message);
                this.btnRead.Enabled = true;
            }
        }

        private  Boolean checkNumRange(int no)
        {
            if (rb0999.Checked&&(no > 999 || no < 0))
            {
                MessageBox.Show("编号只能是0-999");
                return false;
            }
            else if (rb099.Checked && (no > 255 || no < 0))
            {
                MessageBox.Show("编号只能是0-255");
                return false;
            }
            else
                return true;
        }

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType.Equals(SerialData.Eof)) return;
            byte[] buf = new byte[1024];
            int len = serialPort1.Read(buf, 0, 1024);
            if (len > 0)
            {
                arInfo.ReadDataToBuffer(buf, 0, len);
                if (arInfo.DataBuffer.Length > 4)
                {
                    int toread = Convert.ToInt32((arInfo.DataBuffer[2]<<8)+arInfo.DataBuffer[3]);
                    if (arInfo is ModulusYuanInfo)
                    {
                        if (arInfo.DataBuffer.Length >= (toread+6))
                            bWaitread = false;
                    }
                    else
                    {
                        if (arInfo.DataBuffer.Length >= (toread + 6) )//&& arInfo.DataBuffer.Length >= 2048)
                            bWaitread = false;
                    }
                }
            }
            else
            {
                bWaitread = false;
            }
            
        }



        internal void SaveData()
        {
            String txt = tbread.Text;
            String filename=String.Format("{0}\\{1}",AppDomain.CurrentDomain.BaseDirectory,DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss.txt"));
            File.WriteAllText(filename,txt);
        }

        private void numRangeCheckedChanged(object sender, EventArgs e)
        {
            if (sender is RadioButton)
            {
                //unCheckAllNumRange();
                if ((sender as RadioButton).Checked && sender == rb0999)
                {
                    change999(true);
                }
                else
                    change999(false);
            }
            
        }

        private void unCheckAllNumRange()
        {
            rb099.Checked = false;
            rb0999.Checked = false;
        }

        private void change999(bool rslt)
        {
            GerneralConfig.Is999 = rslt;
            AbstractRecordInfo.set999(rslt);
        }


        private void defaultNumRangeCheck()
        {
            unCheckAllNumRange();
            if (GerneralConfig.Is999)
            {
                rb0999.Checked = true;
            }
            else rb099.Checked = true;
        }

        private void btnclearno_Click(object sender, EventArgs e)
        {
            numUpDwn.Value = 0;
        }
        
    }
}
