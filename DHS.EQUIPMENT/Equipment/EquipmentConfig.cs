using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHS.EQUIPMENT.Common;

namespace DHS.EQUIPMENT
{
    class CEquipmentData
    {
        private Dictionary<int, int> _iChannelMapping = new Dictionary<int, int>();
        
        private string[] _sIPaddress = new string[_Constant.frmCount];
        private int[] _iPort = new int[_Constant.frmCount];
        
        private string _sPLCIpAddress;
        private int _iPLCDbNumber;

        private string _sMESIpAddress;
        private int _iMESPort;
        private string[] _sEquipmentID = new string[_Constant.frmCount];

        private int _iAutoRemeasureCount;
        private int _iRemeasureMaxCount;

        private int _iPLCChannelNo;
        private int _iPLCNetworkNumber;
        private int _iPLCStationNumber;

        private int[] _iRemeasureUse = new int[_Constant.ChannelCount];
        private int[] _iRemeasureNG = new int[_Constant.ChannelCount];

        private double _dOcvMinValue;
        private double _dIRMin;
        private double _dIRMax;
        private double _dIRRemeaMin;
        private double _dIRRemeaMax;
        private double _dOCVMin;
        private double _dOCVMax;
        private double _dOCVRemeaMin;
        private double _dOCVRemeaMax;
        private int _iMaxVoltage;
        private int _iMaxCurrent;
        private int _iMaxTime;
        private int _iVoltage;
        private int _iCurrent;
        private int _iTime;
        private int _iPreVoltage;
        private int _iPreCurrent;
        private int _iPreTime;
        private int _iDischargeVoltage;
        private int _iDischargeCurrent;
        private int _iDischargeTime;

        private string _sCell_Model;
        private string _sLot_Number;

        private int _iRemeasureAlarmCount;

        private bool _bLogAllChannel;

        private bool _bAgingUse;
        private int _iAgingTime;

        private string _sLineNo;

        public Dictionary<int, int> CHANNELMAPPING { get => _iChannelMapping; set => _iChannelMapping = value; }
        
        public string[] IPADDRESS { get => _sIPaddress; set => _sIPaddress = value; }
        public int[] PORT { get => _iPort; set => _iPort = value; }
        
        public string PLCIPADDRESS { get => _sPLCIpAddress; set => _sPLCIpAddress = value; }
        public int PLCDBNUMBER { get => _iPLCDbNumber; set => _iPLCDbNumber = value; }
        public string MESIPADDRESS { get => _sMESIpAddress; set => _sMESIpAddress = value; }
        public int MESPORT { get => _iMESPort; set => _iMESPort = value; }

        public string[] EQUIPMENTID { get => _sEquipmentID; set => _sEquipmentID = value; }

        public double OCVMINVALUE { get => _dOcvMinValue; set => _dOcvMinValue = value; }
        public int AUTOREMEASURECOUNT { get => _iAutoRemeasureCount; set => _iAutoRemeasureCount = value; }
        public int REMEASUREMAXCOUNT { get => _iRemeasureMaxCount; set => _iRemeasureMaxCount = value; }

        public int PLCCHANNELNO { get => _iPLCChannelNo; set => _iPLCChannelNo = value; }
        public int PLCNETWORKNUMBER { get => _iPLCNetworkNumber; set => _iPLCNetworkNumber = value; }
        public int PLCSTATIONNUMBER { get => _iPLCStationNumber; set => _iPLCStationNumber = value; }

        public int IMaxVoltage { get => _iMaxVoltage; set => _iMaxVoltage = value; }
        public int IMaxCurrent { get => _iMaxCurrent; set => _iMaxCurrent = value; }
        public int IMaxTime { get => _iMaxTime; set => _iMaxTime = value; }
        public int IVoltage { get => _iVoltage; set => _iVoltage = value; }
        public int ICurrent { get => _iCurrent; set => _iCurrent = value; }
        public int ITime { get => _iTime; set => _iTime = value; }
        public string SCell_Model { get => _sCell_Model; set => _sCell_Model = value; }
        public string SLot_Number { get => _sLot_Number; set => _sLot_Number = value; }
        public int IRemeasureAlarmCount { get => _iRemeasureAlarmCount; set => _iRemeasureAlarmCount = value; }
        
        public bool BLOGALLCHANNEL { get => _bLogAllChannel; set => _bLogAllChannel = value; }
        public bool BAgingUse { get => _bAgingUse; set => _bAgingUse = value; }
        public int IAgingTime { get => _iAgingTime; set => _iAgingTime = value; }

        public string SLINENO { get => _sLineNo; set => _sLineNo = value; }
        public int IPREVOLTAGE { get => _iPreVoltage; set => _iPreVoltage = value; }
        public int IPRECURRENT { get => _iPreCurrent; set => _iPreCurrent = value; }
        public int IPRETIME { get => _iPreTime; set => _iPreTime = value; }
        public int IDischargeVoltage { get => _iDischargeVoltage; set => _iDischargeVoltage = value; }
        public int IDischargeCurrent { get => _iDischargeCurrent; set => _iDischargeCurrent = value; }
        public int IDischargeTime { get => _iDischargeTime; set => _iDischargeTime = value; }
        public int[] REMEASUREUSE { get => _iRemeasureUse; set => _iRemeasureUse = value; }
        public int[] REMEASURENG { get => _iRemeasureNG; set => _iRemeasureNG = value; }
        public double IRMIN { get => _dIRMin; set => _dIRMin = value; }
        public double IRMAX { get => _dIRMax; set => _dIRMax = value; }
        public double IRREMEAMIN { get => _dIRRemeaMin; set => _dIRRemeaMin = value; }
        public double IRREMEAMAX { get => _dIRRemeaMax; set => _dIRRemeaMax = value; }
        public double OCVMIN { get => _dOCVMin; set => _dOCVMin = value; }
        public double OCVMAX { get => _dOCVMax; set => _dOCVMax = value; }
        public double OCVREMEAMIN { get => _dOCVRemeaMin; set => _dOCVRemeaMin = value; }
        public double OCVREMEAMAX { get => _dOCVRemeaMax; set => _dOCVRemeaMax = value; }

        private static CEquipmentData equipmentdata;

        public static CEquipmentData GetInstance()
        {
            if (equipmentdata == null) equipmentdata = new CEquipmentData();
            return equipmentdata;
        }

        public CEquipmentData()
        {
            
        }

    }
}
