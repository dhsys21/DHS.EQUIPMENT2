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
        DataGridView[] dgvPCs = new DataGridView[_Constant.frmCount];
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

            dgvPCs[0] = dgvPC;
            dgvMESs[0] = dgvMES;
            MakeGridView();

            MakeGridView_MESTEST();
        }

        private void MakeGridView_MESTEST()
        {

            dgvCellID.Columns.Add("001", "CH");
            dgvCellID.Columns.Add("002", "CELL ID");
            dgvCellID.Rows.Add(32);

            dgvCellStatus.Columns.Add("001", "CH");
            dgvCellStatus.Columns.Add("002", "CELL STATUS");
            dgvCellStatus.Rows.Add(32);

            dgvCellStatusResult.Columns.Add("001", "CH");
            dgvCellStatusResult.Columns.Add("002", "Result");
            dgvCellStatusResult.Rows.Add(32);

            dgvIR.Columns.Add("001", "CH");
            dgvIR.Columns.Add("002", "IR");
            dgvIR.Rows.Add(32);

            dgvOCV.Columns.Add("001", "CH");
            dgvOCV.Columns.Add("002", "OCV");
            dgvOCV.Rows.Add(32);

            double ir = 0.3886;
            double ocv = 3644.03;

            for (int i = 0; i < 32; i++)
            {
                dgvCellID.Rows[i].Cells[0].Value = (i + 1).ToString();
                dgvCellID.Rows[i].Cells[1].Value = "C000" + (i + 1).ToString("D2");

                dgvCellStatus.Rows[i].Cells[0].Value = (i + 1).ToString();
                dgvCellStatus.Rows[i].Cells[1].Value = "1";

                dgvCellStatusResult.Rows[i].Cells[0].Value = (i + 1).ToString();
                dgvCellStatusResult.Rows[i].Cells[1].Value = "0";

                dgvIR.Rows[i].Cells[0].Value = (i + 1).ToString();
                dgvIR.Rows[i].Cells[1].Value = ir + ((i * 10) / (i % 8 + 4) / 10000.0);

                dgvOCV.Rows[i].Cells[0].Value = (i + 1).ToString();
                dgvOCV.Rows[i].Cells[1].Value = ocv + (i * 5) / (i % 4 + 1);
            }
        }

        private void MakeGridView()
        {
            //* 열 추가
            dgvMESs[0].Columns.Add("001", "MES TAG NAME");
            dgvMESs[0].Columns.Add("002", "MES VALUES");
            dgvMESs[0].Columns[0].Width = 180;
            dgvMESs[0].Columns[1].Width = 250;

            //* 행 추가
            dgvMESs[0].Rows.Add(70);
            dgvMESs[0].Rows[0].Cells[0].Value = "SequenceNo";
            dgvMESs[0].Rows[1].Cells[0].Value = "AcknowledgeNo";
            dgvMESs[0].Rows[2].Cells[0].Value = "EquipmentID";
            dgvMESs[0].Rows[3].Cells[0].Value = "TrayID";
            dgvMESs[0].Rows[4].Cells[0].Value = "TrayStatusCode";
            dgvMESs[0].Rows[5].Cells[0].Value = "ErrorCode";
            dgvMESs[0].Rows[6].Cells[0].Value = "ErrorMessage";

            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                dgvMESs[0].Rows[nIndex + 7].Cells[0].Value = "CellID " + (nIndex + 1).ToString("D2");
                dgvMESs[0].Rows[nIndex + 32 + 7].Cells[0].Value = "CellStatus " + (nIndex + 1).ToString("D2");
            }

            //* 열 추가
            dgvPCs[0].Columns.Add("001", "PC TAG NAME");
            dgvPCs[0].Columns.Add("002", "PC VALUES");
            dgvPCs[0].Columns[0].Width = 200;
            dgvPCs[0].Columns[1].Width = 250;

            //* 행 추가
            dgvPCs[0].Rows.Add(172);
            dgvPCs[0].Rows[0].Cells[0].Value = "SequenceNo";
            dgvPCs[0].Rows[1].Cells[0].Value = "AcknowledgeNo";
            dgvPCs[0].Rows[2].Cells[0].Value = "EquipmentID";
            dgvPCs[0].Rows[3].Cells[0].Value = "TrayID";

            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                dgvPCs[0].Rows[nIndex + 4].Cells[0].Value = "CellID " + (nIndex + 1).ToString("D2");
                dgvPCs[0].Rows[nIndex + 32 + 4].Cells[0].Value = "IR " + (nIndex + 1).ToString("D2");
                dgvPCs[0].Rows[nIndex + 64 + 4].Cells[0].Value = "OCV " + (nIndex + 1).ToString("D2");
                dgvPCs[0].Rows[nIndex + 96 + 4].Cells[0].Value = "CellStatus " + (nIndex + 1).ToString("D2");
            }

            dgvPCs[0].Rows[140].Cells[0].Value = "InterfaceVersionProject";
            dgvPCs[0].Rows[141].Cells[0].Value = "EquipmentName";
            dgvPCs[0].Rows[142].Cells[0].Value = "EquipmentTypeID";
            dgvPCs[0].Rows[143].Cells[0].Value = "LineID";
            dgvPCs[0].Rows[144].Cells[0].Value = "AreaID";
            dgvPCs[0].Rows[145].Cells[0].Value = "EquipmentID";
            dgvPCs[0].Rows[146].Cells[0].Value = "State";
            dgvPCs[0].Rows[147].Cells[0].Value = "Mode";
            dgvPCs[0].Rows[148].Cells[0].Value = "Blocked";
            dgvPCs[0].Rows[149].Cells[0].Value = "Starved";
            dgvPCs[0].Rows[150].Cells[0].Value = "CurrentSpeed";
            dgvPCs[0].Rows[151].Cells[0].Value = "DesignSpeed";
            dgvPCs[0].Rows[152].Cells[0].Value = "TotalCounter";
            dgvPCs[0].Rows[153].Cells[0].Value = "StandstillReason";
            dgvPCs[0].Rows[154].Cells[0].Value = "Stacklight0Color";
            dgvPCs[0].Rows[155].Cells[0].Value = "Stacklight0Behavior";
            dgvPCs[0].Rows[156].Cells[0].Value = "Stacklight1Color";
            dgvPCs[0].Rows[157].Cells[0].Value = "Stacklight1Behavior";
            dgvPCs[0].Rows[158].Cells[0].Value = "Stacklight2Color";
            dgvPCs[0].Rows[159].Cells[0].Value = "Stacklight2Behavior";
            dgvPCs[0].Rows[160].Cells[0].Value = "Stacklight3Color";
            dgvPCs[0].Rows[161].Cells[0].Value = "Stacklight3Behavior";
        }

        public void SetDataToGrid(string[] pcData, string[] mesData)
        {
            #region PLC DATA VIEW
            int nIndex = 0;
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //SequenceNo
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //AcknowledgeNo
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //EquipmentID
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //TrayID
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //TrayStatusCode
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //ErrorCode
            AddDataGridView(dgvMESs[0], mesData[nIndex], nIndex++); //ErrorMessage

            //* nIndex = 6
            for (int i = 0; i < _Constant.ChannelCount; i++)
            {
                AddDataGridView(dgvMESs[0], mesData[i + 7], i + 7); //CellID
                AddDataGridView(dgvMESs[0], mesData[i + 7 + 32], i + 7 + 32); //CellStatus
            }
            #endregion

            #region PC DATA VIEW
            nIndex = 0;
            AddDataGridView(dgvPCs[0], pcData[nIndex], nIndex++); //SequenceNo
            AddDataGridView(dgvPCs[0], pcData[nIndex], nIndex++); //AcknowledgeNo
            AddDataGridView(dgvPCs[0], pcData[nIndex], nIndex++); //EquipmentID
            AddDataGridView(dgvPCs[0], pcData[nIndex], nIndex++); //TrayID

            //* nIndex = 5
            for (int i = 0; i < _Constant.ChannelCount; i++)
            {
                AddDataGridView(dgvPCs[0], pcData[i + 4], i + 4); //CellID
                AddDataGridView(dgvPCs[0], pcData[i + 4 + 32], i + 4 + 32); //IR
                AddDataGridView(dgvPCs[0], pcData[i + 4 + 64], i + 4 + 64); //OCV
                AddDataGridView(dgvPCs[0], pcData[i + 4 + 96], i + 4 + 96); //CellStatus
            }
            #endregion

            #region PLC SYS INFO
            nIndex = 140;
            for (int i = 0; i < 23; i++)
                AddDataGridView(dgvPCs[0], pcData[nIndex + i], nIndex + i);
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
            //WriteValue("ns=2;s=Equipment/EquipmentID", equipmentid, (int)EnumDataType.dtString);
            string node = string.Empty, value = string.Empty;
            int nDataType = 0;

            //node = "ns=2;s=Equipment/" + cbTagList.Text;
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
            string equipmentid = tbEquipmentID.Text;
            string trayid = tbTrayID.Text;
            string[] cellid = new string[32];
            string[] cellstatus = new string[32];
            double[] ir = new double[32];
            double[] ocv = new double[32];

            for(int i = 0; i < 32;i++)
            {
                cellid[i] = dgvCellID.Rows[i].Cells[1].Value.ToString();
                cellstatus[i] = dgvCellStatusResult.Rows[i].Cells[1].Value.ToString();
                ir[i] = Convert.ToDouble(dgvIR.Rows[i].Cells[1].Value.ToString());
                ocv[i] = Convert.ToDouble(dgvOCV.Rows[i].Cells[1].Value.ToString());
            }

            RaiseOnWriteForIR2(equipmentid, trayid, cellid, cellstatus, ir, ocv);
        }

        private void radBtnReadFORIR2_1_Click(object sender, EventArgs e)
        {
            string trayid = tbTrayID.Text;
            string equipmentid = tbEquipmentID.Text;
            string[] cellid = new string[32];
            string[] cellstatus = new string[32];
            string traystatuscode = tbTrayStatusCode.Text;
            string errorcode = tbErrorCode.Text;
            string errormessage = tbErrorMessage.Text;

            for(int i = 0; i < 32; i++)
            {
                cellid[i] = dgvCellID.Rows[i].Cells[1].Value.ToString();
                cellstatus[i] = dgvCellStatus.Rows[i].Cells[1].Value.ToString();
            }

            RaiseOnReadForIR1(cellid, cellstatus, traystatuscode, errorcode, errormessage);
        }

        private void radBtnReadFORIR2_2_Click(object sender, EventArgs e)
        {
            string errorcode = tbErrorCode.Text;
            string errormessage = tbErrorMessage.Text;
            RaiseOnReadForIR2(errorcode, errormessage);
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            string tagname = cbTagList.Text;
            string tagvalue = tbTagValue.Text;
            RaiseOnWritePLCSysInfo(tagname, tagvalue);
        }
        #endregion

    }
}
