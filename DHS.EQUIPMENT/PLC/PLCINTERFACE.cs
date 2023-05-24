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

                dgvPLCs[nIndex].Columns[0].Width = 119;
                dgvPLCs[nIndex].Columns[1].Width = 170;
                dgvPLCs[nIndex].Columns[2].Width = 156;

                //* 행 추가
                dgvPLCs[nIndex].Rows.Add(16);

                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_HEART_BEAT), "PLC HEART BEAT", 0);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_ATUO_MANUAL), "PLC AUTO/MANUAL", 1);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_ERROR), "PLC ERROR", 2);
                
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_TRAY_IN), "PLC TRAY IN", 3);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_TRAY_DOWN), "PLC TRAY DOWN", 4);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_TRAY_UP), "PLC TRAY UP", 5);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_JOB_CHANGE), "PLC JOB CHANGE", 6);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_READY_COMPLETE), "PLC READY COMPLETE", 7);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_UNLOADING_COMPLETE), "PLC UNLOAD COMPLETE", 8);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, 9, "", 9);
                AddTitleGridView(dgvPLCs[nIndex], nIndex, (_Constant.PLC_TRAY_ID), "PLC TRAY ID", 10);


                dgvPLCs[nIndex].ScrollBars = ScrollBars.Both;
                dgvPLCs[nIndex].PerformLayout();
                #endregion

                #region PC
                dgvPCs[nIndex].Columns.Add("001", "PC Address");
                dgvPCs[nIndex].Columns.Add("002", "PC Name");
                dgvPCs[nIndex].Columns.Add("003", "PC Value");

                dgvPCs[nIndex].Columns[0].Width = 120;
                dgvPCs[nIndex].Columns[1].Width = 170;
                dgvPCs[nIndex].Columns[2].Width = 130;

                dgvPCs[nIndex].Rows.Add(16);

                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_HEART_BEAT, "PC HEART BEAT", 0);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_AUTO_MANUAL, "PC AUTO MANUAL", 1);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_ERROR, "PC ERROR", 2);
                
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_TRAY_OUT, "PC TRAY OUT", 3);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_TRAY_DOWN, "PC TRAY DOWN", 4);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_TRAY_UP, "PC TRAY UP", 5);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_MEASUREMENT_WAIT, "PC MEASUREMENT WAIT", 6);

                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_RUNNING, "PC RUNNING", 7);
                AddTitleGridView(dgvPCs[nIndex], nIndex, _Constant.PC_MEASUREMENT_COMPLETE, "PC MEASUREMENT COMPLETE", 8);

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
            
            dgv.Rows[nRow].Cells[0].Value = sAddress;
            dgv.Rows[nRow].Cells[1].Value = sName;
        }

        private void AddDataGridView(DataGridView dgv, int iAddress, int[] pData, int nRow)
        {
            dgv.Rows[nRow].Cells[2].Value = pData[iAddress].ToString();
        }
        private void AddDataGridView(DataGridView dgv, int iAddress, object[] pData, int nRow)
        {
            if(pData[iAddress] != null)
                dgv.Rows[nRow].Cells[2].Value = pData[iAddress].ToString();
        }
        private void AddDataGridView(DataGridView dgv, string value, int nRow)
        {
            dgv.Rows[nRow].Cells[2].Value = value;
        }
        public void SetDataToGrid(int[] pcData, int[] plcData, int nIndex)
        {
            #region PLC DATA VIEW
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_HEART_BEAT, plcData, 0);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_ATUO_MANUAL, plcData, 1);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_ERROR, plcData, 2);
            
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_IN, plcData, 3);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_DOWN, plcData, 4);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_UP, plcData, 5);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_JOB_CHANGE, plcData, 6);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_READY_COMPLETE, plcData, 7);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_UNLOADING_COMPLETE, plcData, 8);
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
        public void SetDataToGrid(object[] pcData, object[] plcData, int nIndex)
        {
            #region PLC DATA VIEW
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_HEART_BEAT, plcData, 0);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_ATUO_MANUAL, plcData, 1);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_ERROR, plcData, 2);

            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_IN, plcData, 3);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_DOWN, plcData, 4);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_UP, plcData, 5);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_JOB_CHANGE, plcData, 6);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_READY_COMPLETE, plcData, 7);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_UNLOADING_COMPLETE, plcData, 8);
            AddDataGridView(dgvPLCs[nIndex], _Constant.PLC_TRAY_ID, plcData, 10);
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
