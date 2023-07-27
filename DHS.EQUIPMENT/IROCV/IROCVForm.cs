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
        }

        #region IROCV ACTION
        public void SetTrayId(string trayid)
        {
            SetValueToLabel(lblTrayId, trayid);
            SetValueToTextBox(tbTrayId, trayid);
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
            lblRecipeTitle.Text = StrLang.LBLRECIPE;
            lblIrRangeTitle.Text = StrLang.LBLIRRANGE;
            lblOcvRangeTitle.Text = StrLang.LBLOCVRANGE;
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

        #region Channel Info Panel
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
        public void SetStageStatus(enumEquipStatus equipstatus, bool _bPlcConnected, int _iPLCAUTOMANUAL, bool _bMesConnected)
        {
            switch (equipstatus)
            {
                case enumEquipStatus.StepVacancy:
                    pictureStatus.Image = Properties.Resources.vacancy;
                    SetValueToLabel(lblStatus, "IR/OCV is ready... ");
                    break;
                case enumEquipStatus.StepTrayIn:
                    pictureStatus.Image = Properties.Resources.TrayIn;
                    SetValueToLabel(lblStatus, "IR/OCV Tray In ...");
                    break;
                case enumEquipStatus.StepReady:
                    pictureStatus.Image = Properties.Resources.Ready;
                    SetValueToLabel(lblStatus, "Tray Up ...");
                    break;
                case enumEquipStatus.StepRun:
                    pictureStatus.Image = Properties.Resources.Run;
                    SetValueToLabel(lblStatus, "IR/OCV is runnig ...");
                    break;
                case enumEquipStatus.StepEnd:
                    pictureStatus.Image = Properties.Resources.End;
                    SetValueToLabel(lblStatus, "IR/OCV Measurement is finish ...");
                    break;
                case enumEquipStatus.StepTrayOut:
                    pictureStatus.Image = Properties.Resources.TrayOut;
                    SetValueToLabel(lblStatus, "IR/OCV Tray Out ...");
                    break;
                case enumEquipStatus.StepManual:
                    pictureStatus.Image = Properties.Resources.Local;
                    SetValueToLabel(lblStatus, "IR/OCV is Manual Mode");
                    //pictureStatus.Image = Image.FromFile(Application.StartupPath + @"\Image\" + "in_tray.png");
                    break;
                case enumEquipStatus.StepNoAnswer:
                    pictureStatus.Image = Properties.Resources.NOANSWER;
                    SetValueToLabel(lblStatus, "IR/OCV is not connected", Color.Red);
                    break;

                default:
                    break;
            }

            //* 2023 07 25 순서 PLC 연결 -> MES 연결 -> PLC 매뉴얼
            if (_bPlcConnected == false)
            {
                SetValueToLabel(lblStatus, "PLC is not connected", Color.Red);
            }
            //* 2023 07 25 아직 MES 연결안되어서 주석처리함
            //else if(_bMesConnected == false)
            //{
            //    SetValueToLabel(lblStatus, "MES is not connected", Color.Red);
            //}
            else if(_iPLCAUTOMANUAL == 0)
            {
                SetValueToLabel(lblStatus, "PLC is Manual Mode", Color.Red);
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
        #endregion

        private void radBtnConfig_Click(object sender, EventArgs e)
        {
            //configForm.ShowDialog();
            //configForm.STAGENO = this.stageno;
            RaiseOnConfigForm(stageno);
        }

        private void radbtn_RESET_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you want to reset IR/OCV ?", "RESET IR/OCV", MessageBoxButtons.YesNo);
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
    }
    
}
