using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHS.EQUIPMENT.Common;
using Telerik.WinControls.UI;

namespace DHS.EQUIPMENT
{
    public partial class IROCVForm : Form
    {
        IROCVConfig configForm;
        IROCVData irocvdata;
        public int stageno;
        private static IROCVForm[] irocvForm = new IROCVForm[_Constant.frmCount];
        public static IROCVForm GetInstance(int nIndex)
        {
            if (irocvForm[nIndex] == null) irocvForm[nIndex] = new IROCVForm();
            return irocvForm[nIndex];
        }
        private Label[] lblProcess = new Label[12];

        #region Delegate
        //* IrocvProcess
        public delegate void GridViewCellMouseEnter(int channelno, int stageno);
        public event GridViewCellMouseEnter OnGridViewCellMouseEnter = null;
        protected void RaiseOnGridViewCellMouseEnter(int channelno, int stageno)
        {
            if (OnGridViewCellMouseEnter != null)
            {
                OnGridViewCellMouseEnter(channelno, stageno);
            }
        }

        public delegate void OperationMode(int stageno, bool bAuto);
        public event OperationMode OnOperationMode = null;
        protected void RaiseOnOperationMode(int stageno, bool bAuto)
        {
            if (OnOperationMode != null)
            {
                OnOperationMode(stageno, bAuto);
            }
        }

        public delegate void IROCVResetClick(int stageno);
        public event IROCVResetClick OnIROCVReset = null;
        protected void RaiseOnIROCVReset(int stageno)
        {
            if (OnIROCVReset != null)
            {
                OnIROCVReset(stageno);
            }
        }

        public delegate void NGInfoClick(int stageno);
        public event NGInfoClick OnNGInfo = null;
        protected void RaiseOnNGInfo(int stageno)
        {
            if (OnNGInfo != null)
            {
                OnNGInfo(stageno);
            }
        }

        public delegate void RemeasureInfoClick(int stageno);
        public event RemeasureInfoClick OnRemeasureInfo = null;
        protected void RaiseOnRemeasureInfo(int stageno)
        {
            if (OnRemeasureInfo != null)
            {
                OnRemeasureInfo(stageno);
            }
        }

        public delegate void ConfigFormClick(int stageno);
        public event ConfigFormClick OnConfigForm = null;
        protected void RaiseOnConfigForm(int stageno)
        {
            if (OnConfigForm != null)
            {
                OnConfigForm(stageno);
            }
        }
        #endregion

        public IROCVForm()
        {
            InitializeComponent();

            //* Config Form
            configForm = new IROCVConfig();

            //* make panel (측정 표시 판넬)
            makeGridView();
            initGridView();

            for(int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
            {
                //* irocv data
                irocvdata = IROCVData.GetInstance(nIndex);
            }

            //* Color 적용
            lblOCVNgColor.BackColor = _Constant.ColorOCVNG;
            lblIRNgColor.BackColor = _Constant.ColorIRNG;
            lblOutflowColor.BackColor = _Constant.ColorOutFlow;
            lblReadyColor.BackColor = _Constant.ColorReady;
            lblNoCellColor.BackColor = _Constant.ColorNoCell;

            //* Process 초기화
            lblProcess[0] = lblReady;
            lblProcess[1] = lblTrayIn;
            lblProcess[2] = lblBarcode;
            lblProcess[3] = lblRequestTrayInfo;
            lblProcess[4] = lblReplyTrayInfo;
            lblProcess[5] = lblProbeDown;
            lblProcess[6] = lblMeasure;
            lblProcess[7] = lblProbeOpen;
            lblProcess[8] = lblDataCollection;
            lblProcess[9] = lblDataCollectionReply;
            lblProcess[10] = lblFinish;
            lblProcess[11] = lblTrayOut;
        }

        #region IROCV ACTION
        public void SetTrayId(string trayid)
        {
            SetValueToTextBox(tbTrayId, trayid);
            SetValueToLabel(lblTrayId, trayid);
        }
        public void SetMesInfo(string traystatuscode, int errorcode, string errmessage)
        {
            SetValueToLabel2(lblTrayStatusCode, traystatuscode);
            SetValueToLabel2(lblErrorCode, errorcode.ToString());
            SetValueToLabel2(lblErrorMessage, errmessage);
        }
        public void SetControlMessage(string controlmessage)
        {
            SetValueToLabel2(radlblControlMessage, controlmessage);
        }
        public void SetIrSpec(double irmin, double irmax, double irremeamin, double irremeamax)
        {
            string irspec = string.Empty, irremeaspec = string.Empty;
            irspec = irmin.ToString() + " ~ " + irmax.ToString();
            irremeaspec = irremeamin.ToString() + " ~ " + irremeamax.ToString();
            SetValueToLabel(lblIrSpec, irspec);
            SetValueToLabel(lblIrRemeasureSpec, irremeaspec);
        }
        public void SetOcvSpec(double ocvmin, double ocvmax, double ocvremeamin, double ocvremeamax)
        {
            string ocvspec = string.Empty, ocvremeaspec = string.Empty;
            ocvspec = ocvmin.ToString() + " ~ " + ocvmax.ToString();
            ocvremeaspec = ocvremeamin.ToString() + " ~ " + ocvremeamax.ToString();
            SetValueToLabel(lblOcvSpec, ocvspec);
            SetValueToLabel(lblOcvRemeasureSpec, ocvremeaspec);
        }
        #endregion

        #region Method
        public void ChangeIROCVFormLanguage(enumLanguage enumLanguageType)
        {
            radbtn_AUTO.Text = StrLang.AUTO;
            radbtn_MANU.Text = StrLang.MANUAL;
            if(enumLanguageType == enumLanguage.Kor || enumLanguageType == enumLanguage.Eng)
                radbtn_MANU.Font = new Font("Arial", 14, FontStyle.Bold);
            else if(enumLanguageType == enumLanguage.Nor)
                radbtn_MANU.Font = new Font("Arial", 12, FontStyle.Bold);
            
            radbtn_RESET.Text = StrLang.RESET;
            radbtn_NGINFO.Text = StrLang.NGINFO;
            radBtnConfig.Text = StrLang.CONFIG;
            radbtn_TRAYOUT.Text = StrLang.TRAYOUT;

            lblReady.Text = StrLang.PNLREADY;
            lblTrayIn.Text = StrLang.PNLTRAYIN;
            lblBarcode.Text = StrLang.PNLBARCODE;
            lblMeasure.Text = StrLang.PNLMEASURE;
            lblFinish.Text = StrLang.PNLFINISH;
            lblProbeOpen.Text = StrLang.PNLTRAYDOWN;
            lblProbeDown.Text = StrLang.PNLTRAYUP;
            lblTrayOut.Text = StrLang.PNLTRAYOUT;

            lblStatusTitle.Text = StrLang.LBLSTATUS;
            lblTrayIdTitle.Text = StrLang.LBLTRAYID;
            lblProcessTitle.Text = StrLang.LBLPROCESS;
            lblIrRangeTitle.Text = StrLang.LBLIRRANGE;
            lblOcvRangeTitle.Text = StrLang.LBLOCVRANGE;
            lblIrRemeaTitle.Text = StrLang.LBLIRREMEASURE;
            lblOcvRemeaTitle.Text = StrLang.LBLOCVREMEASURE;
            lblErrorCodeTitle.Text = StrLang.LBLERRORCODE;
            lblErrorMessageTitle.Text = StrLang.LBLERRORMESSAGE;

            SetValueToLabel(lblStatus, StrLang.PLCCONNECTION, Color.Red);
            SetValueToLabel(lblStatus, StrLang.STEPVACANCY);
        }
        public void SetStageTitle(string stagename)
        {
            SetValueToLabel2(lblTitle, stagename);
        }
        public void SetIROCVConnectionStatus(enumConnectionState enumstatus)
        {
            if(enumstatus == enumConnectionState.Connected)
            {
                if(radpanel_connectionstatus.ForeColor != Color.Blue)
                {
                    SetValueToPanel(radpanel_connectionstatus, "IR/OCV is connected");
                    SetColorToPanel(radpanel_connectionstatus, Color.Blue);
                }
            }
            else
            {
                if (radpanel_connectionstatus.ForeColor != Color.Red)
                {
                    SetValueToPanel(radpanel_connectionstatus, "IR/OCV is not connected");
                    SetColorToPanel(radpanel_connectionstatus, Color.Red);
                }
            }

        }
        private void SetValueToPanel(RadPanel pnl, string value)
        {
            if (pnl.InvokeRequired)
            {
                // 작업쓰레드인 경우
                pnl.BeginInvoke(new Action(() => pnl.Text = value));
            }
            else
            {
                // UI 쓰레드인 경우
                pnl.Text = value;
            }
        }
        private void SetColorToPanel(RadPanel pnl, Color clr)
        {
            if (pnl.InvokeRequired)
            {
                // 작업쓰레드인 경우
                pnl.BeginInvoke(new Action(() => pnl.ForeColor = clr));
            }
            else
            {
                // UI 쓰레드인 경우
                pnl.ForeColor = clr;
            }
        }
        private void SetValueToLabel2(Label lbl, string value)
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
        private void SetValueToLabel2(RadLabel lbl, string value)
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
        private void SetValueToLabel(Label lbl, string value, Color? clr = null)
        {
            if (lbl.InvokeRequired)
            {
                // 작업쓰레드인 경우
                lbl.BeginInvoke(new Action(() => lbl.Text = value));
                lbl.BackColor = clr.HasValue ? clr.Value : Color.White;
                if (lbl.BackColor == Color.Red) lbl.ForeColor = Color.White;
                else if (lbl.BackColor == Color.White) lbl.ForeColor = Color.Black;
            }
            else
            {
                // UI 쓰레드인 경우
                lbl.Text = value;
                lbl.BackColor = clr.HasValue ? clr.Value : Color.White;
                if (lbl.BackColor == Color.Red) lbl.ForeColor = Color.White;
                else if (lbl.BackColor == Color.White) lbl.ForeColor = Color.Black;
            }
        }
        private void SetColorToLabel(Label lbl, Color clr)
        {
            if (lbl.InvokeRequired)
            {
                // 작업쓰레드인 경우
                lbl.BeginInvoke(new Action(() => lbl.BackColor = clr));
            }
            else
            {
                // UI 쓰레드인 경우
                lbl.BackColor = clr;
            }
        }
        private void SetValueToTextBox(TextBox txt, string value)
        {
            if (txt.InvokeRequired)
            {
                // 작업쓰레드인 경우
                txt.BeginInvoke(new Action(() => txt.Text = value));
            }
            else
            {
                // UI 쓰레드인 경우
                txt.Text = value;
            }
        }
        #endregion

        #region TRAY ID
        private void lblTrayId_DoubleClick(object sender, EventArgs e)
        {
            tbTrayId.Visible = true;
            tbTrayId.BringToFront();
        }

        private void tbTrayId_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                lblTrayId.Text = tbTrayId.Text;
                tbTrayId.Visible = false;
            }
        }
        #endregion

        #region Channel Info Panel - init
        private void makeGridView()
        {
            #region 행/열 제목, 갯수
            for (int nIndex = 0; nIndex < 16; nIndex++)
            {
                gridView.Columns.Add("CH" + nIndex.ToString("D2"), (nIndex + 1).ToString("D2"));
                gridView.Columns[nIndex].Width = 36;

            }

            gridView.Rows.Clear();
            gridView.Rows.Add(2);
            for (int i = 0; i < 2; i++)
            {
                gridView.Rows[i].Height = 22;
                /// Divider line
                if (i % 2 == 1)
                    gridView.Rows[i].DividerHeight = 2;
            }

            for (int i = 0; i < 32; i++)
            {
                gridView.Rows[i / 16].Cells[i % 16].Tag = i;
            }

            gridView.RowHeadersVisible = false;
            gridView.ColumnHeadersVisible = false;
            gridView.RowsDefaultCellStyle.BackColor = _Constant.ColorReady;
            gridView.AlternatingRowsDefaultCellStyle.BackColor = _Constant.ColorReady;
            gridView.ClearSelection();
            #endregion
        }
        public void initGridView()
        {
            gridView.RowsDefaultCellStyle.BackColor = _Constant.ColorReady;
            gridView.ClearSelection();

            for (int nIndex = 0; nIndex < 32; nIndex++)
            {
                gridView.Rows[nIndex / 16].Cells[nIndex % 16].Style.BackColor = _Constant.ColorReady;
            }
        }

        private void gridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            int tag;
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    var tagObj = gridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
                    tag = 0;
                    if (tagObj != null)
                        tag = int.Parse(tagObj.ToString());
                    DataGridViewCell cell = this.gridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

                    cell.ToolTipText = (tag + 1)
                        + " (" + (tag / 16 + 1).ToString() + " - " + (tag % 16 + 1).ToString() + ")";

                    lblChannel.Text = (tag + 1)
                        + " (" + (tag / 16 + 1).ToString() + " - " + (tag % 16 + 1).ToString() + ")";
                    //* 방법 1
                    //lblIR.Text = irocvdata.IR_AFTERVALUE[tag].ToString();
                    //lblOCV.Text = irocvdata.OCV[tag].ToString();

                    //* 방법 2
                    RaiseOnGridViewCellMouseEnter(tag, this.stageno);
                }
            }
            catch (Exception ex) { }
        }
        public void SetValueToLabel(double ir, double ocv)
        {
            lblIR.Text = ir.ToString();
            lblOCV.Text = ocv.ToString();
        }
        #endregion

        #region Stage Status Picture
        public void SetProcessStatus(enumProcess enumprocess)
        {
            for (int i = 0; i < 12; i++)
                SetColorToLabel(lblProcess[i], Color.DarkGray);

            for (int i = 0; i <= (int)enumprocess; i++)
                SetColorToLabel(lblProcess[i], Color.LightGreen);
        }
        public void SetStageStatus(enumEquipStatus equipstatus, bool _bPlcConnected, int _iPLCAUTOMANUAL, int _iPLCERROR, bool _bMesConnected)
        {
            switch (equipstatus)
            {
                case enumEquipStatus.StepVacancy:
                    pictureStatus.Image = Properties.Resources.vacancy;
                    //SetValueToLabel(lblStatus, "IR/OCV is ready ...");
                    SetValueToLabel(lblStatus, StrLang.STEPVACANCY);
                    break;
                case enumEquipStatus.StepTrayIn:
                    pictureStatus.Image = Properties.Resources.TrayIn;
                    //SetValueToLabel(lblStatus, "IR/OCV Tray In ...");
                    SetValueToLabel(lblStatus, StrLang.STEPTRAYIN);
                    break;
                case enumEquipStatus.StepReady:
                    pictureStatus.Image = Properties.Resources.Ready;
                    //SetValueToLabel(lblStatus, "IR/OCV Tray Up.");
                    SetValueToLabel(lblStatus, StrLang.STEPTRAYUP);
                    break;
                case enumEquipStatus.StepRun:
                    pictureStatus.Image = Properties.Resources.Run;
                    //SetValueToLabel(lblStatus, "IR/OCV is runnig.");
                    SetValueToLabel(lblStatus, StrLang.STEPRUNNING);
                    break;
                case enumEquipStatus.StepEnd:
                    pictureStatus.Image = Properties.Resources.End;
                    //SetValueToLabel(lblStatus, "IR/OCV Measurement is finish.");
                    SetValueToLabel(lblStatus, StrLang.STEPEND);
                    break;
                case enumEquipStatus.StepTrayOut:
                    pictureStatus.Image = Properties.Resources.TrayOut;
                    //SetValueToLabel(lblStatus, "IR/OCV Tray Out.");
                    SetValueToLabel(lblStatus, StrLang.STEPTRAYOUT);
                    break;
                case enumEquipStatus.StepManual:
                    pictureStatus.Image = Properties.Resources.Local;
                    SetValueToLabel(lblStatus, StrLang.IROCVMANUALMODE);
                    //SetValueToLabel(lblStatus, "IR/OCV is Manual Mode");
                    break;
                case enumEquipStatus.StepNoAnswer:
                    pictureStatus.Image = Properties.Resources.NOANSWER;
                    //SetValueToLabel(lblStatus, "IR/OCV is not connected", Color.Red);
                    SetValueToLabel(lblStatus, StrLang.IROCVCONNECTION, Color.Red);
                    break;
                default:
                    break;
            }

            //* 2023 07 25 순서 PLC 연결 -> MES 연결 -> PLC 매뉴얼 -> PLC Error
            if (_bPlcConnected == false)
            {
                //SetValueToLabel(lblStatus, "PLC is not connected", Color.Red);
                SetValueToLabel(lblStatus, StrLang.PLCCONNECTION, Color.Red);
            }
            //* 2023 07 25 아직 MES 연결안되어서 주석처리함
            else if (_bMesConnected == false)
            {
                SetValueToLabel(lblStatus, StrLang.MESCONNECTION, Color.Red);
            }
            else if(_iPLCAUTOMANUAL == 0)
            {
                SetValueToLabel(lblStatus, StrLang.PLCMANUALMODE, Color.Red);
            }
            else if(_iPLCERROR == 1)
            {
                SetValueToLabel(lblStatus, StrLang.PLCERROR, Color.Red);
            }

            
        }
        #endregion

        #region Operation Mode Buttons
        private void radbtn_MANU_Click(object sender, EventArgs e)
        {
            RaiseOnOperationMode(this.stageno, false);
        }

        private void radbtn_AUTO_Click(object sender, EventArgs e)
        {
            RaiseOnOperationMode(this.stageno, true);
        }
        private void radBtnConfig_Click(object sender, EventArgs e)
        {
            //configForm.ShowDialog();
            //configForm.STAGENO = this.stageno;
            RaiseOnConfigForm(stageno);
        }

        private void radbtn_RESET_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show(StrLang.IROCVRESETMSG, StrLang.IROCVRESETTITLE, MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                RaiseOnIROCVReset(this.stageno);
            }

        }

        private void radbtn_NGINFO_Click(object sender, EventArgs e)
        {
            RaiseOnNGInfo(this.stageno);
        }

        private void radbtn_TRAYOUT_Click(object sender, EventArgs e)
        {
            RaiseOnRemeasureInfo(this.stageno);
        }
        #endregion

    }
}
