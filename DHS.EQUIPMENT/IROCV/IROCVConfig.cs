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
    public partial class IROCVConfig : Form
    {
        Util util = new Util();
        CEquipmentData _system;
        IROCVData[] irocvdata = new IROCVData[_Constant.frmCount];

        private Dictionary<string, string> configvalue = new Dictionary<string, string>();
        private int _iStage;
        public int STAGENO { get => _iStage; set => _iStage = value; }

        #region Delegate
        public delegate void SaveButtonClick(int stageno);
        public event SaveButtonClick OnSaveButtonClick = null;
        protected void RaiseOnSaveConfig(int stageno)
        {
            if (OnSaveButtonClick != null)
            {
                OnSaveButtonClick(stageno);
            }
        }

        #endregion

        private static IROCVConfig[] irocvconfig = new IROCVConfig[_Constant.frmCount];
        public static IROCVConfig GetInstance(int nIndex)
        {
            if (irocvconfig[nIndex] == null) irocvconfig[nIndex] = new IROCVConfig();
            return irocvconfig[nIndex];
        }
        public IROCVConfig()
        {
            InitializeComponent();

            _system = CEquipmentData.GetInstance();
        }

        public void SetStageSystemValue()
        {
            tbStageName.Text = _system.STAGENAME[_iStage];
            tbStageNo.Text = _system.STAGENO[_iStage];

            tbIrocvIpaddress.Text = _system.IPADDRESS[_iStage];

            tbOcvMinOutflow.Text = _system.OCVMINVALUE.ToString();
            tbAutoRemeasureCount.Text = _system.AUTOREMEASURECOUNT[_iStage].ToString();
        }

        private void radBtnSave_Click(object sender, EventArgs e)
        {
            _system.STAGENAME[STAGENO] = tbStageName.Text;
            _system.STAGENO[STAGENO] = tbStageNo.Text;

            _system.IPADDRESS[STAGENO] = tbIrocvIpaddress.Text;
            _system.OCVMINVALUE = Convert.ToDouble(tbOcvMinOutflow.Text);
            _system.AUTOREMEASURECOUNT[STAGENO] = Convert.ToInt32(tbAutoRemeasureCount.Text);

            double dIrMin = 0.0, dIrMax = 0.0;
            double dIrRemeaMin = 0.0, dIrRemeaMax = 0.0;
            double dOcvMin = 0.0, dOcvMax = 0.0;
            double dOcvRemeaMin = 0.0, dOcvRemeaMax = 0.0;

            //* IR SPEC
            _system.IRMIN = util.TryParseDouble(tbIRMin.Text, dIrMin);
            _system.IRMAX = util.TryParseDouble(tbIRMax.Text, dIrMax);
            _system.IRREMEAMIN = util.TryParseDouble(tbIRRemeaMin.Text, dIrRemeaMin);
            _system.IRREMEAMAX = util.TryParseDouble(tbIRRemeaMax.Text, dIrRemeaMax);

            //* OCV SPEC
            _system.OCVMIN = util.TryParseDouble(tbOcvMin.Text, dOcvMin);
            _system.OCVMAX = util.TryParseDouble(tbOcvMax.Text, dOcvMax);
            _system.OCVREMEAMIN = util.TryParseDouble(tbOcvRemeaMin.Text, dOcvRemeaMin);
            _system.OCVREMEAMAX = util.TryParseDouble(tbOcvRemeaMax.Text, dOcvRemeaMax);

            RaiseOnSaveConfig(STAGENO);
            this.Hide();
        }

        private void radBtnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void IROCVConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
        
    }
}
