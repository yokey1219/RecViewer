using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RecordFileUtil
{
    public class LengzhutiInfo : AbstractRecordInfo
    {
        
        protected List<IXYNode> nodes;
        protected List<IXYNode> specialnodes;
        protected int maxwendingdu;
        protected int maxliuzhi;
        internal int xdiv = 1000;//位移改3位小数 xidv=100;
        internal int ydiv = 100;
        internal static float xdivf = 1000f;//位移改3位小数 xidvf=100f;
        internal static float ydivf = 100f;
        //protected int maxoffset;
        //protected int eb;
        //protected int sb;
        protected int rt;
        protected int et;
        protected int st;

        public int MaxWendingdu { get { return maxwendingdu; } }
        public int MaxLiuzhi { get { return maxliuzhi; } }
        

    

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
                height = (int)((bytes[idx++] << 8) | bytes[idx++]);//高度在前
                width = (int)((bytes[idx++] << 8) | bytes[idx++]);//长度在后面
                temp = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//Convert.ToInt32((int)((bytes[idx++] << 8) | bytes[idx++]));
                loadspeed = (int)((bytes[idx++] << 8) | bytes[idx++]);
                nodecnt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                sensor = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //shiyanno1 = (int)bytes[idx++];
                //shiyanno2 = (int)bytes[idx++];

                rt = (int)((bytes[idx++] << 8) | bytes[idx++]);//idx++;idx++;
                maxwendingdu = (int)((bytes[idx++] << 8) | bytes[idx++]);
                maxliuzhi = (int)((bytes[idx++] << 8) | bytes[idx++]);
                et = (int)((bytes[idx++] << 8) | bytes[idx++]);//idx++;idx++;
                st = (int)((bytes[idx++] << 8) | bytes[idx++]);//idx++;idx++;
                //maxoffset = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //eb = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //sb = (int)((bytes[idx++] << 8) | bytes[idx++]);
                
                //int stime = 0;
                nodes.Add(new LengzhutiNodeInfo(0, 0));
                while (idx < (bytes.Length - 2))
                {

                    int kn = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//(int)((bytes[idx++] << 8) | bytes[idx++]);
                    int offset = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//(int)((bytes[idx++] << 8) | bytes[idx++]);
                    if (kn < 0) kn = 0;
                    if (offset < 0) offset = 0;
                    nodes.Add(new LengzhutiNodeInfo(offset, kn));
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
                specialnodes.Add(new LengzhutiNodeInfo(maxliuzhi, maxwendingdu));
                this.shuffer(specialnodes[0], nodes);

            }
        }

        protected override void makeSendBufferInternal()
        {
            sendbuffer[2] = 2;
        }

        public override List<string> getCSVLines()
        {
            throw new NotImplementedException();
        }

        public override void LoadFromCSV(string[] strs)
        {
            this.initCharFormat();

            int idx=this.LoadHeaderFromCSV(strs, 1);
   
            idx++;
            idx++;

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
            dr[0] = "试件高度";
            dr[1] = String.Format("{0:f1}mm", this.Height / 10f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试件长度";
            dr[1] = String.Format("{0:f1}mm", this.Diameter / 10f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            //dr[0] = "编号";
            //dr[1] = this.no;
            //dt.Rows.Add(dr);
            dr[0] = "试验编号";
            dr[1] = String.Format("{0}-{1}", shiyanno1, shiyanno2);
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
            dr[0] = "最大点压力";
            dr[1] = String.Format("{0:f2}KN", this.maxwendingdu / ydivf);// String.Format("{0:f3}MPa", this.rb / 1000f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点位移";
            dr[1] = String.Format("{0:f3}mm", this.maxliuzhi / xdivf);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "抗压强度";
            dr[1] = String.Format("{0:f3}Mpa", this.rt / 1000f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "压缩应变";
            dr[1] = String.Format("{0:f5}", this.et / 100000f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "劲度模量";
            dr[1] = String.Format("{0:f1}Mpa", this.st / 10f);
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

            //试件跨度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.sensor = Convert.ToInt32(strarr[1].Replace("KN",""));
            //String[] bianhaostrarr = strarr[1].Split('-');
            //shiyanno1 = Convert.ToInt32(bianhaostrarr[0]);
            //if (bianhaostrarr.Length > 1)
            //    shiyanno2 = Convert.ToInt32(bianhaostrarr[1]);
            //else
            //    shiyanno2 = 1;

            //最大点压力
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxwendingdu = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("KN", "")) * ydiv);

            //最大点位移
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxliuzhi = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * xdiv);

            //抗压强度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.rt = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("Mpa", "")) * 1000);


            //压缩应变
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.et = Convert.ToInt32(Convert.ToDouble(strarr[1]) * 100000);


            //劲度模量
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.st = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("Mpa", "")) * 10);


            thedate = String.Format("{0}年{1}月{2}日{3}时{4}分", year, month, day, hour, minute);
            this.shuffer(specialnodes[0], nodes);
            return idx;
        }

        protected override int LoadBodyFromCSV(String[] strs, int index)
        {
            int idx = index;
            String[] strarr;

            nodes = new List<IXYNode>();
            nodes.Add(new LengzhutiNodeInfo(0, 0));
            for (; idx < strs.Length; idx++)
            {
                strarr = strs[idx].Split(AbstractRecordInfo.csvsepchar);
                int kpa = Convert.ToInt32(Convert.ToDouble(strarr[0]) * ydiv);
                int off = Convert.ToInt32(Convert.ToDouble(strarr[1]) * xdiv);
                nodes.Add(new LengzhutiNodeInfo(off, kpa));
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
            specialnodes.Add(new LengzhutiNodeInfo(maxliuzhi, maxwendingdu));
            return idx;
        }


        public override int NodeCntIdx
        {
            get
            {
                return 7;
                //}
                //DataTable dt = new DataTable();
                //dt.Columns.Add();
                //dt.Columns.Add();
                //DataRow dr = dt.NewRow();
                //dr[0] = "试验日期";
                //dr[1] = String.Format("{0}-{1}-{2} {3}:{4}", this.year, this.month, this.day, this.hour, this.minute);
                //dt.Rows.Add(dr);

                //dr = dt.NewRow();
                //dr[0] = "试件高度";
                //dr[1] = String.Format("{0:f1}mm", this.Height / 10f);
                //dt.Rows.Add(dr);

                //dr = dt.NewRow();
                //dr[0] = "试件长度";
                //dr[1] = String.Format("{0:f1}mm", this.Diameter / 10f);
                //dt.Rows.Add(dr);

                //dr = dt.NewRow();
                //dr[0] = "传感器";
                //dr[1] = String.Format("{0}KN", this.sensor);
                //dt.Rows.Add(dr);
                ////dr[0] = "试验编号";
                ////dr[1] = String.Format("{0}-{1}", shiyanno1, shiyanno2);
                ////dt.Rows.Add(dr);

                //dr = dt.NewRow();
                //dr[0] = "温度";
                //dr[1] = String.Format("{0}℃", this.temp);
                //dt.Rows.Add(dr);

                //dr = dt.NewRow();
                //dr[0] = "记录点数";
                //dr[1] = this.nodecnt;
                //dt.Rows.Add(dr);


                //dr = dt.NewRow();
                //dr[0] = "最大点压力";
                //dr[1] = String.Format("{0:f2}KN", this.maxwendingdu / ydivf);// String.Format("{0:f3}MPa", this.rb / 1000f);
                //dt.Rows.Add(dr);

                //dr = dt.NewRow();
                //dr[0] = "最大点位移";
                //dr[1] = String.Format("{0:f3}mm", this.maxliuzhi / xdivf);
                //dt.Rows.Add(dr);

                //dr = dt.NewRow();
                //dr[0] = "抗压强度";
                //dr[1] = String.Format("{0:f3}Mpa", this.rt / 1000f);
                //dt.Rows.Add(dr);

                //dr = dt.NewRow();
                //dr[0] = "压缩应变";
                //dr[1] = String.Format("{0:f5}", this.et / 100000f);
                //dt.Rows.Add(dr);

                //dr = dt.NewRow();
                //dr[0] = "劲度模量";
                //dr[1] = String.Format("{0:f1}Mpa", this.st / 10f);
                //dt.Rows.Add(dr);

                //displaymaxidx = dt.Rows.Count - 1;
                //return dt;
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
            dr[0] = "试件长度";
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


            dr = dt.NewRow();
            dr[0] = "最大点压力";
            dr[1] = String.Format("{0:f2}", this.maxwendingdu / ydivf);// String.Format("{0:f3}MPa", this.rb / 1000f);
            dr[2] = "KN";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大点位移";
            dr[1] = String.Format("{0:f3}", this.maxliuzhi / xdivf);
            dr[2] = "mm";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "抗压强度";
            dr[1] = String.Format("{0:f3}", this.rt / 1000f);
            dr[2] = "Mpa";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "压缩应变";
            dr[1] = String.Format("{0:f5}", this.et / 100000f);
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "劲度模量";
            dr[1] = String.Format("{0:f1}", this.st / 10f);
            dr[2] = "Mpa";
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
                    dr[0] = String.Format("{0:f2}", node.getNodeY());
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

        

        /*public override List<EditableItem> GetEditableList()
        {
            List<EditableItem> list = new List<EditableItem>();
            list.Add(new EditableItem("挠度修正", KUAZHONGNAODU));
            return list;
        }*/

       /* public override string GetEditableValuStr(string valuename)
        {
            if (valuename.Equals(KUAZHONGNAODU))
            {
                return String.Format("{0:f2}mm", this.maxoffset / 100f);
            }
            else
                return base.GetEditableValuStr(valuename);
        }*/
    }

    public class LengzhutiNodeInfo : IXYNode
    {
        internal int kn;
        internal int offset;

        public int KN { get { return kn; } }
        public int Offset { get { return offset; } }

        public LengzhutiNodeInfo(int offset, int kn)
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
            float x = this.offset / LengzhutiInfo.xdivf;
            return x;
        }

        public double getNodeY()
        {
            float y = this.kn / LengzhutiInfo.ydivf;
            return y;
        }

        #endregion
    }
}
