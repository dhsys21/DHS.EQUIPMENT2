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

        #region IR/OCV DATA - stage
        private string _sTRAYID;
        private string _sCELLTYPE;
        private string _sBATCHID;
        private string _sOLDBATCHID;
        private string _sLOTID;
        private string _sLOTNUMBER;
        private string _sCELLSERIAL;
        private int iCELLCOUNT;
        private bool _bFIRST;
        private double _dIRMin;
        private double _dIRMax;
        private double _dOCVMin;
        private double _dOCVMax;
        private DateTime _dtArriveTime;
        private DateTime _dtFinishTime;

        //* CELL 정보 0 - no cell, 1 - cell exist
        private int[] _iCELL = new int[_Constant.ChannelCount];
        
        private double[] _dIR_ORIGINALVALUE = new double[_Constant.ChannelCount];
        private double[] _dIR_AFTERVALUE = new double[_Constant.ChannelCount];
        private double[] _dOCV = new double[_Constant.ChannelCount];
        //* Measure Result 0 - OK, 2 - IR Error, 3 - OCV Error
        private int[] _iMEASURERESULT = new int[_Constant.ChannelCount];
        //* Remeasure Cell Count
        private int _iREMEASURECELLCOUNT;
        //* Remeasure Mode true - remeasure ng channel, false - remeausre all or test all
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

        public string TRAYID { get => _sTRAYID; set => _sTRAYID = value; }
        public string CELLTYPE { get => _sCELLTYPE; set => _sCELLTYPE = value; }
        public string BATCHID { get => _sBATCHID; set => _sBATCHID = value; }
        public string OLDBATCHID { get => _sOLDBATCHID; set => _sOLDBATCHID = value; }
        public string LOTID { get => _sLOTID; set => _sLOTID = value; }
        public string LOTNUMBER { get => _sLOTNUMBER; set => _sLOTNUMBER = value; }
        public string CELLSERIAL { get => _sCELLSERIAL; set => _sCELLSERIAL = value; }
        public int CELLCOUNT { get => iCELLCOUNT; set => iCELLCOUNT = value; }
        public bool FIRST { get => _bFIRST; set => _bFIRST = value; }
        public DateTime ARRIVETIME { get => _dtArriveTime; set => _dtArriveTime = value; }
        public DateTime FINISHTIME { get => _dtFinishTime; set => _dtFinishTime = value; }
        public int[] CELL { get => _iCELL; set => _iCELL = value; }
        public double[] IR_ORIGINALVALUE { get => _dIR_ORIGINALVALUE; set => _dIR_ORIGINALVALUE = value; }
        public double[] IR_AFTERVALUE { get => _dIR_AFTERVALUE; set => _dIR_AFTERVALUE = value; }
        public double[] OCV { get => _dOCV; set => _dOCV = value; }
        public int[] MEASURERESULT { get => _iMEASURERESULT; set => _iMEASURERESULT = value; }
        public int REMEASURECELLCOUNT { get => _iREMEASURECELLCOUNT; set => _iREMEASURECELLCOUNT = value; }
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
        public double IRMin { get => _dIRMin; set => _dIRMin = value; }
        public double IRMax { get => _dIRMax; set => _dIRMax = value; }
        public double OCVMin { get => _dOCVMin; set => _dOCVMin = value; }
        public double OCVMax { get => _dOCVMax; set => _dOCVMax = value; }

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
            _sCELLTYPE = string.Empty;
            _sBATCHID = string.Empty;
            _sOLDBATCHID = string.Empty;
            _sLOTID = string.Empty;
            _sLOTNUMBER = string.Empty;
            _sCELLSERIAL = string.Empty;
            iCELLCOUNT = 0;
            _bFIRST = true;
            _bREMEASURE = false;

            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                _iCELL[nIndex] = 1;
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
                    
                    SetOcvValue(channel, dValue * 1000.0);
                }

            }
            catch(Exception ex)
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

            if(equipMode == enumEquipMode.OFFSET)
            {
                if (_dIR_ORIGINALVALUE[channel_no] == 0) _clrIR[channel_no] = _Constant.ColorIRNG;
                else _clrIR[channel_no] = _Constant.ColorMeasure;
            }
            else
            {
                if (_dIR_AFTERVALUE[channel_no] == 0) _clrIR[channel_no] = _Constant.ColorIRNG;
                else _clrIR[channel_no] = _Constant.ColorIR;
            }
            
        }
        public void SetOcvValue(int channel, double ocv)
        {
            int channel_no = _system.CHANNELMAPPING[channel] - 1;
            if (ocv == 10000) _dOCV[channel_no] = 0.0;
            else
            _dOCV[channel_no] = ocv - _dOCV_OFFSET;

            //* OCV 값을 측정을 했는지 확인
            _iOCVRESULT[channel_no] = 1;

            if (_dOCV[channel_no] < 100) _clrOCV[channel_no] = _Constant.ColorOCVNG;
            
            if(CELL[channel_no] == 0 && _dOCV[channel_no] >= _dOCVMin)
            {
                _clrOCV[channel_no] = _Constant.ColorOutFlow;
            }
        }
        public void SetIRMinMax(double dMin, double dMax)
        {
            _dIRMin = dMin;
            _dIRMax = dMax;
        }
        public void SetOCVMinMax(double dMin, double dMax)
        {
            _dOCVMin = dMin;
            _dOCVMax = dMax;
        }
        public void SetArriveTime()
        {
            _dtArriveTime = DateTime.Now;
        }
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
