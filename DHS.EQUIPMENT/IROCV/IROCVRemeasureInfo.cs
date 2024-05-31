using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.VirtualKeyboard;

namespace DHS.EQUIPMENT
{
    public partial class IROCVRemeasureInfo : Form
    {
        public int _iStage;

        #region Delegate
        public delegate void RemeasureAllClick(int stageno);
        public event RemeasureAllClick OnRemeasureAllClick = null;
        protected void RaiseOnRemeasureAll(int stageno)
        {
            if (OnRemeasureAllClick != null)
            {
                OnRemeasureAllClick(stageno);
            }
        }
        public delegate void RemeasureClick(int stageno);
        public event RemeasureClick OnRemeasureClick = null;
        protected void RaiseOnRemeasure(int stageno)
        {
            if (OnRemeasureClick != null)
            {
                OnRemeasureClick(stageno);
            }
        }
        public delegate void TrayOutClick(int stageno);
        public event TrayOutClick OnTrayOutClick = null;
        protected void RaiseOnTrayOut(int stageno)
        {
            if (OnTrayOutClick != null)
            {
                OnTrayOutClick(stageno);
            }
        }
        #endregion
        private static IROCVRemeasureInfo remeasureinfoForm = new IROCVRemeasureInfo();
        public static IROCVRemeasureInfo GetInstance()
        {
            if (remeasureinfoForm == null) remeasureinfoForm = new IROCVRemeasureInfo();
            return remeasureinfoForm;
        }
        public IROCVRemeasureInfo()
        {
            InitializeComponent();

            remeasureinfoForm = this;
        }

        public void InitData(int stageno, int remeasurelen)
        {
            _iStage = stageno;

            dgvNGList.Rows.Clear();
            dgvNGList.Rows.Add(remeasurelen + 2);
        }

        private void radbtn_TrayOut_Click(object sender, EventArgs e)
        {
            RaiseOnTrayOut(this._iStage);
        }

        private void radbtn_RemeasureAll_Click(object sender, EventArgs e)
        {
            RaiseOnRemeasureAll(this._iStage);
        }
        public void AddRemeasureList(int nRow, int channel, int errCode, double irvalue, double ocvvalue)
        {
            string desc = string.Empty;
            if (errCode == 2) desc = "IR NG";
            else if (errCode == 3) desc = "OCV NG";
            else if (errCode == 4) desc = "IR REMEASURE NG";
            else if (errCode == 5) desc = "OCV REMEASURE NG";

            dgvNGList.Rows[nRow].Cells[0].Value = channel.ToString();
            dgvNGList.Rows[nRow].Cells[1].Value = desc;
            dgvNGList.Rows[nRow].Cells[2].Value = irvalue.ToString();
            dgvNGList.Rows[nRow].Cells[3].Value = ocvvalue.ToString();
        }

        private void dgvNGList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            dgvNGList.Rows[0].Selected = false;
            dgvNGList.ClearSelection();
        }
    }
}
