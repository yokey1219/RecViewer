using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RecordFileUtil
{
    public class ModulusYuanInfo : AbstractRecordInfo
    {
        
        protected List<IXYNode> nodes;
        protected List<IXYNode> specialnodes;
        protected int maxstrength;
        protected int maxoffset;
        protected int modulus;
        internal int xdiv = 1000;
        internal int ydiv = 1000;//位移改3位小数 yidv=100;
        internal static float xdivf = 1000f;
        internal static float ydivf = 1000f;//位移改3位小数 yidvf=100f;
        //protected int maxoffset;
        //protected int eb;
        //protected int sb;

        public int MaxStrength { get { return maxstrength; } }
        public int MaxOffset { get { return maxoffset; } }
        public int Modulus { get { return modulus; } }
        

    

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
            chartformat.Xname = "Mpa";
            chartformat.Yname = "mm";
            chartformat.Xmin = 0;
            chartformat.Ymin = 0;
            chartformat.Xmax = 6;
            chartformat.Ymax = 2;
            chartformat.Xinterval = 1;
            chartformat.Yinterval = 0.4;
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
                width = (int)((bytes[idx++] << 8) | bytes[idx++]);//直径在后
                temp = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//Convert.ToInt32((int)((bytes[idx++] << 8) | bytes[idx++]));
                loadspeed = (int)((bytes[idx++] << 8) | bytes[idx++]);
                nodecnt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                sensor = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //shiyanno1 = (int)bytes[idx++];
                //shiyanno2 = (int)bytes[idx++];
                
                maxstrength = (int)((bytes[idx++] << 8) | bytes[idx++]);
                modulus = (int)((bytes[idx++] << 8) | bytes[idx++]);
                maxoffset = (int)((bytes[idx++] << 8) | bytes[idx++]);
                idx++;
                idx++;
                idx++;
                idx++;
                //maxoffset = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //eb = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //sb = (int)((bytes[idx++] << 8) | bytes[idx++]);
                
                //int stime = 0;
                //nodes.Add(new WendingduNodeInfo(0, 0));
                int lastx = 0;
                int _nodecnt = nodecnt;
                while (_nodecnt>0&&idx < (bytes.Length - 2))
                {

                    int mpa = (int)((bytes[idx++] << 8) | bytes[idx++]);
                    int offset = (int)((bytes[idx++] << 8) | bytes[idx++]);
                    nodes.Add(new ModulusYuanNodeInfo(lastx,offset));
                    nodes.Add(new ModulusYuanNodeInfo(mpa , offset));
                    lastx = mpa;
                    while (mpa > chartformat.Xmax*xdiv)
                    {
                        chartformat.Xmax += chartformat.Xinterval;
                    }
                    while (offset > chartformat.Ymax*ydiv)
                    {
                        chartformat.Ymax += chartformat.Yinterval;
                    }
                    _nodecnt--;
                }

                //specialnodes = new List<IXYNode>();
                //specialnodes.Add(new WendingduNodeInfo(maxoffset, maxstrength));
                

            }
        }

        protected override void makeSendBufferInternal()
        {
            sendbuffer[2] = 1;
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
            dr[0] = "试验日期";
            dr[1] = String.Format("{0}-{1}-{2} {3}:{4}", this.year, this.month, this.day, this.hour, this.minute);
            dt.Rows.Add(dr);

            /*dr = dt.NewRow();
            dr[0] = "试件高度";
            dr[1] = String.Format("{0:f1}mm", this.Height / 10f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试件直径";
            dr[1] = String.Format("{0:f1}mm", this.Diameter / 10f);
            dt.Rows.Add(dr);
            */
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

            //dr = dt.NewRow();
            //dr[0] = "记录点数";
            //dr[1] = this.nodecnt;
            //dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr[0] = "回弹模量";
            dr[1] = String.Format("{0:f1}Mpa", this.modulus / 10f);// String.Format("{0:f3}MPa", this.rb / 1000f);
            dt.Rows.Add(dr);

            /*dr = dt.NewRow();
            dr[0] = "最大变形";
            dr[1] = String.Format("{0:f2}mm", this.maxoffset / 100f);
            dt.Rows.Add(dr);
            */
            int cnt = 0;
            foreach (IXYNode node in this.nodes)
            {
                cnt++;
                //if (node.getX() != 0 && node.getY() != 0)
                if (cnt % 2 == 0)
                {
                    dr = dt.NewRow();
                    dr[0] = String.Format("{0:f3}Mpa", node.getNodeX());
                    dr[1] = String.Format("{0:f3}mm", node.getNodeY());
                    dt.Rows.Add(dr);
                }
            }

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
            this.sensor = Convert.ToInt32(Convert.ToInt32(strarr[1].Replace("KN", "")));
            //String[] bianhaostrarr = strarr[1].Split('-');
            //shiyanno1 = Convert.ToInt32(bianhaostrarr[0]);
            //if (bianhaostrarr.Length > 1)
            //    shiyanno2 = Convert.ToInt32(bianhaostrarr[1]);
            //else
            //    shiyanno2 = 1;

            //最大点压力
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxstrength = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("KN", "")) * 100);

            //回弹模量
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.modulus = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("Mpa", "")) * 10);

            //最大点位移
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.maxoffset = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * ydiv);


            thedate = String.Format("{0}年{1}月{2}日{3}时{4}分", year, month, day, hour, minute);
            return idx;
        }

        protected override int LoadBodyFromCSV(String[] strs, int index)
        {
            int idx = index;
            String[] strarr;

            nodes = new List<IXYNode>();
            //nodes.Add(new ModulusYuanNodeInfo(0, 0));
            int lastx = 0;
            for (; idx < strs.Length; idx++)
            {
                strarr = strs[idx].Split(AbstractRecordInfo.csvsepchar);
                int kpa = Convert.ToInt32(Convert.ToDouble(strarr[0]) * xdiv);
                int off = Convert.ToInt32(Convert.ToDouble(strarr[1]) * ydiv);
                nodes.Add(new ModulusYuanNodeInfo(lastx, off));
                nodes.Add(new ModulusYuanNodeInfo(kpa, off));
                lastx = kpa;
                while (kpa > chartformat.Xmax * xdiv)
                {
                    chartformat.Xmax += chartformat.Xinterval;
                }
                while (off > chartformat.Ymax * ydiv)
                {
                    chartformat.Ymax += chartformat.Yinterval;
                }
            }
            return idx;
        }


        public override int NodeCntIdx
        {
            get
            //DataTable dt = new DataTable();
            //dt.Columns.Add();
            //dt.Columns.Add();
            //DataRow dr = dt.NewRow();
            //dr[0] = "试验日期";
            //dr[1] = String.Format("{0}-{1}-{2} {3}:{4}", this.year, this.month, this.day, this.hour, this.minute);
            //dt.Rows.Add(dr);

            ///*dr = dt.NewRow();
            //dr[0] = "试件高度";
            //dr[1] = String.Format("{0:f1}mm", this.Height / 10f);
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr[0] = "试件直径";
            //dr[1] = String.Format("{0:f1}mm", this.Diameter / 10f);
            //dt.Rows.Add(dr);
            //*/
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

            ////dr = dt.NewRow();
            ////dr[0] = "记录点数";
            ////dr[1] = this.nodecnt;
            ////dt.Rows.Add(dr);


            //dr = dt.NewRow();
            //dr[0] = "回弹模量";
            //dr[1] = String.Format("{0:f1}Mpa", this.modulus / 10f);// String.Format("{0:f3}MPa", this.rb / 1000f);
            //dt.Rows.Add(dr);

            ///*dr = dt.NewRow();
            //dr[0] = "最大变形";
            //dr[1] = String.Format("{0:f2}mm", this.maxoffset / 100f);
            //dt.Rows.Add(dr);
            //*/
            //int cnt = 0;
            //foreach (IXYNode node in this.nodes)
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
            dr[0] = "最大压力";
            dr[1] = String.Format("{0:f2}", this.maxstrength / 100);// String.Format("{0:f3}MPa", this.rb / 1000f);
            dr[2] = "KN";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "回弹模量";
            dr[1] = String.Format("{0:f1}", this.modulus / 10f);// String.Format("{0:f3}MPa", this.rb / 1000f);
            dr[2] = "Mpa";
            dr[3] = false;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "最大变形";
            dr[1] = String.Format("{0:f3}", this.maxoffset / ydivf);
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
            dt.Columns[0].ColumnName = "压强(Mpa)";
            dt.Columns[1].ColumnName = "回弹(mm)";
            DataRow dr = null;

            int cnt = 0;
            foreach (IXYNode node in this.nodes)
            {
                //if (node.getX() != 0 && node.getY() != 0)
                    cnt++;
                    //if (node.getX() != 0 && node.getY() != 0)
                    if (cnt % 2 == 0)
                    {
                        dr = dt.NewRow();
                        dr[0] = String.Format("{0:f3}", node.getNodeX());
                        dr[1] = String.Format("{0:f3}", node.getNodeY());
                        dt.Rows.Add(dr);
                    }
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

    public class ModulusYuanNodeInfo : IXYNode
    {
        internal int offset;
        internal int mpa;

        public int KN { get { return offset; } }
        public int Offset { get { return mpa; } }

        public ModulusYuanNodeInfo(int mpa, int offset)
        {
            this.offset = offset;
            this.mpa = mpa;
        }

        #region IXYNode Members

        public int getX()
        {
            return this.mpa;
        }

        public int getY()
        {
            return this.offset;
        }

        public double getNodeX()
        {
            float x = this.mpa / ModulusYuanInfo.xdivf;
            return x;
        }

        public double getNodeY()
        {
            float y = this.offset / ModulusYuanInfo.ydivf;
            return y;
        }

        #endregion
    }
}
