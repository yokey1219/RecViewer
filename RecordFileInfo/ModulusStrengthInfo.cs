﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RecordFileUtil
{
    public class ModulusStrengthInfo:AbstractRecordInfo
    {
        protected  const String KUAZHONGNAODU = "跨中挠度";
        protected List<IXYNode> nodes;
        protected List<IXYNode> specialnodes;
        protected int rb;
        protected int maxstrength;
        protected int maxoffset;
        protected int eb;
        protected int sb;

        public int RB { get { return rb; } }
        public int MaxStrength { get { return maxstrength; } }
        public int MaxOffset { get { return maxoffset; } }
        public int EB { get { return eb; } }
        public int SB { get { return sb; } }

    

        public override List<IXYNode> getXYNodes()
        {
            return nodes;
        }

        public override List<IXYNode> getSpecialNodes()
        {
            return  specialnodes;;
        }

        public override void initCharFormat()
        {
            chartformat = new ChartFormat();
            chartformat.Xname = "mm";
            chartformat.Yname = "KN";
            chartformat.Xmin = 0;
            chartformat.Ymin = 0;
            chartformat.Xmax = 10;
            chartformat.Ymax = 2.5;
            chartformat.Xinterval = 2;
            chartformat.Yinterval = 0.5;
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
                width = (int)((bytes[idx++] << 8) | bytes[idx++]);
                height = (int)((bytes[idx++] << 8) | bytes[idx++]);
                temp = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X}{1:X}", bytes[idx++], bytes[idx++]), 16));//Convert.ToInt32((int)((bytes[idx++] << 8) | bytes[idx++]));
                loadspeed = (int)((bytes[idx++] << 8) | bytes[idx++]);
                nodecnt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                sensor = (int)((bytes[idx++] << 8) | bytes[idx++]);
                rb = (int)((bytes[idx++] << 8) | bytes[idx++]);
                maxstrength = (int)((bytes[idx++] << 8) | bytes[idx++]);
                maxoffset = (int)((bytes[idx++] << 8) | bytes[idx++]);
                eb = (int)((bytes[idx++] << 8) | bytes[idx++]);
                sb = (int)((bytes[idx++] << 8) | bytes[idx++]);
                
                //int stime = 0;
                nodes.Add(new ModulusStengthNodeInfo(0, 0));
                int oldx, oldy;
                oldx = oldy = 0;
                while (idx < (bytes.Length - 2))
                {

                    int kn = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X}{1:X}", bytes[idx++], bytes[idx++]), 16));//(int)((bytes[idx++] << 8) | bytes[idx++]);
                    int offset = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X}{1:X}", bytes[idx++], bytes[idx++]), 16));//(int)((bytes[idx++] << 8) | bytes[idx++]);

                    if (kn < 0 || offset < 0) { kn = oldx; offset = oldy; }

                    nodes.Add(new ModulusStengthNodeInfo(offset,kn));
                    
                    oldx = kn;
                    oldy = offset;
                    
                    while (offset > chartformat.Xmax*100)
                    {
                        chartformat.Xmax += chartformat.Xinterval;
                    }
                    while (kn > chartformat.Ymax*1000)
                    {
                        chartformat.Ymax += chartformat.Yinterval;
                    }
                }

                specialnodes = new List<IXYNode>();
                specialnodes.Add(new ModulusStengthNodeInfo(maxoffset, maxstrength));
                

            }
        }

        protected override void makeSendBufferInternal()
        {
            sendbuffer[2] = 3;
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

            //试件跨度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.sensor = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", ""))*10);
            
            //RB
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.rb = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("MPa", "")) * 1000);

            //最大点压力
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxstrength = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("KN", "")) * 1000);

            //最大点位移
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxoffset = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * 100);

            //EB
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.eb = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("με", "")) /100);

            //SB
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.sb = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("MPa", "")) * 10);
   
            idx++;
            idx++;
            nodes = new List<IXYNode>();
            nodes.Add(new ModulusStengthNodeInfo(0, 0));
            for (; idx < strs.Length; idx++)
            {
                strarr = strs[idx].Split(AbstractRecordInfo.csvsepchar);
                int kpa = Convert.ToInt32(Convert.ToDouble(strarr[0]) * 1000);
                int off = Convert.ToInt32(Convert.ToDouble(strarr[1]) * 100);
                nodes.Add(new ModulusStengthNodeInfo(off, kpa));
                while(off>chartformat.Xmax*100)
                {
                    chartformat.Xmax += chartformat.Xinterval;
                }
                while (kpa > chartformat.Ymax * 1000)
                {
                    chartformat.Ymax += chartformat.Yinterval;
                }
            }

            specialnodes = new List<IXYNode>();
            specialnodes.Add(new ModulusStengthNodeInfo(maxoffset, maxstrength));

            thedate = String.Format("{0}年{1}月{2}日{3}时{4}分", year, month, day, hour, minute);

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
            dr[0] = "试件宽度";
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
            dr[0] = "试件跨径";
            dr[1] = String.Format("{0:f1}mm", this.sensor/10f);
            dt.Rows.Add(dr);

            double _rb;
            _rb = (double)3 * (this.sensor / 10f) * (this.maxstrength) / (this.width / 10f) / (this.height / 10f) / (this.height / 10f)/2;
            dr = dt.NewRow();
            dr[0] = "RB";
            dr[1] = String.Format("{0:f1}MPa", _rb);// String.Format("{0:f3}MPa", this.rb / 1000f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点压力";
            dr[1] = String.Format("{0:f3}KN", this.maxstrength / 1000f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点位移";
            dr[1] = String.Format("{0:f2}mm", this.maxoffset / 100f);
            dt.Rows.Add(dr);

            double _eb;
            _eb = (double)6 * (this.height / 10f) * (this.maxoffset / 100f) / (this.sensor / 10f) / (this.sensor / 10f);
            dr = dt.NewRow();
            dr[0] = "εB";//"EB";
            //dr[1] = String.Format("{0:d} ×10\u207b\u2076 με", this.eb *1000);
            dr[1] = String.Format("{0:f0} με", _eb*1000000);// String.Format("{0:d} με", this.eb * 100);
            dt.Rows.Add(dr);
            
            dr = dt.NewRow();
            dr[0] = "SB";
            dr[1] = String.Format("{0:f1}MPa", _rb/_eb); // String.Format("{0:f1}MPa", this.sb / 10f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "压力(KN)";
            dr[1] = "挠度(mm)";
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
            if (p.Equals(KUAZHONGNAODU))
            {
                newvalue = newvalue.Replace("mm", "");
                int newmaxoff = Convert.ToInt32(Convert.ToDouble(newvalue) * 100);
                int oldmax = this.maxoffset;
                this.maxoffset = newmaxoff;
                this.eb = this.eb*1000 * this.maxoffset/oldmax/1000;
                this.sb = this.sb * oldmax / this.maxoffset;

                int _offset = newmaxoff - oldmax;

                //List<IXYNode> _nodes = new List<IXYNode>();
                //_nodes.Add(new ModulusStengthNodeInfo(0, 0));
                foreach (IXYNode node in nodes)
                {
                   // if (node.getX() != 0)
                    //{
                        ModulusStengthNodeInfo _node = node as ModulusStengthNodeInfo;
                        if (_node != null)
                        {
                            _node.offset += _offset;
                        }
                    //}
                }
                specialnodes.Clear();
                specialnodes.Add(new ModulusStengthNodeInfo(maxoffset, maxstrength));

            }
        }

        public override List<EditableItem> GetEditableList()
        {
            List<EditableItem> list = new List<EditableItem>();
            list.Add(new EditableItem("挠度修正", KUAZHONGNAODU));
            return list;
        }

        public override string GetEditableValuStr(string valuename)
        {
            if (valuename.Equals(KUAZHONGNAODU))
            {
                return String.Format("{0:f2}mm", this.maxoffset / 100f);
            }
            else
                return base.GetEditableValuStr(valuename);
        }
    }

    public class ModulusStengthNodeInfo : IXYNode
    {
        internal int kn;
        internal int offset;

        public int KN { get { return kn; } }
        public int Offset { get { return offset; } }

        public ModulusStengthNodeInfo(int offset, int kn)
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
            float y = this.kn / 1000f;
            return y;
        }

        #endregion
    }
}
