using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace RecordFileUtil
{
    public class LIQIHanliangInfo:AbstractRecordInfo
    {
        
        protected List<IXYNode> nodes;
        protected List<IXYNode> specialnodes;
        
        protected int shijianzhiliang;
        protected int wendu;
        protected int youshibibuchang;
        protected int hanyoubibuchang;
        protected int youshibi;
        protected int hanyoubi;
        protected int shiyantime;
        protected int endwedu;
        protected int endzhiliang;   

        public override List<IXYNode> getXYNodes()
        {
            return nodes;
        }

        public override void initCharFormat()
        {
            chartformat = new ChartFormat();
            chartformat.Xname = "分";
            chartformat.Yname = "%";
            chartformat.Xmin = 0;
            chartformat.Ymin = 0;
            chartformat.Xmax = 100;
            chartformat.Ymax = 10;
            chartformat.Xinterval = 20;
            chartformat.Yinterval = 2;
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
                if (bytes.Length > (length + 6))
                {
                    buffer = new byte[length + 6];
                    Array.Copy(bytes, 0, buffer, 0, buffer.Length);
                    bytes = buffer;
                }
                year = (int)((bytes[idx++] << 8) | bytes[idx++]);
                month = (int)((bytes[idx++] << 8) | bytes[idx++]);
                day = (int)((bytes[idx++] << 8) | bytes[idx++]);
                hour = (int)((bytes[idx++] << 8) | bytes[idx++]);
                minute = (int)((bytes[idx++] << 8) | bytes[idx++]);
                second = (int)((bytes[idx++] << 8) | bytes[idx++]);
                thedate = String.Format("{0}年{1}月{2}日{3}时{4}分{5}秒", year, month, day, hour, minute,second);
                nodecnt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                wendu = (int)((bytes[idx++] << 8) | bytes[idx++]);
                //temp = Convert.ToInt32(Convert.ToInt16(String.Format("{0:X2}{1:X2}", bytes[idx++], bytes[idx++]), 16));//Convert.ToInt32((int)((bytes[idx++] << 8) | bytes[idx++]));
                shijianzhiliang = (int)((bytes[idx++] << 8) | bytes[idx++]);
                youshibibuchang = (int)((bytes[idx++] << 8) | bytes[idx++]);
                hanyoubibuchang = (int)((bytes[idx++] << 8) | bytes[idx++]);
                youshibi = (int)((bytes[idx++] << 8) | bytes[idx++]);
                hanyoubi = (int)((bytes[idx++] << 8) | bytes[idx++]);
                shiyantime = (int)((bytes[idx++] << 8) | bytes[idx++]);
                endwedu = (int)((bytes[idx++] << 8) | bytes[idx++]);
                endzhiliang = (int)((bytes[idx++] << 8) | bytes[idx++]);
                
                //int stime = 0;
                nodes.Add(new LIQIHanNodeInfo(0, 0));
                int x = 0;
                while (idx < (bytes.Length - 2))
                {

                    int _percnt = (int)((bytes[idx++] << 8) | bytes[idx++]);
                    if (_percnt == 0) break;
                    //int offset = (int)((bytes[idx++] << 8) | bytes[idx++]);
                    x += 15;
                    nodes.Add(new LIQIHanNodeInfo(x,_percnt));
                    while (x > chartformat.Xmax*60)
                    {
                        chartformat.Xmax += chartformat.Xinterval;
                    }
                    while (_percnt > chartformat.Ymax*1000)
                    {
                        chartformat.Ymax += chartformat.Yinterval;
                    }
                }
            }
        }

        protected override void makeSendBufferInternal()
        {
            sendbuffer[2] = 0;
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
            this.second = dt.Second;
            //编号
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.no = Convert.ToInt32(strarr[1]);
            this.MakeSendBuffer();
            this.SetReadNo(no);
            //试验温度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.wendu = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("℃", "")));
            //试件质量
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.shijianzhiliang = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("g", "")) * 10);

            //油石比补偿系数
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.youshibibuchang = Convert.ToInt32(Convert.ToDouble(strarr[1])*1000);

            //含油比补偿系数
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.hanyoubibuchang = Convert.ToInt32(Convert.ToDouble(strarr[1])*1000);


            //记录点数
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.nodecnt = Convert.ToInt32(strarr[1]);

            //油石比
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.youshibi = Convert.ToInt32(Convert.ToDouble(strarr[1])*1000);
            
            //含油比
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.hanyoubi = Convert.ToInt32(Convert.ToDouble(strarr[1]) * 1000);

            //试验用时
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.shiyantime = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("分", "")));

            //试验结束时的温度
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.endwedu = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("℃", "")) * 10);

            //试验结束时的质量
            strarr = strs[idx++].Split(AbstractRecordInfo.csvsepchar);
            this.endzhiliang = Convert.ToInt32(Convert.ToDouble(strarr[1].Replace("g", "")) *10);

            
            idx++;
            idx++;
            nodes = new List<IXYNode>();
            nodes.Add(new LIQIHanNodeInfo(0, 0));
            int x = 0;
            for (; idx < strs.Length; idx++)
            {
                strarr = strs[idx].Split(AbstractRecordInfo.csvsepchar);
                int y = Convert.ToInt32(Convert.ToDouble(strarr[0]) * 1000);
                x += 15;
                nodes.Add(new LIQIHanNodeInfo(x, y));
                while(y>chartformat.Xmax*60)
                {
                    chartformat.Xmax += chartformat.Xinterval;
                }
                while (y > chartformat.Ymax * 1000)
                {
                    chartformat.Ymax += chartformat.Yinterval;
                }
            }

            //specialnodes = new List<IXYNode>();
            //specialnodes.Add(new LIQIHanNodeInfo(wendu, shijianzhiliang));

            thedate = String.Format("{0}年{1}月{2}日{3}时{4}分{5}秒", year, month, day, hour, minute,second);

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
            dr[1] = String.Format("{0}-{1}-{2} {3}:{4}:{5}", this.year, this.month, this.day, this.hour, this.minute,this.second);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "编号";
            dr[1] = this.no;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试验温度";
            dr[1] = String.Format("{0}℃", this.wendu);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试件质量";
            dr[1] = String.Format("{0:f1}g", this.shijianzhiliang / 10f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "油石比补偿系数";
            dr[1] = String.Format("{0:f3}", this.youshibibuchang/1000f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "含油比补偿系数";
            dr[1] = String.Format("{0:f3} ", this.hanyoubibuchang/1000f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "记录点数";
            dr[1] = this.nodecnt;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "油石比";
            dr[1] = String.Format("{0:f3}", this.youshibi/1000f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "含油比";
            dr[1] = String.Format("{0:f3}", this.hanyoubi / 1000f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "试验用时";
            dr[1] = String.Format("{0}分", this.shiyantime);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "结束温度";
            dr[1] = String.Format("{0:f1}℃", this.endwedu / 10f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "结束质量";
            dr[1] = String.Format("{0:f1}g", this.endzhiliang / 10f);
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "";
            dr[1] = "";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr[0] = "质量损失率";
            dr[1] = "";
            dt.Rows.Add(dr);


            foreach (IXYNode node in this.nodes)
            {
                if (node.getX() != 0 && node.getY() != 0)
                {
                    dr = dt.NewRow();
                    dr[0] = String.Format("{0:f3}", node.getNodeY());
                    dr[1] = "";//String.Format("{0:f2}", node.getNodeX());
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }



        public override List<IXYNode> getSpecialNodes()
        {
            return null;
        }
    }

    public class LIQIHanNodeInfo:IXYNode
    {
        internal int baifenbi;
        internal int time;

        public int KN { get { return baifenbi; } }
        public int Offset { get { return time; } }

        public LIQIHanNodeInfo(int x, int y)
        {
            this.baifenbi = y;
            this.time = x;
        }

        #region IXYNode Members

        public int getX()
        {
            return this.time;
        }

        public int getY()
        {
            return this.baifenbi;
        }

        public double getNodeX()
        {
            float x = this.time / 60f;
            return x;
        }

        public double getNodeY()
        {
            float y = this.baifenbi / 1000f;
            return y;
        }

        #endregion
    }
}
