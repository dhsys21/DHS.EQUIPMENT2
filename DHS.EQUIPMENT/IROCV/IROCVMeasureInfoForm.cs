using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using DHS.EQUIPMENT.Common;

namespace DHS.EQUIPMENT
{
    public partial class IROCVMeasureInfoForm : Form
    {
        public int _iStage;
        public bool _bManualMode;
        private int _iOffsetChannel = 0;

        double[] _dStandard = new double[_Constant.ChannelCount];
        double[] _dMeasured = new double[_Constant.ChannelCount];
        double[] _dOffset = new double[_Constant.ChannelCount];

        int LABEL_TITLE_LEFT = 64 + 3;
        int LABEL_TITLE_TOP = 2;
        int LABEL_TITLE_WIDTH = 72;
        int LABEL_TITLE_HEIGHT = 42;
        int LABEL_DATA_HEIGHT = 32;
        private Label[] lblIR = new Label[_Constant.ChannelCount];
        private Label[] lblOCV = new Label[_Constant.ChannelCount];
        private Label[] lblIRStandard = new Label[_Constant.ChannelCount];
        private Label[] lblIRMeasure = new Label[_Constant.ChannelCount];
        Util util;

        //* chart
        //Series lineSeries = new Series();
        ChartArea myChartArea = new ChartArea();

        #region Delegate
        //* PLC
        public delegate void ProbeOpenClick(int stageno);
        public event ProbeOpenClick OnProbeOpenClick = null;
        protected void RaiseOnProbeOpen(int stageno)
        {
            if (OnProbeOpenClick != null)
            {
                OnProbeOpenClick(stageno);
            }
        }

        public delegate void ProbeCloseClick(int stageno);
        public event ProbeCloseClick OnProbeCloseClick = null;
        protected void RaiseOnProbeClose(int stageno)
        {
            if (OnProbeCloseClick != null)
            {
                OnProbeCloseClick(stageno);
            }
        }

        //* IROCV - Offset
        public delegate void OffsetApplyClick(int stageno, string[] strOffset, double[] offset);
        public event OffsetApplyClick OnOffsetApplyClick = null;
        protected void RaiseOnOffsetApply(int stageno, string[] strOffset, double[] offset)
        {
            if (OnOffsetApplyClick != null)
            {
                OnOffsetApplyClick(stageno, strOffset, offset);
            }
        }

        public delegate void OffsetSaveClick(int stageno, string[] strOffset);
        public event OffsetSaveClick OnOffsetSaveClick = null;
        protected void RaiseOnOffsetSave(int stageno, string[] strOffset)
        {
            if (OnOffsetSaveClick != null)
            {
                OnOffsetSaveClick(stageno, strOffset);
            }
        }

        public delegate void OffsetStartClick(int stageno, int count);
        public event OffsetStartClick OnOffsetStartClick = null;
        protected void RaiseOnOffsetStart(int stageno, int count)
        {
            if (OnOffsetStartClick != null)
            {
                OnOffsetStartClick(stageno, count);
            }
        }
        
        public delegate void OffsetStopClick(int stageno);
        public event OffsetStopClick OnOffsetStopClick = null;
        protected void RaiseOnOffsetStop(int stageno)
        {
            if (OnOffsetStopClick != null)
            {
                OnOffsetStopClick(stageno);
            }
        }
        
        public delegate void OffsetCmdIrClick(int stageno, int channel);
        public event OffsetCmdIrClick OnOffsetCmdIrClick = null;
        protected void RaiseOnOffsetCmdIr(int stageno, int channel)
        {
            if (OnOffsetCmdIrClick != null)
            {
                OnOffsetCmdIrClick(stageno, channel);
            }
        }
        
        public delegate void OffsetOpenClick(int stageno);
        public event OffsetOpenClick OnOffsetOpenClick = null;
        protected void RaiseOnOffsetOpen(int stageno)
        {
            if (OnOffsetOpenClick != null)
            {
                OnOffsetOpenClick(stageno);
            }
        }
        
        //* IROCV MSA
        public delegate void MsaStartClick(int stageno, int count);
        public event MsaStartClick OnMsaStartClick = null;
        protected void RaiseOnMsaStart(int stageno, int count)
        {
            if (OnMsaStartClick != null)
            {
                OnMsaStartClick(stageno, count);
            }
        }
        public delegate void MsaStopClick(int stageno);
        public event MsaStopClick OnMsaStopClick = null;
        protected void RaiseOnMsaStop(int stageno)
        {
            if (OnMsaStopClick != null)
            {
                OnMsaStopClick(stageno);
            }
        }

        //* IROCV Manual Inspection
        public delegate void IRFetchClick(int stageno, int channel);
        public event IRFetchClick OnIRFetchClick = null;
        protected void RaiseOnIRFetch(int stageno, int channel)
        {
            if (OnIRFetchClick != null)
            {
                OnIRFetchClick(stageno, channel);
            }
        }
        public delegate void OCVFetchClick(int stageno, int channel);
        public event OCVFetchClick OnOCVFetchClick = null;
        protected void RaiseOnOCVFetch(int stageno, int channel)
        {
            if (OnOCVFetchClick != null)
            {
                OnOCVFetchClick(stageno, channel);
            }
        }
        public delegate void AmsStartClick(int stageno);
        public event AmsStartClick OnAmsStartClick = null;
        protected void RaiseOnAmsStart(int stageno)
        {
            if (OnAmsStartClick != null)
            {
                OnAmsStartClick(stageno);
            }
        }
        public delegate void AmsStopClick(int stageno);
        public event AmsStopClick OnAmsStopClick = null;
        protected void RaiseOnAmsStop(int stageno)
        {
            if (OnAmsStopClick != null)
            {
                OnAmsStopClick(stageno);
            }
        }
        public delegate void InitDataClick(int stageno);
        public event InitDataClick OnInitDataClick = null;
        protected void RaiseOnInitData(int stageno)
        {
            if (OnInitDataClick != null)
            {
                OnInitDataClick(stageno);
            }
        }
        #endregion

        private static IROCVMeasureInfoForm measureinfoForm = new IROCVMeasureInfoForm();
        public static IROCVMeasureInfoForm GetInstance()
        {
            if (measureinfoForm == null) measureinfoForm = new IROCVMeasureInfoForm();
            return measureinfoForm;
        }
        public IROCVMeasureInfoForm()
        {
            InitializeComponent();

            measureinfoForm = this;

            //* ir/ocv value panel
            MakeLabelTitle();
            MakeLabelData();

            //* Chart - IR
            InitIRChart();

            //* Chart - OCV
            InitOCVChart();
        }

        public void SetOperationMode(bool bAuto)
        {
            if(bAuto == true)
            {
                radpnl_Title.Visible = true;
                radpnl_Title.BringToFront();
                radpanel_ManualMode.Visible = false;
            }
            else if(bAuto == false)
            {
                radpnl_Title.Visible = false;
                radpanel_ManualMode.Visible = true;
                radpanel_ManualMode.BringToFront();
            }

            //* Show Measure Value Panel
            ShowMeasureValues();
        }
        public void SetMsaCurrentCount(int nCount)
        {
            SetValueToLabel(lblMsaCurrentCount, nCount.ToString());
        }
        public void SetOffsetCurrentCount(int nCount)
        {
            SetValueToLabel(lblOffsetCurrentCount, nCount.ToString());
        }

        public void SetOffsetValueToLabel(IROCVData irocvdata)
        {
            for(int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                lblIRStandard[nIndex].Text = irocvdata.IR_STANDARD[nIndex].ToString();
                lblIRMeasure[nIndex].Text = irocvdata.IR_MEASURE[nIndex].ToString();
            }
        }
        private bool SetOffsetValue(out string[] strOffsetValue, out double[] offsets)
        {
            bool bCheck = true;
            string[] strOffset = new string[_Constant.ChannelCount];
            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                _dOffset[nIndex] = 0.0;
                _dStandard[nIndex] = Convert.ToDouble(lblIRStandard[nIndex].Text);
                _dMeasured[nIndex] = Convert.ToDouble(lblIRMeasure[nIndex].Text);
                _dOffset[nIndex] = _dMeasured[nIndex] - _dStandard[nIndex];

                if (_dMeasured[nIndex] == 0) bCheck = false;

                strOffset[nIndex] = _dStandard[nIndex] + "," + _dMeasured[nIndex] + "," + _dOffset[nIndex].ToString("F4");
            }

            strOffsetValue = strOffset;
            offsets = _dOffset;
            return bCheck;
        }

        #region Display Data
        public void InitData(int stageno)
        {
            _iStage = stageno;
            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                SetValueToLabel(lblIR[nIndex], "0.0000", _Constant.ColorIR);
                SetValueToChart(nIndex, -1, IRCHART);

                SetValueToLabel(lblOCV[nIndex], "0.00", _Constant.ColorOCV);
                SetValueToChart(nIndex, -1, OCVCHART);
            }
        }
        public void InitOffsetData(int stageno)
        {
            for(int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                SetValueToLabel(lblIRMeasure[nIndex], "0.000", _Constant.ColorMeasure);
                SetValueToChart(nIndex, - 1, IRCHART);
            }
        }
        public void InitDisplayChannelInfo(int stageno, IROCVData irocvdata, enumEquipMode equipMode)
        {
            if(stageno == _iStage)
            {
                for(int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                {
                    if (equipMode == enumEquipMode.OFFSET)
                    {
                        SetValueToLabel(lblIRMeasure[nIndex], "0.0000", _Constant.ColorMeasure);
                    }
                    else if(equipMode == enumEquipMode.MANUAL)
                    {
                        SetValueToLabel(lblIR[nIndex], "0.0000", _Constant.ColorIR);
                        SetValueToLabel(lblOCV[nIndex], "0.00", _Constant.ColorOCV);
                    }
                    else
                    {
                        //if (irocvdata.CELL[nIndex] == 1)
                        //{
                        SetValueToLabel(lblIR[nIndex], "0.0000", _Constant.ColorIR);
                        SetValueToLabel(lblOCV[nIndex], "0.00", _Constant.ColorOCV);
                        //}
                        //else
                        //{
                        //    SetValueToLabel(lblIR[nIndex], "0.0000", _Constant.ColorNoCell);
                        //    SetValueToLabel(lblOCV[nIndex], "0.00", _Constant.ColorNoCell);
                        //}
                    }
                }
            }
        }
        public void DisplayChannelInfo(int stageno, IROCVData irocvdata, enumEquipMode equipMode)
        {
            if(stageno == _iStage)
            {
                for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                {
                    if(irocvdata.IRRESULT[nIndex] == 1)
                    {
                        if(equipMode == enumEquipMode.OFFSET)
                        {
                            SetValueToLabel(lblIRMeasure[nIndex], irocvdata.IR_ORIGINALVALUE[nIndex].ToString("F4"), irocvdata.IRCOLOR[nIndex]);
                            SetValueToChart(nIndex, irocvdata.IR_ORIGINALVALUE[nIndex], IRCHART);
                        }
                        else
                        {
                            if(irocvdata.CELL[nIndex] == 1)
                            {
                                SetValueToLabel(lblIR[nIndex], irocvdata.IR_AFTERVALUE[nIndex].ToString("F4"), irocvdata.IRCOLOR[nIndex]);
                                SetValueToChart(nIndex, irocvdata.IR_AFTERVALUE[nIndex], IRCHART);
                            }
                        }
                        
                    }
                    if (irocvdata.OCVRESULT[nIndex] == 1)
                    {
                        SetValueToLabel(lblOCV[nIndex], irocvdata.OCV[nIndex].ToString("F2"), irocvdata.OCVCOLOR[nIndex]);
                        SetValueToChart(nIndex, irocvdata.OCV[nIndex], OCVCHART);
                    }

                }
            }
        }
        public void DisplayChannelInfo(int channel, int stageno, IROCVData irocvdata, enumEquipMode equipMode)
        {
            if (stageno == _iStage)
            {
                for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                {
                    if ((nIndex == channel) && irocvdata.IRRESULT[nIndex] == 1)
                    {
                        if (equipMode == enumEquipMode.OFFSET)
                        {
                            SetValueToLabel(lblIRMeasure[nIndex], irocvdata.IR_ORIGINALVALUE[nIndex].ToString("F4"), irocvdata.IRCOLOR[nIndex]);
                            SetValueToChart(nIndex, irocvdata.IR_ORIGINALVALUE[nIndex], IRCHART);
                        }
                        else
                        {
                            if (irocvdata.CELL[nIndex] == 1)
                            {
                                SetValueToLabel(lblIR[nIndex], irocvdata.IR_AFTERVALUE[nIndex].ToString("F4"), irocvdata.IRCOLOR[nIndex]);
                                SetValueToChart(nIndex, irocvdata.IR_AFTERVALUE[nIndex], IRCHART);
                            }
                        }

                    }
                    if ((nIndex == channel) && irocvdata.OCVRESULT[nIndex] == 1)
                    {
                        SetValueToLabel(lblOCV[nIndex], irocvdata.OCV[nIndex].ToString("F2"), irocvdata.OCVCOLOR[nIndex]);
                        SetValueToChart(nIndex, irocvdata.OCV[nIndex], OCVCHART);
                    }
                }
            }
        }
        public void DisplayOffsetAverage(int stageno, double[] offsetvalue)
        {
            if (stageno == _iStage)
            {
                for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                {
                    SetValueToLabel(lblIRMeasure[nIndex], offsetvalue[nIndex].ToString("F4"));
                    SetValueToChart(nIndex, offsetvalue[nIndex], IRCHART);
                }
            }
        }
        private void SetValueToLabel(Label lbl, string value)
        {
            if (lbl.InvokeRequired)
            {
                // 작업쓰레드인 경우
                lbl.BeginInvoke(new Action(() => lbl.Text = value));
            }
            else
            {
                // UI 쓰레드인 경우
                lbl.Text = value;
            }
        }
        private void SetValueToLabel(Label lbl, string value, Color clr)
        {
            if (lbl.InvokeRequired)
            {
                // 작업쓰레드인 경우
                lbl.BeginInvoke(new Action(() => lbl.Text = value));
                lbl.BackColor = clr;
            }
            else
            {
                // UI 쓰레드인 경우
                lbl.Text = value;
                lbl.BackColor = clr;
            }
        }
        #endregion

        #region Make Panel - ir/ocv values
        private void MakeLabelTitle()
        {
            int nx = LABEL_TITLE_LEFT;
            int ny = LABEL_TITLE_TOP;
            int width = LABEL_TITLE_WIDTH;
            int height = LABEL_TITLE_HEIGHT;
            for (int nIndex = 0; nIndex < 32;)
            {
                Label lbl = new Label();
                SetLabelOption_Title(lbl, width, height, nx, ny, (nIndex + 1).ToString(), nIndex);

                nIndex = nIndex + 1;
                nx = nx + lbl.Width + 2;
                if (nIndex % 4 == 0) nx += 2;
                if (nIndex % 16 == 0)
                {
                    ny = ny + LABEL_TITLE_HEIGHT + (LABEL_DATA_HEIGHT * 2) + 4;
                    nx = LABEL_TITLE_LEFT;
                }
            }
        }
        private void MakeLabelData()
        {
            int nx = LABEL_TITLE_LEFT;
            int ny = LABEL_TITLE_HEIGHT + 3;
            int width = LABEL_TITLE_WIDTH;
            int height = LABEL_DATA_HEIGHT;
            string label_text = string.Empty;
            for (int nIndex = 0; nIndex < _Constant.ChannelCount;)
            {
                label_text = (nIndex / 16 + 1).ToString() + " - " + (nIndex % 16 + 1).ToString();

                lblIR[nIndex] = new DoubleBufferedLabel();//new Label();
                SetLabelOption_Data(lblIR[nIndex], width, height, nx, ny, label_text, _Constant.ColorIR, nIndex);
                lblIR[nIndex].DoubleClick += new System.EventHandler(this.lblIR_Click);

                lblIRStandard[nIndex] = new DoubleBufferedLabel();//new Label();
                SetLabelOption_Data(lblIRStandard[nIndex], width, height, nx, ny, "0.000", _Constant.ColorStandard, nIndex);

                lblOCV[nIndex] = new DoubleBufferedLabel();//new Label();
                SetLabelOption_Data(lblOCV[nIndex], width, height, nx, ny + height + 1, label_text, _Constant.ColorOCV, nIndex);
                lblOCV[nIndex].DoubleClick += new System.EventHandler(this.lblIR_Click);

                lblIRMeasure[nIndex] = new DoubleBufferedLabel();//new Label();
                SetLabelOption_Data(lblIRMeasure[nIndex], width, height, nx, ny + height + 1, "0.000", _Constant.ColorMeasure, nIndex);
                lblIRMeasure[nIndex].DoubleClick += new System.EventHandler(this.lblIRMeasure_Click);

                nIndex = nIndex + 1;
                nx = nx + width + 2;
                if (nIndex % 4 == 0) nx += 2;
                if(nIndex % 16 == 0)
                {
                    ny = ny + LABEL_TITLE_HEIGHT + (LABEL_DATA_HEIGHT * 2) + 4; 
                    nx = LABEL_TITLE_LEFT;
                }
            }
        }

        private void lblIR_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            int channel_no = int.Parse(lbl.Tag.ToString());
            tbChannelNo.Text = (channel_no + 1).ToString();
            tbOffsetChannel.Text = (channel_no + 1).ToString();
        }

        private void lblIRMeasure_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            int channel_no = int.Parse(lbl.Tag.ToString());
            _iOffsetChannel = channel_no;

            radPnlChangeOffset.Visible = true;
            radPnlChangeOffset.BringToFront();
            tbMeasureIR.Text = lbl.Text;
            SetValueToLabel(lblChangeOffsetTitle, "Change Value - Ch : " + (channel_no + 1));
        }

        private void SetLabelOption_Title(Label lbl, int width, int height, int left, int top, string text, int index)
        {
            lbl.Visible = true;
            lbl.BackColor = Color.DarkGray;
            lbl.Width = width;
            lbl.Height = height;
            lbl.Font = new Font("Arial", 18, FontStyle.Bold);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Text = text;
            pBase.Controls.Add(lbl);
            lbl.Left = left;
            lbl.Top = top;
        }
        private void SetLabelOption_Data(Label lbl, int width, int height, int left, int top, string text, Color clr, int index)
        {
            lbl.Visible = true;
            lbl.BackColor = clr;
            lbl.Width = width;
            lbl.Height = height;
            lbl.Font = new Font("Arial", 12, FontStyle.Bold);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Text = text;
            pBase.Controls.Add(lbl);
            lbl.Left = left;
            lbl.Top = top;
            lbl.Tag = index;
        }
        public void ShowMeasureValues()
        {
            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                lblIR[nIndex].BringToFront();
                lblOCV[nIndex].BringToFront();
            }

            lblData1.Text = lblData3.Text = "IR";
            lblData2.Text = lblData4.Text = "OCV";

            lblData1.BackColor = lblData3.BackColor = Color.Wheat;
            lblData2.BackColor = lblData4.BackColor = Color.White;

            lblData1.Font = lblData2.Font = lblData3.Font = lblData4.Font = new Font("Arial", 10, FontStyle.Bold);
        }
        public void ShowOffsetValues()
        {
            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                lblIRStandard[nIndex].BringToFront();
                lblIRMeasure[nIndex].BringToFront();
            }

            lblData1.Text = lblData3.Text = "STANDARD";
            lblData2.Text = lblData4.Text = "MEASURE";

            lblData1.BackColor = lblData3.BackColor = Color.FromArgb(144, 202, 249);
            lblData2.BackColor = lblData4.BackColor = Color.GhostWhite;

            lblData1.Font = lblData2.Font = lblData3.Font = lblData4.Font = new Font("Arial", 8, FontStyle.Bold);
        }
        #endregion

        #region chart
        private void InitIRChart()
        {
            IRCHART.Series[0].ChartType = SeriesChartType.Line;
            IRCHART.Series[0].Name = "IR CHART";
            IRCHART.Series[0].XValueType = ChartValueType.Int32;
            IRCHART.Series[0].IsVisibleInLegend = false;

            for(int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                IRCHART.Series[0].Points.AddXY(nIndex + 1, -1);
            }

            IRCHART.ChartAreas[0].AxisX.Interval = 1;
            IRCHART.ChartAreas[0].AxisX.Minimum = 1;
            IRCHART.ChartAreas[0].AxisX.Maximum = 32;

            IRCHART.ChartAreas[0].AxisY.Interval = 0.03;
            IRCHART.ChartAreas[0].AxisY.Minimum = 0.2;
            IRCHART.ChartAreas[0].AxisY.Maximum = 0.6;

        }
        private void InitOCVChart()
        {
            OCVCHART.Series[0].ChartType = SeriesChartType.Line;
            OCVCHART.Series[0].Name = "OCV CHART";
            OCVCHART.Series[0].XValueType = ChartValueType.Int32;
            OCVCHART.Series[0].IsVisibleInLegend = false;

            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                OCVCHART.Series[0].Points.AddXY(nIndex + 1, -1);
            }

            OCVCHART.ChartAreas[0].AxisX.Interval = 1;
            OCVCHART.ChartAreas[0].AxisX.Minimum = 1;
            OCVCHART.ChartAreas[0].AxisX.Maximum = 32;

            OCVCHART.ChartAreas[0].AxisY.Interval = 320;
            OCVCHART.ChartAreas[0].AxisY.Minimum = 1000;
            OCVCHART.ChartAreas[0].AxisY.Maximum = 4200;

        }
        private void SetValueToChart(int channel, double value, Chart chart)
        {
            if(chart.InvokeRequired)
            {
                MethodInvoker action = delegate { chart.Series[0].Points[channel].YValues[0] = value; chart.Invalidate(); };
                chart.Invoke(action);
            }
            else
            {
                chart.Series[0].Points[channel].YValues[0] = value;
                chart.Invalidate();
            }

        }
        private void ClearChart(Chart chart)
        {
            //for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            //{
            //    chart.Series[0].Points.RemoveAt(0);
            //}
                
            //chart.Series[0].Points.AddXY(0, -1);

            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                chart.Series[0].Points[nIndex].YValues[0] = -1;
            }
        }
        public void InitChart()
        {
            ClearChart(IRCHART);
            ClearChart(OCVCHART);
        }

        #endregion

        #region Control Event
        private void radbtn_Init_Click(object sender, EventArgs e)
        {
            InitData(this._iStage);
            RaiseOnInitData(this._iStage); 
        }
        private void radbtn_ProbeOpen_Click(object sender, EventArgs e)
        {
            RaiseOnProbeOpen(this._iStage);
        }
        private void radbtn_ProbeClose_Click(object sender, EventArgs e)
        {
            RaiseOnProbeClose(this._iStage);
        }
        private void radbtn_MsaStart_Click(object sender, EventArgs e)
        {
            ShowMeasureValues();
            InitData(this._iStage);

            int count = Convert.ToInt32(tbMsaCount.Text);
            RaiseOnMsaStart(this._iStage, count);
        }
        private void radbtn_MsaStop_Click(object sender, EventArgs e)
        {
            ShowMeasureValues();
            RaiseOnMsaStop(this._iStage);
        }
        private void radbtn_AMSSTART_Click(object sender, EventArgs e)
        {
            //InitData(this._iStage);
            RaiseOnInitData(this._iStage);
            RaiseOnAmsStart(this._iStage);
        }
        private void radbtn_AMSSTOP_Click(object sender, EventArgs e)
        {
            RaiseOnAmsStop(this._iStage);
        }
        private void radbtn_IR_Click(object sender, EventArgs e)
        {
            int channel = Convert.ToInt32(tbChannelNo.Text);
            RaiseOnIRFetch(this._iStage, channel);
        }
        private void radbtn_OCV_Click(object sender, EventArgs e)
        {
            int channel = Convert.ToInt32(tbChannelNo.Text);
            RaiseOnOCVFetch(this._iStage, channel);
        }
        private void radBtnMeasure_Click(object sender, EventArgs e)
        {
            ShowMeasureValues();
        }
        private void radBtnOffset_Click(object sender, EventArgs e)
        {
            ShowOffsetValues();
        }
        private void radBtnAllOffset_Click(object sender, EventArgs e)
        {
            ShowOffsetValues();
            InitData(this._iStage);

            int count = Convert.ToInt32(tbRepeatCount.Text);
            RaiseOnOffsetStart(this._iStage, count);
        }
        private void radBtnStopAllOffset_Click(object sender, EventArgs e)
        {
            ShowOffsetValues();
            RaiseOnOffsetStop(this._iStage);
        }
        private void radBtnApplyOffset_Click(object sender, EventArgs e)
        {
            ShowOffsetValues();

            string[] strOffset;
            double[] offset;
            if (SetOffsetValue(out strOffset, out offset) == true)
                RaiseOnOffsetApply(this._iStage, strOffset, offset);
            else
                MessageBox.Show("There are more than 1 zero value in measure field.", "Apply offset Error.");
        }
        private void radBtnOffsetIR_Click(object sender, EventArgs e)
        {
            int channel = Convert.ToInt32(tbOffsetChannel.Text);
            RaiseOnOffsetCmdIr(this._iStage, channel);
        }
        private void radBtnOffsetOpen_Click(object sender, EventArgs e)
        {
            ShowOffsetValues();

            RaiseOnOffsetOpen(this._iStage);
        }
        private void radBtnOffsetSave_Click(object sender, EventArgs e)
        {
            ShowOffsetValues();

            string[] strOffset;
            double[] offset;
            SetOffsetValue(out strOffset, out offset);

            RaiseOnOffsetSave(this._iStage, strOffset);
        }
        #endregion

        private void radBtnOK_Click(object sender, EventArgs e)
        {
            lblIRMeasure[_iOffsetChannel].Text = tbMeasureIR.Text;
            radPnlChangeOffset.Visible = false;
        }

        public void ChangeIROCVMeasureInfoFormLanguage(enumLanguage enumLanguageType)
        {
            radlbl_manualmode.Text = StrLang.MANUALMODE;
            lblManualTitle.Text = StrLang.LBLMANUALTITLE;
            radbtn_AMSSTART.Text = StrLang.AMSSTART;
            radbtn_AMSSTOP.Text = StrLang.AMSSTOP;
            radbtn_Init.Text = StrLang.INITDATA;
            lblTraySettingTitle.Text = StrLang.LBLTRAYSETTING;
            radbtn_ProbeClose.Text = StrLang.BTNTRAYUP;
            radbtn_ProbeOpen.Text = StrLang.BTNTRAYDOWN;
            radBtnSaveData.Text = StrLang.BTNSAVEDATA;
            radbtn_MsaStart.Text = StrLang.BTNMSASTART;
            radbtn_MsaStop.Text = StrLang.BTNMSASTOP;
        }
    }
}
