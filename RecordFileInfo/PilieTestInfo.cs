﻿using System;
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
        internal int xdiv = 1000;//位移改3位小数 xidv=100;
        internal int ydiv = 100;
        internal static float xdivf = 1000f;//位移改3位小数 xidvf=100;
        internal static float ydivf = 100f;

        protected const float RT_SCALA_CONST = 10000f;

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
                temp = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//Convert.ToInt32((int)((bytes[idx++] << 8) | bytes[idx++]));
                loadspeed = (int)((bytes[idx++] << 8) | bytes[idx++]);
                nodecnt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                sensor = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //shiyanno1 = (int)bytes[idx++];
                //shiyanno2 = (int)bytes[idx++];
                
                rt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                maxstrength = (int)((bytes[idx++] << 8) | bytes[idx++]);
                maxoffset = (int)((bytes[idx++] << 8) | bytes[idx++]);
                et = (int)((bytes[idx++] << 8) | bytes[idx++]);
                st = (int)((bytes[idx++] << 8) | bytes[idx++]);

                //int stime = 0;
                nodes.Add(new PilieNodeInfo(0, 0));
                while (idx < (bytes.Length - 2))
                {

                    int kn = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));// (int)((bytes[idx++] << 8) | bytes[idx++]);
                    int offset = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//(int)((bytes[idx++] << 8) | bytes[idx++]);
                    if (kn < 0) kn = 0;
                    if (offset < 0) offset = 0;
                    nodes.Add(new PilieNodeInfo(offset, kn));
                    while (offset > chartformat.Xmax * xdiv)
                    {
                        chartformat.Xmax += chartformat.Xinterval;
                    }
                    while (kn > chartformat.Ymax * ydiv)
                    {
                        chartformat.Ymax += chartformat.Yinterval;
                    }
                    if (nodes.Count > nodecnt)
                        break;
                }

                specialnodes = new List<IXYNode>();
                specialnodes.Add(new PilieNodeInfo(maxoffset, maxstrength));
                this.shuffer(specialnodes[0], nodes);


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
            //dr[0] = "编号";
            //dr[1] = this.no;
            //dt.Rows.Add(dr);
            dr[0] = "试验编号";
            dr[1] = String.Format("{0}-{1}", shiyanno1, shiyanno2);
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
            dr[1] = String.Format("{0:f3}mm", this.maxoffset / xdivf);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "抗拉强度";
            dr[1] = String.Format("{0:f4}MPa", this.rt / RT_SCALA_CONST);//String.Format("{0:f3}MPa", _rt);// String.Format("{0:f3}MPa", this.rb / 1000f);
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

        protected override int LoadHeaderFromCSV(String[] strs, int index)
        {
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
            //String[] bianhaostrarr = strarr[1].Split('-');
            //shiyanno1 = Convert.ToInt32(bianhaostrarr[0]);
            //if (bianhaostrarr.Length > 1)
            //    shiyanno2 = Convert.ToInt32(bianhaostrarr[1]);
            //else
            //    shiyanno2 = 1;

            //RT
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.rt = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("MPa", "")) * RT_SCALA_CONST);

            //最大点压力
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxstrength = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("KN", "")) * 100);

            //最大点位移
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxoffset = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * xdiv);

            //ET
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.et = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("10\u207b\u2076", "")));

            //ST
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.st = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("MPa", "")) * 100);


            thedate = String.Format("{0}年{1}月{2}日{3}时{4}分", year, month, day, hour, minute);
            return idx;
        }

        protected override int LoadBodyFromCSV(String[] strs, int index)
        {
            int idx = index;
            String[] strarr;

            nodes = new List<IXYNode>();
            nodes.Add(new PilieNodeInfo(0, 0));
            for (; idx < strs.Length; idx++)
            {
                strarr = strs[idx].Split(AbstractRecordInfo.csvsepchar);
                int kpa = Convert.ToInt32(Convert.ToDouble(strarr[0]) * PilieTestInfo.ydivf);
                int off = Convert.ToInt32(Convert.ToDouble(strarr[1]) * PilieTestInfo.xdivf);
                nodes.Add(new PilieNodeInfo(off, kpa));
                while (off > chartformat.Xmax * xdiv)
                {
                    chartformat.Xmax += chartformat.Xinterval;
                }
                while (kpa > chartformat.Ymax * ydiv)
                {
                    chartformat.Ymax += chartformat.Yinterval;
                }
            }

            specialnodes = new List<IXYNode>();
            specialnodes.Add(new PilieNodeInfo(maxoffset, maxstrength));
            this.shuffer(specialnodes[0], nodes);
            return idx;
        }


        public override int NodeCntIdx
        {
            get
            {
                return 7;
            }
           // DataTable dt = new DataTable();
           // dt.Columns.Add();
           // dt.Columns.Add();
           // DataRow dr = dt.NewRow();
           // dr = dt.NewRow();
           // dr[0] = "试验日期";
           // dr[1] = String.Format("{0}-{1}-{2} {3}:{4}", this.year, this.month, this.day, this.hour, this.minute);
           // dt.Rows.Add(dr);

           // dr = dt.NewRow();
           // dr[0] = "传感器";
           // dr[1] = String.Format("{0}KN", this.sensor);
           // dt.Rows.Add(dr);
           // //dr[0] = "试验编号";
           // //dr[1] = String.Format("{0}-{1}", shiyanno1, shiyanno2);
           // //dt.Rows.Add(dr);

           // dr = dt.NewRow();
           // dr[0] = "试件直径";
           // dr[1] = String.Format("{0:f1}mm", this.Diameter / 10f);
           // dt.Rows.Add(dr);

           // dr = dt.NewRow();
           // dr[0] = "试件高度";
           // dr[1] = String.Format("{0:f1}mm", this.Height / 10f);
           // dt.Rows.Add(dr);

           // dr = dt.NewRow();
           // dr[0] = "温度";
           // dr[1] = String.Format("{0}℃", this.temp);
           // dt.Rows.Add(dr);

           // dr = dt.NewRow();
           // dr[0] = "加载速度";
           // dr[1] = String.Format("{0}mm/min", this.loadspeed);
           // dt.Rows.Add(dr);

           // dr = dt.NewRow();
           // dr[0] = "记录点数";
           // dr[1] = this.nodecnt;
           // dt.Rows.Add(dr);

           ///* dr = dt.NewRow();
           // dr[0] = "感应器";
           // dr[1] = String.Format("{0}KN", this.sensor);
           // dt.Rows.Add(dr);
           // */
           // double _rt = rta * ((double)maxstrength * 10f) / ((double)height / 10f);
           // double _xt = (double)(maxoffset/100f) * (0.135 + 0.5 * u) / (1.794 - 0.0314 * u);
           // double _et = eta * _xt*1000000;// *((double)maxoffset / 100f);
           // double _st = sta * ((double)maxstrength * 10f) / _xt;// ((double)maxoffset / 100f);
            

           // dr = dt.NewRow();
           // dr[0] = "最大点压力";
           // dr[1] = String.Format("{0:f2}KN", this.maxstrength / 100f);
           // dt.Rows.Add(dr);

           // dr = dt.NewRow();
           // dr[0] = "最大点位移";
           // dr[1] = String.Format("{0:f3}mm", this.maxoffset / xdivf);
           // dt.Rows.Add(dr);

           // dr = dt.NewRow();
           // dr[0] = "抗拉强度";
           // dr[1] = String.Format("{0:f4}MPa", this.rt / RT_SCALA_CONST);//String.Format("{0:f3}MPa", _rt);// String.Format("{0:f3}MPa", this.rb / 1000f);
           // dt.Rows.Add(dr);

            
           // dr = dt.NewRow();
           // dr[0] = "拉伸应变";//"EB";
           // //dr[1] = String.Format("{0:d} ×10\u207b\u2076 με", this.eb *1000);
           // dr[1] = String.Format("{0:d} 10\u207b\u2076", this.et);//String.Format("{0:d} 10\u207b\u2076", Convert.ToInt32(_et));// String.Format("{0:d} με", this.eb * 100);
           // dt.Rows.Add(dr);

           // dr = dt.NewRow();
           // dr[0] = "劲度模量";
           // dr[1] = String.Format("{0:f2}MPa", this.st/100f);//String.Format("{0:f2}MPa", _st); // String.Format("{0:f1}MPa", this.sb / 10f);
           // dt.Rows.Add(dr);

           // displaymaxidx = dt.Rows.Count - 1;
           // return dt;
        }

        public override DataTable getHeaderTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            dt.Columns.Add();//unit
            dt.Columns.Add();//is readonly
            DataRow dr = dt.NewRow();
            dr[0] = "试验模式";
            dr[1] = this.recordname;
            dr[3] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试验日期";
            dr[1] = String.Format("{0}-{1}-{2} {3}:{4}", this.year, this.month, this.day, this.hour, this.minute);
            dr[3] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "编号";
            dr[1] = this.no;
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试件直径";
            dr[1] = String.Format("{0:f1}", this.Diameter / 10f);
            dr[2] = "mm";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试件高度";
            dr[1] = String.Format("{0:f1}", this.Height / 10f);
            dr[2] = "mm";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "温度";
            dr[1] = String.Format("{0}", this.temp);
            dr[2] = "℃";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "加载速度";
            dr[1] = String.Format("{0}", this.loadspeed);
            dr[2] = "mm/min";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "记录点数";
            dr[1] = this.nodecnt;
            dr[3] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "传感器";
            dr[1] = String.Format("{0}KN", this.sensor);
            dt.Rows.Add(dr);
            //dr[0] = "试验编号";
            //dr[1] = String.Format("{0}-{1}", this.shiyanno1, this.shiyanno2);
            //dt.Rows.Add(dr);
            dr[0] = "试验编号";
            dr[1] = String.Format("{0}-{1}", this.shiyanno1, this.shiyanno2);
            dr[3] = false;
            dt.Rows.Add(dr);

            double _rt = rta * ((double)maxstrength * 10f) / ((double)height / 10f);
            double _xt = (double)(maxoffset / 100f) * (0.135 + 0.5 * u) / (1.794 - 0.0314 * u);
            double _et = eta * _xt * 1000000;// *((double)maxoffset / 100f);
            double _st = sta * ((double)maxstrength * 10f) / _xt;// ((double)maxoffset / 100f);

            dr = dt.NewRow();
            dr[0] = "抗拉强度";
            dr[1] = String.Format("{0:f4}", this.rt / RT_SCALA_CONST);//String.Format("{0:f3}MPa", _rt);// String.Format("{0:f3}MPa", this.rb / 1000f);
            dr[2] = "MPa";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点压力";
            dr[1] = String.Format("{0:f2}", this.maxstrength / 100f);
            dr[2] = "KN";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点位移";
            dr[1] = String.Format("{0:f3}", this.maxoffset / xdivf);
            dr[2] = "mm";
            dr[3] = false;
            dt.Rows.Add(dr);




            dr = dt.NewRow();
            dr[0] = "拉伸应变";//"EB";
            //dr[1] = String.Format("{0:d} ×10\u207b\u2076 με", this.eb *1000);
            dr[1] = String.Format("{0:d}", this.et);//String.Format("{0:d}", Convert.ToInt32(_et));// String.Format("{0:d} με", this.eb * 100);
            dr[2] = " 10\u207b\u2076";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "劲度模量";
            dr[1] = String.Format("{0:f2}", this.st / 100f); //String.Format("{0:f2}MPa", _st); // String.Format("{0:f1}MPa", this.sb / 10f);
            dr[2] = "MPa";
            dr[3] = false;
            dt.Rows.Add(dr);
            return dt;

        }

        public override DataTable getBodyTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            dt.Columns[0].ColumnName = "压力(KN)";
            dt.Columns[1].ColumnName = "位移(mm)";
            DataRow dr = null;

            int idx = 0;
            foreach (IXYNode node in this.nodes)
            {
                //if (node.getX() != 0 && node.getY() != 0)
                if (idx++ > 0)
                {
                    dr = dt.NewRow();
                    dr[0] = String.Format("{0:f3}", node.getNodeY());
                    dr[1] = String.Format("{0:f3}", node.getNodeX());
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
                int newmaxoff = Convert.ToInt32(Convert.ToDouble(newvalue)*xdiv);
                int oldmax = this.maxoffset;
                this.maxoffset = newmaxoff;
                double _rt = rta * ((double)maxstrength * 10f) / ((double)height / 10f);
                double _xt = (double)(maxoffset / 100f) * (0.135 + 0.5 * u) / (1.794 - 0.0314 * u);
                double _et = eta * _xt * 1000000;// *((double)maxoffset / 100f);
                double _st = sta * ((double)maxstrength * 10f) / _xt;// ((double)maxoffset / 100f);

                this.rt = Convert.ToInt32(_rt * RT_SCALA_CONST);
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
                return String.Format("{0:f3}mm", this.maxoffset / xdivf);
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
            float x = this.offset / PilieTestInfo.xdivf;
            return x;
        }

        public double getNodeY()
        {
            float y = this.kn / PilieTestInfo.ydivf;
            return y;
        }

        #endregion
    }
}
