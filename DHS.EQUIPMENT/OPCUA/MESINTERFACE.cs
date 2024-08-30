﻿using System;
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
        //MesClient mesclient = new MesClient();
        private static MESINTERFACE mesform;
        DataGridView[] dgvPLCs = new DataGridView[_Constant.frmCount];
        DataGridView[] dgvMESs = new DataGridView[_Constant.frmCount];

        #region MES 시뮬레이션
        public delegate void WriteButtonClick(string node, string value, int nDataType);
        public event WriteButtonClick OnWriteButtonClick = null;
        protected void RaiseOnWriteMesTag(string node, string value, int nDataType)
        {
            if (OnWriteButtonClick != null)
            {
                OnWriteButtonClick(node, value, nDataType);
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
        public delegate void WriteMesValues(int stageno, TrayRequestInfo trayRequestInfo);
        public event WriteMesValues OnWriteMesValues = null;
        protected void RaiseOnMesValues(int stageno, TrayRequestInfo trayRequestInfo)
        {
            if (OnWriteMesValues != null)
            {
                OnWriteMesValues(stageno, trayRequestInfo);
            }
        }
        public delegate void WriteIROCVValues(int stageno, IrocvDataCollection irocvDataCollection);
        public event WriteIROCVValues OnWriteIROCVValues = null;
        protected void RaiseOnIROCVValues(int stageno, IrocvDataCollection irocvDataCollection)
        {
            if (OnWriteIROCVValues != null)
            {
                OnWriteIROCVValues(stageno, irocvDataCollection);
            }
        }
        public delegate void CallMethod(string tagParent, string Tag);
        public event CallMethod OnCallMethod = null;
        protected void RaiseOnCallMethod(string tagParent, string Tag)
        {
            if (OnCallMethod != null)
            {
                OnCallMethod(tagParent, Tag);
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
            this.Width = 990;
            radpnl_MESTEST.Visible = false;

            dgvPLCs[0] = dgvPLC;
            dgvMESs[0] = dgvMES;
            MakeGridView();

            //* for test
            MakeGridViewForDemo();
        }
        private void MESINTERFACE_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        #region mes demo value
        private void MakeGridViewForDemo()
        {
            dgvCellId.Rows.Add(_Constant.ChannelCount);
            dgvMesCellStatus.Rows.Add(_Constant.ChannelCount);
            dgvIrocvCellStatus.Rows.Add(_Constant.ChannelCount);
            dgvIr.Rows.Add(_Constant.ChannelCount);
            dgvOcv.Rows.Add(_Constant.ChannelCount);

            double[] irs = new double[_Constant.ChannelCount];
            double[] ocvs = new double[_Constant.ChannelCount];
            double ir = 38.21;
            double ocv = 3523.82;
            Random random = new Random();

            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                //* cell id
                dgvCellId.Rows[nIndex].Cells[0].Value = (nIndex + 1).ToString();
                dgvCellId.Rows[nIndex].Cells[1].Value = "CELL" + (nIndex + 1).ToString("D3");

                //* mes cell status
                dgvMesCellStatus.Rows[nIndex].Cells[0].Value = (nIndex + 1).ToString();
                dgvMesCellStatus.Rows[nIndex].Cells[1].Value = "1";

                //* IROCV cell status
                dgvIrocvCellStatus.Rows[nIndex].Cells[0].Value = (nIndex + 1).ToString();
                dgvIrocvCellStatus.Rows[nIndex].Cells[1].Value = "0";

                //* ir values
                irs[nIndex] = ir + random.NextDouble();
                dgvIr.Rows[nIndex].Cells[0].Value = (nIndex + 1).ToString();
                dgvIr.Rows[nIndex].Cells[1].Value = irs[nIndex].ToString("F2");

                //* ocv values
                ocvs[nIndex] = ocv + random.NextDouble();
                dgvOcv.Rows[nIndex].Cells[0].Value = (nIndex + 1).ToString();
                dgvOcv.Rows[nIndex].Cells[1].Value = ocvs[nIndex].ToString("F2");
            }
        }
        #endregion mes demo value

        #region MES Interface Value Data Grid View
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
            dgvPLCs[0].Rows.Add(35);
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
            dgvPLCs[0].Rows[13].Cells[0].Value = "State";
            dgvPLCs[0].Rows[14].Cells[0].Value = "StandstillReason";
            dgvPLCs[0].Rows[15].Cells[0].Value = "Starved";
            dgvPLCs[0].Rows[16].Cells[0].Value = "TotaCounter";
            dgvPLCs[0].Rows[17].Cells[0].Value = "Stacklight0Color";
            dgvPLCs[0].Rows[18].Cells[0].Value = "Stacklight0Behavior";
            dgvPLCs[0].Rows[19].Cells[0].Value = "Stacklight1Color";
            dgvPLCs[0].Rows[20].Cells[0].Value = "Stacklight1Behavior";
            dgvPLCs[0].Rows[21].Cells[0].Value = "Stacklight2Color";
            dgvPLCs[0].Rows[22].Cells[0].Value = "Stacklight2Behavior";
            dgvPLCs[0].Rows[23].Cells[0].Value = "Stacklight3Color";
            dgvPLCs[0].Rows[24].Cells[0].Value = "Stacklight3Behavior";
            dgvPLCs[0].Rows[25].Cells[0].Value = "Stacklight4Color";
            dgvPLCs[0].Rows[26].Cells[0].Value = "Stacklight4Behavior";
            dgvPLCs[0].Rows[27].Cells[0].Value = "Stacklight5Color";
            dgvPLCs[0].Rows[28].Cells[0].Value = "Stacklight5Behavior";
        }
        public void SetDataToGrid(string[] plcData, string[] mesData)
        {
            #region PLC DATA VIEW
            AddDataGridView(dgvMESs[0], mesData[0], 1); //SequenceNo
            AddDataGridView(dgvMESs[0], mesData[1], 2); //AcknowledgeNo
            AddDataGridView(dgvMESs[0], mesData[2], 4); //EquipmentID
            AddDataGridView(dgvMESs[0], mesData[3], 5); //TrayID
            #endregion

            #region PLC SYS INFO
            for (int i = 0; i < plcData.Length; i++)
                AddDataGridView(dgvPLCs[0], plcData[i], i);
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
        private void radpnl_MesInterfaceTitle_Click(object sender, EventArgs e)
        {
            radpnl_MESTEST.Visible = !radpnl_MESTEST.Visible;
            if (radpnl_MESTEST.Visible == true) this.Width = 1420;
            else this.Width = 990;
        }
        #endregion  MES Interface Value Data Grid View

        #region MES 시뮬레이션
        public void ShowReadMesValues(string strMessage)
        {
            tbMsg.Text += strMessage + Environment.NewLine;
        }
        private void radBtnWriteValue_Click(object sender, EventArgs e)
        {
            string node = string.Empty, value = string.Empty;
            int nDataType = 0;
            node = cbTagList.Text;
            value = tbTagValue.Text;
            //OnWriteButtonClick(node, value, nDataType);
            RaiseOnWriteMesTag(node, value, nDataType);
        }
        private void radBtnWritePLCInfo_Click(object sender, EventArgs e)
        {
            string tagname = cbPLCInfoTagList.Text;
            string tagvalue = tbPLCInfoTagValue.Text;
            RaiseOnWritePLCSysInfo(tagname, tagvalue);
        }
        private void radBtnMesValues_Click(object sender, EventArgs e)
        {
            TrayRequestInfo trayRequestInfo = new TrayRequestInfo();
            trayRequestInfo.CellID = null;
            trayRequestInfo.CellStatus = null;
            trayRequestInfo.TrayStatusCode = null;
            trayRequestInfo.ErrorCode = null;
            trayRequestInfo.ErrorMessage = null;
            RaiseOnMesValues(0, trayRequestInfo);
        }

        private void radBtnIrocvValues_Click(object sender, EventArgs e)
        {
            string[] cellids = new string[_Constant.ChannelCount];
            string[] cellstatus = new string[_Constant.ChannelCount];
            string[] irs = new string[_Constant.ChannelCount];
            string[] ocvs = new string[_Constant.ChannelCount];

            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                cellids[nIndex] = dgvCellId.Rows[nIndex].Cells[1].Value.ToString();
                cellstatus[nIndex] = dgvIrocvCellStatus.Rows[nIndex].Cells[1].Value.ToString();
                irs[nIndex] = dgvIr.Rows[nIndex].Cells[1].Value.ToString();
                ocvs[nIndex] = dgvOcv.Rows[nIndex].Cells[1].Value.ToString();
            }

            IrocvDataCollection irocvDataCollection = new IrocvDataCollection();
            irocvDataCollection.EquipmentID = tbEquipmentID.Text;
            irocvDataCollection.TrayID = tbTrayID.Text;
            irocvDataCollection.CellID = cellids;
            irocvDataCollection.CellStatus = cellstatus;
            irocvDataCollection.IR = Array.ConvertAll(irs, double.Parse); ;
            irocvDataCollection.OCV = Array.ConvertAll(ocvs, double.Parse); ;
            RaiseOnIROCVValues(0, irocvDataCollection);
        }
        #endregion

        private void radBtnCallMethod_Click(object sender, EventArgs e)
        {
            RaiseOnCallMethod("ns=2;i=5031", "ns=2;i=7004");
        }
    }
}
