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
    public partial class MESINTERFACE : Form
    {
        private static MESINTERFACE mesform;
        DataGridView[] dgvPCs = new DataGridView[_Constant.frmCount];
        DataGridView[] dgvMESs = new DataGridView[_Constant.frmCount];

        public static MESINTERFACE GetInstance()
        {
            if (mesform == null) mesform = new MESINTERFACE();
            return mesform;
        }
        public MESINTERFACE()
        {
            InitializeComponent();

            mesform = this;
            this.Width = 1000;
            radpnl_MESTEST.Visible = false;

            dgvPCs[0] = dgvPC;
            dgvMESs[0] = dgvMES;
            MakeGridView();
        }
        private void MakeGridView()
        {
            dgvMESs[0].Rows.Add(70);
            dgvMESs[0].Rows[0].Cells[0].Value = "SequenceNo";
            dgvMESs[0].Rows[1].Cells[0].Value = "AcknowledgeNo";
            dgvMESs[0].Rows[2].Cells[0].Value = "EquipmentID";
            dgvMESs[0].Rows[3].Cells[0].Value = "TrayID";
            dgvMESs[0].Rows[4].Cells[0].Value = "RecipeID";
            dgvMESs[0].Rows[5].Cells[0].Value = "Bypass";

            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                dgvMESs[0].Rows[nIndex + 6].Cells[0].Value = "CellID " + (nIndex + 1).ToString("D2");
                dgvMESs[0].Rows[nIndex + 32 + 6].Cells[0].Value = "CellStatus " + (nIndex + 1).ToString("D2");
            }


            dgvPCs[0].Rows.Add(133);
            dgvPCs[0].Rows[0].Cells[0].Value = "SequenceNo";
            dgvPCs[0].Rows[1].Cells[0].Value = "AcknowledgeNo";
            dgvPCs[0].Rows[2].Cells[0].Value = "EquipmentID";
            dgvPCs[0].Rows[3].Cells[0].Value = "TrayID";
            dgvPCs[0].Rows[4].Cells[0].Value = "RecipeID";

            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                dgvPCs[0].Rows[nIndex + 5].Cells[0].Value = "CellID " + (nIndex + 1).ToString("D2");
                dgvPCs[0].Rows[nIndex + 32 + 5].Cells[0].Value = "IR " + (nIndex + 1).ToString("D2");
                dgvPCs[0].Rows[nIndex + 64 + 5].Cells[0].Value = "OCV " + (nIndex + 1).ToString("D2");
                dgvPCs[0].Rows[nIndex + 96 + 5].Cells[0].Value = "RESULT " + (nIndex + 1).ToString("D2");
            }
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
        private void AddDataGridView(DataGridView dgv, string value, int nRow)
        {
            dgv.Rows[nRow].Cells[1].Value = value;
        }
        private void AddDataGridView(DataGridView dgv, string[] pData, int nRow)
        {
            for(int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                if (pData[nIndex] != null)
                    dgv.Rows[nRow++].Cells[1].Value = pData[nIndex].ToString();
            }
        }
        private void AddDataGridView(DataGridView dgv, int[] pData, int nRow)
        {
            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                dgv.Rows[nRow++].Cells[1].Value = pData[nIndex].ToString();
            }
        }
        private void radpnl_MesInterfaceTitle_Paint(object sender, PaintEventArgs e)
        {
            radpnl_MESTEST.Visible = !radpnl_MESTEST.Visible;
            if (radpnl_MESTEST.Visible == true) this.Width = 1380;
            else this.Width = 1000;
        }

        private void MESINTERFACE_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
