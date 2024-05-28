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

namespace DHS.EQUIPMENT
{
    public partial class PLCINTERFACE : Form
    {
        private static PLCINTERFACE plcform;
        DataGridView[] dgvPCs = new DataGridView[_Constant.frmCount];
        DataGridView[] dgvPLCs = new DataGridView[_Constant.frmCount];

        public delegate void WritePLCClick(int stageno, string tagname, int nValue);
        public event WritePLCClick OnWritePLCClick = null;
        protected void RaiseOnWritePLC(int stageno, string tagname, int nValue)
        {
            if (OnWritePLCClick != null)
            {
                OnWritePLCClick(stageno, tagname, nValue);
            }
        }
        public static PLCINTERFACE GetInstance()
        {
            if (plcform == null) plcform = new PLCINTERFACE();
            return plcform;
        }
        public PLCINTERFACE()
        {
            InitializeComponent();

            plcform = this;
            this.Width = 1000;
            radpnl_PLCTEST.Visible = false;
            //this.Width = 1380;

            dgvPCs[0] = dgvPC1;
            dgvPLCs[0] = dgvPLC1;
            MakeGridView();
        }

        private void MakeGridView()
        {
            for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
            {
                #region PLC
                //* 열 추가
                dgvPLCs[nIndex].Columns.Add("001", "PLC Address");
                dgvPLCs[nIndex].Columns.Add("002", "PLC Name");
                dgvPLCs[nIndex].Columns.Add("003", "PLC Value");

                dgvPLCs[nIndex].Columns[0].Width = 104;
                dgvPLCs[nIndex].Columns[1].Width = 170;
                dgvPLCs[nIndex].Columns[2].Width = 156;

                //* 행 추가
                dgvPLCs[nIndex].Rows.Add(16 + 30); //* PLC SYS 정보 추가

                int nRowIndex = 0;
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_HEART_BEAT), "PLC HEART BEAT", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_ATUO_MANUAL), "PLC AUTO/MANUAL", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_ERROR), "PLC ERROR", nRowIndex++);
                
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_TRAY_IN), "PLC TRAY IN", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_TRAY_DOWN), "PLC TRAY DOWN", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_TRAY_UP), "PLC TRAY UP", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_JOB_CHANGE), "PLC JOB CHANGE", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_READY_COMPLETE), "PLC READY COMPLETE", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_UNLOADING_COMPLETE), "PLC UNLOAD COMPLETE", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, nRowIndex, "", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_TRAY_ID), "PLC TRAY ID", nRowIndex++);

                dgvPLCs[nIndex].ScrollBars = ScrollBars.Both;
                dgvPLCs[nIndex].PerformLayout();
                #endregion

                #region PLC SYSTEM INFO
                nRowIndex = 13;
                AddTitleGridView(dgvPLCs[nIndex], nIndex, 9999, "EQUIPMENT INFORMATION", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_INTERFACE_VERSION_PROJECT, "PLC INTERFACE VERSION", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_EQUIPMENT_NAME, "PLC EQUIPMENT NAME", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_EQUIPMENT_TYPE_ID, "EQUIPMENT TYPE ID", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_LINE_ID, "LINE ID", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_AREA_ID, "AREA ID", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_VENDOR_ID, "VENDOR ID", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_EQUIPMENT_ID, "EQUIPMENT ID", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STATE, "STATE", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_MODE, "MODE", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_BLOCKED, "BLOCKED", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STARVED, "STARVED", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_CURRNET_SPEED, "CURRENT SPEED", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_DESIGNED_SPEED, "DESIGN SPEED", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_TOTAL_COUNTER, "TOTAL COUNTER", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STAND_STILL_REASON, "STAND STILL REASON", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STACK_LIGHT0_COLOR, "STACK LIGHT 0 COLOR", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STACK_LIGHT0_COLOR, "STACK LIGHT 0 BEHAVIOR", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STACK_LIGHT1_COLOR, "STACK LIGHT 1 COLOR", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STACK_LIGHT1_COLOR, "STACK LIGHT 1 BEHAVIOR", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STACK_LIGHT2_COLOR, "STACK LIGHT 2 COLOR", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STACK_LIGHT2_COLOR, "STACK LIGHT 2 BEHAVIOR", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STACK_LIGHT3_COLOR, "STACK LIGHT 3 COLOR", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STACK_LIGHT3_COLOR, "STACK LIGHT 3 BEHAVIOR", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STACK_LIGHT4_COLOR, "STACK LIGHT 4 COLOR", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STACK_LIGHT4_COLOR, "STACK LIGHT 4 BEHAVIOR", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STACK_LIGHT5_COLOR, "STACK LIGHT 5 COLOR", nRowIndex++);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, _Constant.PLC_STACK_LIGHT5_COLOR, "STACK LIGHT 5 BEHAVIOR", nRowIndex++);
                #endregion

                #region PC
                dgvPCs[nIndex].Columns.Add("001", "PC Address");
                dgvPCs[nIndex].Columns.Add("002", "PC Name");
                dgvPCs[nIndex].Columns.Add("003", "PC Value");

                dgvPCs[nIndex].Columns[0].Width = 120;
                dgvPCs[nIndex].Columns[1].Width = 170;
                dgvPCs[nIndex].Columns[2].Width = 130;

                dgvPCs[nIndex].Rows.Add(16);

                nRowIndex = 0;
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_HEART_BEAT, "PC HEART BEAT", nRowIndex++);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_AUTO_MANUAL, "PC AUTO MANUAL", nRowIndex++);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_ERROR, "PC ERROR", nRowIndex++);
                
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_TRAY_OUT, "PC TRAY OUT", nRowIndex++);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_TRAY_DOWN, "PC TRAY DOWN", nRowIndex++);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_TRAY_UP, "PC TRAY UP", nRowIndex++);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_MEASUREMENT_WAIT, "PC MEASUREMENT WAIT", nRowIndex++);

                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_RUNNING, "PC RUNNING", nRowIndex++);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_MEASUREMENT_COMPLETE, "PC MEASUREMENT COMPLETE", nRowIndex++);

                dgvPCs[nIndex].ScrollBars = ScrollBars.Both;
                dgvPCs[nIndex].PerformLayout();
                #endregion
            }
        }
        private void AddTitleGridView(DataGridView dgv, int frmIndex, int iAddress, string sName, int nRow)
        {
            string sAddress = "";
            ////if (dgv.Name.Contains("dgvPLC")) sAddress = "MW" + (10000 + (iAddress * 2)).ToString();
            ////else if (dgv.Name.Contains("dgvPC")) sAddress = "MW" + (11000 + (iAddress * 2)).ToString();
            if (dgv.Name.Contains("dgvPLC")) sAddress = (10000 + iAddress).ToString();
            else if (dgv.Name.Contains("dgvPC")) sAddress = (11000 + iAddress).ToString();

            if (iAddress == 9999) sAddress = "Address";
            
            dgv.Rows[nRow].Cells[0].Value = sAddress;
            dgv.Rows[nRow].Cells[1].Value = sName;
        }

        private void AddDataGridView(DataGridView dgv, int iAddress, int[] pData, int nRow)
        {
            dgv.Rows[nRow].Cells[2].Value = pData[iAddress].ToString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dgv">DataGridView 이름</param>
        /// <param name="iAddress"> 데이터의 시작 주소</param>
        /// <param name="pData">데이터</param>
        /// <param name="nRow">DataGridView의 열번호</param>
        private void AddDataGridView(DataGridView dgv, int iAddress, object[] pData, int nRow)
        {
            if(pData[iAddress] != null)
                dgv.Rows[nRow].Cells[2].Value = pData[iAddress].ToString();
        }
        private void AddDataGridView(DataGridView dgv, string value, int nRow)
        {
            dgv.Rows[nRow].Cells[2].Value = value;
        }
        public void SetDataToGrid(int[] pcData, int[] plcData, int[] plcSysData, int nIndex)
        {
            #region PLC DATA VIEW
            int nRowIndex = 0;
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_HEART_BEAT, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_ATUO_MANUAL, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_ERROR, plcData, nRowIndex++);
            
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_IN, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_DOWN, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_UP, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_JOB_CHANGE, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_READY_COMPLETE, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_UNLOADING_COMPLETE, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_ID, plcData, 9);
            #endregion

            #region PC DATA VIEW
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_HEART_BEAT, pcData, 0);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_AUTO_MANUAL, pcData, 1);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_ERROR, pcData, 2);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_TRAY_OUT, pcData, 3);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_TRAY_DOWN, pcData, 4);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_TRAY_UP, pcData, 5);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_MEASUREMENT_WAIT, pcData, 6);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_RUNNING, pcData, 7);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_MEASUREMENT_COMPLETE, pcData, 8);
            #endregion
        }
        /// <summary>
        /// nIndex => grid view에서 순서
        /// </summary>
        public void SetDataToGrid(object[] pcData, object[] plcData, object[] plcSysData, int nIndex)
        {
            #region PLC DATA VIEW
            int nRowIndex = 0;
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_HEART_BEAT, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_ATUO_MANUAL, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_ERROR, plcData, nRowIndex++);

            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_IN, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_DOWN, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_UP, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_JOB_CHANGE, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_READY_COMPLETE, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_UNLOADING_COMPLETE, plcData, nRowIndex++);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_ID, plcData, nRowIndex++);
            #endregion

            #region PLC SYS Data VIEW
            nRowIndex = 14;
            for (int i = 0; i < plcSysData.Length; i++)
                AddDataGridView(dgvPLCs[nIndex], i, plcSysData, nRowIndex++);
            //AddDataGridView(dgvPLCs[nIndex], 0, plcSysData, nRowIndex++);
            //AddDataGridView(dgvPLCs[nIndex], 1, plcSysData, nRowIndex++);
            //AddDataGridView(dgvPLCs[nIndex], 2, plcSysData, nRowIndex++);
            //AddDataGridView(dgvPLCs[nIndex], 3, plcSysData, nRowIndex++);
            //AddDataGridView(dgvPLCs[nIndex], 4, plcSysData, nRowIndex++);
            //AddDataGridView(dgvPLCs[nIndex], 5, plcSysData, nRowIndex++);
            //AddDataGridView(dgvPLCs[nIndex], 6, plcSysData, nRowIndex++);
            #endregion

            #region PC DATA VIEW
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_HEART_BEAT, pcData, 0);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_AUTO_MANUAL, pcData, 1);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_ERROR, pcData, 2);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_TRAY_OUT, pcData, 3);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_TRAY_DOWN, pcData, 4);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_TRAY_UP, pcData, 5);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_MEASUREMENT_WAIT, pcData, 6);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_RUNNING, pcData, 7);
            AddDataGridView(dgvPCs[nIndex], _Constant.PC_MEASUREMENT_COMPLETE, pcData, 8);
            #endregion
        }
        public void SetDataToGrid(string tagname, string value, int nIndex)
        {
            switch(tagname)
            {
                case "MW10000":
                    AddDataGridView(dgvPLCs[nIndex], value, _Constant.PLC_HEART_BEAT);
                    break;
                case "MW10002":
                    AddDataGridView(dgvPLCs[nIndex], value, _Constant.PLC_ATUO_MANUAL);
                    break;
                case "MW10004":
                    AddDataGridView(dgvPLCs[nIndex], value, _Constant.PLC_ERROR);
                    break;
                case "MW10006":
                    AddDataGridView(dgvPLCs[nIndex], value, _Constant.PLC_TRAY_IN);
                    break;
                case "MW10008":
                    AddDataGridView(dgvPLCs[nIndex], value, _Constant.PLC_TRAY_DOWN);
                    break;
                case "MW10010":
                    AddDataGridView(dgvPLCs[nIndex], value, _Constant.PLC_TRAY_UP);
                    break;
                case "MW10012":
                    AddDataGridView(dgvPLCs[nIndex], value, _Constant.PLC_JOB_CHANGE);
                    break;
                case "MW10014":
                    AddDataGridView(dgvPLCs[nIndex], value, _Constant.PLC_READY_COMPLETE);
                    break;
                case "MW10016":
                    AddDataGridView(dgvPLCs[nIndex], value, _Constant.PLC_UNLOADING_COMPLETE);
                    break;
                case "MB10020":
                    AddDataGridView(dgvPLCs[nIndex], value, _Constant.PLC_TRAY_ID);
                    break;
                case "MW11000":
                    AddDataGridView(dgvPCs[nIndex], value, _Constant.PC_HEART_BEAT);
                    break;
                case "MW11002":
                    AddDataGridView(dgvPCs[nIndex], value, _Constant.PC_AUTO_MANUAL);
                    break;
                case "MW11004":
                    AddDataGridView(dgvPCs[nIndex], value, _Constant.PC_ERROR);
                    break;
                case "MW11006":
                    AddDataGridView(dgvPCs[nIndex], value, _Constant.PC_TRAY_OUT);
                    break;
                case "MW11008":
                    AddDataGridView(dgvPCs[nIndex], value, _Constant.PC_TRAY_DOWN);
                    break;
                case "MW11010":
                    AddDataGridView(dgvPCs[nIndex], value, _Constant.PC_TRAY_UP);
                    break;
                case "MW11012":
                    AddDataGridView(dgvPCs[nIndex], value, _Constant.PC_MEASUREMENT_WAIT);
                    break;
                case "MW11014":
                    AddDataGridView(dgvPCs[nIndex], value, _Constant.PC_RUNNING);
                    break;
                case "MW11016":
                    AddDataGridView(dgvPCs[nIndex], value, _Constant.PC_MEASUREMENT_COMPLETE);
                    break;
                default:
                    break;
            }
        }

        private void radpnl_PlcInterfaceTitle_Click(object sender, EventArgs e)
        {
            radpnl_PLCTEST.Visible = !radpnl_PLCTEST.Visible;
            if (radpnl_PLCTEST.Visible == true) this.Width = 1380;
            else this.Width = 1000;
        }

        private void radBtnWriteValue_Click(object sender, EventArgs e)
        {
            //string tagname = tbTagName.Text;

            string tagname = cbPCAddress.Text;
            string tagvalue = tbTagValue.Text;
            //ushort val = Convert.ToUInt16(tagvalue);
            //byte val = Convert.ToByte(tagvalue);
            //SIEMENSS7LIB.WritePLC(tagname, val);
            int val = Convert.ToInt32(tagvalue);

            RaiseOnWritePLC(0, tagname, val);
        }

        private void PLCINTERFACE_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
