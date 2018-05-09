using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RecordFileUtil
{
    public class PilieTestInfo : AbstractRecordInfo
    {
        protected const String MAXOFFSET = "最大点位移";
        protected List<IXYNode> nodes;
        protected List<IXYNode> specialnodes;
        protected int rt;
        protected int maxstrength;
        protected int maxoffset;
        protected int et;
        protected int st;

        public int RT { get { return rt; } }
        public int MaxStrength { get { return maxstrength; } }
        public int MaxOffset { get { return maxoffset; } }
        public int ET { get { return et; } }
        public int ST { get { return st; } }


        protected double rta
        {
            get
            {
                if (width == 1000)
                    return 0.006287;
                if (width == 1500)
                    return 0.00425;
                else
                    return 0;
            }
        }
        protected double u
        {
            get
            {
                if (temp <= 10)
                    return 0.25;
                if (temp < 15)
                    return 0.30;
                if (temp < 20)
                    return 0.35;
                if (temp < 25)
                    return 0.40;
                else
                    return 0.45;
            }
        }


        protected double eta
        {
            get
            {
                return (0.0307 + 0.0936 * u) / (1.35 + 5*u);
            }
        }

        protected double sta
        {
            get
            {
                return (0.27 + u) / (height/10f);
            }
        }



        public override List<IXYNode> getXYNodes()
        {
            return nodes;
        }

        public override List<IXYNode> getSpecialNodes()
        {
            return specialnodes; ;
        }

        public override void initCharFormat()
        {
            chartformat = new ChartFormat();
            chartformat.Xname = "mm";
            chartformat.Yname = "KN";
            chartformat.Xmin = 0;
            chartformat.Ymin = 0;
            chartformat.Xmax = 10;
            chartformat.Ymax = 80;
            chartformat.Xinterval = 2;
            chartformat.Yinterval = 16;
            chartformat.Xtype = 1;
            chartformat.Ytype = 1;
            chartformat.Xreverse = false;
            chartformat.Yreverse = false;
        }

        protected override void LoadInternalData(byte[] bytes)
        {
            this.initCharFormat();

            if (bytes[0] == 0x10 && bytes[1] == 0x04)
            {
                nodes = new List<IXYNode>();
                int idx = 2;
                int length = (int)((bytes[idx++] << 8) | bytes[idx++]);
                year = (int)((bytes[idx++] << 8) | bytes[idx++]);
                month = (int)((bytes[idx++] << 8) | bytes[idx++]);
                day = (int)((bytes[idx++] << 8) | bytes[idx++]);
                hour = (int)((bytes[idx++] << 8) | bytes[idx++]);
                minute = (int)((bytes[idx++] << 8) | bytes[idx++]);
                thedate = String.Format("{0}年{1}月{2}日{3}时{4}分", year, month, day, hour, minute);
                height = (int)((bytes[idx++] << 8) | bytes[idx++]);
                width = (int)((bytes[idx++] << 8) | bytes[idx++]);
                temp = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X}{1:X}", bytes[idx++], bytes[idx++]), 16));//Convert.ToInt32((int)((bytes[idx++] << 8) | bytes[idx++]));
                loadspeed = (int)((bytes[idx++] << 8) | bytes[idx++]);
                nodecnt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                sensor = (int)((bytes[idx++] << 8) | bytes[idx++]);
                rt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                maxstrength = (int)((bytes[idx++] << 8) | bytes[idx++]);
                maxoffset = (int)((bytes[idx++] << 8) | bytes[idx++]);
                et = (int)((bytes[idx++] << 8) | bytes[idx++]);
                st = (int)((bytes[idx++] << 8) | bytes[idx++]);

                //int stime = 0;
                nodes.Add(new PilieNodeInfo(0, 0));
                while (idx < (bytes.Length - 2))
                {

                    int kn = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X}{1:X}", bytes[idx++], bytes[idx++]), 16));// (int)((bytes[idx++] << 8) | bytes[idx++]);
                    int offset = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X}{1:X}", bytes[idx++], bytes[idx++]), 16));//(int)((bytes[idx++] << 8) | bytes[idx++]);
                    if (kn < 0) kn = 0;
                    if (offset < 0) offset = 0;
                    nodes.Add(new PilieNodeInfo(offset, kn));
                    while (offset > chartformat.Xmax * 100)
                    {
                        chartformat.Xmax += chartformat.Xinterval;
                    }
                    while (kn > chartformat.Ymax * 100)
                    {
                        chartformat.Ymax += chartformat.Yinterval;
                    }
                    if (nodes.Count > nodecnt)
                        break;
                }

                specialnodes = new List<IXYNode>();
                specialnodes.Add(new PilieNodeInfo(maxoffset, maxstrength));


            }
        }

        protected override void makeSendBufferInternal()
        {
            sendbuffer[2] = 4;
        }

        public override List<string> getCSVLines()
        {
            throw new NotImplementedException();
        }

        public override void LoadFromCSV(string[] strs)
        {
            this.initCharFormat();
            int idx = 1;
            String[] strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            //日期
            DateTime dt = DateTime.Parse(strarr[1]);
            this.year = dt.Year;
            this.month = dt.Month;
            this.day = dt.Day;
            this.hour = dt.Hour;
            this.minute = dt.Minute;
            //编号
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.no = Convert.ToInt32(strarr[1]);
            this.MakeSendBuffer();
            this.SetReadNo(no);
            //试件宽度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.width = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * 10);
            //试件高度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.height = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * 10);

            //温度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.temp = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("℃", "")));

            //速度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.loadspeed = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm/min", "")));


            //记录点数
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.nodecnt = Convert.ToInt32(strarr[1]);

            //感应器
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.sensor = Convert.ToInt32(strarr[1].Replace("KN", ""));

            //RT
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.rt = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("MPa", "")) * 1000);

            //最大点压力
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxstrength = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("KN", "")) * 100);

            //最大点位移
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxoffset = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * 100);

            //ET
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.et = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("10\u207b\u2076", "")));

            //ST
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.st = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("MPa", "")) * 100);

            idx++;
            idx++;
            nodes = new List<IXYNode>();
            nodes.Add(new PilieNodeInfo(0, 0));
            for (; idx < strs.Length; idx++)
            {
                strarr = strs[idx].Split(AbstractRecordInfo.csvsepchar);
                int kpa = Convert.ToInt32(Convert.ToDouble(strarr[0]) * 100);
                int off = Convert.ToInt32(Convert.ToDouble(strarr[1]) * 100);
                nodes.Add(new PilieNodeInfo(off, kpa));
                while (off > chartformat.Xmax * 100)
                {
                    chartformat.Xmax += chartformat.Xinterval;
                }
                while (kpa > chartformat.Ymax * 100)
                {
                    chartformat.Ymax += chartformat.Yinterval;
                }
            }

            specialnodes = new List<IXYNode>();
            specialnodes.Add(new PilieNodeInfo(maxoffset, maxstrength));

            thedate = String.Format("{0}年{1}月{2}日{3}时{4}分", year, month, day, hour, minute);

        }

        public override DataTable getDispalyTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            DataRow dr = dt.NewRow();
            dr = dt.NewRow();
            dr[0] = "试验日期";
            dr[1] = String.Format("{0}-{1}-{2} {3}:{4}", this.year, this.month, this.day, this.hour, this.minute);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "编号";
            dr[1] = this.no;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试件直径";
            dr[1] = String.Format("{0:f1}mm", this.Diameter / 10f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试件高度";
            dr[1] = String.Format("{0:f1}mm", this.Height / 10f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "温度";
            dr[1] = String.Format("{0}℃", this.temp);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "加载速度";
            dr[1] = String.Format("{0}mm/min", this.loadspeed);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "记录点数";
            dr[1] = this.nodecnt;
            dt.Rows.Add(dr);

           /* dr = dt.NewRow();
            dr[0] = "感应器";
            dr[1] = String.Format("{0}KN", this.sensor);
            dt.Rows.Add(dr);
            */
            double _rt = rta * ((double)maxstrength * 10f) / ((double)height / 10f);
            double _xt = (double)(maxoffset/100f) * (0.135 + 0.5 * u) / (1.794 - 0.0314 * u);
            double _et = eta * _xt*1000000;// *((double)maxoffset / 100f);
            double _st = sta * ((double)maxstrength * 10f) / _xt;// ((double)maxoffset / 100f);
            

            dr = dt.NewRow();
            dr[0] = "最大点压力";
            dr[1] = String.Format("{0:f2}KN", this.maxstrength / 100f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点位移";
            dr[1] = String.Format("{0:f2}mm", this.maxoffset / 100f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "抗拉强度";
            dr[1] =  String.Format("{0:f3}MPa", this.rt / 1000f);//String.Format("{0:f3}MPa", _rt);// String.Format("{0:f3}MPa", this.rb / 1000f);
            dt.Rows.Add(dr);

            
            dr = dt.NewRow();
            dr[0] = "拉伸应变";//"EB";
            //dr[1] = String.Format("{0:d} ×10\u207b\u2076 με", this.eb *1000);
            dr[1] = String.Format("{0:d} 10\u207b\u2076", this.et);//String.Format("{0:d} 10\u207b\u2076", Convert.ToInt32(_et));// String.Format("{0:d} με", this.eb * 100);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "劲度模量";
            dr[1] = String.Format("{0:f2}MPa", this.st/100f);//String.Format("{0:f2}MPa", _st); // String.Format("{0:f1}MPa", this.sb / 10f);
            dt.Rows.Add(dr);

            displaymaxidx = dt.Rows.Count - 1;
            return dt;
        }

        public override System.Data.DataTable getDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            DataRow dr = dt.NewRow();
            dr[0] = "试验模式";
            dr[1] = this.recordname;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试验日期";
            dr[1] = String.Format("{0}-{1}-{2} {3}:{4}", this.year, this.month, this.day, this.hour, this.minute);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "编号";
            dr[1] = this.no;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试件直径";
            dr[1] = String.Format("{0:f1}mm", this.Diameter / 10f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试件高度";
            dr[1] = String.Format("{0:f1}mm", this.Height / 10f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "温度";
            dr[1] = String.Format("{0}℃", this.temp);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "加载速度";
            dr[1] = String.Format("{0}mm/min", this.loadspeed);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "记录点数";
            dr[1] = this.nodecnt;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "感应器";
            dr[1] = String.Format("{0}KN", this.sensor);
            dt.Rows.Add(dr);

            double _rt = rta * ((double)maxstrength * 10f) / ((double)height / 10f);
            double _xt = (double)(maxoffset / 100f) * (0.135 + 0.5 * u) / (1.794 - 0.0314 * u);
            double _et = eta * _xt * 1000000;// *((double)maxoffset / 100f);
            double _st = sta * ((double)maxstrength * 10f) / _xt;// ((double)maxoffset / 100f);
            
            dr = dt.NewRow();
            dr[0] = "抗拉强度";
            dr[1] = String.Format("{0:f3}MPa", this.rt/1000f);//String.Format("{0:f3}MPa", _rt);// String.Format("{0:f3}MPa", this.rb / 1000f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点压力";
            dr[1] = String.Format("{0:f2}KN", this.maxstrength / 100f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点位移";
            dr[1] = String.Format("{0:f2}mm", this.maxoffset / 100f);
            dt.Rows.Add(dr);

            

            
            dr = dt.NewRow();
            dr[0] = "拉伸应变";//"EB";
            //dr[1] = String.Format("{0:d} ×10\u207b\u2076 με", this.eb *1000);
            dr[1] = String.Format("{0:d} 10\u207b\u2076", this.et);//String.Format("{0:d}", Convert.ToInt32(_et));// String.Format("{0:d} με", this.eb * 100);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "劲度模量";
            dr[1] = String.Format("{0:f2}MPa", this.st/100f); //String.Format("{0:f2}MPa", _st); // String.Format("{0:f1}MPa", this.sb / 10f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "压力(KN)";
            dr[1] = "位移(mm)";
            dt.Rows.Add(dr);


            foreach (IXYNode node in this.nodes)
            {
                if (node.getX() != 0 && node.getY() != 0)
                {
                    dr = dt.NewRow();
                    dr[0] = String.Format("{0:f3}", node.getNodeY());
                    dr[1] = String.Format("{0:f2}", node.getNodeX());
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public override void EditValue(string p, string newvalue)
        {

            //this.sensor
            if (p.Equals(MAXOFFSET))
            {
                newvalue = newvalue.Replace("mm", "");
                int newmaxoff = Convert.ToInt32(Convert.ToDouble(newvalue)*100);
                int oldmax = this.maxoffset;
                this.maxoffset = newmaxoff;
                double _rt = rta * ((double)maxstrength * 10f) / ((double)height / 10f);
                double _xt = (double)(maxoffset / 100f) * (0.135 + 0.5 * u) / (1.794 - 0.0314 * u);
                double _et = eta * _xt * 1000000;// *((double)maxoffset / 100f);
                double _st = sta * ((double)maxstrength * 10f) / _xt;// ((double)maxoffset / 100f);

                this.rt = Convert.ToInt32(_rt * 1000);
                this.et=Convert.ToInt32(_et);
                this.st = Convert.ToInt32(_st * 100);

                int _offset = newmaxoff - oldmax;

                //List<IXYNode> _nodes = new List<IXYNode>();
                //_nodes.Add(new ModulusStengthNodeInfo(0, 0));
                foreach (IXYNode node in nodes)
                {
                    // if (node.getX() != 0)
                    //{
                    PilieNodeInfo _node = node as PilieNodeInfo;
                    if (_node != null)
                    {
                        _node.offset += _offset;
                    }
                    //}
                }
                specialnodes.Clear();
                specialnodes.Add(new PilieNodeInfo(maxoffset, maxstrength));

            }
        }

        public override List<EditableItem> GetEditableList()
        {
            List<EditableItem> list = new List<EditableItem>();
            list.Add(new EditableItem("位移修正", MAXOFFSET));
            return list;
        }

        public override string GetEditableValuStr(string valuename)
        {
            if (valuename.Equals(MAXOFFSET))
            {
                return String.Format("{0:f2}mm", this.maxoffset / 100f);
            }
            else
                return base.GetEditableValuStr(valuename);
        }
    }

    public class PilieNodeInfo : IXYNode
    {
        internal int kn;
        internal int offset;

        public int KN { get { return kn; } }
        public int Offset { get { return offset; } }

        public PilieNodeInfo(int offset, int kn)
        {
            this.kn = kn;
            this.offset = offset;
        }

        #region IXYNode Members

        public int getX()
        {
            return this.offset;
        }

        public int getY()
        {
            return this.kn;
        }

        public double getNodeX()
        {
            float x = this.offset / 100f;
            return x;
        }

        public double getNodeY()
        {
            float y = this.kn / 100f;
            return y;
        }

        #endregion
    }
}
