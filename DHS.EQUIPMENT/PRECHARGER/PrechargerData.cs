using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHS.EQUIPMENT.Common;

namespace DHS.EQUIPMENT
{
    public class PrechargerData
    {
        Util util = new Util();
        CEquipmentData _system = CEquipmentData.GetInstance();

        private static PrechargerData[] prechargerdata = new PrechargerData[_Constant.frmCount];
        public static PrechargerData GetInstance(int nIndex)
        {
            if (prechargerdata[nIndex] == null) prechargerdata[nIndex] = new PrechargerData();
            return prechargerdata[nIndex];
        }

        #region precharger data
        private int _iStageNo;
        private string _sStageStatus;
        private int _iStepIndex;
        private int _iStepTime;
        private int _iTestTime;
        private int _iDataTime;
        private string[] _sVolt = new string[_Constant.ChannelCount];
        private string[] _sCurr = new string[_Constant.ChannelCount];
        private string[] _sCapa = new string[_Constant.ChannelCount];
        private string[] _sCode = new string[_Constant.ChannelCount];
        private Color[] _clrChannelColor = new Color[_Constant.ChannelCount];
        private string[] _sErrorMessage = new string[_Constant.ChannelCount];
        private bool[] _bMeasureResult = new bool[_Constant.ChannelCount];
        private int[] _iErrorTimeCount = new int[_Constant.ChannelCount];
        private string[] _sCellSerial = new string[_Constant.ChannelCount];
        private bool[] _bCell = new bool[_Constant.ChannelCount];

        private bool _bRst;
        private bool _bSet;
        private bool _bAms;
        private bool _bAmf;
        private bool _bTrayin;
        private bool _bCharging;
        private bool _bEndCharging;
        private string _sTrayId;
        private int _iCellCount;
        private string _sMdlName;
        private string _sBatchId;
        private string _sLotId;
        private string _sCellModel;
        private string _sLotNumber;
        private DateTime _dtArriveTime;
        private DateTime _dtFinishTime;
        #endregion

        #region SETTING VALUEs
        private string _sSetVoltage;
        private string _sSetCurrent;
        private string _sSetTime;
        private string _sParam;
        private double _dSetVoltage;
        private double _dSetCurrent;
        private int _iSetTime;
        public string SETVOLTAGE { get => _sSetVoltage; set => _sSetVoltage = value; }
        public string SETCURRENT { get => _sSetCurrent; set => _sSetCurrent = value; }
        public string SETTIME { get => _sSetTime; set => _sSetTime = value; }
        public string PARAM { get => _sParam; set => _sParam = value; }
        public double DSETVOLTAGE { get => _dSetVoltage; set => _dSetVoltage = value; }
        public double DSETCURRENT { get => _dSetCurrent; set => _dSetCurrent = value; }
        public int ISETTIME { get => _iSetTime; set => _iSetTime = value; }
        #endregion

        public int STAGENO { get => _iStageNo; set => _iStageNo = value; }
        public string STAGESTATUS { get => _sStageStatus; set => _sStageStatus = value; }
        public int STEPINDEX { get => _iStepIndex; set => _iStepIndex = value; }

        public int STEPTIME { get => _iStepTime; set => _iStepTime = value; }
        public int TESTTIME { get => _iTestTime; set => _iTestTime = value; }
        public int DATATIME { get => _iDataTime; set => _iDataTime = value; }
        public string[] VOLT { get => _sVolt; set => _sVolt = value; }
        public string[] CURR { get => _sCurr; set => _sCurr = value; }
        public string[] CAPA { get => _sCapa; set => _sCapa = value; }
        public string[] CODE { get => _sCode; set => _sCode = value; }
        public bool RST { get => _bRst; set => _bRst = value; }
        public bool SET { get => _bSet; set => _bSet = value; }
        public bool AMS { get => _bAms; set => _bAms = value; }
        public bool AMF { get => _bAmf; set => _bAmf = value; }
        public bool TRAYIN { get => _bTrayin; set => _bTrayin = value; }
        public bool CHARGING { get => _bCharging; set => _bCharging = value; }
        public bool ENDCHARGING { get => _bEndCharging; set => _bEndCharging = value; }
        public string TRAYID { get => _sTrayId; set => _sTrayId = value; }
        public bool[] CELL { get => _bCell; set => _bCell = value; }

        public Color[] CHANNELCOLOR { get => _clrChannelColor; set => _clrChannelColor = value; }

        public string[] ERRORMSG { get => _sErrorMessage; set => _sErrorMessage = value; }
        public int CELLCOUNT { get => _iCellCount; set => _iCellCount = value; }
        public bool[] MEASURERESULT { get => _bMeasureResult; set => _bMeasureResult = value; }
        public int[] ERRORTIMECOUNT { get => _iErrorTimeCount; set => _iErrorTimeCount = value; }
        public string[] CELLSERIAL { get => _sCellSerial; set => _sCellSerial = value; }
        public string MDLNAME { get => _sMdlName; set => _sMdlName = value; }
        public string BATCHID { get => _sBatchId; set => _sBatchId = value; }
        public string LOTID { get => _sLotId; set => _sLotId = value; }
        public string CELLMODEL { get => _sCellModel; set => _sCellModel = value; }
        public string LOTNUMBER { get => _sLotNumber; set => _sLotNumber = value; }

        public DateTime FINISHTIME { get => _dtFinishTime; set => _dtFinishTime = value; }
        public DateTime ARRIVETIME { get => _dtArriveTime; set => _dtArriveTime = value; }



        #region Delegate
        public delegate void delegateMainDisplayTestTime(int stageno, string sStep, string sTesttime);
        public event delegateMainDisplayTestTime OnDisplayTestTime = null;
        protected void RaiseOnDisplayTestTime(int stageno, string sStep, string sTesttime)
        {
            if (OnDisplayTestTime != null)
            {
                OnDisplayTestTime(stageno, sStep, sTesttime);
            }
        }
        #endregion

        public void InitData(int stageno)
        {
            _iStageNo = stageno;

            _bAmf = false;
            _bAms = false;
            _bRst = false;
            _bSet = false;
            _bEndCharging = false;
            _bCharging = false;

            _sStageStatus = string.Empty;
            _iTestTime = 0;
            _iStepIndex = 0;
            _iStepTime = 0;
            _iDataTime = 0;

            System.Array.Clear(_sCode, 0, _Constant.ChannelCount);
            //System.Array.Clear(_sVolt, 0, _Constant.ChannelCount);
            //System.Array.Clear(_sCurr, 0, _Constant.ChannelCount);
            System.Array.Clear(_sCapa, 0, _Constant.ChannelCount);

            _sTrayId = string.Empty;
            _bTrayin = false;
            _iCellCount = 0;
            System.Array.Clear(_bCell, 0, _Constant.ChannelCount);
            System.Array.Clear(_sCellSerial, 0, _Constant.ChannelCount);
            System.Array.Clear(_iErrorTimeCount, 0, _Constant.ChannelCount);
            System.Array.Clear(_bMeasureResult, 0, _Constant.ChannelCount);
            _sMdlName = string.Empty;
            _sBatchId = string.Empty;
            _sLotId = string.Empty;
            _sCellModel = string.Empty;
            _sLotNumber = string.Empty;

            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                _clrChannelColor[nIndex] = _Constant.ColorReady;
                _sErrorMessage[nIndex] = string.Empty;
                _sVolt[nIndex] = "0";
                _sCurr[nIndex] = "0";
            }

        }

        public void SetTrayInfo(bool[] bExist)
        {
            for (int i = 0; i < bExist.Length; i++)
                _bCell[i] = bExist[i];
        }
        public void SetTrayInfo(int iChannel, bool bExist)
        {
            _bCell[iChannel] = bExist;
        }

        public void SetVoltages(double[] voltages)
        {
            for (int cIndex = 0; cIndex < _Constant.ChannelCount; cIndex++)
            {
                _sVolt[cIndex] = (voltages[cIndex] * 1000.0).ToString("F2");
            }
        }
        public void SetCurrents(double[] currents)
        {
            for (int cIndex = 0; cIndex < _Constant.ChannelCount; cIndex++)
            {
                _sCurr[cIndex] = (currents[cIndex] * 1000.0).ToString("F1");
            }
        }

        

        public void SetResultData(int stageno, PrechargerData CPreData)
        {
            int pos = 0;
            double dTempCurr = 0.0;

            //for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            //{
            //    //pos = chMap[nIndex + 1];
            //    pos = nIndex;
            //    double.TryParse(CPreData.CURR[nIndex], out dTempCurr);
            //    if (CPreData.CELL[nIndex] == true)
            //    {
            //        if (dTempCurr <= 50 || CPreData.CHANNELCOLOR[nIndex] == _Constant.ColorError || CPreData.CHANNELCOLOR[nIndex] == _Constant.ColorFlow)
            //        {
            //            CPreData.MEASURERESULT[nIndex] = false;
            //        }
            //        else
            //        {
            //            CPreData.MEASURERESULT[nIndex] = true;
            //        }
            //    }
            //    else if (CPreData.CELL[nIndex] == false)
            //    {
            //        //* 셀이 없으면 colorerror 가 될 수 없음.(???)
            //        if (CPreData.CHANNELCOLOR[nIndex] == _Constant.ColorError || CPreData.CHANNELCOLOR[nIndex] == _Constant.ColorFlow)
            //        {
            //            CPreData.MEASURERESULT[nIndex] = false;
            //        }
            //        else
            //        {
            //            CPreData.MEASURERESULT[nIndex] = true;
            //        }
            //    }
            //}
        }



        public void SetParms()
        {
            string param;
            param = "1";
            param = param + Convert.ToInt32(_sSetVoltage).ToString("D4");
            param = param + Convert.ToInt32(_sSetCurrent).ToString("D4");
            param = param + Convert.ToInt32(_sSetTime).ToString("D4");
            param = param + Convert.ToInt32(50).ToString("D4");

            _sParam = param;
        }

        private string GetPreChargerStatus(string status)
        {
            switch (status)
            {
                case "0x00": return "Wait(00)";
                case "0x01": return "Contact(01)";
                case "0x02": return "ContactFinished(02)";
                case "0x03": return "Charging(03)";
                case "0x04": return "Discharging(04)";
                case "0x05": return "Rest(05)";
                case "0x06": return "StepFinished(06)";
                case "0x07": return "Calibration(07)";
                case "0x08": return "Ready(08)";
                case "0x10": return "CmuxComm(10)";
                case "0x20": return "CmuxStatus(20)";
                case "0x30": return "PowerOVP Error(30)";
                case "0x40": return "PowerUVP Error(40)";
                case "0x50": return "DTri Error(50)";
                default: return status;
            }
        }

    }
}
