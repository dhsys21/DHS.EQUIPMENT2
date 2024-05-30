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
            dgvPCs[0].Rows.Add(133);
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
                cbTagType.Text = "UInt32Arr";
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
    }
}
