using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RecordFileUtil;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace RecViewer
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void tsm_openfile_Click(object sender, EventArgs e)
        {
            this.testload();
        }

        

        private void testload()
        {
            RecordOpenDialog dialog = new RecordOpenDialog();
            String filename=dialog.SelectFile();
            if (filename != null)
            {
                //CBRRecordFileInfo info = new CBRRecordFileInfo();
                // info.LoadData(filename);
                if (filename.EndsWith(".rec.bin"))
                {
                    AbstractRecordInfo info = LoadDataFromFile(filename);
                    RenderChart(info);
                    //MessageBox.Show(info.CheckCRC().ToString());
                    
                }
                else if (filename.EndsWith(".csv"))
                {
                    try
                    {
                        String[] strs = File.ReadAllLines(filename, Encoding.UTF8);
                        String str = strs[0];
                        String[] strarr = str.Split(AbstractRecordInfo.csvsepchar);
                        if (strarr.Length == 2)
                        {
                            if (strarr[0] == "试验模式")
                            {
                                AbstractRecordInfo info = RecordInfoFactory.CreateInfo(strarr[1]);
                                if (info != null)
                                {
                                    info.LoadFromCSV(strs);
                                    RenderChart(info);
                                    FillData(info);
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageForm.Show("文件格式错误！", ex);// MessageBox.Show("文件格式错误！");
                    }
                }
                else if (filename.EndsWith(".xml"))
                {
                    try
                    {
                        DataTable dt = ExcelHelper.LoadXMLFile(filename);
                        if (dt != null)
                        {
                            if (dt.Columns.Count == 2)
                            {
                                List<String> slist = new List<string>();
                                foreach (DataRow dr in dt.Rows)
                                {
                                    slist.Add(String.Format("{0},{1}", dr[0], dr[1]));
                                }

                                String[] strs = slist.ToArray();
                                String str = strs[0];
                                String[] strarr = str.Split(AbstractRecordInfo.csvsepchar);
                                if (strarr.Length == 2)
                                {
                                    if (strarr[0] == "试验模式")
                                    {
                                        AbstractRecordInfo info = RecordInfoFactory.CreateInfo(strarr[1]);
                                        if (info != null)
                                        {
                                            info.LoadFromCSV(strs);
                                            RenderChart(info);
                                            FillData(info);
                                        }
                                    }
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageForm.Show("文件格式错误！",ex);
                    }

                }
            }
        }


        AbstractRecordInfo LoadDataFromFile(String file)
        {
            AbstractRecordInfo info = null;
            byte[] buffer=File.ReadAllBytes(file);
            switch (buffer[2])
            {
                //case 0:
                //    info = new CBRRecordFileInfo();
                //    info.LoadData(buffer);
                    //FillData(info as CBRRecordFileInfo);
                //    break;
                case 3:
                    //this.recordname = "回弹模量-强度仪法";
                    info = new ModulusStrengthInfo();
                    info.LoadData(buffer);
                    //FillData(info as ModulusStrengthInfo);
                    break;
                //case 2:
                //    info = new StrengthRecordInfo();
                //    info.LoadData(buffer);
                //    //FillData(info as StrengthRecordInfo);
                //    break;
                //case 3:
                //    //this.recordname = "回弹模量-顶面法";
                //    info = new ModulusTopRecordInfo();
                //    info.LoadData(buffer);
                //    //FillData(info as ModulusTopRecordInfo);
                //    break;
                default:
                    //this.recordname = "unkown";
                    break;
            }
            if (info != null)
            {
                FillData(info);
            }
            return info;
        }

        private AbstractRecordInfo currentInfo = null;


        private void FillData(AbstractRecordInfo info)
        {
            this.Text = info.RecordName;
            this.lblrecordname.Text = info.RecordName;
            currentInfo = info;
            List<EditableItem> _list=info.GetEditableList();
            if (_list.Count == 0)
                tbnaodu.Visible = false;
            else
            {
                tbnaodu.Visible = true;
                tbnaodu.Text = _list[0].ActionName;
                tbnaodu.Tag = _list[0].ValueName;
            }
            if (info is CBRRecordFileInfo)
            {
                FillData(info as CBRRecordFileInfo);
            }
            else if (info is StrengthRecordInfo)
            {
                FillData(info as StrengthRecordInfo);
            }
            else if (info is ModulusTopRecordInfo)
            {
                FillData(info as ModulusTopRecordInfo);
            }
            else if (info is ModulusStrengthInfo)
            {
                FillData(info as ModulusStrengthInfo);
            }
            else if (info is LIQIHanliangInfo)
            {
                FillData(info as LIQIHanliangInfo);
            }
            else
            {
                currentInfo = null;
            }
        }


        private void FillData(ModulusTopRecordInfo info)
        {
            lbldate.Text = info.TheDate;
            lblno.Text = info.No.ToString();
            lbldiameter.Text = String.Format("{0:f1} mm", info.Diameter / 10f);
            lblheight.Text = String.Format("{0:f1} mm", info.Height / 10f);
            lblnodecnt.Text = info.Nodecnt.ToString();
            int i = 1;
            String fmt1 = "{0}级压强= {1:f3} MPa";
            String fmt2 = "回弹形变= {0:f3} mm";
            lbldis1.Text = String.Format(fmt1, i, info.getXYNodes()[i].getNodeX());
            lblinfo1.Text = String.Format(fmt2, info.getXYNodes()[i++].getNodeY());
            lbldis2.Text = String.Format(fmt1, i, info.getXYNodes()[i].getNodeX());
            lblinfo2.Text = String.Format(fmt2, info.getXYNodes()[i++].getNodeY());
            lbldis3.Text = String.Format(fmt1, i, info.getXYNodes()[i].getNodeX());
            lblinfo3.Text = String.Format(fmt2, info.getXYNodes()[i++].getNodeY());
            lbldis4.Text = String.Format(fmt1, i, info.getXYNodes()[i].getNodeX());
            lblinfo4.Text = String.Format(fmt2, info.getXYNodes()[i++].getNodeY());
            lbldis5.Text = String.Format(fmt1, i, info.getXYNodes()[i].getNodeX());
            lblinfo5.Text = String.Format(fmt2, info.getXYNodes()[i++].getNodeY());
            lbldis6.Text = String.Format(fmt1, i, info.getXYNodes()[i].getNodeX());
            lblinfo6.Text = String.Format(fmt2, info.getXYNodes()[i++].getNodeY());
            lbldis7.Text = "";
            lblinfo7.Text = "";
        }

        private void FillData(ModulusStrengthInfo info)
        {
            lbldate.Text = info.TheDate;
            lblno.Text = info.No.ToString();
            lblnodecnt.Text = info.Nodecnt.ToString();
            DataTable dt = info.getDataTable();
            int _idx = 3;
            
            lblheightdis.Text = "试件宽度";
            lblheight.Text=dt.Rows[_idx++][1].ToString();
            lbldiameterdis.Text = "试件高度";
            lbldiameter.Text = dt.Rows[_idx++][1].ToString();
            lbldis1.Text = "温度";
            lblinfo1.Text = dt.Rows[_idx++][1].ToString();
            _idx++;//加载速度
            _idx++;//记录点数
            lbldis2.Text = "试件跨径";
            lblinfo2.Text = dt.Rows[_idx++][1].ToString();
            lbldis5.Text = "RB";
            lblinfo5.Text = dt.Rows[_idx++][1].ToString();
            lbldis3.Text = "最大点压力";
            lblinfo3.Text = dt.Rows[_idx++][1].ToString();
            lbldis4.Text = "跨中挠度";
            lblinfo4.Text = dt.Rows[_idx++][1].ToString();
            
            lbldis6.Text = "EB";
            lblinfo6.Text = dt.Rows[_idx++][1].ToString();
            lbldis7.Text = "SB";
            lblinfo7.Text = dt.Rows[_idx++][1].ToString();

            //lbldiameter.Text = String.Format("{0:f1} mm", info.Diameter / 10f);
            //lblheight.Text = String.Format("{0:f1} mm", info.Height / 10f);
            //lblnodecnt.Text = info.Nodecnt.ToString();
            //int i = 1;
            //String fmt1 = "{0}级压强= {1:f1} kPa";
            //String fmt2 = "回弹形变= {0:f3} mm";
            //lbldis1.Text = String.Format(fmt1, i, info.getXYNodes()[i].getNodeX());
            //lblinfo1.Text = String.Format(fmt2,info.getXYNodes()[i++].getNodeY());
            //lbldis2.Text = String.Format(fmt1, i, info.getXYNodes()[i].getNodeX());
            //lblinfo2.Text = String.Format(fmt2, info.getXYNodes()[i++].getNodeY());
            //lbldis3.Text = String.Format(fmt1, i, info.getXYNodes()[i].getNodeX());
            //lblinfo3.Text = String.Format(fmt2, info.getXYNodes()[i++].getNodeY());
            //lbldis4.Text = String.Format(fmt1, i, info.getXYNodes()[i].getNodeX());
            //lblinfo4.Text = String.Format(fmt2, info.getXYNodes()[i++].getNodeY());
            //lbldis5.Text = String.Format(fmt1, i, info.getXYNodes()[i].getNodeX());
            //lblinfo5.Text = String.Format(fmt2, info.getXYNodes()[i++].getNodeY());
            //lbldis6.Text = String.Format(fmt1, i, info.getXYNodes()[i].getNodeX());
            //lblinfo6.Text = String.Format(fmt2, info.getXYNodes()[i++].getNodeY());
            //lbldis7.Text = "";
            //lblinfo7.Text = "";
        }

        private void FillData(LIQIHanliangInfo info)
        {
            lbldate.Text = info.TheDate;
            lblno.Text = info.No.ToString();
            lblnodecnt.Text = info.Nodecnt.ToString();
            DataTable dt = info.getDataTable();
            int _idx = 3;

            lblheightdis.Text = "试验温度";
            lblheight.Text = dt.Rows[_idx++][1].ToString();
            lbldiameterdis.Text = "试件质量";
            lbldiameter.Text = dt.Rows[_idx++][1].ToString();
            _idx++;//加载速度
            _idx++;//记录点数
            _idx++;
            lbldis1.Text = "油石比";
            lblinfo1.Text = dt.Rows[_idx++][1].ToString();
            
            lbldis2.Text = "含油比";
            lblinfo2.Text = dt.Rows[_idx++][1].ToString();
            lbldis5.Text = "试验用时";
            lblinfo5.Text = dt.Rows[_idx++][1].ToString();
            lbldis3.Text = "结束温度";
            lblinfo3.Text = dt.Rows[_idx++][1].ToString();
            lbldis4.Text = "结束质量";
            lblinfo4.Text = dt.Rows[_idx++][1].ToString();

            lbldis6.Text = "";
            lblinfo6.Text = "";// dt.Rows[_idx++][1].ToString();
            lbldis7.Text = "";
            lblinfo7.Text = "";// dt.Rows[_idx++][1].ToString();
        }

        void lblinfo4_DoubleClick(object sender, EventArgs e)
        {
            EditNaodu();
        }

        protected void EditNaodu()
        {
            if (currentInfo == null) return;
            EditForm ef = new EditForm();
            String oldvalue = lblinfo4.Text;
            String newvalue = ef.EditValue(oldvalue);
            if (oldvalue.Equals(newvalue)) return;
            currentInfo.EditValue(lbldis4.Text, newvalue);
            FillData(currentInfo);
            ReRenderChart(currentInfo);
            ef.Close();
        }

        private void ReRenderChart(AbstractRecordInfo info)
        {
            if (info == null) return;
            chart1.Series.Clear();

            Series series1 = new Series();//chart1.Series.Add("CBR");
            series1.ChartType = SeriesChartType.Line;
            //线条宽度
            series1.BorderWidth = 1;
            //阴影宽度
            series1.ShadowOffset = 0;
            //是否显示在图例集合Legends
            series1.IsVisibleInLegend = false;
            //线条上数据点上是否有数据显示
            series1.IsValueShownAsLabel = false;
            //线条颜色
            series1.Color = Color.Black;// Color.Yellow;
            //设置曲线X轴的显示类型
            series1.XValueType = info.Chartformat.Xtype == 0 ? ChartValueType.Int32 : ChartValueType.Double;//ChartValueType.Int32;
            //设置数据点的类型
            series1.MarkerStyle = MarkerStyle.None;
            //线条数据点的大小
            series1.MarkerSize = 5;
            series1.YValueType = info.Chartformat.Ytype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Double;

            double x = Double.MinValue;
            //double y = Double.MinValue;
            foreach (IXYNode ninfo in info.getXYNodes())
            {
                double nodex, nodey;
                nodex = ninfo.getNodeX();
                nodey = ninfo.getNodeY();
                if (nodex < x) nodex = x;
                else
                    x = nodex;
                if (nodex < x) nodex = x;
                else
                    x = nodex;

                //if (nodey < y) nodey = y;
                //else
                //    y = nodey;
                series1.Points.AddXY(nodex, nodey);//series1.Points.AddXY(ninfo.getNodeX(), ninfo.getNodeY());
            }



            chart1.Series.Add(series1);

            if (info.getSpecialNodes() != null)
            {
                foreach (IXYNode ninfo in info.getSpecialNodes())
                {
                    Series series50 = new Series();
                    series50.ChartType = SeriesChartType.Line;
                    series50.BorderWidth = 1;
                    series50.ShadowOffset = 0;
                    series50.IsVisibleInLegend = true;
                    series50.IsValueShownAsLabel = false;
                    series50.Color = Color.Red;
                    series50.XValueType = info.Chartformat.Xtype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Int32;
                    series50.YValueType = info.Chartformat.Ytype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Double;
                    series50.MarkerStyle = MarkerStyle.None;
                    chart1.Series.Add(series50);
                    series50.Points.AddXY(0, ninfo.getNodeY());
                    series50.Points.AddXY(ninfo.getNodeX(), ninfo.getNodeY());
                    series50.Points.AddXY(ninfo.getNodeX(), 0);
                }
            }
        }

        protected void FillData(CBRRecordFileInfo info)
        {
            lbldate.Text = info.TheDate;
            lblno.Text = info.No.ToString();
            lbldiameter.Text = String.Format("{0:f1} mm", info.Diameter / 10f);
            lblheight.Text = String.Format("{0:f1} mm", info.Height / 10f);
            lblnodecnt.Text = info.Nodecnt.ToString();
            lbldis1.Text = "2.5mm CBR";
            lblinfo1.Text = String.Format("{0:f2} %", info.Cbr25 / 100f);
            lbldis2.Text = "2.5mm 压强";
            lblinfo2.Text = String.Format("{0} KPa", info.Kpa25);
            lbldis3.Text = "5mm CBR";
            lblinfo3.Text = String.Format("{0:f2} %", info.Cbr50 / 100f); 
            lbldis4.Text = "5mm 压强";
            lblinfo4.Text = String.Format("{0} KPa", info.Kpa50);
            lbldis5.Text = "";
            lblinfo5.Text = "";
            lbldis6.Text = "";
            lblinfo6.Text = "";
            lbldis7.Text = "";
            lblinfo7.Text = "";
        }

        protected void FillData(StrengthRecordInfo info)
        {
            lbldate.Text = info.TheDate;
            lblno.Text = info.No.ToString();
            lbldiameter.Text = String.Format("{0:f1} mm", info.Diameter / 10f);
            lblheight.Text = String.Format("{0:f1} mm", info.Height / 10f);
            lblnodecnt.Text = info.Nodecnt.ToString();
            lbldis1.Text = "峰值压力";
            lblinfo1.Text = String.Format("{0:f2} KN", info.Maxstrength / 100f);
            lbldis2.Text = "";
            lblinfo2.Text = "";
            lbldis3.Text = "";
            lblinfo3.Text = "";
            lbldis4.Text = "";
            lblinfo4.Text = "";
            lbldis5.Text = "";
            lblinfo5.Text = "";
            lbldis6.Text = "";
            lblinfo6.Text = "";
            lbldis7.Text = "";
            lblinfo7.Text = "";
        }


        private void RenderChart(AbstractRecordInfo info)
        {
            if (info == null) return;
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.Legends.Clear();

            #region 设置图表区属性
            ChartArea chartArea = new ChartArea("Default");

            //设置Y轴刻度间隔大小
            chartArea.AxisY.Interval = info.Chartformat.Yinterval;// 2;
            chartArea.AxisY.Minimum = info.Chartformat.Ymin;// 0;
            chartArea.AxisY.Maximum = info.Chartformat.Ymax;//10;
            chartArea.AxisY.IsReversed = info.Chartformat.Yreverse;
            //设置Y轴的数据类型格式
            //chartArea.AxisY.LabelStyle.Format = "C";
            //设置背景色
            chartArea.BackColor = Color.White;// Color.FromArgb(64, 165, 191, 228);
            //设置背景渐变方式
            chartArea.BackGradientStyle = GradientStyle.None;// GradientStyle.TopBottom;
            //设置渐变和阴影的辅助背景色
            chartArea.BackSecondaryColor = Color.Black;
            //设置边框颜色
            chartArea.BorderColor = Color.FromArgb(64, 64, 64, 64);
            //设置阴影颜色
            chartArea.ShadowColor = Color.Transparent;
            //设置X轴和Y轴线条的颜色
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            //设置X轴和Y轴线条的宽度
            chartArea.AxisX.LineWidth = 1;
            chartArea.AxisY.LineWidth = 1;
            //设置X轴和Y轴的标题
            chartArea.AxisX.Title = info.Chartformat.Xname;// "Kpa";
            chartArea.AxisY.Title = info.Chartformat.Yname;// "mm";
            //设置图表区网格横纵线条的颜色
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            //设置图表区网格横纵线条的宽度
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            //设置坐标轴刻度线不延长出来
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;

            chartArea.AxisX.MinorGrid.Enabled = true;
            chartArea.AxisY.MinorGrid.Enabled = true;
            chartArea.AxisX.MinorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MinorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
            chartArea.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dash;


            //开启下面两句能够隐藏网格线条
            //chartArea.AxisX.MajorGrid.Enabled = false;
            //chartArea.AxisY.MajorGrid.Enabled = false;
            //设置X轴的显示类型及显示方式
            chartArea.AxisX.Interval = info.Chartformat.Xinterval;//4000; //设置为0表示由控件自动分配
            chartArea.AxisX.IsStartedFromZero = true;
            chartArea.AxisX.Minimum = info.Chartformat.Xmin;//0;
            chartArea.AxisX.Maximum = info.Chartformat.Xmax;//20000;
            chartArea.AxisX.IsReversed = info.Chartformat.Xreverse;
            //chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            //chartArea.AxisX.IntervalType = DateTimeIntervalType.Minutes;

            //chartArea.AxisX.LabelStyle.IsStaggered = true;
            //chartArea.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            //设置文本角度
            //chartArea.AxisX.LabelStyle.Angle = 45;
            //设置文本自适应
            //chartArea.AxisX.IsLabelAutoFit = true;
            //设置X轴允许拖动放大
            //chartArea.CursorX.IsUserEnabled = true;
           // chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.Interval = 0;
            chartArea.CursorX.IntervalOffset = 0;
            chartArea.CursorX.IntervalType = DateTimeIntervalType.Minutes;
            chartArea.AxisX.ScaleView.Zoomable = false;
            chartArea.AxisX.ScrollBar.IsPositionedInside = false;

            chartArea.AxisY.ScaleView.Zoomable = false;

            //设置中短线（还没看到效果）
            //chartArea.AxisY.ScaleBreakStyle.Enabled = true;
            //chartArea.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 47;
            //chartArea.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            //chartArea.AxisY.ScaleBreakStyle.Spacing = 2;
            //chartArea.AxisY.ScaleBreakStyle.LineColor = Color.Red;
            //chartArea.AxisY.ScaleBreakStyle.LineWidth = 10;

            chart1.ChartAreas.Add(chartArea);
            #endregion


            Series series1 = new Series();//chart1.Series.Add("CBR");
            series1.ChartType = SeriesChartType.Line;
            //线条宽度
            series1.BorderWidth = 1;
            //阴影宽度
            series1.ShadowOffset = 0;
            //是否显示在图例集合Legends
            series1.IsVisibleInLegend = false;
            //线条上数据点上是否有数据显示
            series1.IsValueShownAsLabel = false;
            //线条颜色
            series1.Color = Color.Black;// Color.Yellow;
            //设置曲线X轴的显示类型
            series1.XValueType = info.Chartformat.Xtype == 0 ? ChartValueType.Int32 : ChartValueType.Double ;//ChartValueType.Int32;
            //设置数据点的类型
            series1.MarkerStyle = MarkerStyle.None;
            //线条数据点的大小
            series1.MarkerSize = 5;
            series1.YValueType = info.Chartformat.Ytype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Double;


            //线条1：下限横线
            /*Series series25 = new Series("cbr25");
            series25.ChartType = SeriesChartType.Line;
            series25.BorderWidth = 1;
            series25.ShadowOffset = 0;
            series25.IsVisibleInLegend = true;
            series25.IsValueShownAsLabel = false;
            series25.Color = Color.Red;
            series25.XValueType = info.Chartformat.Xtype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Int32;
            series25.YValueType = info.Chartformat.Ytype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Double;
            series25.MarkerStyle = MarkerStyle.None;
            chart1.Series.Add(series25);

            //线条3：上限横线
            Series series50 = new Series("cbr50");
            series50.ChartType = SeriesChartType.Line;
            series50.BorderWidth = 1;
            series50.ShadowOffset = 0;
            series50.IsVisibleInLegend = true;
            series50.IsValueShownAsLabel = false;
            series50.Color = Color.Red;
            series50.XValueType = info.Chartformat.Xtype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Int32;
            series25.YValueType = info.Chartformat.Ytype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Double;
            series50.MarkerStyle = MarkerStyle.None;
            chart1.Series.Add(series50);

            series25.Points.AddXY(0, 2.5f);
            series25.Points.AddXY(info.Kpa25, 2.5f);
            series25.Points.AddXY(info.Kpa25, 0);

            series50.Points.AddXY(0, 5.0f);
            series50.Points.AddXY(info.Kpa50, 5.0f);
            series50.Points.AddXY(info.Kpa50, 0);
             * */

            //foreach (CBRRecordNodeInfo ninfo in info.Nodes)
            //{
            //    float y = ninfo.Offset / 1000f;
            //    series1.Points.AddXY(ninfo.KPa, y);
            //}
            double x = Double.MinValue;
            double y = Double.MinValue;
            
/*
            foreach (IXYNode ninfo in info.getXYNodes())
            {
                double nodex, nodey;
                nodex = ninfo.getNodeX();
                nodey = ninfo.getNodeY();
                //X轴防止倒退
                if (nodex < x) nodex = x;
                else
                    x = nodex;
                series1.Points.AddXY(nodex,nodey);//series1.Points.AddXY(ninfo.getNodeX(), ninfo.getNodeY());
            }
 */
            int idx = 0;
            int nodecnt = info.getXYNodes().Count;
            List<IXYNode> list=info.getXYNodes();
            for(idx=0;idx<nodecnt;idx++)
            //foreach (IXYNode ninfo in info.getXYNodes())
            {
                IXYNode ninfo, prev, next;
                ninfo = list[idx];
                double nodex, nodey;
                nodex = ninfo.getNodeX();
                nodey = ninfo.getNodeY();
                //X轴防止倒退
                if (nodex < x) nodex = x;
                else
                    x = nodex;

                //平均值法去掉离异y点值
                if (idx > 0&&idx<nodecnt-1)
                {
                    prev = list[idx - 1];
                    next = list[idx + 1];
                    double y1, y2;
                    y1 = prev.getNodeY();
                    y2 = next.getNodeY();
                    y = (y1 + y2) / 2;
                    if (Math.Abs(nodey - y) > Math.Abs(y1-y2))
                    {
                        nodey = y;
                    }
                }
                
                series1.Points.AddXY(nodex, nodey);//series1.Points.AddXY(ninfo.getNodeX(), ninfo.getNodeY());
            }
          
           
            chart1.Series.Add(series1);

            if (info.getSpecialNodes() != null)
            {
                foreach (IXYNode ninfo in info.getSpecialNodes())
                {
                    Series series50 = new Series();
                    series50.ChartType = SeriesChartType.Line;
                    series50.BorderWidth = 1;
                    series50.ShadowOffset = 0;
                    series50.IsVisibleInLegend = true;
                    series50.IsValueShownAsLabel = false;
                    series50.Color = Color.Red;
                    series50.XValueType = info.Chartformat.Xtype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Int32;
                    series50.YValueType = info.Chartformat.Ytype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Double;
                    series50.MarkerStyle = MarkerStyle.None;
                    chart1.Series.Add(series50);
                    series50.Points.AddXY(0, ninfo.getNodeY());
                    series50.Points.AddXY(ninfo.getNodeX(), ninfo.getNodeY());
                    series50.Points.AddXY(ninfo.getNodeX(), 0);
                }
            }
            currentx = 1;
            currenty = 1;

        }

        double currentx=0;
        double currenty=0;

        private void RenderChart(AbstractRecordInfo info, double x, double y)
        {
            if (info == null) return;
            ChartArea chartArea=chart1.ChartAreas["Default"];
            chartArea.AxisY.Interval = info.Chartformat.Yinterval * y;// 2
            chartArea.AxisY.Maximum = info.Chartformat.Ymax * y;//10;
            chartArea.AxisX.Interval = info.Chartformat.Xinterval * x;//4000; //设置为0表示由控件自动分配
            chartArea.AxisX.Maximum = info.Chartformat.Xmax * x;//20000;
            info.Chartformat.Yinterval = chartArea.AxisY.Interval;
            info.Chartformat.Ymax = chartArea.AxisY.Maximum;
            info.Chartformat.Xinterval = chartArea.AxisX.Interval;
            info.Chartformat.Xmax = chartArea.AxisX.Maximum;
            
        }

        private void _RenderChart(AbstractRecordInfo info,double x,double y)
        {
            if (info == null) return;
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.Legends.Clear();

            #region 设置图表区属性
            ChartArea chartArea = new ChartArea("Default");

            //设置Y轴刻度间隔大小
            chartArea.AxisY.Interval = info.Chartformat.Yinterval*y;// 2;
            chartArea.AxisY.Minimum = info.Chartformat.Ymin;// 0;
            chartArea.AxisY.Maximum = info.Chartformat.Ymax*y;//10;
            chartArea.AxisY.IsReversed = info.Chartformat.Yreverse;
            //设置Y轴的数据类型格式
            //chartArea.AxisY.LabelStyle.Format = "C";
            //设置背景色
            chartArea.BackColor = Color.White;// Color.FromArgb(64, 165, 191, 228);
            //设置背景渐变方式
            chartArea.BackGradientStyle = GradientStyle.None;// GradientStyle.TopBottom;
            //设置渐变和阴影的辅助背景色
            chartArea.BackSecondaryColor = Color.Black;
            //设置边框颜色
            chartArea.BorderColor = Color.FromArgb(64, 64, 64, 64);
            //设置阴影颜色
            chartArea.ShadowColor = Color.Transparent;
            //设置X轴和Y轴线条的颜色
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            //设置X轴和Y轴线条的宽度
            chartArea.AxisX.LineWidth = 1;
            chartArea.AxisY.LineWidth = 1;
            //设置X轴和Y轴的标题
            chartArea.AxisX.Title = info.Chartformat.Xname;// "Kpa";
            chartArea.AxisY.Title = info.Chartformat.Yname;// "mm";
            //设置图表区网格横纵线条的颜色
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            //设置图表区网格横纵线条的宽度
            chartArea.AxisX.MajorGrid.LineWidth = 1;
            chartArea.AxisY.MajorGrid.LineWidth = 1;
            //设置坐标轴刻度线不延长出来
            chartArea.AxisX.MajorTickMark.Enabled = false;
            chartArea.AxisY.MajorTickMark.Enabled = false;
            //开启下面两句能够隐藏网格线条
            //chartArea.AxisX.MajorGrid.Enabled = false;
            //chartArea.AxisY.MajorGrid.Enabled = false;

            chartArea.AxisX.MinorGrid.Enabled = true;
            chartArea.AxisY.MinorGrid.Enabled = true;
            chartArea.AxisX.MinorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MinorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MinorGrid.LineDashStyle = ChartDashStyle.Dash;
            chartArea.AxisY.MinorGrid.LineDashStyle = ChartDashStyle.Dash;

            //设置X轴的显示类型及显示方式
            chartArea.AxisX.Interval = info.Chartformat.Xinterval*x;//4000; //设置为0表示由控件自动分配
            chartArea.AxisX.IsStartedFromZero = true;
            chartArea.AxisX.Minimum = info.Chartformat.Xmin;//0;
            chartArea.AxisX.Maximum = info.Chartformat.Xmax*x;//20000;
            chartArea.AxisX.IsReversed = info.Chartformat.Xreverse;
            //chartArea.AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            //chartArea.AxisX.IntervalType = DateTimeIntervalType.Minutes;

            //chartArea.AxisX.LabelStyle.IsStaggered = true;
            //chartArea.AxisX.MajorGrid.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.IntervalType = DateTimeIntervalType.Minutes;
            //chartArea.AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm:ss";
            //设置文本角度
            //chartArea.AxisX.LabelStyle.Angle = 45;
            //设置文本自适应
            //chartArea.AxisX.IsLabelAutoFit = true;
            //设置X轴允许拖动放大
            //chartArea.CursorX.IsUserEnabled = true;
            // chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.CursorX.Interval = 0;
            chartArea.CursorX.IntervalOffset = 0;
            chartArea.CursorX.IntervalType = DateTimeIntervalType.Minutes;
            chartArea.AxisX.ScaleView.Zoomable = false;
            chartArea.AxisX.ScrollBar.IsPositionedInside = false;

            chartArea.AxisY.ScaleView.Zoomable = false;

            //设置中短线（还没看到效果）
            //chartArea.AxisY.ScaleBreakStyle.Enabled = true;
            //chartArea.AxisY.ScaleBreakStyle.CollapsibleSpaceThreshold = 47;
            //chartArea.AxisY.ScaleBreakStyle.BreakLineStyle = BreakLineStyle.Wave;
            //chartArea.AxisY.ScaleBreakStyle.Spacing = 2;
            //chartArea.AxisY.ScaleBreakStyle.LineColor = Color.Red;
            //chartArea.AxisY.ScaleBreakStyle.LineWidth = 10;

            chart1.ChartAreas.Add(chartArea);
            #endregion


            Series series1 = new Series();//chart1.Series.Add("CBR");
            series1.ChartType = SeriesChartType.Line;
            //线条宽度
            series1.BorderWidth = 1;
            //阴影宽度
            series1.ShadowOffset = 0;
            //是否显示在图例集合Legends
            series1.IsVisibleInLegend = false;
            //线条上数据点上是否有数据显示
            series1.IsValueShownAsLabel = false;
            //线条颜色
            series1.Color = Color.Black;// Color.Yellow;
            //设置曲线X轴的显示类型
            series1.XValueType = info.Chartformat.Xtype == 0 ? ChartValueType.Int32 : ChartValueType.Double;//ChartValueType.Int32;
            //设置数据点的类型
            series1.MarkerStyle = MarkerStyle.None;
            //线条数据点的大小
            series1.MarkerSize = 5;
            series1.YValueType = info.Chartformat.Ytype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Double;


            //线条1：下限横线
            /*Series series25 = new Series("cbr25");
            series25.ChartType = SeriesChartType.Line;
            series25.BorderWidth = 1;
            series25.ShadowOffset = 0;
            series25.IsVisibleInLegend = true;
            series25.IsValueShownAsLabel = false;
            series25.Color = Color.Red;
            series25.XValueType = info.Chartformat.Xtype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Int32;
            series25.YValueType = info.Chartformat.Ytype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Double;
            series25.MarkerStyle = MarkerStyle.None;
            chart1.Series.Add(series25);

            //线条3：上限横线
            Series series50 = new Series("cbr50");
            series50.ChartType = SeriesChartType.Line;
            series50.BorderWidth = 1;
            series50.ShadowOffset = 0;
            series50.IsVisibleInLegend = true;
            series50.IsValueShownAsLabel = false;
            series50.Color = Color.Red;
            series50.XValueType = info.Chartformat.Xtype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Int32;
            series25.YValueType = info.Chartformat.Ytype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Double;
            series50.MarkerStyle = MarkerStyle.None;
            chart1.Series.Add(series50);

            series25.Points.AddXY(0, 2.5f);
            series25.Points.AddXY(info.Kpa25, 2.5f);
            series25.Points.AddXY(info.Kpa25, 0);

            series50.Points.AddXY(0, 5.0f);
            series50.Points.AddXY(info.Kpa50, 5.0f);
            series50.Points.AddXY(info.Kpa50, 0);
             * */

            //foreach (CBRRecordNodeInfo ninfo in info.Nodes)
            //{
            //    float y = ninfo.Offset / 1000f;
            //    series1.Points.AddXY(ninfo.KPa, y);
            //}
            foreach (IXYNode ninfo in info.getXYNodes())
            {
                if(ninfo.getNodeX()<=chartArea.AxisX.Maximum&&ninfo.getNodeY()<=chartArea.AxisY.Maximum)
                    series1.Points.AddXY(ninfo.getNodeX(), ninfo.getNodeY());
            }



            chart1.Series.Add(series1);

            if (info.getSpecialNodes() != null)
            {
                foreach (IXYNode ninfo in info.getSpecialNodes())
                {
                    Series series50 = new Series();
                    series50.ChartType = SeriesChartType.Line;
                    series50.BorderWidth = 1;
                    series50.ShadowOffset = 0;
                    series50.IsVisibleInLegend = true;
                    series50.IsValueShownAsLabel = false;
                    series50.Color = Color.Red;
                    series50.XValueType = info.Chartformat.Xtype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Int32;
                    series50.YValueType = info.Chartformat.Ytype == 0 ? ChartValueType.Int32 : ChartValueType.Double;// ChartValueType.Double;
                    series50.MarkerStyle = MarkerStyle.None;
                    chart1.Series.Add(series50);
                    series50.Points.AddXY(0, ninfo.getNodeY());
                    series50.Points.AddXY(ninfo.getNodeX(), ninfo.getNodeY());
                    series50.Points.AddXY(ninfo.getNodeX(), 0);
                }
            }

        }

        private void tsm_uploaddata_Click(object sender, EventArgs e)
        {
            ReadData rd = new ReadData();
            rd.ShowDialog();
            if (rd.isReaded)
            {
                try
                {
                    FillData(rd.Info);
                    RenderChart(rd.Info);
                }
                catch (Exception ex)
                {
                    MessageForm.Show("数据解析出错！", ex);
                    rd.SaveData();
                }
            }
            rd.Close();

            
        }

        private void tsm_saveinfo_Click(object sender, EventArgs e)
        {
            //if (currentInfo != null)
            //{
            //    saveFileDialog1.FileName = String.Format("{0}.rec.bin", currentInfo.TheDate);
            //    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //    {
            //        String filename = saveFileDialog1.FileName;
            //        byte[] bytes = new byte[currentInfo.SendBuffer.Length + currentInfo.DataBuffer.Length];
            //        Array.Copy(currentInfo.SendBuffer, 0, bytes, 0, currentInfo.SendBuffer.Length);
            //        Array.Copy(currentInfo.DataBuffer, 0, bytes, currentInfo.SendBuffer.Length, currentInfo.DataBuffer.Length);
            //        File.WriteAllBytes(filename, bytes);
            //        MessageBox.Show("保存成功");
            //    }
            //}
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            printDocument1.OriginAtMargins = true;
            pageSetupDialog1.EnableMetric = true;

            printDocument1.DefaultPageSettings.Landscape =  false;
            Clear();
           // lblinfo4.DoubleClick += new EventHandler(lblinfo4_DoubleClick);
            /*
            lbldate.Text = "";
            lblno.Text = "";
            lbldiameter.Text = "";
            lblheight.Text = "";
            lblnodecnt.Text = "";
            lbldis1.Text = "";
            lblinfo1.Text = "";
            lbldis2.Text = "";
            lblinfo2.Text = "";
            lbldis3.Text = "";
            lblinfo3.Text = "";
            lbldis4.Text = "";
            lblinfo4.Text = "";
            lbldis5.Text = "";
            lblinfo5.Text = "";
            lbldis6.Text = "";
            lblinfo6.Text = "";
            lbldis7.Text = "";
            lblinfo7.Text = "";
             */
        }


        private void Clear()
        {
            currentInfo = null;
            lbldate.Text = "";
            lblno.Text = "";
            lbldiameter.Text = "";
            lblheight.Text = "";
            lblnodecnt.Text = "";
            lbldis1.Text = "";
            lblinfo1.Text = "";
            lbldis2.Text = "";
            lblinfo2.Text = "";
            lbldis3.Text = "";
            lblinfo3.Text = "";
            lbldis4.Text = "";
            lblinfo4.Text = "";
            lbldis5.Text = "";
            lblinfo5.Text = "";
            lbldis6.Text = "";
            lblinfo6.Text = "";
            lbldis7.Text = "";
            lblinfo7.Text = "";
            lblrecordname.Text = "试验记录名称";
            chart1.ChartAreas.Clear();
            chart1.Series.Clear();
            chart1.Legends.Clear();
        }

        private void tsmclear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void tsmabout_Click(object sender, EventArgs e)
        {
            AboutDialog dlg = new AboutDialog();
            dlg.ShowDialog();
            dlg.Close();
            dlg = null;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            
            ////打印内容 为 整个Form
            //Image myFormImage;
            //myFormImage = new Bitmap(this.Width, this.Height);
            //Graphics g = Graphics.FromImage(myFormImage);
            //g.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, this.Size);
            //e.Graphics.DrawImage(myFormImage, 0, 0);
            String old1, old2;
            old1 = groupboxchart.Text;
            old2 = groupboxinfo.Text;
            groupboxchart.Text = "";
            groupboxinfo.Text = "";
            if (printDocument1.DefaultPageSettings.Landscape)
            {
                
                Bitmap _newbitmap = new Bitmap(pnlfill.Width, pnlfill.Height);
                pnlfill.DrawToBitmap(_newbitmap, new Rectangle(0, 0, _newbitmap.Width, _newbitmap.Height));
                
                int sourcewidth = _newbitmap.Width;
                int sourceheight = _newbitmap.Height;

                int x = e.MarginBounds.Left;
                int y = e.MarginBounds.Top;
                int targetwidth = e.MarginBounds.Width;
                int targetheight = e.MarginBounds.Height - 80;


                int width = 0;
                int height = 0;

                if (sourcewidth > targetwidth && sourceheight > targetheight)
                {
                    width = targetwidth;
                    height = (width * sourceheight) / sourcewidth;
                    if (height > targetheight)
                    {
                        height = targetheight;
                        width = (height * sourcewidth) / sourceheight;
                    }
                }
                else if (sourcewidth > targetwidth)
                {
                    width = targetwidth;
                    height = (width * sourceheight) / sourcewidth;
                }
                else if (sourceheight > targetheight)
                {
                    height = targetheight;
                    width = (height * sourcewidth) / sourceheight;
                }
                else
                {
                    width = sourcewidth;
                    height = sourceheight;
                }
                
                e.Graphics.PageUnit = GraphicsUnit.Display;
                RectangleF imgrect = new RectangleF(x * 100 / e.Graphics.DpiX, y * 100 / e.Graphics.DpiY, width, height);
                e.Graphics.DrawImage(_newbitmap, imgrect);
                RectangleF txtrect = new RectangleF(imgrect.X, imgrect.Y + imgrect.Height + 40, imgrect.Width, 40);
                e.Graphics.DrawString("试验员__________________________________", new Font(Font.SystemFontName, 10), Brushes.Black, txtrect);


               
            }
            else
            {
                Bitmap _newbitmapchart = new Bitmap(groupboxchart.Width, groupboxchart.Height);
                groupboxchart.DrawToBitmap(_newbitmapchart, new Rectangle(0, 0, _newbitmapchart.Width, _newbitmapchart.Height));

                int sourcewidth = _newbitmapchart.Width;
                int sourceheight = _newbitmapchart.Height;

                int x = e.MarginBounds.Left;
                int y = e.MarginBounds.Top;
                int targetwidth = e.MarginBounds.Width;
                int targetheight = e.MarginBounds.Height;


                int width = 0;
                int height = 0;

                width = targetwidth;
                height = (width * sourceheight) / sourcewidth;

                /*if (sourcewidth > targetwidth && sourceheight > targetheight)
                {
                    width = targetwidth;
                    height = (width * sourceheight) / sourcewidth;
                    if (height > targetheight)
                    {
                        height = targetheight;
                        width = (height * sourcewidth) / sourceheight;
                    }
                }
                else if (sourcewidth > targetwidth)
                {
                    width = targetwidth;
                    height = (width * sourceheight) / sourcewidth;
                }
                else if (sourceheight > targetheight)
                {
                    height = targetheight;
                    width = (height * sourcewidth) / sourceheight;
                }
                else
                {
                    width = sourcewidth;
                    height = sourceheight;
                }*/
                


                e.Graphics.PageUnit = GraphicsUnit.Display;

                SizeF size=e.Graphics.MeasureString(lblrecordname.Text,new Font(Font.SystemFontName,16));

                int txtxmargin = targetwidth / 3;
                int txtmargin = 40;
                int txtheight = (targetheight - 80 - Convert.ToInt32(size.Height)-20 - height) / 5- txtmargin;
                if (txtheight < 40)
                {
                    txtmargin = txtheight;
                    txtheight = 40;
                }


                PointF headpoint = new PointF(x * 100 / e.Graphics.DpiX, y * 100 / e.Graphics.DpiY);
                e.Graphics.DrawString(lblrecordname.Text, new Font(Font.SystemFontName, 16),Brushes.Black, new PointF(headpoint.X+(targetwidth-Convert.ToInt32(size.Width))/2, headpoint.Y));

                PointF firstpoint = new PointF(headpoint.X, headpoint.Y+size.Height+20);

                e.Graphics.DrawString(String.Format("{0}:{1}",lbldatedis.Text,lbldate.Text), new Font(Font.SystemFontName, 10), Brushes.Black, firstpoint);
                e.Graphics.DrawString(String.Format("{0}:{1}", lblnodis.Text, lblno.Text), new Font(Font.SystemFontName, 10), Brushes.Black, new PointF(firstpoint.X, firstpoint.Y + (txtmargin + txtheight)));
                e.Graphics.DrawString(String.Format("{0}:{1}", lblheightdis.Text, lblheight.Text), new Font(Font.SystemFontName, 10), Brushes.Black, new PointF(firstpoint.X + txtxmargin, firstpoint.Y + (txtmargin + txtheight)));
                e.Graphics.DrawString(String.Format("{0}:{1}", lbldiameterdis.Text, lbldiameter.Text), new Font(Font.SystemFontName, 10), Brushes.Black, new PointF(firstpoint.X+txtxmargin*2, firstpoint.Y + (txtmargin+txtheight)));
                e.Graphics.DrawString(String.Format("{0}:{1}", lblnodecntdis.Text, lblnodecnt.Text), new Font(Font.SystemFontName, 10), Brushes.Black, new PointF(firstpoint.X , firstpoint.Y + (txtmargin+txtheight)*2));
                e.Graphics.DrawString(String.Format("{0}:{1}", lbldis1.Text, lblinfo1.Text), new Font(Font.SystemFontName, 10), Brushes.Black, new PointF(firstpoint.X + txtxmargin, firstpoint.Y + (txtmargin+txtheight)*2));
                e.Graphics.DrawString(String.Format("{0}:{1}", lbldis2.Text, lblinfo2.Text), new Font(Font.SystemFontName, 10), Brushes.Black, new PointF(firstpoint.X + txtxmargin * 2, firstpoint.Y + (txtmargin + txtheight) * 2));
                e.Graphics.DrawString(String.Format("{0}:{1}", lbldis3.Text, lblinfo3.Text), new Font(Font.SystemFontName, 10), Brushes.Black, new PointF(firstpoint.X , firstpoint.Y + (txtmargin+txtheight) * 3));
                e.Graphics.DrawString(String.Format("{0}:{1}", lbldis4.Text, lblinfo4.Text), new Font(Font.SystemFontName, 10), Brushes.Black, new PointF(firstpoint.X + txtxmargin, firstpoint.Y + (txtmargin + txtheight) * 3));
                e.Graphics.DrawString(String.Format("{0}:{1}", lbldis5.Text, lblinfo5.Text), new Font(Font.SystemFontName, 10), Brushes.Black, new PointF(firstpoint.X + txtxmargin * 2, firstpoint.Y + (txtmargin + txtheight) * 3));
                e.Graphics.DrawString(String.Format("{0}:{1}", lbldis6.Text, lblinfo6.Text), new Font(Font.SystemFontName, 10), Brushes.Black, new PointF(firstpoint.X, firstpoint.Y + (txtmargin+txtheight) * 4));
                e.Graphics.DrawString(String.Format("{0}:{1}", lbldis7.Text, lblinfo7.Text), new Font(Font.SystemFontName, 10), Brushes.Black, new PointF(firstpoint.X + txtxmargin , firstpoint.Y + (txtmargin+txtheight) * 4));



                RectangleF imgrect = new RectangleF(firstpoint.X, firstpoint.Y + (txtmargin + txtheight) * 4, width, height);
                e.Graphics.DrawImage(_newbitmapchart, imgrect);
                RectangleF txtrect = new RectangleF(imgrect.X, imgrect.Y + imgrect.Height + 40, imgrect.Width, 40);
                e.Graphics.DrawString("试验员__________________________________", new Font(Font.SystemFontName, 10), Brushes.Black, txtrect);
            }

            groupboxchart.Text = old1;
            groupboxinfo.Text = old2;
        }

        private void tsmprint_Click(object sender, EventArgs e)
        {
            //if (this.printDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    this.printDocument1.PrinterSettings = this.printDialog1.PrinterSettings;
            //    this.printDocument1.Print();
            //}
        }

        private void tsmiprintset_Click(object sender, EventArgs e)
        {
            this.pageSetupDialog1.ShowDialog(); 
        }

        private void tsmiprintpreview_Click(object sender, EventArgs e)
        {
            this.printPreviewDialog1.ShowDialog();
        }

        private void tsmiprint_Click(object sender, EventArgs e)
        {
            if (currentInfo != null)
            {
                if (this.printDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.printDocument1.DocumentName = String.Format("{0}_{1}", currentInfo.RecordName, currentInfo.TheDate);
                    //this.printDocument1.PrinterSettings = this.printDialog1.PrinterSettings;
                    this.printDocument1.Print();
                }
            }
        }

        private void tsmitobin_Click(object sender, EventArgs e)
        {
            if (currentInfo != null)
            {
                saveFileDialog1.FileName = String.Format("{0}_{1}.rec.bin",currentInfo.RecordName, currentInfo.TheDate);
                saveFileDialog1.Filter = ".rec.bin|*.rec.bin";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    String filename = saveFileDialog1.FileName;
                    if (!filename.EndsWith(".rec.bin"))
                        filename = String.Format("{0}.rec.bin", filename);
                    byte[] bytes = new byte[currentInfo.SendBuffer.Length + currentInfo.DataBuffer.Length];
                    Array.Copy(currentInfo.SendBuffer, 0, bytes, 0, currentInfo.SendBuffer.Length);
                    Array.Copy(currentInfo.DataBuffer, 0, bytes, currentInfo.SendBuffer.Length, currentInfo.DataBuffer.Length);
                    File.WriteAllBytes(filename, bytes);
                    MessageBox.Show("保存成功");
                }
            }
        }

        private void tsmitocsv_Click(object sender, EventArgs e)
        {
            if (currentInfo != null)
            {
                saveFileDialog1.FileName = String.Format("{0}_{1}.csv", currentInfo.RecordName, currentInfo.TheDate);
                saveFileDialog1.Filter = ".csv|*.csv";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    String filename = saveFileDialog1.FileName;
                    if (!filename.EndsWith(".csv"))
                        filename = String.Format("{0}.csv", filename);
                    File.WriteAllLines(filename,currentInfo.getCSVLines().ToArray(),Encoding.UTF8);
                    MessageBox.Show("保存成功");
                }
            }
        }

        private void tsmitoxml_Click(object sender, EventArgs e)
        {
            if (currentInfo != null)
            {
                saveFileDialog1.FileName = String.Format("{0}_{1}.xml", currentInfo.RecordName, currentInfo.TheDate);
                saveFileDialog1.Filter = ".xml|*.xml";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    String filename = saveFileDialog1.FileName;
                    if (!filename.EndsWith(".xml"))
                        filename = String.Format("{0}.xml", filename);
                    ExcelHelper.ExportFile(currentInfo.getDataTable(), filename, false);
                    //File.WriteAllLines(filename, currentInfo.getCSVLines().ToArray(), Encoding.UTF8);
                    MessageBox.Show("保存成功");
                }
            }
        }

        private void tbxzoomout_Click(object sender, EventArgs e)
        {
            if (currentInfo != null)
            {
                currentx *= 0.5;
                RenderChart(currentInfo, 0.5,1);//RenderChart(currentInfo, currentx, currenty);
            }
        }

        private void tbxzommin_Click(object sender, EventArgs e)
        {
            if (currentInfo != null)
            {
                currentx *= 2;
                RenderChart(currentInfo, 2, 1);//RenderChart(currentInfo, currentx, currenty);
            }
        }

        private void tbyzoomout_Click(object sender, EventArgs e)
        {
            if (currentInfo != null)
            {
                currenty *= 0.5;
                RenderChart(currentInfo, 1, 0.5);//RenderChart(currentInfo, currentx, currenty);
            }
        }

        private void tbyzoomin_Click(object sender, EventArgs e)
        {
            if (currentInfo != null)
            {
                currenty *= 2;
                RenderChart(currentInfo, 1, 2);//RenderChart(currentInfo, currentx, currenty);
            }
        }

        private void tbzoomreset_Click(object sender, EventArgs e)
        {
            if (currentInfo != null)
            {
                //currentx =1;
                //currenty = 1;
                RenderChart(currentInfo, 1/currentx, 1/currenty);
                currentx = 1;
                currenty = 1;
            }
        }

        private void tbnaodu_Click(object sender, EventArgs e)
        {
            //EditNaodu();
            ToolStripItem tsi = sender as ToolStripItem;
            if (tsi == null) return;
            String valuename = tsi.Tag as String;
            if (valuename == null) return;
            if (currentInfo == null) return;
            EditForm ef = new EditForm();
            String oldvalue = currentInfo.GetEditableValuStr(valuename);//lblinfo4.Text;
            String newvalue = ef.EditValue(oldvalue);
            if (oldvalue.Equals(newvalue)) return;
            currentInfo.EditValue(valuename, newvalue);
            FillData(currentInfo);
            ReRenderChart(currentInfo);
            ef.Close();
        }
       
    }
}
