using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RecordFileUtil
{
    public class ModeInjectionInfo : AbstractRecordInfo
    {
        protected List<IXYNode> nodes;
        protected List<IXYNode> specialnodes;
        protected int maxstrength;
        protected int maxoffset;
        protected int antistrength;//抗剪强度 改 贯入强度
        protected int injectionstrength;//贯入强度 改 贯入应力
        internal int xdiv = 1000;
        internal int ydiv = 1000;
        internal static float xdivf = 1000f;
        internal static float ydivf = 1000f;
        protected int yatouwidth;//压头直径

        public int MaxStrength { get { return maxstrength; } }
        public int MaxOffset { get { return maxoffset; } }
        public int AntiStrength { get { return antistrength; } }
        public int InjectionStrength { get { return injectionstrength; } }

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
            chartformat.Ymax = 8;
            chartformat.Xinterval = 2;
            chartformat.Yinterval = 1.6;
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
                //0
                year = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //1
                month = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //2
                day = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //3
                hour = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //4
                minute = (int)((bytes[idx++] << 8) | bytes[idx++]);
                thedate = String.Format("{0}年{1}月{2}日{3}时{4}分", year, month, day, hour, minute);
                //5
                height = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //6
                width = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //7
                temp = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//Convert.ToInt32((int)((bytes[idx++] << 8) | bytes[idx++]));
                //8
                loadspeed = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //9
                nodecnt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //sensor = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //A
                shiyanno1 = (int)bytes[idx++];
                shiyanno2 = (int)bytes[idx++];
                //B
                //压头直径
                yatouwidth = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //idx++;
                //idx++;
                //rb = (int)((bytes[idx++] << 8) | bytes[idx++]);
                maxstrength = (int)((bytes[idx++] << 8) | bytes[idx++]);
                maxoffset = (int)((bytes[idx++] << 8) | bytes[idx++]);
                antistrength = (int)((bytes[idx++] << 8) | bytes[idx++]);
                injectionstrength = (int)((bytes[idx++] << 8) | bytes[idx++]);

                nodes.Add(new ModeInjectionNodeInfo(0, 0));
                while (idx < (bytes.Length - 2))
                {

                    int kn = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//(int)((bytes[idx++] << 8) | bytes[idx++]);
                    int offset = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//(int)((bytes[idx++] << 8) | bytes[idx++]);
                    if (kn < 0) kn = 0;
                    if (offset < 0) offset = 0;
                    nodes.Add(new ModeInjectionNodeInfo(kn, offset));
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
                specialnodes.Add(new ModeInjectionNodeInfo(maxstrength,maxoffset));


            }
        }

        protected override void makeSendBufferInternal()
        {
            sendbuffer[2] = 6;
        }

        public override List<string> getCSVLines()
        {
            throw new NotImplementedException();
        }

        public override void LoadFromCSV(string[] strs)
        {
            this.initCharFormat();

            int idx=this.LoadHeaderFromCSV(strs, 1);

            this.LoadBodyFromCSV(strs, idx);
            

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
            dr[1] = String.Format("{0} mm/min", this.loadspeed);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "记录点数";
            dr[1] = this.nodecnt;
            dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr[0] = "试验编号";
            //dr[1] = String.Format("{0}-{1}", this.shiyanno1, this.shiyanno2);
            //dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr[0] = "最大点压力";
            dr[1] = String.Format("{0:f3}KN", this.maxstrength / 1000f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点形变";
            dr[1] = String.Format("{0:f3}mm", this.maxoffset / xdivf);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "贯入强度";// "抗剪强度";
            dr[1] = String.Format("{0:f4}MPa", this.antistrength / 1000f);// String.Format("{0:f3}MPa", _rb);//
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "贯入应力";// "贯入强度";
            dr[1] = String.Format("{0:f4}MPa", this.injectionstrength / 1000f);// String.Format("{0:f3}MPa", _rb);//
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "压头直径";
            dr[1] = String.Format("{0:f1}mm", this.yatouwidth / 10f);// String.Format("{0:f3}MPa", _rb);//
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

            //试件高度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.height = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * 10);

            //试件宽度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.width = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * 10);


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
                //xdiv = 100;
                //xdivf = 100f;
            }
            else
                this.nodecnt = Convert.ToInt32(strarr[1]);


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
            }


            //最大点压力
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxstrength = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("KN", "")) * xdiv);

            //最大点位移
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxoffset = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * ydiv);

            //抗剪强度  改  贯入强度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.antistrength = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("MPa", "")) * 1000);

            //贯入强度 改 贯入应力
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.injectionstrength = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("MPa", "")) * 1000);

            //压头直径
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            if (strarr[0].Equals("压头直径"))
            {
                this.yatouwidth = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * 10);
                idx++;
                idx++;
            }
            else
            {
                idx++;
            }

            thedate = String.Format("{0}年{1}月{2}日{3}时{4}分", year, month, day, hour, minute);
            return idx;
        }

        protected override int LoadBodyFromCSV(String[] strs, int index)
        {
            int idx = index;
            String[] strarr;
            nodes = new List<IXYNode>();
            nodes.Add(new ModeInjectionNodeInfo(0, 0));
            for (; idx < strs.Length; idx++)
            {
                strarr = strs[idx].Split(AbstractRecordInfo.csvsepchar);
                int kn = Convert.ToInt32(Convert.ToDouble(strarr[0]) * xdiv);
                int off = Convert.ToInt32(Convert.ToDouble(strarr[1]) * ydiv);
                nodes.Add(new ModeInjectionNodeInfo(kn, off));
                while (off > chartformat.Xmax * xdiv)
                {
                    chartformat.Xmax += chartformat.Xinterval;
                }
                while (kn > chartformat.Ymax * ydiv)
                {
                    chartformat.Ymax += chartformat.Yinterval;
                }
            }

            specialnodes = new List<IXYNode>();
            specialnodes.Add(new ModeInjectionNodeInfo(maxstrength, maxoffset));
            return idx;
        }

        public override int NodeCntIdx
        {
            get
            {
                return 7;
            }
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
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试验编号";
            dr[1] = String.Format("{0}-{1}", this.shiyanno1, this.shiyanno2);
            dr[3] = false;
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr[0] = "最大点压力";
            dr[1] = String.Format("{0:f3}", this.maxstrength / 1000f);
            dr[2] = "KN";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点形变";
            dr[1] = String.Format("{0:f3}", this.maxoffset / xdivf);
            dr[2] = "mm";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "贯入强度";// "抗剪强度";
            dr[1] = String.Format("{0:f4}", this.antistrength / 1000f);// String.Format("{0:f3}MPa", _rb);//
            dr[2] = "MPa";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "贯入应力";//"贯入强度";
            dr[1] = String.Format("{0:f4}", this.injectionstrength / 1000f);// String.Format("{0:f3}MPa", _rb);//
            dr[2] = "MPa";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "压头直径";
            dr[1] = String.Format("{0:f1}", this.yatouwidth / 10f);// String.Format("{0:f3}MPa", _rb);//
            dr[2] = "mm";
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
            dt.Columns[1].ColumnName = "形变(mm)";
            DataRow dr = null;

            int idx = 0;
            foreach (IXYNode node in this.nodes)
            {
                //if (node.getX() != 0 && node.getY() != 0)
                if (idx++ > 0)
                {
                    dr = dt.NewRow();
                    dr[0] = String.Format("{0:f3}", node.getNodeX());
                    dr[1] = String.Format("{0:f3}", node.getNodeY());
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
                dr[1] = String.Format("{0}{1}", dr_header[1], dr_header[2]);
                dt.Rows.Add(dr);
            }

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dt.Rows.Add(dr);

            DataTable dt_body = getBodyTable();
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


        /*
        public override void EditValue(string p, string newvalue)
        {
            if (p.Equals(KUAZHONGNAODU))
            {
                newvalue = newvalue.Replace("mm", "");
                int newmaxoff = Convert.ToInt32(Convert.ToDouble(newvalue) * xdiv);
                int oldmax = this.maxoffset;
                this.maxoffset = newmaxoff;

                double _rb;
                _rb = (double)3 * (this.sensor / 10f) * (this.maxstrength) / (this.width / 10f) / (this.height / 10f) / (this.height / 10f) / 2;
                double _eb;
                _eb = (double)6 * (this.height / 10f) * (this.maxoffset / xdivf) / (this.sensor / 10f) / (this.sensor / 10f);
                this.eb = Convert.ToInt32(_eb * 10000000);
                this.sb = Convert.ToInt32((double)_rb * 10 / _eb);


                int _offset = newmaxoff - oldmax;

                foreach (IXYNode node in nodes)
                {
                    ModeInjectionNodeInfo _node = node as ModeInjectionNodeInfo;
                    if (_node != null)
                    {
                        _node.offset += _offset;
                    }
                }
                specialnodes.Clear();
                specialnodes.Add(new ModeInjectionNodeInfo(maxoffset, maxstrength));

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
         */
    }

    public class ModeInjectionNodeInfo : IXYNode
    {
        internal int kn;
        internal int offset;

        public int KN { get { return kn; } }
        public int Offset { get { return offset; } }

        public ModeInjectionNodeInfo(int kn,int offset)
        {
            this.kn = kn;
            this.offset = offset;
        }

        #region IXYNode Members

        public int getX()
        {
            return this.kn;
        }

        public int getY()
        {
            return this.offset;
        }

        public double getNodeX()
        {
            float x = this.offset / ModeInjectionInfo.xdivf;
            return x;
        }

        public double getNodeY()
        {
            float y = this.kn / ModeInjectionInfo.ydivf;
            return y;
        }

        #endregion
    }
}
