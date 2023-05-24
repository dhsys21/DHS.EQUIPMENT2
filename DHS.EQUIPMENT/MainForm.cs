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

            //* PLC INTERFACE
            plcForm = PLCINTERFACE.GetInstance();

            //* Config Form
            configForm = new ConfigForm();
            configForm.OnSaveButtonClick += _CONFIGFORM_SaveConfigure;

            //* IROCV FORM 추가
            AddEquipPanel();

            //* Connection Status
            _tmrConnectionChange.Interval = 1000;
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
            SetConnectionStatus();
        }

        private void SetConnectionStatus()
        {
            if(_EQProcess.IROCVCONNECTED[0] == true)
            {
                if(lblIROCVConnection.BackColor != Color.Lime)
                    SetColorToLabel(lblIROCVConnection, Color.Lime);
            }
            else
            {
                if (lblIROCVConnection.BackColor != Color.Red)
                    SetColorToLabel(lblIROCVConnection, Color.Red);
            }

            if(_EQProcess.PLCCONNECTED == true)
            {
                if(lblPLCConnection.BackColor != Color.Lime)
                    SetColorToLabel(lblPLCConnection, Color.Lime);
            }
            else
            {
                if (lblPLCConnection.BackColor != Color.Red)
                    SetColorToLabel(lblPLCConnection, Color.Red);
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
