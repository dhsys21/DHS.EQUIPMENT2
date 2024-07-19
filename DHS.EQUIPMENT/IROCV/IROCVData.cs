using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHS.EQUIPMENT.Common;

namespace DHS.EQUIPMENT
{
    public class IROCVData
    {
        Util util = new Util();
        CEquipmentData _system = CEquipmentData.GetInstance();

        private static IROCVData[] irocvdata = new IROCVData[_Constant.frmCount];
        public static IROCVData GetInstance(int nIndex)
        {
            if (irocvdata[nIndex] == null) irocvdata[nIndex] = new IROCVData();
            return irocvdata[nIndex];
        }

        #region IR/OCV DATA
        private string _sEQUIPMENTID;
        private string _sTRAYID;
        private string _sTRAYSTATUSCODE;
        private string _sERRORCODE;
        private string _sERRORMESSAGE;
        private string _sTAGPATHNO;
        private string _sCELLTYPE;
        private string _sBATCHID;
        private string _sOLDBATCHID;
        private string _sLOTID;
        private string _sLOTNUMBER;
        private string _sCELLSERIAL;
        private int _iCELLCOUNT;
        private int _iMESRESULT;

        private bool _bFIRST;
        private DateTime _dtArriveTime;
        private DateTime _dtStartTime;
        private DateTime _dtFinishTime;

        private string _sLog;

        //* CELL 정보 0 - no cell, 1 - cell exist
        private int[] _iCELL = new int[_Constant.ChannelCount];
        private string[] _sCELLID = new string[_Constant.ChannelCount];
        private string[] _sCELLSTATUSMES = new string[_Constant.ChannelCount];
        private string[] _sCELLSTATUSIROCV = new string[_Constant.ChannelCount];

        private double[] _dIR_ORIGINALVALUE = new double[_Constant.ChannelCount];
        private double[] _dIR_AFTERVALUE = new double[_Constant.ChannelCount];
        private double[] _dOCV = new double[_Constant.ChannelCount];
        //* Measure Result 0 - OK, 2 - IR Error, 3 - OCV Error
        private int[] _iMEASURERESULT = new int[_Constant.ChannelCount];
        //* Remeasure Cell Count (ng count 갯수)
        private int _iREMEASURECELLCOUNT;
        //* Remeasure Count (자동 재측정 횟수)
        private int _iREMEASURECOUNT;
        //* Remeasure Mode true - remeasure ng channel(부분재측정), false - remeausre all or test all(전체재측정)
        private bool _bREMEASURE;
        //* Remeasure Count per every cell
        private int[] _iCellNgCount = new int[_Constant.ChannelCount];
        //* IR Result 0 - not measured, 1 - measured
        private int[] _iIRRESULT = new int[_Constant.ChannelCount];
        //* OCV Result 0 - not measured, 1 - measured
        private int[] _iOCVRESULT = new int[_Constant.ChannelCount];
        private Color[] _clrCHANNEL = new Color[_Constant.ChannelCount];
        private Color[] _clrIR = new Color[_Constant.ChannelCount];
        private Color[] _clrOCV = new Color[_Constant.ChannelCount];

        //*OFFSET(보정) : standard - 실측데이터, measure - IROCV 로 측정한 값, offset : measure - standard
        private double[] _dIR_STANDARD = new double[_Constant.ChannelCount];
        private double[] _dIR_MEASURE = new double[_Constant.ChannelCount];
        private double[] _dIR_OFFSET = new double[_Constant.ChannelCount];
        private double[] _dIR_OFFSETAVERAGE = new double[_Constant.ChannelCount];
        private double _dOCV_OFFSET;

        //* Channel offset (해당 셀 전후로 셀이 없는 경우 offset값
        private double[] _dIR_CHANNELOFFSET = new double[_Constant.ChannelCount];

        public string EQUIPMENTID { get => _sEQUIPMENTID; set => _sEQUIPMENTID = value; }
        public string TRAYID { get => _sTRAYID; set => _sTRAYID = value; }
        public string CELLTYPE { get => _sCELLTYPE; set => _sCELLTYPE = value; }
        public string BATCHID { get => _sBATCHID; set => _sBATCHID = value; }
        public string OLDBATCHID { get => _sOLDBATCHID; set => _sOLDBATCHID = value; }
        public string LOTID { get => _sLOTID; set => _sLOTID = value; }
        public string LOTNUMBER { get => _sLOTNUMBER; set => _sLOTNUMBER = value; }
        public string CELLSERIAL { get => _sCELLSERIAL; set => _sCELLSERIAL = value; }
        public int CELLCOUNT { get => _iCELLCOUNT; set => _iCELLCOUNT = value; }
        public bool FIRST { get => _bFIRST; set => _bFIRST = value; }
        public DateTime ARRIVETIME { get => _dtArriveTime; set => _dtArriveTime = value; }
        public DateTime STARTTIME { get => _dtStartTime; set => _dtStartTime = value; }
        public DateTime FINISHTIME { get => _dtFinishTime; set => _dtFinishTime = value; }
        public int[] CELL { get => _iCELL; set => _iCELL = value; }
        public double[] IR_ORIGINALVALUE { get => _dIR_ORIGINALVALUE; set => _dIR_ORIGINALVALUE = value; }
        public double[] IR_AFTERVALUE { get => _dIR_AFTERVALUE; set => _dIR_AFTERVALUE = value; }
        public double[] OCV { get => _dOCV; set => _dOCV = value; }
        public int[] MEASURERESULT { get => _iMEASURERESULT; set => _iMEASURERESULT = value; }
        public int REMEASURECELLCOUNT { get => _iREMEASURECELLCOUNT; set => _iREMEASURECELLCOUNT = value; }
        public int REMEASURECOUNT { get => _iREMEASURECOUNT; set => _iREMEASURECOUNT = value; }
        public bool REMEASURE { get => _bREMEASURE; set => _bREMEASURE = value; }
        public int[] CELLNGCOUNT { get => _iCellNgCount; set => _iCellNgCount = value; }
        public int[] IRRESULT { get => _iIRRESULT; set => _iIRRESULT = value; }
        public int[] OCVRESULT { get => _iOCVRESULT; set => _iOCVRESULT = value; }
        public Color[] CHANNELCOLOR { get => _clrCHANNEL; set => _clrCHANNEL = value; }
        public Color[] IRCOLOR { get => _clrIR; set => _clrIR = value; }
        public Color[] OCVCOLOR { get => _clrOCV; set => _clrOCV = value; }
        public double[] IR_STANDARD { get => _dIR_STANDARD; set => _dIR_STANDARD = value; }
        public double[] IR_MEASURE { get => _dIR_MEASURE; set => _dIR_MEASURE = value; }
        public double[] IR_OFFSET { get => _dIR_OFFSET; set => _dIR_OFFSET = value; }
        public double[] IR_OFFSETMEASUREAVERAGE { get => _dIR_OFFSETAVERAGE; set => _dIR_OFFSETAVERAGE = value; }
        public double OCV_OFFSET { get => _dOCV_OFFSET; set => _dOCV_OFFSET = value; }
        public double[] IR_CHANNELOFFSET { get => _dIR_CHANNELOFFSET; set => _dIR_CHANNELOFFSET = value; }
        public string[] CELLID { get => _sCELLID; set => _sCELLID = value; }
        /// <summary>
        /// MES Cell Status in Tray Info
        /// </summary>
        public string[] CELLSTATUSMES { get => _sCELLSTATUSMES; set => _sCELLSTATUSMES = value; }
        /// <summary>
        ///  IROCV Measuring Result
        /// </summary>
        public string[] CELLSTATUSIROCV { get => _sCELLSTATUSIROCV; set => _sCELLSTATUSIROCV = value; }
        public string TAGPATHNO { get => _sTAGPATHNO; set => _sTAGPATHNO = value; }
        public int MESRESULT { get => _iMESRESULT; set => _iMESRESULT = value; }
        public string TRAYSTATUSCODE { get => _sTRAYSTATUSCODE; set => _sTRAYSTATUSCODE = value; }
        public string ERRORCODE { get => _sERRORCODE; set => _sERRORCODE = value; }
        public string ERRORMESSAGE { get => _sERRORMESSAGE; set => _sERRORMESSAGE = value; }
        public string LOG{ get => _sLog; set => _sLog = value; }

        #endregion

        public IROCVData()
        {
            //*OFFSET(보정) : standard - 실측데이터, measure - irocv 로 측정한 값 
            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                _dIR_STANDARD[nIndex] = 0.0;
                _dIR_MEASURE[nIndex] = 0.0;
                _dIR_OFFSET[nIndex] = 0.0;
                _dIR_OFFSETAVERAGE[nIndex] = 0.0;
            }
            _dOCV_OFFSET = 0.0;
        }
        public void InitData()
        {
            _sTRAYID = string.Empty;
            _sTRAYSTATUSCODE = string.Empty;
            _sERRORCODE = string.Empty;
            _sERRORMESSAGE = string.Empty;
            _sCELLTYPE = string.Empty;
            _sBATCHID = string.Empty;
            _sOLDBATCHID = string.Empty;
            _sLOTID = string.Empty;
            _sLOTNUMBER = string.Empty;
            _sCELLSERIAL = string.Empty;
            _iCELLCOUNT = 0;
            _bFIRST = true;
            _bREMEASURE = false;
            _iMESRESULT = 0; //* 1 -> Tray Emission, 2 -> Tray Retry
            _iREMEASURECELLCOUNT = 0;
            _iREMEASURECOUNT = 0;

            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                _iCELL[nIndex] = 0;
                _sCELLID[nIndex] = string.Empty;
                _sCELLSTATUSMES[nIndex] = string.Empty;
                _sCELLSTATUSIROCV[nIndex] = string.Empty;
                _dIR_ORIGINALVALUE[nIndex] = 0.000;
                _dIR_AFTERVALUE[nIndex] = 0.000;
                _dOCV[nIndex] = 0.0;
                _iMEASURERESULT[nIndex] = 0;
                _iIRRESULT[nIndex] = 0;
                _iOCVRESULT[nIndex] = 0;
                _clrCHANNEL[nIndex] = Color.White;
                _clrIR[nIndex] = _Constant.ColorIR;
                _clrOCV[nIndex] = _Constant.ColorOCV;
            }
        }

        public void InitOffset()
        {
            for(int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                _dIR_STANDARD[nIndex] = 0.0;
                _dIR_MEASURE[nIndex] = 0.0;
                _clrCHANNEL[nIndex] = Color.White;
            }
        }
        
        public void SetTrayInfo(int[] iExist)
        {
            for (int i = 0; i < iExist.Length; i++)
                CELL[i] = iExist[i];
        }
        public void SetTrayInfoForManual()
        {
            for (int i = 0; i < _Constant.ChannelCount; i++)
                _iCELL[i] = 1;
        }

        public void SetValue(string param, string type, enumEquipMode equipMode)
        {
            string msg_ir = string.Empty;
            string msg_ocv = string.Empty;
            int channel = 0;
            double dValue = 0.0;
            int channelLength = 3;
            int checksumLength = 2;
            try
            {
                if (type == "IR")
                {
                    string[] values = param.Split(',');
                    channel = Convert.ToInt32(values[0].Substring(0, 3));
                    dValue = Convert.ToDouble(values[0].Substring(3, values[0].Length - 3));
                    
                    SetIrValue(channel, dValue * 1000.0, equipMode);
                }
                else if(type == "OCV")
                {
                    channel = Convert.ToInt32(param.Substring(0, 3));
                    dValue = Convert.ToDouble(param.Substring(3, param.Length - channelLength - checksumLength));
                    
                    SetOcvValue(channel, dValue * 1000.0, equipMode);
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void SetValue(string param, string type, enumEquipMode equipMode, IROCVData irocvdatatest)
        {
            string msg_ir = string.Empty;
            string msg_ocv = string.Empty;
            int channel = 0;
            double dValue = 0.0;
            int channelLength = 3;
            int checksumLength = 2;
            try
            {
                if (type == "IR")
                {
                    string[] values = param.Split(',');
                    channel = Convert.ToInt32(values[0].Substring(0, 3));
                    dValue = Convert.ToDouble(values[0].Substring(3, values[0].Length - 3));

                    SetIrValue(channel, dValue * 1000.0, equipMode);
                    //* for test 2024 06 03 임시로 랜덤값 저장
                    //SetIrValue(channel, irocvdatatest.IR_AFTERVALUE[channel - 1], equipMode);
                }
                else if (type == "OCV")
                {
                    channel = Convert.ToInt32(param.Substring(0, 3));
                    dValue = Convert.ToDouble(param.Substring(3, param.Length - channelLength - checksumLength));

                    SetOcvValue(channel, dValue * 1000.0, equipMode);
                    //* for test 2024 06 03 임시로 랜덤값 저장
                    //SetOcvValue(channel, irocvdatatest.OCV[channel - 1], equipMode);    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void SetIrValue(int channel, double ir, enumEquipMode equipMode)
        {
            int channel_no = _system.CHANNELMAPPING[channel] - 1;
            if(ir < 30)
            {
                _dIR_ORIGINALVALUE[channel_no] = ir;
                _dIR_AFTERVALUE[channel_no] = ir - _dIR_OFFSET[channel_no];
            }
            else
            {
                _dIR_ORIGINALVALUE[channel_no] = 0.0;
                _dIR_AFTERVALUE[channel_no] = 0.0;
            }

            //* IR 값을 측정을 했는지 확인
            _iIRRESULT[channel_no] = 1;

            if (equipMode == enumEquipMode.OFFSET)
            {
                if (_dIR_ORIGINALVALUE[channel_no] == 0) _clrIR[channel_no] = _Constant.ColorIRNG;
                else _clrIR[channel_no] = _Constant.ColorMeasure;
            }
            else
            {
                if (_dIR_AFTERVALUE[channel_no] == 0) 
                    _clrIR[channel_no] = _Constant.ColorIRNG;
                else if (_dIR_AFTERVALUE[channel_no] < _system.IRMIN || _dIR_AFTERVALUE[channel_no] > _system.IRMAX)
                    _clrIR[channel_no] = _Constant.ColorIRNG;
                else if (_dIR_AFTERVALUE[channel_no] < _system.IRREMEAMIN || _dIR_AFTERVALUE[channel_no] > _system.IRREMEAMAX)
                    _clrIR[channel_no] = _Constant.ColorIRNG;
                else 
                    _clrIR[channel_no] = _Constant.ColorIR;
            }
            
        }
        public void SetOcvValue(int channel, double ocv, enumEquipMode equipMode)
        {
            int channel_no = _system.CHANNELMAPPING[channel] - 1;
            if (ocv >= 10000) 
                _dOCV[channel_no] = 0.0;
            else
                _dOCV[channel_no] = ocv - _dOCV_OFFSET;

            //* OCV 값을 측정을 했는지 확인
            _iOCVRESULT[channel_no] = 1;

            //* 1000 mV는 임의로 설정해 놓은값. 셀이 없는데 전압이 뜨면 outflow
            if (equipMode == enumEquipMode.AUTO && CELL[channel_no] == 0 && _dOCV[channel_no] >= 1000)
                _clrOCV[channel_no] = _Constant.ColorOutFlow;
            //* spec 확인을 하기때문에 필요 없음
            //else if (_dOCV[channel_no] < 100)   
            //    _clrOCV[channel_no] = _Constant.ColorOCVNG;
            else if (_dOCV[channel_no] < _system.OCVMIN || _dOCV[channel_no] > _system.OCVMAX)
                _clrOCV[channel_no] = _Constant.ColorOCVNG;
            else if (_dOCV[channel_no] < _system.OCVREMEAMIN || _dOCV[channel_no] > _system.OCVREMEAMAX)
                _clrOCV[channel_no] = _Constant.ColorOCVNG;
            else
                _clrOCV[channel_no] = _Constant.ColorOCV;
        }
        //* Tray In 시간
        public void SetArriveTime()
        {
            _dtArriveTime = DateTime.Now;
        }
        //* Mes 에서 tray info 받은 시간
        public void SetStartTime()
        {
            _dtStartTime = DateTime.Now;
        }
        //* 측정 끝나고 결과 파일 작성 시간
        public void SetFinishTime()
        {
            _dtFinishTime = DateTime.Now;
        }
    }

    public class IRValue
    {
        private int channel;
        private double ir;

        public IRValue(int channel, double ir)
        {
            this.channel = channel;
            this.ir = ir;
        }

        public int CHANNEL { get => channel; set => channel = value; }
        public double IR { get => ir; set => ir = value; }
    }
    public class OCVValue
    {
        private int channel;
        private double ocv;
        public OCVValue(int channel, double ocv)
        {
            this.channel = channel;
            this.ocv = ocv;
        }
        public int CHANNEL { get => channel; set => channel = value; }
        public double OCV { get => ocv; set => ocv = value; }
    }
}
