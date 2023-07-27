using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        public void InitData(int stageno)
        {
            _iStage = stageno;
        }

        private void radbtn_AllRemeasure_Click(object sender, EventArgs e)
        {
            RaiseOnRemeasureAll(this._iStage);
        }

        private void radbtn_Remeasure_Click(object sender, EventArgs e)
        {
            RaiseOnRemeasure(this._iStage);
        }

        private void radbtn_TrayOut_Click(object sender, EventArgs e)
        {
            RaiseOnTrayOut(this._iStage);
        }
    }
}
