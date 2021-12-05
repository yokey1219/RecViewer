using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RecordFileUtil
{
    public class ModulusTopRecordInfo:AbstractRecordInfo
    {
        protected List<IXYNode> nodes;

        public override List<IXYNode> getXYNodes()
        {
            return nodes;
        }

        public override List<IXYNode> getSpecialNodes()
        {
            return null;
        }

        public override void initCharFormat()
        {
            chartformat = new ChartFormat();
            chartformat.Xname = "MPa";
            chartformat.Yname = "mm";
            chartformat.Xmin = 0;
            chartformat.Ymin = 0;
            chartformat.Xmax = 1.2;
            chartformat.Ymax = 0.5;
            chartformat.Xinterval = 0.3;
            chartformat.Yinterval = 0.1;
            chartformat.Xtype = 1;
            chartformat.Ytype = 1;
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
                sensor = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //shiyanno1 = (int)bytes[idx++];
                //shiyanno2 = (int)bytes[idx++];
                
                idx++;
                idx++;
                nodecnt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                idx++;
                idx++;
                idx++;
                idx++;
                idx++;
                idx++;// maxstrength = (int)((bytes[idx++] << 8) | bytes[idx++]);
                idx++;
                idx++;// maxtime = (int)((bytes[idx++] << 8) | bytes[idx++]);
                idx++;
                idx++;
                idx++;
                idx++;
                //int stime = 0;
                nodes.Add(new ModulusTopNodeInfo(0, 0));
                while (idx < (bytes.Length - 2))
                {

                    int kpa = (int)((bytes[idx++] << 8) | bytes[idx++]);
                    int offset = (int)((bytes[idx++] << 8) | bytes[idx++]);
                    nodes.Add(new ModulusTopNodeInfo(kpa, offset));
                    while (kpa > chartformat.Xmax * 1000)
                    {
                        chartformat.Xmax += chartformat.Xinterval;
                    }
                    while (offset > chartformat.Ymax * 1000)
                    {
                        chartformat.Ymax += chartformat.Yinterval;
                    }
                }
            }
            base.LoadInternalData(bytes);
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
            //试件直径
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.width = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * 10);
            //试件高度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.height = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * 10);
            //传感器
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.sensor = Convert.ToInt32(strarr[1].Replace("KN", ""));
            //String[] bianhaostrarr = strarr[1].Split('-');
            //shiyanno1 = Convert.ToInt32(bianhaostrarr[0]);
            //if (bianhaostrarr.Length > 1)
            //    shiyanno2 = Convert.ToInt32(bianhaostrarr[1]);
            //else
            //    shiyanno2 = 1;
            idx++;
            //记录点数
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.nodecnt = Convert.ToInt32(strarr[1]);
           
            idx++;
            idx++;
            nodes = new List<IXYNode>();
            nodes.Add(new ModulusTopNodeInfo(0, 0));
            for (; idx < strs.Length; idx++)
            {
                strarr = strs[idx].Split(AbstractRecordInfo.csvsepchar);
                int kpa = Convert.ToInt32(Convert.ToDouble(strarr[0]) * 1000);
                int off = Convert.ToInt32(Convert.ToDouble(strarr[1]) * 1000);
                nodes.Add(new ModulusTopNodeInfo(kpa, off));
                while (kpa > chartformat.Xmax * 1000)
                {
                    chartformat.Xmax += chartformat.Xinterval;
                }
                while (off > chartformat.Ymax * 1000)
                {
                    chartformat.Ymax += chartformat.Yinterval;
                }
            }
          
            thedate = String.Format("{0}年{1}月{2}日{3}时{4}分", year, month, day, hour, minute);

            base.LoadFromCSV(strs);
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
            dr[0] = "传感器大小";
            dr[1] = String.Format("{0}KN", this.sensor);
            dt.Rows.Add(dr);
            //dr[0] = "试验编号";
            //dr[1] = String.Format("{0}-{1}", this.shiyanno1, this.shiyanno2);
            //dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr[0] = "记录点数";
            dr[1] = this.nodecnt;
            dt.Rows.Add(dr);

           

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "压强(Mpa)";
            dr[1] = "形变(mm)";
            dt.Rows.Add(dr);


            foreach (IXYNode node in this.nodes)
            {
                if (node.getX() != 0 && node.getY() != 0)
                {
                    dr = dt.NewRow();
                    dr[0] = String.Format("{0:f3}", node.getNodeX());
                    dr[1] = String.Format("{0:f3}", node.getNodeY());
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }
    }

    public class ModulusTopNodeInfo:IXYNode
    {
        protected int mpa;
        protected int offset;

        public int Mpa { get { return mpa; } }
        public int Offset { get { return offset; } }

        public ModulusTopNodeInfo(int mpa, int offset)
        {
            this.mpa = mpa;
            this.offset = offset;
        }

        #region IXYNode Members

        public int getX()
        {
            return mpa;
        }

        public int getY()
        {
            return offset;
        }

        public double getNodeX()
        {
            float x = mpa / 1000f;
            return x;
        }

        public double getNodeY()
        {
            float y = offset / 1000f;
            return y;
        }

        #endregion
    }
}
