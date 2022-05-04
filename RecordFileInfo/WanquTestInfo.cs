﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RecordFileUtil
{
    public class WanquTestInfo:AbstractRecordInfo
    {
        protected  const String KUAZHONGNAODU = "跨中挠度";
        protected List<IXYNode> nodes;
        protected List<IXYNode> specialnodes;
        protected int rb;
        protected int maxstrength;
        protected int maxoffset;
        protected int eb;
        protected int sb;
        internal int xdiv = 1000;//位移改3位小数 xidv=100;
        internal int ydiv = 1000;
        internal static float xdivf = 1000f;//位移改3位小数 xidvf=100;
        internal static float ydivf = 1000f;

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
                temp = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//Convert.ToInt32((int)((bytes[idx++] << 8) | bytes[idx++]));
                sensor = (int)((bytes[idx++] << 8) | bytes[idx++]);
                nodecnt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //sensor = (int)((bytes[idx++] << 8) | bytes[idx++]);
                shiyanno1 = (int)bytes[idx++];
                shiyanno2 = (int)bytes[idx++];
                
                rb = (int)((bytes[idx++] << 8) | bytes[idx++]);
                maxstrength = (int)((bytes[idx++] << 8) | bytes[idx++]);
                maxoffset = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //eb = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //sb = (int)((bytes[idx++] << 8) | bytes[idx++]);
                eb = (int)((bytes[idx++] << 32) | bytes[idx++] << 16 | (bytes[idx++] << 8) | bytes[idx++]); 
                sb=Convert.ToInt32((double)(rb/100.0) / ((double)eb/10000000.0));

                double _rb;
                _rb = (double)3 * (this.sensor / 10f) * (this.maxstrength) / (this.width / 10f) / (this.height / 10f) / (this.height / 10f) / 2;
                double _eb;
                _eb = (double)6 * (this.height / 10f) * (this.maxoffset / xdivf) / (this.sensor / 10f) / (this.sensor / 10f);
                int __eb = Convert.ToInt32(_eb * 10000000);
                int _diff = __eb - eb;
                if(_diff<-500||_diff>500)
                    eb += 65536;
                nodes.Add(new WanquNodeInfo(0, 0));
                while (idx < (bytes.Length - 2))
                {

                    int kn = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//(int)((bytes[idx++] << 8) | bytes[idx++]);
                    int offset = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//(int)((bytes[idx++] << 8) | bytes[idx++]);
                    if (kn < 0) kn = 0;
                    if (offset < 0) offset = 0;
                    nodes.Add(new WanquNodeInfo(offset,kn));
                    while (offset > chartformat.Xmax*xdiv)
                    {
                        chartformat.Xmax += chartformat.Xinterval;
                    }
                    while (kn > chartformat.Ymax*ydiv)
                    {
                        chartformat.Ymax += chartformat.Yinterval;
                    }

                    if (nodes.Count > nodecnt)
                        break;
                }

                specialnodes = new List<IXYNode>();
                specialnodes.Add(new WanquNodeInfo(maxoffset, maxstrength));
                

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
            int idx = LoadHeaderFromCSV(strs,1);
   
            idx++;
            idx++;

            LoadBodyFromCSV(strs, idx);

        }

        public override DataTable getDispalyTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            DataRow dr = dt.NewRow();
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
            dr[0] = "记录点数";
            dr[1] = this.nodecnt;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试件跨径";
            dr[1] = String.Format("{0:f1}mm", this.sensor / 10f);
            dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr[0] = "传感器大小";
            //dr[1] = String.Format("{0}KN", this.sensor);
            //dt.Rows.Add(dr);
            //dr[0] = "试验编号";
            //dr[1] = String.Format("{0}-{1}", this.shiyanno1, this.shiyanno2);
            //dt.Rows.Add(dr);
            

            dr = dt.NewRow();
            dr[0] = "最大点压力";
            dr[1] = String.Format("{0:f3}KN", this.maxstrength / 1000f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "跨中挠度";
            dr[1] = String.Format("{0:f3}mm", this.maxoffset / xdivf);
            dt.Rows.Add(dr);

            double _rb;
            _rb = (double)3 * (this.sensor / 10f) * (this.maxstrength) / (this.width / 10f) / (this.height / 10f) / (this.height / 10f) / 2;
            dr = dt.NewRow();
            dr[0] = "RB";
            dr[1] = String.Format("{0:f3}MPa", this.rb / 1000f);// String.Format("{0:f3}MPa", _rb);//
            dt.Rows.Add(dr);

            double _eb;
            _eb = (double)6 * (this.height / 10f) * (this.maxoffset / xdivf) / (this.sensor / 10f) / (this.sensor / 10f);
            dr = dt.NewRow();
            dr[0] = "εB";//"EB";
            //dr[1] = String.Format("{0:d} ×10\u207b\u2076 με", this.eb *1000);
            dr[1] = String.Format("{0:f1} ×10\u207b\u2076 με", this.eb /10f); //String.Format("{0:f1} ×10\u207b\u2076 με", _eb * 1000000);// String.Format("{0:d} με", this.eb * 100);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "SB";
            dr[1] = String.Format("{0:f1}MPa", this.sb / 10f); //String.Format("{0:f1}MPa", _rb / _eb); // 
            dt.Rows.Add(dr);

            displaymaxidx = dt.Rows.Count - 1;
            return dt;
        }

        protected override int LoadHeaderFromCSV(String[] strs,int index)
        {
            int idx = index;
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
            //strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            //this.loadspeed = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm/min", "")));


            //记录点数
            xdiv = 1000;
            xdivf = 1000f;
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            if (strarr[0].Equals("加载速度"))
            {
                this.loadspeed = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm/min", "")));
                strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
                this.nodecnt = Convert.ToInt32(strarr[1]);
                xdiv = 100;
                xdivf = 100f;
            }
            else
                this.nodecnt = Convert.ToInt32(strarr[1]);

            //试件跨度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.sensor = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * 10);

            //试验编号
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            if (strarr[0].Equals("试验编号"))
            {
                String[] bianhaostrarr = strarr[1].Split('-');
                shiyanno1 = Convert.ToInt32(bianhaostrarr[0]);
                if (bianhaostrarr.Length > 1)
                    shiyanno2 = Convert.ToInt32(bianhaostrarr[1]);
                else
                    shiyanno2 = 1;

                //RB
                strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            }
            this.rb = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("MPa", "")) * 1000);

            //最大点压力
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxstrength = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("KN", "")) * 1000);

            //最大点位移
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxoffset = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * xdiv);

            //EB
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.eb = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("με", "").Replace("×10\u207b\u2076", "")) * 10);

            //SB
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.sb = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("MPa", "")) * 10);

            thedate = String.Format("{0}年{1}月{2}日{3}时{4}分", year, month, day, hour, minute);
            return idx;
        }

        protected override int LoadBodyFromCSV(String[] strs,int index)
        {
            int idx = index;
            String[] strarr;
            nodes = new List<IXYNode>();
            nodes.Add(new WanquNodeInfo(0, 0));
            for (; idx < strs.Length; idx++)
            {
                strarr = strs[idx].Split(AbstractRecordInfo.csvsepchar);
                int kpa = Convert.ToInt32(Convert.ToDouble(strarr[0]) * 1000);
                int off = Convert.ToInt32(Convert.ToDouble(strarr[1]) * xdiv);
                nodes.Add(new WanquNodeInfo(off, kpa));
                while (off > chartformat.Xmax * xdiv)
                {
                    chartformat.Xmax += chartformat.Xinterval;
                }
                while (kpa > chartformat.Ymax * 1000)
                {
                    chartformat.Ymax += chartformat.Yinterval;
                }
            }

            specialnodes = new List<IXYNode>();
            specialnodes.Add(new WanquNodeInfo(maxoffset, maxstrength));
            return idx;
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
            dr[2] = "";
            dr[3] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试验日期";
            dr[1] = String.Format("{0}-{1}-{2} {3}:{4}", this.year, this.month, this.day, this.hour, this.minute);
            dr[3] = true;//is readonly
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "编号";
            dr[1] = this.no;
            dr[3] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试件宽度";
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

            //dr = dt.NewRow();
            //dr[0] = "加载速度";
            //dr[1] = String.Format("{0}mm/min", this.loadspeed);
            //dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "记录点数";
            dr[1] = this.nodecnt;
            dr[3] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试件跨径";
            dr[1] = String.Format("{0:f1}", this.sensor / 10f);
            dr[2] = "mm";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试验编号";
            dr[1] = String.Format("{0}-{1}", this.shiyanno1, this.shiyanno2);
            dr[3] = false;
            dt.Rows.Add(dr);

            double _rb;
            _rb = (double)3 * (this.sensor / 10f) * (this.maxstrength) / (this.width / 10f) / (this.height / 10f) / (this.height / 10f) / 2;
            dr = dt.NewRow();
            dr[0] = "RB";
            dr[1] = String.Format("{0:f3}", this.rb / 1000f); //String.Format("{0:f3}MPa", _rb);//
            dr[2] = "MPa";
            dr[3] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点压力";
            dr[1] = String.Format("{0:f3}", this.maxstrength / 1000f);
            dr[2] = "KN";
            dr[3] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点位移";
            dr[1] = String.Format("{0:f3}", this.maxoffset / xdivf);
            dr[2] = "mm";
            dr[3] = true;
            dt.Rows.Add(dr);

            double _eb;
            _eb = (double)6 * (this.height / 10f) * (this.maxoffset / 100f) / (this.sensor / 10f) / (this.sensor / 10f);
            dr = dt.NewRow();
            dr[0] = "εB";//"EB";
            //dr[1] = String.Format("{0:d} ×10\u207b\u2076 με", this.eb *1000);
            dr[1] = String.Format("{0:f1} ", this.eb / 10f);// String.Format("{0:f1} ×10\u207b\u2076 με", _eb * 1000000);// 
            dr[2] = "×10\u207b\u2076 με";
            dr[3] = true;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "SB";
            dr[1] = String.Format("{0:f1}", this.sb / 10f);//String.Format("{0:f1}MPa", _rb/_eb); //
            dr[2] = "MPa";
            dr[3] = true;
            dt.Rows.Add(dr);
            return dt;

        }

        public override DataTable getBodyTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            dt.Columns[0].ColumnName = "压力(KN)";
            dt.Columns[1].ColumnName = "挠度(mm)";
            DataRow dr = null;

            int idx = 0;
            foreach (IXYNode node in this.nodes)
            {
                //if (node.getX() != 0 && node.getY() != 0)
                if(idx++>0)
                {
                    dr = dt.NewRow();
                    dr[0] = String.Format("{0:f3}", node.getNodeY());
                    dr[1] = String.Format("{0:f3}", node.getNodeX());
                    dt.Rows.Add(dr);

                }
            }
            return dt;
        }

        public override System.Data.DataTable getDataTable()
        {
           
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            DataTable dt_header = getHeaderTable();
            DataRow dr;
            foreach (DataRow dr_header in dt_header.Rows)
            {
                dr = dt.NewRow();
                dr[0] = dr_header[0];
                dr[1] = String.Format("{0}{1}",dr_header[1],dr_header[2]);
                dt.Rows.Add(dr);
            }

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dt.Rows.Add(dr);
            
            DataTable dt_body=getBodyTable();
            dr = dt.NewRow();
            dr[0] = dt_body.Columns[0].ColumnName;
            dr[1] = dt_body.Columns[1].ColumnName;
            dt.Rows.Add(dr);

            foreach (DataRow dr_body in dt_body.Rows)
            {
                dr = dt.NewRow();
                dr[0] = dr_body[0];
                dr[1] = dr_body[1];
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public override void EditValue(string p, string newvalue)
        {

            //this.sensor
            if (p.Equals(KUAZHONGNAODU))
            {
                newvalue = newvalue.Replace("mm", "");
                int newmaxoff = Convert.ToInt32(Convert.ToDouble(newvalue) * xdiv);
                int oldmax = this.maxoffset;
                this.maxoffset = newmaxoff;
                //this.eb = this.eb*1000 * this.maxoffset/oldmax/1000;
                //this.sb = this.sb * oldmax / this.maxoffset;

                double _rb;
                _rb = (double)3 * (this.sensor / 10f) * (this.maxstrength) / (this.width / 10f) / (this.height / 10f) / (this.height / 10f)/2;
                double _eb;
                _eb = (double)6 * (this.height / 10f) * (this.maxoffset / xdivf) / (this.sensor / 10f) / (this.sensor / 10f);
                this.eb = Convert.ToInt32(_eb * 10000000);
                this.sb = Convert.ToInt32((double)_rb * 10 / _eb);


                int _offset = newmaxoff - oldmax;

                //List<IXYNode> _nodes = new List<IXYNode>();
                //_nodes.Add(new ModulusStengthNodeInfo(0, 0));
                foreach (IXYNode node in nodes)
                {
                   // if (node.getX() != 0)
                    //{
                        WanquNodeInfo _node = node as WanquNodeInfo;
                        if (_node != null)
                        {
                            _node.offset += _offset;
                        }
                    //}
                }
                specialnodes.Clear();
                specialnodes.Add(new WanquNodeInfo(maxoffset, maxstrength));

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
                return String.Format("{0:f3}mm", this.maxoffset / xdivf);
            }
            else
                return base.GetEditableValuStr(valuename);
        }
    }

    public class WanquNodeInfo : IXYNode
    {
        internal int kn;
        internal int offset;

        public int KN { get { return kn; } }
        public int Offset { get { return offset; } }

        public WanquNodeInfo(int offset, int kn)
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
            float x = this.offset / WanquTestInfo.xdivf;
            return x;
        }

        public double getNodeY()
        {
            float y = this.kn / WanquTestInfo.ydivf;
            return y;
        }

        #endregion
    }
}
