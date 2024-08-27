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
using S7.Net;
using static DHS.EQUIPMENT.MesClient;

namespace DHS.EQUIPMENT
{
    public partial class MESINTERFACE : Form
    {
        private static MESINTERFACE mesform;
        DataGridView[] dgvPLCs = new DataGridView[_Constant.frmCount];
        DataGridView[] dgvMESs = new DataGridView[_Constant.frmCount];

        public delegate void WriteButtonClick(string node, string value, int nDataType);
        public event WriteButtonClick OnWriteButtonClick = null;
        protected void RaiseOnWriteMes(string node, string value, int nDataType)
        {
            if (OnWriteButtonClick != null)
            {
                OnWriteButtonClick(node, value, nDataType);
            }
        }
        #region MES 시뮬레이션
        public delegate void WriteForIR1(string equipmentid, string trayid);
        public event WriteForIR1 OnWriteForIR1 = null;
        protected void RaiseOnWriteForIR1(string equipmentid, string trayid)
        {
            if (OnWriteForIR1 != null)
            {
                OnWriteForIR1(equipmentid, trayid);
            }
        }
        public delegate void WriteForIR2(string equipmentid, string trayid, string[] cellid, string[] cellstatus, double[] ir, double[] ocv);
        public event WriteForIR2 OnWriteForIR2 = null;
        protected void RaiseOnWriteForIR2(string equipmentid, string trayid, string[] cellid, string[] cellstatus, double[] ir, double[] ocv)
        {
            if (OnWriteForIR2 != null)
            {
                OnWriteForIR2(equipmentid, trayid, cellid, cellstatus, ir, ocv);
            }
        }
        public delegate void WritePLCSysInfo(string tagname, string tagvalue);
        public event WritePLCSysInfo OnWritePLCSysInfo = null;
        protected void RaiseOnWritePLCSysInfo(string tagname, string tagvalue)
        {
            if (OnWritePLCSysInfo != null)
            {
                OnWritePLCSysInfo(tagname, tagvalue);
            }
        }
        public delegate void ReadForIR1(string[] cellid, string[] cellstatus, string traystatuscode, string errorcode, string errormessage);
        public event ReadForIR1 OnReadForIR1 = null;
        protected void RaiseOnReadForIR1(string[] cellid, string[] cellstatus, string traystatuscode, string errorcode, string errormessage)
        {
            if (OnReadForIR1 != null)
            {
                OnReadForIR1(cellid, cellstatus, traystatuscode, errorcode, errormessage);
            }
        }
        public delegate void ReadForIR2(string errorcode, string errormessage);
        public event ReadForIR2 OnReadForIR2 = null;
        protected void RaiseOnReadForIR2(string errorcode, string errormessage)
        {
            if (OnReadForIR2 != null)
            {
                OnReadForIR2(errorcode, errormessage);
            }
        }
        #endregion

        public static MESINTERFACE GetInstance()
        {
            if (mesform == null) mesform = new MESINTERFACE();
            return mesform;
        }
        public MESINTERFACE()
        {
            InitializeComponent();

            mesform = this;
            this.Width = 992;
            radpnl_MESTEST.Visible = false;

            dgvPLCs[0] = dgvPLC;
            dgvMESs[0] = dgvMES;
            MakeGridView();
        }

        private void MakeGridView()
        {
            //* 열 추가
            dgvMESs[0].Columns.Add("001", "MES TAG NAME");
            dgvMESs[0].Columns.Add("002", "MES TAG VALUE");
            dgvMESs[0].Columns[0].Width = 300;
            dgvMESs[0].Columns[1].Width = 145;

            //* 행 추가
            dgvMESs[0].Rows.Add(10);
            dgvMESs[0].Rows[0].Cells[0].Value = "FORIR 2.1 RequestTrayInfo";
            dgvMESs[0].Rows[1].Cells[0].Value = "   Acknowledge No.";
            dgvMESs[0].Rows[2].Cells[0].Value = "   Sequence No.";
            dgvMESs[0].Rows[3].Cells[0].Value = "FORIR 2.2 DataCollection";
            dgvMESs[0].Rows[4].Cells[0].Value = "   Acknowledge No.";
            dgvMESs[0].Rows[5].Cells[0].Value = "   Sequence No.";

            //* 열 추가
            dgvPLCs[0].Columns.Add("001", "PLC TAG NAME");
            dgvPLCs[0].Columns.Add("002", "PLC TAG VALUE");
            dgvPLCs[0].Columns[0].Width = 300;
            dgvPLCs[0].Columns[1].Width = 150;

            //* 행 추가
            dgvPLCs[0].Rows.Add(30);
            //* Equipment Information
            dgvPLCs[0].Rows[0].Cells[0].Value = "AreaID";
            dgvPLCs[0].Rows[1].Cells[0].Value = "EquipmentID";
            dgvPLCs[0].Rows[2].Cells[0].Value = "EquipmentName";
            dgvPLCs[0].Rows[3].Cells[0].Value = "EquipmentTypeID";
            dgvPLCs[0].Rows[4].Cells[0].Value = "InterfaceVersionProject";
            dgvPLCs[0].Rows[5].Cells[0].Value = "LineID";
            dgvPLCs[0].Rows[6].Cells[0].Value = "VendorID";

            //* Equipment Status
            dgvPLCs[0].Rows[7].Cells[0].Value = "Blocked";
            dgvPLCs[0].Rows[8].Cells[0].Value = "CurrentCycleTime";
            dgvPLCs[0].Rows[9].Cells[0].Value = "DefectCounter"; //* PLC에서 정보를 주지 않음. PC에서 정리해서 사용
            dgvPLCs[0].Rows[10].Cells[0].Value = "DesignCycleTime";
            dgvPLCs[0].Rows[11].Cells[0].Value = "GoodCounter"; //* PLC에서 정보를 주지 않음. PC에서 정리해서 사용
            dgvPLCs[0].Rows[12].Cells[0].Value = "Mode";
            dgvPLCs[0].Rows[13].Cells[0].Value = "Stacklight0Color";
            dgvPLCs[0].Rows[14].Cells[0].Value = "Stacklight0Behavior";
            dgvPLCs[0].Rows[15].Cells[0].Value = "Stacklight1Color";
            dgvPLCs[0].Rows[16].Cells[0].Value = "Stacklight1Behavior";
            dgvPLCs[0].Rows[17].Cells[0].Value = "Stacklight2Color";
            dgvPLCs[0].Rows[18].Cells[0].Value = "Stacklight2Behavior";
            dgvPLCs[0].Rows[19].Cells[0].Value = "Stacklight3Color";
            dgvPLCs[0].Rows[20].Cells[0].Value = "Stacklight3Behavior";
            dgvPLCs[0].Rows[21].Cells[0].Value = "Stacklight4Color";
            dgvPLCs[0].Rows[22].Cells[0].Value = "Stacklight4Behavior";
            dgvPLCs[0].Rows[23].Cells[0].Value = "Stacklight5Color";
            dgvPLCs[0].Rows[24].Cells[0].Value = "Stacklight5Behavior";
            dgvPLCs[0].Rows[25].Cells[0].Value = "StandstillReason";
            dgvPLCs[0].Rows[26].Cells[0].Value = "Starved";
            dgvPLCs[0].Rows[27].Cells[0].Value = "State";
            dgvPLCs[0].Rows[28].Cells[0].Value = "TotaCounter";
        }

        public void SetDataToGrid(string[] plcData, string[] mesData)
        {
            #region PLC DATA VIEW
            int nIndex = 0;
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //SequenceNo
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //AcknowledgeNo
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //EquipmentID
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //TrayID
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //TrayStatusCode
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //ErrorCode

            #endregion

            #region PLC SYS INFO
            nIndex = 0;
            for (int i = 0; i < 23; i++)
                AddDataGridView(dgvPLCs[0], plcData[nIndex + i], nIndex + i);
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
            
        }

        private void MESINTERFACE_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void radpnl_MesInterfaceTitle_Click(object sender, EventArgs e)
        {
            radpnl_MESTEST.Visible = !radpnl_MESTEST.Visible;
            if (radpnl_MESTEST.Visible == true) this.Width = 1380;
            else this.Width = 1000;
        }

        private void radBtnWriteValue_Click(object sender, EventArgs e)
        {
           string node = string.Empty, value = string.Empty;
            int nDataType = 0;
            node = cbTagList.Text;
            value = tbTagValue.Text;
            nDataType = cbTagType.SelectedIndex;
            OnWriteButtonClick(node, value, nDataType);
        }

        private void cbTagList_SelectedValueChanged(object sender, EventArgs e)
        {
            string cbTagname = cbTagList.Text;
            if (cbTagname == "SequenceNo" || cbTagname == "AcknowledgeNo")
            {
                cbTagType.Text = "UInt32";
            }
            else if(cbTagname == "EquipmentID" || cbTagname == "TrayID" || cbTagname == "RecipeID")
            {
                cbTagType.Text = "String";
            }
            else if(cbTagname == "CellID")
            {
                cbTagType.Text = "StringArr";

                string tempvalue = "";
                for (int i = 0; i < 32; i++)
                    tempvalue += (i + 1).ToString("D3") + ",";
                tbTagValue.Text = tempvalue;
            }
            else if(cbTagname == "IR" || cbTagname == "OCV")
            {
                cbTagType.Text = "FloatArr";
                string tempvalue = "";
                for (int i = 0; i < 32; i++)
                    tempvalue += (i + 1).ToString("D3") + ",";
                tbTagValue.Text = tempvalue;
            }
            else if (cbTagname == "CellStatus")
            {
                cbTagType.Text = "StringArr";
                string tempvalue = "";
                for (int i = 0; i < 32; i++)
                    tempvalue += "1,";
                tbTagValue.Text = tempvalue;
            }
        }

        #region MES 시뮬레이션
        public void ShowReadMesValues(string strMessage)
        {
            tbMsg.Text += strMessage + Environment.NewLine;
        }
        private void radBtnWriteForir2_1_Click(object sender, EventArgs e)
        {
            //* trayid, equipment idd
            string trayid = tbTrayID.Text;
            string equipmentid = tbEquipmentID.Text;
            RaiseOnWriteForIR1(equipmentid, trayid);
        }

        private void radBtnWriteForir2_2_Click(object sender, EventArgs e)
        {
            //RaiseOnWriteForIR2(equipmentid, trayid, cellid, cellstatus, ir, ocv);
        }

        private void radBtnReadFORIR2_1_Click(object sender, EventArgs e)
        {
            //RaiseOnReadForIR1(cellid, cellstatus, traystatuscode, errorcode, errormessage);
        }

        private void radBtnReadFORIR2_2_Click(object sender, EventArgs e)
        {
            string errorcode = tbErrorCode.Text;
            string errormessage = tbErrorMessage.Text;
            RaiseOnReadForIR2(errorcode, errormessage);
        }
        #endregion

        private void radBtnWritePLCInfo_Click(object sender, EventArgs e)
        {
            string tagname = cbPLCInfoTagList.Text;
            string tagvalue = tbPLCInfoTagValue.Text;
            RaiseOnWritePLCSysInfo(tagname, tagvalue);
        }
    }
}
