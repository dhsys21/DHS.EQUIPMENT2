using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using DHS.EQUIPMENT.Common;

namespace DHS.EQUIPMENT
{
    public partial class MainForm : Form
    {
        PLCINTERFACE plcForm;
        ConfigForm configForm;
        CEquipmentData _system;
        Util util = new Util();
        private IrocvProcess _EQProcess = null;
        public IROCVForm[] nForm = new IROCVForm[_Constant.frmCount];
        public IROCVMeasureInfoForm measureinfo;
        //* Error Form
        private ErrorForm _errorForm;

        public Timer _tmrConnectionChange = new Timer();

        public MainForm()
        {
            InitializeComponent();
            //* 실행시 윈도우즈 최대크기
            //this.WindowState = FormWindowState.Maximized;
            this.CenterToScreen();
            this.Width = 1920;
            this.Height = 1050;
            this.Left = 0;
            this.Top = 0;

            //* Title 색상 하얀색으로 - picturebox에 있는 label은 배경색이 투명이 안되기 때문에 코딩으로 처리
            radlbl_Title.Parent = pboxTitle;
            radlbl_Title.ForeColor = Color.White;

            //* Connection Status Panel 색상 하얀색으로 - 위와 동일
            pnlConnection.Parent = pboxTitle;
            pnlLanguage.Parent = pboxTitle;

            //* PLC INTERFACE
            plcForm = PLCINTERFACE.GetInstance();

            //* Config Form
            configForm = new ConfigForm();
            configForm.OnSaveButtonClick += _CONFIGFORM_SaveConfigure;

            //* Error Form
            _errorForm = ErrorForm.GetInstance();
            _errorForm.StartPosition = FormStartPosition.CenterScreen;

            //* IROCV FORM 추가
            AddEquipPanel();

            //* Connection Status
            _tmrConnectionChange.Interval = 2000;
            _tmrConnectionChange.Tick += new EventHandler(ConnectionChangeTimer_Tick);
            _tmrConnectionChange.Enabled = true;
        }

        private void _CONFIGFORM_SaveConfigure()
        {
            SaveConfigFile();

            ReadConfigFile();
        }

        private void ConnectionChangeTimer_Tick(object sender, EventArgs e)
        {
            //* 2023 07 25 PLC, IR/OCV, MES 연결이 안되면 PC_Error에 신호 ON
            //* 2023 07 25 이 에러를 에러창을 띄워서 처리하는 것으로 수정해야 함
            SetConnectionStatus();
        }

        string PcErrorMsg = string.Empty;
        private void SetConnectionStatus()
        {
            if(_EQProcess.IROCVCONNECTED[0] == true)
            {
                PcErrorMsg = "IROCV is connected.";
                _EQProcess.PLC_SETERROR(0, 0, PcErrorMsg, enumStageError.NoError);
                if (lblIROCVConnection.BackColor != Color.Lime)
                {
                    SetColorToLabel(lblIROCVConnection, Color.Lime);
                    //_EQProcess.PLC_SETERROR(0, 0, PcErrorMsg, enumStageError.NoError);
                }
                    
            }
            else
            {
                PcErrorMsg = "IROCV is not connected.";
                _EQProcess.PLC_SETERROR(0, 1, PcErrorMsg, enumStageError.IROCVDisconnected);
                if (lblIROCVConnection.BackColor != Color.Red)
                {
                    SetColorToLabel(lblIROCVConnection, Color.Red);
                    //_EQProcess.PLC_SETERROR(0, 1, PcErrorMsg, enumStageError.IROCVDisconnected);
                }
                    
            }

            if(_EQProcess.PLCCONNECTED == true)
            {
                PcErrorMsg = "PLC is connected.";
                if (lblPLCConnection.BackColor != Color.Lime)
                {
                    SetColorToLabel(lblPLCConnection, Color.Lime);
                    _EQProcess.PLC_SETERROR(0, 0, PcErrorMsg, enumStageError.NoError);
                }
                    
            }
            else
            {
                PcErrorMsg = "PLC is not connected.";
                if (lblPLCConnection.BackColor != Color.Red)
                {
                    SetColorToLabel(lblPLCConnection, Color.Red);
                    _EQProcess.PLC_SETERROR(0, 1, PcErrorMsg, enumStageError.PLCDisconnected);
                }
                    
            }

            if (_EQProcess.MESCONNECTED == true)
            {
                PcErrorMsg = "MES is connected.";
                if (lblMESConnection.BackColor != Color.Lime)
                {
                    SetColorToLabel(lblMESConnection, Color.Lime);
                    _EQProcess.PLC_SETERROR(0, 0, PcErrorMsg, enumStageError.NoError);
                }
                    
            }
            else
            {
                PcErrorMsg = "MES is not connected.";
                if (lblMESConnection.BackColor != Color.Red)
                {
                    SetColorToLabel(lblMESConnection, Color.Red);
                    _EQProcess.PLC_SETERROR(0, 1, PcErrorMsg, enumStageError.MESDisconnected);
                }
                    
            }
        }

        private void AddEquipPanel()
        {
            Panel[] nPanel = new DoubleBufferedPanel[_Constant.frmCount];
            Panel nPanel2 = new DoubleBufferedPanel();

            int nx = 10, ny = 5;

            for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
            {
                nForm[nIndex] = IROCVForm.GetInstance(nIndex);
                nForm[nIndex].stageno = nIndex;

                nPanel[nIndex] = new DoubleBufferedPanel();
                util.loadFormIntoPanel(nForm[nIndex], nPanel[nIndex]);
                BasePanel.Controls.Add(nPanel[nIndex]);
                nPanel[nIndex].Dock = DockStyle.Fill;


                measureinfo = IROCVMeasureInfoForm.GetInstance();
                measureinfo._iStage = nIndex;

                nPanel2 = new DoubleBufferedPanel();
                util.loadFormIntoPanel(measureinfo, nPanel2);
                MeasurePanel.Controls.Add(nPanel2);
                nPanel2.Dock = DockStyle.Fill;
                
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

        #region Delegate Event - _EQProcess
        private void __EQProcess_IROCVConnection(int stageno, enumConnectionState enumStatus)
        {
            if (enumStatus == enumConnectionState.Connected) SetColorToLabel(lblIROCVConnection, Color.Lime);
            else SetColorToLabel(lblIROCVConnection, Color.Red);
        }
        #endregion

        #region Control Event
        private void radbtn_Init_Click(object sender, EventArgs e)
        {
            _EQProcess.IROCV_Initialize(0);
            _EQProcess.PLC_Initialize(0);
        }
        private void radbtn_PLC_Click(object sender, EventArgs e)
        {
            try
            {
                plcForm.Show();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        private void MainForm_Load(object sender, EventArgs e)
        {
            _system = CEquipmentData.GetInstance();
            ReadConfigFile();

            _EQProcess = IrocvProcess.GetInstance();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var result = MessageBox.Show("Are you want to exit IR/OCV ?", "EXIT IR/OCV", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                if (_EQProcess != null) _EQProcess.close();
                //* Thread stop
                Process.GetCurrentProcess().Kill();
                // application stop
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void radbtn_Config_Click(object sender, EventArgs e)
        {
            configForm.ShowDialog();
        }

        #region Config Read / Save
        private void SaveConfigFile()
        {
            int stageno = 0;
            string filename = _Constant.BIN_PATH + "MainSystemInfo.inf";

            util.saveConfig(filename, "MES", "IPADDRESS", _system.MESIPADDRESS);
            util.saveConfig(filename, "MES", "PORT", _system.MESPORT.ToString());
            util.saveConfig(filename, "PLC", "IPADDRESS", _system.PLCIPADDRESS);
            util.saveConfig(filename, "PLC", "DB NUMBER", _system.PLCDBNUMBER.ToString());
        }
        private void ReadConfigFile()
        {
            string filename = _Constant.BIN_PATH + "MainSystemInfo.inf";
            try
            {
                if (File.Exists(filename))
                {
                    _system.MESIPADDRESS = util.readConfig(filename, "MES", "IPADDRESS");
                    _system.MESPORT = Convert.ToInt32(util.readConfig(filename, "MES", "PORT"));
                    _system.PLCIPADDRESS = util.readConfig(filename, "PLC", "IPADDRESS");
                    _system.PLCDBNUMBER = Convert.ToInt32(util.readConfig(filename, "PLC", "DB NUMBER"));

                    configForm.SetSystemValue();

                    SIEMENSS7LIB.ChangeSetting(_system.PLCIPADDRESS, _system.PLCDBNUMBER);
                }
                else
                {
                    configForm.Left = 1400;
                    configForm.Show();
                }
            }
            catch(Exception ex)
            {
                configForm.Left = 1400;
                configForm.Show();
                Console.WriteLine(ex.ToString());
            }
            
        }
        #endregion Config  Read / Save

        private void lblPLCConnection_Click(object sender, EventArgs e)
        {
            _errorForm.ShowMessage(enumStageError.IROCVDisconnected, 0);
            _errorForm.ShowMessage(enumStageError.IROCVNoResponse, 0);
        }

        private void lblLangKo_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("ko-KR");
            ChangeLanguage();
        }

        private void lblLangEn_Click(object sender, EventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");
            ChangeLanguage();
        }

        private void ChangeLanguage()
        {
            ChangeMainFormLanguage();
            for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
                nForm[nIndex].ChangeIROCVFormLanguage();
        }
        private void ChangeMainFormLanguage()
        {
            radbtn_Init.Text = StrLang.Initialize;
            radbtn_Config.Text = StrLang.CONFIG;
        }
    }

    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }
    }
    public class DoubleBufferedLabel : Label
    {
        public DoubleBufferedLabel()
        {
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }
    }
    public class DoubleBufferedText : TextBox
    {
        public DoubleBufferedText()
        {
            this.SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();
        }
    }
}
