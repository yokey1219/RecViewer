using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RecordFileUtil
{
    public abstract class AbstractRecordInfo
    {
        protected String recordname;

        protected byte[] sendbuffer;
        protected int no;

        public int No { get { return no; } }
        public byte[] SendBuffer { get { return sendbuffer; } }

        protected ChartFormat chartformat;
        protected byte[] buffer=null;
        protected int length;
        protected byte crch;
        protected byte crcl;
        protected int year;
        protected int month;
        protected int day;
        protected int hour;
        protected int minute;
        protected String thedate;//日期
        protected int width;//直径
        protected int height;//高度
        protected int temp;//试验温度
        protected int loadspeed;//加载速度
        protected int sensor;//传感器
        protected int nonuse1;//空
        protected int nodecnt;//点数

        public String RecordName { get { return recordname; } }
        public String TheDate { get { return thedate; } }
        public int Diameter { get { return width; } }
        public int Height { get { return height; } }
        public int Sensor { get { return sensor; } }
        public int Nodecnt { get { return nodecnt; } }
        public int LoadSpeed { get { return loadspeed; } }
        public int Temp { get { return temp; } }
        public ChartFormat Chartformat { get { return chartformat; } }

        public byte[] DataBuffer { get { return buffer; } }

        public abstract List<IXYNode> getXYNodes();
        public abstract List<IXYNode> getSpecialNodes();
        public abstract List<String> getCSVLines();
        public abstract void LoadFromCSV(String[] strs);
        public abstract void initCharFormat();
        public virtual DataTable getDataTable()
        {
            return null;
        }

        public static char csvsepchar=',';
        public static String csvfmt = "{0},{1}";

        public void SetReadNo(int no)
        {
            sendbuffer[3] = Convert.ToByte(no);
            this.dosendCRC();
        }

        public void MakeSendBuffer()
        {
            this.sendbuffer = new byte[8];
            sendbuffer[0] = 0x10;
            sendbuffer[1] = 0x04;
            sendbuffer[4] = 0xff;
            sendbuffer[5] = 0xff;
            this.makeSendBufferInternal();
            this.dosendCRC();
        }

        protected void dosendCRC()
        {
            byte[] tocrc = new byte[6];
            Array.Copy(sendbuffer, 0, tocrc, 0, 6);
            byte[] crcbuf = UtilTools.CRCCalc(tocrc);
            sendbuffer[6] = crcbuf[0];
            sendbuffer[7] = crcbuf[1];
        }

        protected abstract void makeSendBufferInternal();


        public void ReadDataToBuffer(byte[] buf, int offset, int len)
        {
            if (buffer == null)
            {
                buffer = new byte[len];
                Array.Copy(buf, offset, buffer, 0, len);
            }
            else
            {
                byte[] newbuffer = new byte[buffer.Length + len];
                Array.Copy(buffer, 0, newbuffer,0, buffer.Length);
                Array.Copy(buf, offset, newbuffer, buffer.Length, len);
                buffer = newbuffer;
            }
        }

        public void ReadFinish()
        {
            this.ParseSendBuffer();
            this.LoadInternalData(buffer);
        }

        public void ProcBufferWhenReadEnd(int len)
        {
            if (buffer.Length > len)
            {
                byte[] tocrc = new byte[len];
                Array.Copy(buffer, tocrc, len);
                buffer = tocrc;
            }
        }

        public void LoadData(byte[] buf)
        {
            this.LoadSendBuffer(buf);
            byte[] bytes = new byte[buf.Length - 8];
            Array.Copy(buf, 8,bytes, 0,bytes.Length );
            buffer = bytes;
            this.LoadInternalData(bytes);
        }

        protected  abstract void LoadInternalData(byte[] bytes);

        internal void SetName(String name)
        {
            this.recordname = name;
        }

        protected void LoadSendBuffer(byte[] buf)
        {
            this.sendbuffer = new byte[8];
            Array.Copy(buf , 0, this.sendbuffer, 0, 8);
            ParseSendBuffer();
        }

        protected void ParseSendBuffer()
        {
            this.recordname = RecordInfoFactory.GetRecordName(sendbuffer[2]);
            //switch (sendbuffer[2])
            //{
            //    case 0:
            //        this.recordname = "承载比(CBR)";
            //        break;
            //    case 1:
            //        this.recordname = "回弹模量-强度仪法";
            //        break;
            //    case 2:
            //        this.recordname = "无侧限抗压强度";
            //        break;
            //    case 3:
            //        this.recordname = "回弹模量-顶面法";
            //        break;
            //    default:
            //        this.recordname = "unkown";
            //        break;
            //}
            this.no = Convert.ToInt32(sendbuffer[3]);
        }

        public Boolean CheckCRC()
        {
            return UtilTools.DataCRC(ref buffer,buffer.Length);
            //int len = buffer.Length;
            //byte[] tocrc = new byte[buffer.Length - 2];
            //Array.Copy(buffer,tocrc, len - 2);
            //byte[] crc = UtilTools.CRCCalc(tocrc);
            //if (crc[0] == buffer[len - 2] && crc[1] == buffer[len - 1])
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
        }


        public  virtual void EditValue(string p, string newvalue)
        {
            throw new NotImplementedException();
        }

        public virtual List<EditableItem> GetEditableList()
        {
            return new List<EditableItem>();
        }

        public virtual String GetEditableValuStr(String valuename)
        {
            return String.Empty;
        }
    }

    public class EditableItem
    {
        protected String actionanme;
        protected String valuename;
        public String ActionName { get { return actionanme; } }
        public String ValueName { get { return valuename; } }
        public EditableItem(String action, String value)
        {
            this.actionanme = action;
            this.valuename = value;
        }

    }
}
