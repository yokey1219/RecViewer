using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;

namespace RecordFileUtil
{
    public class CBRRecordFileInfo:AbstractRecordInfo
    {
        protected int cbr25;
        protected int cbr50;
        protected int kpa25;
        protected int kpa50;
        protected int nouse2;
        protected int nouse3;
        protected List<IXYNode> nodes;
        protected List<IXYNode> specialnodes;

        
        public int Cbr25 { get { return cbr25; } }
        public int Cbr50 { get { return cbr50; } }
        public int Kpa25 { get { return kpa25; } }
        public int Kpa50 { get { return kpa50; } }

        public List<IXYNode> Nodes { get { return nodes; } }

        public override void initCharFormat()
        {
            chartformat = new ChartFormat();
            chartformat.Xname = "KPa";
            chartformat.Yname = "mm";
            chartformat.Xmin = 0;
            chartformat.Ymin = 0;
            chartformat.Xmax = 20000;
            chartformat.Ymax = 10;
            chartformat.Xinterval = 4000;
            chartformat.Yinterval = 2;
            chartformat.Xtype = 0;
            chartformat.Ytype = 1;
        }

        protected  override  void LoadInternalData(byte[] bytes)
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
                //sensor = (int)((bytes[idx++] << 8) | bytes[idx++]);
                shiyanno1 = (int)bytes[idx++];
                shiyanno2 = (int)bytes[idx++];
                
                idx++;
                idx++;
                nodecnt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                cbr25 = (int)((bytes[idx++] << 8) | bytes[idx++]);
                cbr50 = (int)((bytes[idx++] << 8) | bytes[idx++]);
                kpa25 = (int)((bytes[idx++] << 8) | bytes[idx++]);
                kpa50 = (int)((bytes[idx++] << 8) | bytes[idx++]);
                idx++;
                idx++;
                idx++;
                idx++;
                while (idx < (bytes.Length - 2))
                {
                    int kpa = (int)((bytes[idx++] << 8) | bytes[idx++]);
                    int off = (int)((bytes[idx++] << 8) | bytes[idx++]);
                    nodes.Add(new CBRRecordNodeInfo(kpa, off));
                    while (kpa > chartformat.Xmax)
                    {
                        chartformat.Xmax += chartformat.Xinterval;
                    }
                    while (off > chartformat.Ymax * 1000)
                    {
                        chartformat.Ymax += chartformat.Yinterval;
                    }
                }

                specialnodes = new List<IXYNode>();
                specialnodes.Add(new CBRRecordNodeInfo(kpa25, 2500));
                specialnodes.Add(new CBRRecordNodeInfo(kpa50, 5000));
            }
        }

        public override List<IXYNode> getXYNodes()
        {
            return nodes;
        }

        public override List<IXYNode> getSpecialNodes()
        {
            return specialnodes;
        }

        protected override void makeSendBufferInternal()
        {
            sendbuffer[2] = 0;
        }

        public override List<string> getCSVLines()
        {
            List<String> strs = new List<string>();
            strs.Add(String.Format(AbstractRecordInfo.csvfmt, "试验模式", this.recordname));
            strs.Add(String.Format(AbstractRecordInfo.csvfmt, "试验日期", String.Format("{0}-{1}-{2} {3}:{4}",this.year,this.month,this.day,this.hour,this.minute)));
            strs.Add(String.Format(AbstractRecordInfo.csvfmt, "编号", this.no));
            strs.Add(String.Format(AbstractRecordInfo.csvfmt, "试件直径", String.Format("{0:f1}mm", this.Diameter / 10f)));
            strs.Add(String.Format(AbstractRecordInfo.csvfmt, "试件高度", String.Format("{0:f1}mm", this.Height / 10f)));
            strs.Add(String.Format(AbstractRecordInfo.csvfmt, "传感器", String.Format("{0}KN",this.sensor)));
            strs.Add("\t");
            strs.Add(String.Format(AbstractRecordInfo.csvfmt, "记录点数", this.nodecnt));
            strs.Add(String.Format(AbstractRecordInfo.csvfmt, "2.5mmCBR", String.Format("{0:f2}%", this.Cbr25 / 100f)));
            strs.Add(String.Format(AbstractRecordInfo.csvfmt, "5.0mmCBR", String.Format("{0:f2}%", this.Cbr50 / 100f)));
            strs.Add(String.Format(AbstractRecordInfo.csvfmt, "2.5mm压强", String.Format("{0}KPa", this.Kpa25)));
            strs.Add(String.Format(AbstractRecordInfo.csvfmt, "5.0mm压强", String.Format("{0}KPa", this.Kpa50)));
            strs.Add("\t");
            strs.Add("压强(Kpa),形变(mm)");
            foreach(IXYNode node in this.nodes)
            {
                strs.Add(String.Format(AbstractRecordInfo.csvfmt,node.getNodeX(),node.getNodeY()));
            }

            return strs;
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
            this.width=Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", ""))*10);
            //试件高度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.height = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("mm", "")) * 10);
            //传感器
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            //this.sensor = Convert.ToInt32(strarr[1].Replace("KN", ""));
            String[] bianhaostrarr = strarr[1].Split('-');
            shiyanno1 = Convert.ToInt32(bianhaostrarr[0]);
            if (bianhaostrarr.Length > 1)
                shiyanno2 = Convert.ToInt32(bianhaostrarr[1]);
            else
                shiyanno2 = 1;
            idx++;
            //记录点数
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.nodecnt = Convert.ToInt32(strarr[1]);
            //2.5CBR
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.cbr25 = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("%", "")) * 100);
            //5.0CBR
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.cbr50 = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("%", "")) * 100);
            //2.5压强
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.kpa25 = Convert.ToInt32(strarr[1].Replace("KPa", ""));
            //5.0压强
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.kpa50 = Convert.ToInt32(strarr[1].Replace("KPa", ""));
            idx++;
            idx++;
            nodes = new List<IXYNode>();
            for (; idx < strs.Length; idx++)
            {
                strarr = strs[idx].Split(AbstractRecordInfo.csvsepchar);
                int kpa = Convert.ToInt32(strarr[0]);
                int off = Convert.ToInt32(Convert.ToDouble(strarr[1])* 1000);
                nodes.Add(new CBRRecordNodeInfo(kpa, off));
                while (kpa > chartformat.Xmax)
                {
                    chartformat.Xmax += chartformat.Xinterval;
                }
                while (off > chartformat.Ymax * 1000)
                {
                    chartformat.Ymax += chartformat.Yinterval;
                }
            }
            specialnodes = new List<IXYNode>();
            specialnodes.Add(new CBRRecordNodeInfo(kpa25, 2500));
            specialnodes.Add(new CBRRecordNodeInfo(kpa50, 5000));
            thedate = String.Format("{0}年{1}月{2}日{3}时{4}分", year, month, day, hour, minute);

        }

        public override DataTable getDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add();
            dt.Columns.Add();
            DataRow dr=dt.NewRow();
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
            //dr[0] = "传感器大小";
            //dr[1] = String.Format("{0}KN", this.sensor);
            //dt.Rows.Add(dr);
            dr[0] = "试验编号";
            dr[1] = String.Format("{0}-{1}", this.shiyanno1, this.shiyanno2);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr[0] = "记录点数";
            dr[1] = this.nodecnt;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "2.5mmCBR";
            dr[1] = String.Format("{0:f2}%", this.Cbr25 / 100f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "5.0mmCBR";
            dr[1] = String.Format("{0:f2}%", this.Cbr50 / 100f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "2.5mm压强";
            dr[1] = String.Format("{0}KPa", this.Kpa25);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "5.0mm压强";
            dr[1] = String.Format("{0}KPa", this.Kpa50);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "压强(Kpa)";
            dr[1] = "形变(mm)";
            dt.Rows.Add(dr);
            

            foreach (IXYNode node in this.nodes)
            {
                dr = dt.NewRow();
                dr[0] = String.Format("{0}",node.getNodeX());
                dr[1] = String.Format("{0:f2}",node.getNodeY());
                dt.Rows.Add(dr);
                
            }

            return dt;
        }
    }

    public class CBRRecordNodeInfo:IXYNode
    {
        protected int kpa;
        protected int offset;

        public int KPa { get { return kpa; } }
        public int Offset { get { return offset; } }

        public CBRRecordNodeInfo(int kpa, int off)
        {
            this.kpa = kpa;
            this.offset = off;
        }

        #region IXYNode Members

        public int getX()
        {
            return kpa;
        }

        public int getY()
        {
            return offset;
        }

        #endregion

        #region IXYNode Members


        public double getNodeX()
        {
            return this.kpa;
        }

        public double getNodeY()
        {
            float y = this.Offset / 1000f;
            return y;
        }

        #endregion
    }
}
