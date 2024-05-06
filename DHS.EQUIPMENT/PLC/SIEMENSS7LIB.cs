using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHS.EQUIPMENT.Common;
using S7.Net;
using S7.Net.Internal;
using S7.Net.Protocol;
using S7.Net.Types;

namespace DHS.EQUIPMENT
{
    class SIEMENSS7LIB
    {
        public static bool connection = false;
        private bool _bconnected = false;
        public static string _strHost;
        public static int _iDB_Number;
        public bool isRead = false;
        public bool isWrite = false;
        const int Rack = 0;
        const int Slot = 1;
        public static Plc SIEMENSPLC;
        public static object plcvalue = string.Empty;

        private object[] _objPlcValues = new object[_Constant.PLC_DATA_LENGTH];
        private object[] _objPcValues = new object[_Constant.PC_DATA_LENGTH];

        public bool CONNECTED { get => _bconnected; set => _bconnected = value; }

        #region - PLC DATA
        private List<string> _PlcTagNames = new List<string>();
        public List<string> PLCTAGNAMES { get => _PlcTagNames; set => _PlcTagNames = value; }
        private Dictionary<string, object> _dPlcValues = new Dictionary<string, object>();
        //public Dictionary<string, object> PLCVALUES { get => _dPlcValues; set => _dPlcValues = value; }
        public object[] PLCVALUES { get => _objPlcValues; set => _objPlcValues = value; }
        public object[] PCVALUES { get => _objPcValues; set => _objPcValues = value; }

        private int _iPlcHeartBeat;
        private int _iPlcAutoManual;
        private int _iPlcError;
        private int _iPlcTrayIn;
        private int _iPlcTrayDown;
        private int _iPlcTrayUp;
        private int _iPlcJobChange;
        private int _iPlcReadyComplete;
        private int _iPlcUnloadingComplete;
        private string _sPlcTrayId;

        private int _iPcHeartBeat;
        private int _iPcAutoManual;
        private int _iPcError;
        private int _iPcTrayOut;
        private int _iPcTrayDown;
        private int _iPcTrayUp;
        private int _iPcMeasurementWait;
        private int _iPcRunning;
        private int _iPcMeasurementComplete;

        public int PLCHEARTBEAT { get => _iPlcHeartBeat; set => _iPlcHeartBeat = value; }
        public int PLCAUTOMANUAL { get => _iPlcAutoManual; set => _iPlcAutoManual = value; }
        public int PLCERROR { get => _iPlcError; set => _iPlcError = value; }
        public int PLCTRAYIN { get => _iPlcTrayIn; set => _iPlcTrayIn = value; }
        public int PLCTRAYDOWN { get => _iPlcTrayDown; set => _iPlcTrayDown = value; }
        public int PLCTRAYUP { get => _iPlcTrayUp; set => _iPlcTrayUp = value; }
        public int PLCJOBCHANGE { get => _iPlcJobChange; set => _iPlcJobChange = value; }
        public int PLCREADYCOMPLETE { get => _iPlcReadyComplete; set => _iPlcReadyComplete = value; }
        public int PLCUNLOADINGCOMPLETE { get => _iPlcUnloadingComplete; set => _iPlcUnloadingComplete = value; }
        public string PLCTRAYID { get => _sPlcTrayId; set => _sPlcTrayId = value; }
        public int PCHEARTBEAT { get => _iPcHeartBeat; set => _iPcHeartBeat = value; }
        public int PCAUTOMANUAL { get => _iPcAutoManual; set => _iPcAutoManual = value; }
        public int PCERROR { get => _iPcError; set => _iPcError = value; }
        public int PCTRAYOUT { get => _iPcTrayOut; set => _iPcTrayOut = value; }
        public int PCTRAYDOWN { get => _iPcTrayDown; set => _iPcTrayDown = value; }
        public int PCTRAYUP { get => _iPcTrayUp; set => _iPcTrayUp = value; }
        public int PCMEASUREMENTWAIT { get => _iPcMeasurementWait; set => _iPcMeasurementWait = value; }
        public int PCRUNNING { get => _iPcRunning; set => _iPcRunning = value; }
        public int PCMEASUREMENTCOMPLETE { get => _iPcMeasurementComplete; set => _iPcMeasurementComplete = value; }

        #endregion

        static System.Windows.Forms.Timer _tmrPLCRead = new System.Windows.Forms.Timer();
        static System.Windows.Forms.Timer _tmrPLCWrite = new System.Windows.Forms.Timer();
        static System.Windows.Forms.Timer _tmrCheckHeartBeat = new System.Windows.Forms.Timer();

        private static SIEMENSS7LIB siemenss7lib = new SIEMENSS7LIB();
        public static SIEMENSS7LIB GetInstance()
        {
            if (siemenss7lib == null) siemenss7lib = new SIEMENSS7LIB();
            return siemenss7lib;
        }

        public SIEMENSS7LIB(string HOST, int DB_NUMBER)
        {
            _strHost = HOST;
            _iDB_Number = DB_NUMBER;

            _tmrPLCRead.Interval = 1000;
            _tmrPLCRead.Tick += new EventHandler(PLCReadTimer_Tick);
            _tmrPLCWrite.Interval = 1000;
            _tmrPLCWrite.Tick += new EventHandler(PLCWriteTimer_Tick);

            _tmrCheckHeartBeat.Interval = 1000;
            _tmrCheckHeartBeat.Tick += new EventHandler(CheckHeartBeatTimer_Tick);
            _tmrCheckHeartBeat.Enabled = true;

            SetPlcTagName();
            ConnectPLCAsync();
            
            if(connection == true)
            {
                _bconnected = true;
                isRead = true;
                isWrite = false;

                //* PLC Timer
                _tmrPLCRead.Enabled = true;

                _tmrPLCWrite.Enabled = true;
            }
            else
                _bconnected = false;
        }
        public SIEMENSS7LIB()
        {

        }

        private void SetPlcTagName()
        {
            //* PLC Tag Names
            _PlcTagNames.Add("MW10000");
            _PlcTagNames.Add("MW10002");
            _PlcTagNames.Add("MW10004");
            _PlcTagNames.Add("MW10006");
            _PlcTagNames.Add("MW10008");
            _PlcTagNames.Add("MW10010");
            _PlcTagNames.Add("MW10012");
            _PlcTagNames.Add("MW10014");
            _PlcTagNames.Add("MW10016");
            _PlcTagNames.Add("MW10020");

            //* PC Tag Names
            _PlcTagNames.Add("MW11000");
            _PlcTagNames.Add("MW11002");
            _PlcTagNames.Add("MW11004");
            _PlcTagNames.Add("MW11006");
            _PlcTagNames.Add("MW11008");
            _PlcTagNames.Add("MW11010");
            _PlcTagNames.Add("MW11012");
            _PlcTagNames.Add("MW11014");
            _PlcTagNames.Add("MW11016");

            foreach (var tagname in _PlcTagNames)
            {
                _dPlcValues.Add(tagname, 0);
            }
        }

        #region PLC Connection
        public static void ChangeSetting(string HOST, int DB_NUMBER)
        {
            _strHost = HOST;
            _iDB_Number = DB_NUMBER;

            ConnectPLCAsync();
        }
        int iNoHeartBeatCount = 0;
        private void CheckHeartBeatTimer_Tick(object sender, EventArgs e)
        {
            if (SIEMENSPLC != null && SIEMENSPLC.IsConnected == false)
            {
                iNoHeartBeatCount++;
                if (iNoHeartBeatCount >= 2 && iNoHeartBeatCount < 5)
                {
                    DisconnectPLC();
                    iNoHeartBeatCount = 10;
                }

                if (iNoHeartBeatCount >= 8)
                {
                    ConnectPLCAsync();
                    iNoHeartBeatCount = 5;
                }
            }
            else
            {
                iNoHeartBeatCount = 0;
            }
        }
        public static async Task<bool> ConnectPLCAsync()
        {
            try
            {
                SIEMENSPLC = new Plc(CpuType.S71500, _strHost, Rack, Slot);
                //SIEMENSPLC.Open();
                await SIEMENSPLC.OpenAsync();

                if (SIEMENSPLC.IsConnected)
                {
                    _tmrPLCRead.Enabled = true;
                    _tmrPLCWrite.Enabled = true;
                    connection = true;
                    return true;
                }
            }
            catch (Exception ex)
            {
                //connection = false;
                //_tmrPLCRead.Enabled = false;
                //_tmrPLCWriteHeartBeat.Enabled = false;
                Console.WriteLine(ex.ToString());
            }

            return false;
        }
        public static void DisconnectPLC()
        {
            if (connection)
            {
                SIEMENSPLC.Close();
                connection = false;

                _tmrPLCRead.Enabled = false;
                _tmrPLCWrite.Enabled = false;
            }
        }
        #endregion

        #region PLC Action
        public void PLC_Initialize(int stageno)
        {
            SetPCError(stageno, 0);
            SetTrayOut(stageno, 0);
            SetTrayDown(stageno, 0);
            SetTrayUp(stageno, 0);
            SetMeasurementWait(stageno, 0);
            SetMeasurementRunning(stageno, 0);
            SetMeasurementComplete(stageno, 0);
        }
        public void PLC_DBInitialize(int stageno)
        {
            _iPcError = 0;
            _iPcTrayOut = 0;
            _iPcTrayDown = 0;
            _iPcTrayUp = 0;
            _iPcMeasurementWait = 0;
            _iPcRunning = 0;
            _iPcMeasurementComplete = 0;
        }
        public void SetHeartBeat(int stageno, int iValue)
        {
            isRead = false;
            _iPcHeartBeat = iValue;
            //string tagname = "MW11000"; // probe close
            //ushort val = Convert.ToUInt16(iValue);
            //SIEMENSS7LIB.WritePLC(tagname, val);
        }
        public void SetAutoMode(int stageno, int iValue)
        {
            _iPcAutoManual = iValue;

            //string tagname = "MW11002"; // probe close
            //ushort val = Convert.ToUInt16(iValue);
            //SIEMENSS7LIB.WritePLC(tagname, val);
        }
        public void SetPCError(int stageno, int iValue)
        {
            _iPcError = iValue;

            //string tagname = "MW11004"; // probe close
            //ushort val = Convert.ToUInt16(iValue);
            //SIEMENSS7LIB.WritePLC(tagname, val);
        }
        public void SetTrayOut(int stageno, int iValue)
        {
            isRead = false;
            _iPcTrayOut = iValue;
            //string tagname = "MW11006"; // probe close
            //ushort val = Convert.ToUInt16(iValue);
            //SIEMENSS7LIB.WritePLC(tagname, val);
        }
        public void SetTrayDown(int stageno, int iValue)
        {
            isRead = false;
            _iPcTrayDown = iValue;
           
            //string tagname = "MW11008"; // probe open
            //ushort val = Convert.ToUInt16(iValue);
            //SIEMENSS7LIB.WritePLC(tagname, val);
        }
        public void SetTrayUp(int stageno, int iValue)
        {
            isRead = false;
            _iPcTrayUp = iValue;

            //string tagname = "MW11010"; // probe close
            //ushort val = Convert.ToUInt16(iValue);
            //SIEMENSS7LIB.WritePLC(tagname, val);
        }

        public void SetMeasurementWait(int stageno, int iValue)
        {
            _iPcMeasurementWait = iValue;

            //string tagname = "MW11012"; // probe close
            //ushort val = Convert.ToUInt16(iValue);
            //SIEMENSS7LIB.WritePLC(tagname, val);
        }
        public void SetMeasurementRunning(int stageno, int iValue)
        {
            _iPcRunning = iValue;

            //string tagname = "MW11014"; // probe close
            //ushort val = Convert.ToUInt16(iValue);
            //SIEMENSS7LIB.WritePLC(tagname, val);
        }
        public void SetMeasurementComplete(int stageno, int iValue)
        {
            _iPcMeasurementComplete = iValue;

            //string tagname = "MW11016"; // probe close
            //ushort val = Convert.ToUInt16(iValue);
            //SIEMENSS7LIB.WritePLC(tagname, val);
        }
        #endregion

        #region PLC Read / Write Timer
        ushort ushortHeartBeat = 0;
        private void PLCWriteTimer_Tick(object sender, EventArgs e)
        {
            //WritePLC("MW11000", ushortHeartBeat);
            if(isRead == false)
            {
                WritePLCDBTimer();
                isRead = true;
            }
        }

        private void PLCReadTimer_Tick(object sender, EventArgs e)
        {
            //* Task 사용
            //Task.Factory.StartNew(new Action<object>(ReadPLCTagThread), null);
            //ReadPLCTagTimer(null);
            if(isRead == true)
            {
                ReadPLCDBTimer();
                isRead = false;
            }
                
        }
        #endregion

        #region PLC Read / Write Value
        //* PLC Read
        private void ReadPLCTagThread(object obj)
        {
            Stopwatch sw;
            //string tagname = string.Empty;
            while (isRead)
            {
                try
                {
                    //for(int nIndex = 0; nIndex < 9; nIndex++)
                    //{
                    //    tagname = "MW" + (11000 + nIndex).ToString();
                    //    plcvalue = SIEMENSPLC.Read(tagname);
                    //    SetPLCData(tagname, plcvalue);
                    //    Console.WriteLine(plcvalue);
                    //}
                    sw = new Stopwatch();
                    sw.Start();
                    foreach (var tagname in _PlcTagNames)
                    {
                        plcvalue = SIEMENSPLC.Read(tagname);
                        _dPlcValues[tagname] = plcvalue;// .Add(tagname, plcvalue);
                        SetPLCData(tagname, plcvalue);
                        //Console.WriteLine(plcvalue);
                    }
                    Console.WriteLine(sw.ElapsedMilliseconds.ToString() + " ms");
                    isRead = false;
                }
                catch (Exception ex)
                {
                    plcvalue = ex.ToString();
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        private void ReadPLCTagTimer(object obj)
        {
            Stopwatch sw;
            try
            {
                sw = new Stopwatch();
                sw.Start();
                foreach (var tagname in _PlcTagNames)
                {
                    plcvalue = SIEMENSPLC.Read(tagname);
                    _dPlcValues[tagname] = plcvalue;// .Add(tagname, plcvalue);
                    SetPLCData(tagname, plcvalue);
                    //Console.WriteLine(plcvalue);
                }
                plcvalue = SIEMENSS7LIB.ReadString("MB10020", 20);
                SetPLCData("MB10020", plcvalue);
                Console.WriteLine(sw.ElapsedMilliseconds.ToString() + " ms");
                isRead = false;
            }
            catch (Exception ex)
            {
                plcvalue = ex.ToString();
                Console.WriteLine(ex.ToString());
            }
        }
        private void ReadPLCDBTimer()
        {
            Stopwatch sw;
            byte[] plcValues = new byte[_Constant.PLC_DATA_LENGTH];
            byte[] pcValues = new byte[_Constant.PC_DATA_LENGTH];
            try
            {
                sw = new Stopwatch();
                sw.Start();

                pcValues = SIEMENSPLC.ReadBytes(DataType.DataBlock, _iDB_Number, 0, _Constant.PC_DATA_LENGTH);
                SetPCData(pcValues);

                plcValues = SIEMENSPLC.ReadBytes(DataType.DataBlock, _iDB_Number, 52, _Constant.PLC_DATA_LENGTH);
                SetPLCData(plcValues);

                Console.WriteLine(sw.ElapsedMilliseconds.ToString() + " ms");
                isRead = false;
            }
            catch (Exception ex)
            {
                plcvalue = ex.ToString();
                Console.WriteLine(ex.ToString());
            }
        }
        private void SetPLCData(byte[] values)
        {
            int[] plc_result = Array.ConvertAll(values, Convert.ToInt32);

            _iPlcHeartBeat = plc_result[_Constant.PLC_HEART_BEAT];
            _iPlcAutoManual = plc_result[_Constant.PLC_ATUO_MANUAL];
            _iPlcError = plc_result[_Constant.PLC_ERROR];
            _iPlcTrayIn = plc_result[_Constant.PLC_TRAY_IN];
            _iPlcTrayDown = plc_result[_Constant.PLC_TRAY_DOWN];
            _iPlcTrayUp = plc_result[_Constant.PLC_TRAY_UP];
            _iPlcJobChange = plc_result[_Constant.PLC_JOB_CHANGE];
            _iPlcReadyComplete = plc_result[_Constant.PLC_READY_COMPLETE];
            _iPlcUnloadingComplete = plc_result[_Constant.PLC_UNLOADING_COMPLETE];

            //* Tray ID
            byte[] trayid = new byte[_Constant.TRAY_ID_LENGTH];
            for (int nIndex = 0; nIndex < _Constant.TRAY_ID_LENGTH; nIndex++)
                trayid[nIndex] = values[_Constant.PLC_TRAY_ID + nIndex];

            _sPlcTrayId = Encoding.Default.GetString(trayid);

            //* PLC Values에 넣기
            for (int i = 0; i < plc_result.Length; i++)
                _objPlcValues[i] = plc_result[i];
            _objPlcValues[_Constant.PLC_TRAY_ID] = _sPlcTrayId;
        }
        private void SetPCData(byte[] values)
        {
            int[] pc_result = Array.ConvertAll(values, Convert.ToInt32);
            _iPcHeartBeat = pc_result[_Constant.PC_HEART_BEAT];
            _iPcAutoManual = pc_result[_Constant.PC_AUTO_MANUAL];
            _iPcError = pc_result[_Constant.PC_ERROR];
            _iPcTrayOut = pc_result[_Constant.PC_TRAY_OUT];
            _iPcTrayDown = pc_result[_Constant.PC_TRAY_DOWN];
            _iPcTrayUp = pc_result[_Constant.PC_TRAY_UP];
            _iPcMeasurementWait = pc_result[_Constant.PC_MEASUREMENT_WAIT];
            _iPcRunning = pc_result[_Constant.PC_RUNNING];
            _iPcMeasurementComplete = pc_result[_Constant.PC_MEASUREMENT_COMPLETE];

            //* PC Values에 넣기
            for (int i = 0; i < pc_result.Length; i++)
                _objPcValues[i] = pc_result[i];
        }
        private void SetPLCData(string tagname, object value)
        {
            switch (tagname)
            {
                case "MW10000":
                    _iPlcHeartBeat = Convert.ToUInt16(value);
                    break;
                case "MW10002":
                    _iPlcAutoManual = Convert.ToUInt16(value);
                    break;
                case "MW10004":
                    _iPlcError = Convert.ToUInt16(value);
                    break;
                case "MW10006":
                    _iPlcTrayIn = Convert.ToUInt16(value);
                    break;
                case "MW10008":
                    _iPlcTrayDown = Convert.ToUInt16(value);
                    break;
                case "MW10010":
                    _iPlcTrayUp = Convert.ToUInt16(value);
                    break;
                case "MW10012":
                    _iPlcJobChange = Convert.ToUInt16(value);
                    break;
                case "MW10014":
                    _iPlcReadyComplete = Convert.ToUInt16(value);
                    break;
                case "MW10016":
                    _iPlcUnloadingComplete = Convert.ToUInt16(value);
                    break;
                case "MB10020":
                    _sPlcTrayId = value.ToString();
                    break;
                case "MW11000":
                    _iPcHeartBeat = Convert.ToUInt16(value);
                    break;
                case "MW11002":
                    _iPcAutoManual = Convert.ToUInt16(value);
                    break;
                case "MW11004":
                    _iPcError = Convert.ToUInt16(value);
                    break;
                case "MW11006":
                    _iPcTrayOut = Convert.ToUInt16(value);
                    break;
                case "MW11008":
                    _iPcTrayDown = Convert.ToUInt16(value);
                    break;
                case "MW11010":
                    _iPcTrayUp = Convert.ToUInt16(value);
                    break;
                case "MW11012":
                    _iPcMeasurementWait = Convert.ToUInt16(value);
                    break;
                case "MW11014":
                    _iPcRunning = Convert.ToUInt16(value);
                    break;
                case "MW11016":
                    _iPcMeasurementComplete = Convert.ToUInt16(value);
                    break;
                default:
                    break;
            }
        }
        public static void ReadPLC(string tagname)
        {
            try
            {
                plcvalue = SIEMENSPLC.Read(tagname);
                Console.WriteLine(plcvalue);
            }
            catch (Exception ex)
            {
                plcvalue = ex.ToString();
                Console.WriteLine(ex.ToString());
            }
        }
        public static string ReadString(string tagname, int tagcount)
        {
            string tagname_db = tagname.Substring(0, 2);
            string tagname_index = tagname.Substring(2, 5);
            int tIndex = Convert.ToInt32(tagname_index);
            object objValue;
            byte[] bytevalues = new byte[tagcount];
            string trayid = string.Empty;
            try
            {
                for (int i = 0; i < tagcount; i++)
                {
                    objValue = SIEMENSPLC.Read(tagname_db + (tIndex + i).ToString());
                    bytevalues[i] = Convert.ToByte(objValue);

                }
                trayid = Encoding.Default.GetString(bytevalues);
            }
            catch (Exception ex)
            {
                plcvalue = ex.ToString();
                Console.WriteLine(ex.ToString());
            }

            return trayid;
        }

        //* PLC Write
        private void WritePLCDBTimer()
        {
            byte[] pcValues = new byte[_Constant.PC_DATA_LENGTH];
            _iPcHeartBeat = _iPcHeartBeat == 0 ? 1 : 0;
            pcValues[_Constant.PC_HEART_BEAT] = Convert.ToByte(_iPcHeartBeat);
            pcValues[_Constant.PC_AUTO_MANUAL] = Convert.ToByte(_iPcAutoManual);
            pcValues[_Constant.PC_ERROR] = Convert.ToByte(_iPcError);
            pcValues[_Constant.PC_TRAY_OUT] = Convert.ToByte(_iPcTrayOut);
            pcValues[_Constant.PC_TRAY_DOWN] = Convert.ToByte(_iPcTrayDown);
            pcValues[_Constant.PC_TRAY_UP] = Convert.ToByte(_iPcTrayUp);
            pcValues[_Constant.PC_MEASUREMENT_WAIT] = Convert.ToByte(_iPcMeasurementWait);
            pcValues[_Constant.PC_RUNNING] = Convert.ToByte(_iPcRunning);
            pcValues[_Constant.PC_MEASUREMENT_COMPLETE] = Convert.ToByte(_iPcMeasurementComplete);

            Stopwatch sw;
            try
            {
                sw = new Stopwatch();
                sw.Start();

                SIEMENSPLC.WriteBytes(DataType.DataBlock, _iDB_Number, 0, pcValues);

                Console.WriteLine(sw.ElapsedMilliseconds.ToString() + " ms");
            }
            catch (Exception ex)
            {
                plcvalue = ex.ToString();
                Console.WriteLine(ex.ToString());
            }
        }
        public static void WritePLC(string tagname, ushort value)
        {
            try
            {
                SIEMENSPLC.Write(tagname, value);

                //SIEMENSPLC.WriteBytes(DataType.DataBlock, 0, )
            }
            catch (Exception ex)
            {
                plcvalue = ex.ToString();
                Console.WriteLine(ex.ToString());
            }
        }
        public static void WritePLC(string tagname, byte value)
        {
            try
            {
                SIEMENSPLC.Write(tagname, value);

                //SIEMENSPLC.WriteBytes(DataType.DataBlock, 0, )
            }
            catch (Exception ex)
            {
                plcvalue = ex.ToString();
                Console.WriteLine(ex.ToString());
            }
        }
        #endregion

        private static string hex2str(string hex)
        {
            string result = "";
            foreach (Match m in (new Regex(@"[0-9A-F]{2,2}", RegexOptions.IgnoreCase)).Matches(hex))
                result += ((char)Convert.ToInt32(m.Value, 16)).ToString();
            return result;
        }
    }
}
