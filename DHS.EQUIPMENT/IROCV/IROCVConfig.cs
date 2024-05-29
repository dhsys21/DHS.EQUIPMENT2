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
            tbEquipmentID.Text = _system.EQUIPMENTID;

            tbIrocvIpaddress.Text = _system.IPADDRESS[_iStage];

            tbOcvMinOutflow.Text = _system.OCVMINVALUE.ToString();
            tbRemeasureCount.Text = _system.REMEASURECOUNT.ToString();
            tbRemeasureMaxCount.Text = _system.REMEASUREMAXCOUNT.ToString();
            tbRemeasurePercent.Text = _system.REMEASUREPERCENT.ToString();

            tbIRMin.Text = _system.IRMIN.ToString();
            tbIRMax.Text = _system.IRMAX.ToString();
            tbIRRemeaMin.Text = _system.IRREMEAMIN.ToString();
            tbIRRemeaMax.Text = _system.IRREMEAMAX.ToString();
            
            tbOcvMin.Text = _system.OCVMIN.ToString();
            tbOcvMax.Text = _system.OCVMAX.ToString();
            tbOcvRemeaMin.Text = _system.OCVREMEAMIN.ToString();
            tbOcvRemeaMax.Text = _system.OCVREMEAMAX.ToString();
        }
        private void SaveConfig(int stageno)
        {
            _system.EQUIPMENTID = tbEquipmentID.Text;

            //* IROCV CONTROLLER IP ADDRESS
            _system.IPADDRESS[stageno] = tbIrocvIpaddress.Text;

            //* OCV MIN VALUE
            double ocvminvalue = 0.0;
            _system.OCVMINVALUE = util.TryParseDouble(tbOcvMinOutflow.Text, ocvminvalue);
            tbOcvMinOutflow.Text = _system.OCVMINVALUE.ToString();

            //* REMEASURE COUNT (재측정 횟수)
            int remeasurecount = 0;
            _system.REMEASURECOUNT = util.TryParseInt(tbRemeasureCount.Text, remeasurecount);
            tbRemeasureCount.Text = _system.REMEASURECOUNT.ToString();

            //* REMEASURE MAX COUNT (불량 갯수가 일정수 이하면 컨택상태에서 불량셀만 재측정)
            int remeasuremaxcount = 0;
            _system.REMEASUREMAXCOUNT = util.TryParseInt(tbRemeasureMaxCount.Text, remeasuremaxcount);
            tbRemeasureMaxCount.Text = _system.REMEASUREMAXCOUNT.ToString();

            //* REMEASURE PERCENT (재측정 하기 위한 불량 percent)
            int remeasurepercent = 0;
            _system.REMEASUREPERCENT = util.TryParseInt(tbRemeasurePercent.Text, remeasurepercent);
            tbRemeasurePercent.Text = _system.REMEASUREPERCENT.ToString();

            //* MES 사용여부
            _system.UNUSEMES = chkUnuseMes.Checked;

            double dIrMin = 0.0, dIrMax = 0.0;
            double dIrRemeaMin = 0.0, dIrRemeaMax = 0.0;
            double dOcvMin = 0.0, dOcvMax = 0.0;
            double dOcvRemeaMin = 0.0, dOcvRemeaMax = 0.0;

            //* IR SPEC
            _system.IRMIN = util.TryParseDouble(tbIRMin.Text, dIrMin);
            tbIRMin.Text = _system.IRMIN.ToString();

            _system.IRMAX = util.TryParseDouble(tbIRMax.Text, dIrMax);
            tbIRMax.Text = _system.IRMAX.ToString();

            _system.IRREMEAMIN = util.TryParseDouble(tbIRRemeaMin.Text, dIrRemeaMin);
            tbIRRemeaMin.Text = _system.IRREMEAMIN.ToString();

            _system.IRREMEAMAX = util.TryParseDouble(tbIRRemeaMax.Text, dIrRemeaMax);
            tbIRRemeaMax.Text = _system.IRREMEAMAX.ToString();

            //* OCV SPEC
            _system.OCVMIN = util.TryParseDouble(tbOcvMin.Text, dOcvMin);
            tbOcvMin.Text = _system.OCVMIN.ToString();

            _system.OCVMAX = util.TryParseDouble(tbOcvMax.Text, dOcvMax);
            tbOcvMax.Text = _system.OCVMAX.ToString();

            _system.OCVREMEAMIN = util.TryParseDouble(tbOcvRemeaMin.Text, dOcvRemeaMin);
            tbOcvRemeaMin.Text = _system.OCVREMEAMIN.ToString();

            _system.OCVREMEAMAX = util.TryParseDouble(tbOcvRemeaMax.Text, dOcvRemeaMax);
            tbOcvRemeaMax.Text= _system.OCVREMEAMAX.ToString();

            RaiseOnSaveConfig(stageno);
        }
        private void radBtnSave_Click(object sender, EventArgs e)
        {
            SaveConfig(STAGENO);
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
