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
    public partial class ConfigForm : Form
    {
        CEquipmentData _system;
        #region Delegate
        public delegate void SaveButtonClick();
        public event SaveButtonClick OnSaveButtonClick = null;
        protected void RaiseOnSaveConfig()
        {
            if (OnSaveButtonClick != null)
            {
                OnSaveButtonClick();
            }
        }

        #endregion
        public ConfigForm()
        {
            InitializeComponent();
            _system = CEquipmentData.GetInstance();
        }

        public void SetSystemValue()
        {
            tbMesIpaddress.Text = _system.MESIPADDRESS;
            tbMesPort.Text = _system.MESPORT.ToString();

            tbPlcIpaddress.Text = _system.PLCIPADDRESS;
            tbPlcDbNumber.Text = _system.PLCDBNUMBER.ToString();
            tbPlcDbNumberSys.Text = _system.PLCDBNUMBERSYS.ToString();
        }

        private void radBtnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void radBtnSave_Click(object sender, EventArgs e)
        {
            _system.MESIPADDRESS = tbMesIpaddress.Text;
            _system.MESPORT = Convert.ToInt32(tbMesPort.Text);

            _system.PLCIPADDRESS = tbPlcIpaddress.Text;
            _system.PLCDBNUMBER = Convert.ToInt32(tbPlcDbNumber.Text);
            _system.PLCDBNUMBERSYS = Convert.ToInt32(tbPlcDbNumberSys.Text);

            RaiseOnSaveConfig();
            this.Hide();
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
