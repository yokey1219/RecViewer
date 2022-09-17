using System;
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

        }

        private AbstractRecordInfo arInfo = null;
        private Boolean bWaitread = false;
        public AbstractRecordInfo Info { get { return arInfo; } }
        public Boolean isReaded = false;

        private void btnRead_Click(object sender, EventArgs e)
        {
            
            
            AbstractRecordInfo info=RecordInfoFactory.CreateInfo((int)comboBox3.SelectedValue);
            int no = Convert.ToInt32(numericUpDown1.Value * 10 + numericUpDown2.Value);
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
                int time_out_seconds = 60;
                Boolean istimeout = false;

                DateTime begin = DateTime.Now;
                while (bWaitread)
                {

                    TimeSpan timespan = DateTime.Now.Subtract(begin);// int timespan = DateTime.Now.Subtract(begin).Seconds;
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
                }
                serialPort1.DiscardInBuffer();
                serialPort1.Close();
                if (istimeout)
                {
                    //MessageForm.Show("读取超时",new Exception());//MessageBox.Show("读取超时!");
                    if (arInfo.DataBuffer != null)
                    {
                        foreach (byte b in arInfo.DataBuffer)
                            tbread.AppendText(b.ToString("X2") + " ");
                    }
                    this.Close();
                    throw new Exception("读取超时");
                }
                else
                {
                    int toread = Convert.ToInt32((int)(arInfo.DataBuffer[2] << 8) + (int)arInfo.DataBuffer[3]);
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

        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] buf = new byte[1024];
            int len = serialPort1.Read(buf, 0, 1024);
            if (len > 0)
            {
                arInfo.ReadDataToBuffer(buf, 0, len);
                if (arInfo.DataBuffer.Length > 4)
                {
                    int toread = Convert.ToInt32((arInfo.DataBuffer[2]<<8)+arInfo.DataBuffer[3]);
                    if(arInfo.DataBuffer.Length>=(toread+6))//&&arInfo.DataBuffer.Length>=2048)
                        bWaitread = false;
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
            String filename = String.Format("{0}\\{1}.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyy_MM_dd_hh_mm_ss"));
            File.WriteAllText(filename,txt);
        }
    }
}
