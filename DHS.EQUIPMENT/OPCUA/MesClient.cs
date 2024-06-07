using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHS.EQUIPMENT.Common;
using DHS.EQUIPMENT.PLC;
using Opc.Ua;
using OPCUACLIENT;
using static DHS.EQUIPMENT.MesClient;
using static Telerik.WinControls.UI.ValueMapper;

namespace DHS.EQUIPMENT
{
    public class MesClient
    {
        public static bool connection = false;
        public bool isRead = false;
        OPCUACLIENT.OPCUACLIENT opcclient = null;
        IROCVData[] irocvdata = new IROCVData[_Constant.frmCount];

        private string _strLog;

        private string[] mesData = new string[72];
        private string[] pcData = new string[172];

        private int _iMesSequenceNo;
        private int _iMesAcknowledgeNo;

        private int _iMesErrorCode;
        private string _strMesErrorMsg;

        private string _strMesEquipmentID;
        private string _strMesTrayID;
        private string _strMesTrayStatusCode;
        private string[] _strMesCellIDs;
        private string[] _strMesCellStatus;

        private int _iPCSequenceNo;
        private int _iPCAcknowledgeNo;
        private string _strPCEquipmentID;
        private string _strPCTrayID;
        private string[] _strPCCellIDs;
        private string[] _strPCCellStatus;
        private int[] _iPCIRs;
        private int[] _iPCOCVs;
        private float[] _fPCIRs;
        private float[] _fPCOCVs;

        public int MESSEQUENCENO { get => _iMesSequenceNo; set => _iMesSequenceNo = value; }
        public int MESACKNOWLEDGENO { get => _iMesAcknowledgeNo; set => _iMesAcknowledgeNo = value; }
        public string MESEQUIPMENTID { get => _strMesEquipmentID; set => _strMesEquipmentID = value; }
        public string MESTRAYID { get => _strMesTrayID; set => _strMesTrayID = value; }
        public string[] MESCELLIDS { get => _strMesCellIDs; set => _strMesCellIDs = value; }
        public string[] MESCELLSTATUS { get => _strMesCellStatus; set => _strMesCellStatus = value; }
        public int PCSEQUENCENO { get => _iPCSequenceNo; set => _iPCSequenceNo = value; }
        public int PCACKNOWLEDGENO { get => _iPCAcknowledgeNo; set => _iPCAcknowledgeNo = value; }
        public string PCEQUIPMENTID { get => _strPCEquipmentID; set => _strPCEquipmentID = value; }
        public string PCTRAYID { get => _strPCTrayID; set => _strPCTrayID = value; }
        public string[] PCCELLIDS { get => _strPCCellIDs; set => _strPCCellIDs = value; }
        public string[] PCCELLSTATUS { get => _strPCCellStatus; set => _strPCCellStatus = value; }
        //public int[] PCIRS { get => _iPCIRs; set => _iPCIRs = value; }
        //public int[] PCOCVS { get => _iPCOCVs; set => _iPCOCVs = value; }
        public int MESERRORCODE { get => _iMesErrorCode; set => _iMesErrorCode = value; }
        public string MESERRORMSG { get => _strMesErrorMsg; set => _strMesErrorMsg = value; }
        public string MESTRAYSTATUSCODE { get => _strMesTrayStatusCode; set => _strMesTrayStatusCode = value; }
        public float[] PCIRS { get => _fPCIRs; set => _fPCIRs = value; }
        public float[] PCOCVS { get => _fPCOCVs; set => _fPCOCVs = value; }

        static System.Windows.Forms.Timer _tmrMESRead = new System.Windows.Forms.Timer();
        
        public delegate void SetDataToDgv(string[] pcValues, string[] mesValues);
        public event SetDataToDgv OnSetDataToDgv = null;
        protected void RaiseOnSetDataToDgv(string[] pcValues, string[] mesValues)
        {
            if (OnSetDataToDgv != null)
            {
                OnSetDataToDgv(pcValues, mesValues);
            }
        }
        public delegate void SaveMesLog(string mesLog);
        public event SaveMesLog OnSaveMesLog = null;
        protected void RaiseOnSaveMesLog(string mesLog)
        {
            if (OnSaveMesLog != null)
            {
                OnSaveMesLog(mesLog);
            }
        }
        public MesClient()
        {
            _strLog = string.Empty;

            opcclient = new OPCUACLIENT.OPCUACLIENT();

            //* IROCV DATA
            for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
                irocvdata[nIndex] = IROCVData.GetInstance(nIndex);

            SetEquipmentTagList();
            SetMesTagList();

            connection = false;
            //MesClientStartAsync();

            _tmrMESRead.Interval = 1000;
            _tmrMESRead.Tick += new EventHandler(MESReadTimer_Tick);
            _tmrMESRead.Enabled = true;
        }

        public async Task<bool> MesClientStartAsync()
        {
            try
            {
                //await Task.Run(() => opcclient.Connect("opc.tcp://192.168.0.14:48000/IROCV"));
                //connection = true;
                string serverurl = MesServer.IPADDRESS;
                //connection = await Task.FromResult<bool>(opcclient.Connect("opc.tcp://10.2.2.154:48000/IROCV"));
                connection = await Task.FromResult<bool>(opcclient.Connect(serverurl));
                return connection;
                
            } catch(Exception ex)
            {
                connection = false;
                Console.WriteLine(ex.ToString());
            }
            return false;
        }

        #region MES Timer
        private void MESReadTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (connection == true)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    MesReadTimer();

                    RaiseOnSetDataToDgv(pcData, mesData);

                    sw.Stop();
                    //SetValue(sw.ElapsedMilliseconds.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void MesReadTimer()
        {
            try
            {
                #region Get Value from MES Tag
                UInt32 iVal = 0;
                UInt32[] iVals;
                float[] fVals;
                foreach (var tag in MesTagList)
                {
                    switch (tag.tagName)
                    {
                        case "ns=2;s=Mes/SequenceNo":
                            iVal = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            _iMesSequenceNo = (int)iVal;
                            SetValue(0, _iMesSequenceNo, "MES");
                            break;
                        case "ns=2;s=Mes/AcknowledgeNo":
                            iVal = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            _iMesAcknowledgeNo = (int)iVal;
                            SetValue(1, _iMesAcknowledgeNo, "MES");
                            break;
                        case "ns=2;s=Mes/EquipmentID":
                            _strMesEquipmentID = (string)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(2, _strMesEquipmentID, "MES");
                            break;
                        case "ns=2;s=Mes/TrayID":
                            _strMesTrayID = (string)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(3, _strMesTrayID, "MES");
                            break;
                        case "ns=2;s=Mes/TrayStatusCode":
                            _strMesTrayStatusCode = (string)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(4, _strMesTrayStatusCode, "MES");
                            break;
                        case "ns=2;s=Mes/ErrorCode":
                            iVal = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            _iMesErrorCode = (int)iVal;
                            SetValue(5, _iMesErrorCode, "MES");
                            break;
                        case "ns=2;s=Mes/ErrorMessage":
                            _strMesErrorMsg = (string)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(6, _strMesErrorMsg, "MES");
                            break;
                        case "ns=2;s=Mes/CellID":
                            _strMesCellIDs = (string[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(7, _strMesCellIDs, "MES");
                            break;
                        case "ns=2;s=Mes/CellStatus":
                            _strMesCellStatus = (string[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(39, _strMesCellStatus, "MES");
                            break;
                        default:
                            break;
                    }
                }
                #endregion

                #region Get Value from Equipment Tag
                foreach (var tag in EquipTagList)
                {
                    switch (tag.tagName)
                    {
                        case "ns=2;s=Equipment/SequenceNo":
                            iVal = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            _iPCSequenceNo = (int)iVal;
                            SetValue(0, _iPCSequenceNo, "PC");
                            break;
                        case "ns=2;s=Equipment/AcknowledgeNo":
                            iVal = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            _iPCAcknowledgeNo = (int)iVal;
                            SetValue(1, _iPCAcknowledgeNo, "PC");
                            break;
                        case "ns=2;s=Equipment/EquipmentID":
                            _strPCEquipmentID = (string)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(2, _strPCEquipmentID, "PC");
                            break;
                        case "ns=2;s=Equipment/TrayID":
                            _strPCTrayID = (string)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(3, _strPCTrayID, "PC");
                            break;
                        case "ns=2;s=Equipment/CellID":
                            _strPCCellIDs = (string[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(4, _strPCCellIDs, "PC");
                            break;
                        case "ns=2;s=Equipment/CellStatus":
                            _strPCCellStatus = (string[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(100, _strPCCellStatus, "PC");
                            break;
                        case "ns=2;s=Equipment/IR":
                            fVals = (float[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            _fPCIRs = fVals.Select(x => (float)x).ToArray();
                            SetValue(36, _fPCIRs, "PC");
                            break;
                        case "ns=2;s=Equipment/OCV":
                            fVals = (float[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            _fPCOCVs = fVals.Select(x => (float)x).ToArray();
                            SetValue(68, _fPCOCVs, "PC");
                            break;
                        //case "ns=2;s=Equipment/IR":
                        //    iVals = (UInt32[])ReadValue(tag.tagName, (int)tag.tagDataType);
                        //    _iPCIRs = iVals.Select(x => (int)x).ToArray();
                        //    SetValue(36, _iPCIRs, "PC");
                        //    break;
                        //case "ns=2;s=Equipment/OCV":
                        //    iVals = (UInt32[])ReadValue(tag.tagName, (int)tag.tagDataType);
                        //    _iPCOCVs = iVals.Select(x => (int)x).ToArray();
                        //    SetValue(68, _iPCOCVs, "PC");
                        //    break;
                        default:
                            break;
                    }
                }
                #endregion

                #region Get Value from PLC Tag
                string strValue = string.Empty;
                uint uiValue = 0;
                int iValue = 0;
                bool bValue = false;
                float fValue = 0.0f;
                foreach (var tag in PLCTagList)
                {
                    switch (tag.tagName)
                    {
                        case "ns=2;s=PLC/InterfaceVersionProject":
                            strValue = (string)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(140, strValue, "PC");
                            break;
                        case "ns=2;s=PLC/EquipmentName":
                            strValue = (string)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(141, strValue, "PC");
                            break;
                        case "ns=2;s=PLC/EquipmentTypeID":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(142, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/LineID":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(143, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/AreaID":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(144, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/EquipmentID":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(145, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/State":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(146, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/Mode":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(147, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/Blocked":
                            bValue = (Boolean)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(148, bValue, "PC");
                            break;
                        case "ns=2;s=PLC/Starved":
                            bValue = (Boolean)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(149, bValue, "PC");
                            break;
                        case "ns=2;s=PLC/CurrentSpeed":
                            fValue = (float)ReadValue(tag.tagName, (int)tag.tagDataType);
                            //iValue = (int)uiValue;
                            SetValue(150, fValue.ToString(), "PC");
                            break;
                        case "ns=2;s=PLC/DesignSpeed":
                            fValue = (float)ReadValue(tag.tagName, (int)tag.tagDataType);
                            //iValue = (int)uiValue;
                            SetValue(151, fValue.ToString(), "PC");
                            break;
                        case "ns=2;s=PLC/TotalCounter":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(152, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/StandstillReason":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(153, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/Stacklight0Color":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(154, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/Stacklight0Behavior":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(155, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/Stacklight1Color":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(156, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/Stacklight1Behavior":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(157, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/Stacklight2Color":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(158, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/Stacklight2Behavior":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(159, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/Stacklight3Color":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(160, iValue, "PC");
                            break;
                        case "ns=2;s=PLC/Stacklight3Behavior":
                            uiValue = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            iValue = (int)uiValue;
                            SetValue(161, iValue, "PC");
                            break;

                        default:
                            break;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString() );
            }
        }
        #endregion

        #region OPC UA Read Value Method
        //* SetValue는 opc ua 서버에서 tag를 read한 후에 값을 변수에 넣는 것.
        private void SetValue(int row, int value, string type)
        {
            if (type == "MES") mesData[row] = value.ToString();
            else if (type == "PC") pcData[row] = value.ToString();
        }
        private void SetValue(int row, string value, string type)
        {
            if (type == "MES") mesData[row] = value;
            else if (type == "PC") pcData[row] = value;
        }
        private void SetValue(int row, bool value, string type)
        {
            if (type == "MES") mesData[row] = value.ToString();
            else if (type == "PC") pcData[row] = value.ToString();
        }
        private void SetValue(int row, int[] values, string type)
        {
            for(int nIndex = 0; nIndex < values.Length; nIndex++)
            {
                if (type == "MES") mesData[row++] = values[nIndex].ToString();
                else if (type == "PC") pcData[row++] = values[nIndex].ToString();
            }
        }
        private void SetValue(int row, float[] values, string type)
        {
            for (int nIndex = 0; nIndex < values.Length; nIndex++)
            {
                if (type == "MES") mesData[row++] = values[nIndex].ToString();
                else if (type == "PC") pcData[row++] = values[nIndex].ToString();
            }
        }
        private void SetValue(int row, string[] values, string type)
        {
            for (int nIndex = 0; nIndex < values.Length; nIndex++)
            {
                if (type == "MES") mesData[row++] = values[nIndex];
                else if (type == "PC") pcData[row++] = values[nIndex];
            }
        }
        private object ReadValue(string node, int nDataType)
        {
            object objValue = new object();
            try
            {
                switch (nDataType)
                {
                    case (int)MesClient.EnumDataType.dtUInt32:
                        objValue = opcclient.Read<UInt32>(node);
                        if (objValue == null) return 0;
                        break;
                    case (int)MesClient.EnumDataType.dtString:
                        objValue = opcclient.Read<string>(node);
                        if (objValue == null) return string.Empty;
                        break;
                    case (int)MesClient.EnumDataType.dtStringArr:
                        objValue = (string[])opcclient.Read<string[]>(node);
                        if (objValue == null) return string.Empty;
                        break;
                    case (int)MesClient.EnumDataType.dtUInt32Arr:
                        objValue = (UInt32[])opcclient.Read<UInt32[]>(node);
                        if (objValue == null) return 0;
                        break;
                    case (int)MesClient.EnumDataType.dtFloatArr:
                        objValue = (float[])opcclient.Read<float[]>(node);
                        if (objValue == null) return 0;
                        break;
                    case (int)MesClient.EnumDataType.dtBoolean:
                        var bVal = (Boolean)opcclient.Read<bool>(node);
                        objValue = bVal.ToString();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return objValue;
        }
        #endregion

        #region OPC UA Write Value Method
        
        private object WriteValue(string node, string value, int nDataType)
        {
            object objValue = new object();

            try
            {
                switch (nDataType)
                {
                    case (int)MesClient.EnumDataType.dtUInt32:
                        UInt32 iVal = 0;
                        UInt32.TryParse(value, out iVal);
                        opcclient.Write<UInt32>(node, iVal);
                        break;
                    case (int)MesClient.EnumDataType.dtFloat:
                        float fVal = 0.0f;
                        float.TryParse(value, out fVal);
                        opcclient.Write<float>(node, fVal);
                        break;
                    case (int)MesClient.EnumDataType.dtString:
                        string strVal = string.Empty;
                        strVal = value;
                        opcclient.Write<string>(node, strVal);
                        break;
                    case (int)MesClient.EnumDataType.dtStringArr:
                        String[] strVals;
                        strVals = value.Split(',');
                        opcclient.Write<String[]>(node, strVals);
                        break;
                    case (int)MesClient.EnumDataType.dtUInt32Arr:
                        UInt32[] iValArr;
                        string[] strValArr = value.Split(',');
                        iValArr = Array.ConvertAll(strValArr, UInt32.Parse);
                        opcclient.Write<UInt32[]>(node, iValArr);
                        break;
                    case (int)MesClient.EnumDataType.dtBoolean:
                        bool bVal = false;
                        Boolean.TryParse(value, out bVal);
                        opcclient.Write<Boolean>(node, bVal);
                        break;
                    default:
                        break;
                }

                objValue = ReadValue(node, nDataType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return objValue;
        }
        private object WriteValue(string node, string[] values, int nDataType)
        {
            object objValue = new object();

            try
            {
                switch (nDataType)
                {
                    case (int)MesClient.EnumDataType.dtStringArr:
                        opcclient.Write<String[]>(node, values);
                        break;
                    case (int)MesClient.EnumDataType.dtUInt32Arr:
                        UInt32[] iValArr;
                        iValArr = Array.ConvertAll(values, UInt32.Parse);
                        opcclient.Write<UInt32[]>(node, iValArr);
                        break;
                    case (int)MesClient.EnumDataType.dtFloatArr:
                        float[] fValArr;
                        fValArr = Array.ConvertAll(values, float.Parse);
                        opcclient.Write<float[]>(node, fValArr);
                        break;
                    default:
                        break;
                }

                objValue = ReadValue(node, nDataType);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return objValue;
        }
        #endregion

        #region MES Write/Read
        public void WriteValue(string node, string value)
        {
            if (connection == false) return;
            switch (node)
            {
                case "EquipmentID":
                    WriteValue("ns=2;s=Equipment/EquipmentID", value, (int)EnumDataType.dtString);
                    break;
                case "TrayID":
                    WriteValue("ns=2;s=Equipment/TrayID", value, (int)EnumDataType.dtString);
                    break;
                default : break;
            }
        }
        #endregion

        #region Version 2. - MES와 IROCV간 주고 받는 Sequence 별로 구현
        public void test()
        {
            opcclient.CallMethod();
        }
        public void WriteFOEQR2_1(int stageno, string equipmentid, string trayid)
        {
            if(connection == false) return;
            //* EQUIPMENT ID
            WriteValue("ns=2;s=Equipment/EquipmentID", equipmentid, (int)EnumDataType.dtString);

            //* TRAY ID
            WriteValue("ns=2;s=Equipment/TrayID", trayid, (int)EnumDataType.dtString);

            //* Save Log
            _strLog = "Equipment: " + equipmentid + ", Trayid: " + trayid;
            SaveLog(stageno, "FOEQR2.1 IROCV -> MES", _strLog);
        }
        public IROCVData ReadFOEQR2_1(int stageno)
        {
            _strLog = string.Empty;

            if (connection == false) return null;

            //* MES에서 trayid와 equipment id를 다시 주지 않음.
            //* EQUIPMENT ID
            //string equipid = (string)ReadValue("ns=2;s=Mes/EquipmentID", (int)EnumDataType.dtString);
           // _strLog += "Equipment ID: " + equipid;

            //* TRAY ID
            //string trayid = (string)ReadValue("ns=2;s=Mes/TrayID", (int)EnumDataType.dtString);
            //_strLog += ", TRAY ID: " + trayid;

            //* TRAY STATUS CODE
            string traystatuscode = (string)ReadValue("ns=2;s=Mes/TrayStatusCode", (int)EnumDataType.dtString);
            _strLog += ", TRAY STATUS CODE: " + traystatuscode;

            //* ERROR CODE
            UInt32 ecode = (UInt32)ReadValue("ns=2;s=Mes/ErrorCode", (int)EnumDataType.dtUInt32);
            int errorcode = (int)ecode;
            _strLog += ", ERROR CODE: " + errorcode.ToString();

            //* ERROR MESSAGE
            string errormsg = (string)ReadValue("ns=2;s=Mes/ErrorMessage", (int)EnumDataType.dtString);
            _strLog += ", ERROR MESSAGE: " + errormsg;

            //* CELL ID
            string[] cellids = (string[])ReadValue("ns=2;s=Mes/CellID", (int)EnumDataType.dtStringArr);
            for (int nIndex = 0; nIndex < cellids.Length; nIndex++)
            {
                _strLog += ", CELLNO_" + (nIndex + 1).ToString("D2") + ": " + cellids[nIndex];
            }

            //* CELL STATUS
            string[] cellstatus = (string[])ReadValue("ns=2;s=Mes/CellStatus", (int)EnumDataType.dtStringArr);
            for (int nIndex = 0; nIndex < cellstatus.Length; nIndex++)
                _strLog += ", CELLNO_" + (nIndex + 1).ToString("D2") + ": " + cellstatus[nIndex];

            //* Write Log
            SaveLog(stageno, "FOEQR2.1 MES  -> IROCV", _strLog);

            //* irocvdata에 mes data 저장.
            //irocvdata[stageno].InitData();
            //irocvdata[stageno].EQUIPMENTID = equipid; ;
            //irocvdata[stageno].TRAYID = trayid;
            irocvdata[stageno].TRAYSTATUSCODE = traystatuscode;
            irocvdata[stageno].ERRORCODE = errorcode;
            irocvdata[stageno].ERRORMESSAGE = errormsg;
            irocvdata[stageno].CELLID = cellids;
            irocvdata[stageno].CELLSTATUSMES = cellstatus;
            irocvdata[stageno].TAGPATHNO = "FOEQR2.1";
            irocvdata[stageno].LOG = _strLog;
            irocvdata[stageno].CELLCOUNT = 0;
            for (int i = 0; i < cellids.Length; i++)
            {
                if (string.IsNullOrEmpty(cellids[i]) == true) irocvdata[stageno].CELL[i] = 0;
                else
                {
                    irocvdata[stageno].CELL[i] = 1;
                    irocvdata[stageno].CELLCOUNT++;
                }
            }

            return irocvdata[stageno];
        }
        public void WriteFOEQR2_2(int stageno, IROCVData irocvdata)
        {
            _strLog = string.Empty;

            if (connection == false) return;

            //* EQUIPMENT ID
            string equipmentid = irocvdata.EQUIPMENTID;
            WriteValue("ns=2;s=Equipment/EquipmentID", equipmentid, (int)EnumDataType.dtString);
            _strLog += "Equipment ID: " + equipmentid;

            //* TRAY ID
            string trayid = irocvdata.TRAYID;
            WriteValue("ns=2;s=Equipment/TrayID", trayid, (int)EnumDataType.dtString);
            _strLog += ", TRAY ID: " + trayid;

            //* CELL ID
            string[] cellids = irocvdata.CELLID;
            WriteValue("ns=2;s=Equipment/CellID", cellids, (int)EnumDataType.dtStringArr);
            for (int nIndex = 0; nIndex < cellids.Length; nIndex++)
                _strLog += ", CELLNO" + (nIndex + 1).ToString("D2") + ":" + cellids[nIndex];

            //* CELL STATUS
            string[] cellstatus = irocvdata.CELLSTATUSIROCV;
            WriteValue("ns=2;s=Equipment/CellStatus", cellstatus, (int)EnumDataType.dtStringArr);
            for (int nIndex = 0; nIndex < cellstatus.Length; nIndex++)
                _strLog += ", CELLSTATUS" + (nIndex + 1).ToString("D2") + ":" + cellstatus[nIndex];

            //* IR Values
            double[] irs = irocvdata.IR_AFTERVALUE;
            //string[] sIRs = irs.ToString().Split(',');
            string[] strIRs = Array.ConvertAll(irs, x => x.ToString());
            for (int nIndex = 0; nIndex < irs.Length; nIndex++)
                _strLog += ", IR" + (nIndex + 1).ToString("D2") + ":" + irs[nIndex].ToString();
            WriteValue("ns=2;s=Equipment/IR", strIRs, (int)EnumDataType.dtFloatArr);

            //* OCV Values
            double[] ocvs = irocvdata.OCV;
            string[] strOCVs = Array.ConvertAll(ocvs, x => x.ToString());
            for (int nIndex = 0; nIndex < ocvs.Length; nIndex++)
                _strLog += ", OCV" + (nIndex + 1).ToString("D2") + ":" + ocvs[nIndex].ToString();
            WriteValue("ns=2;s=Equipment/OCV", strOCVs, (int)EnumDataType.dtFloatArr);

            //* Write Log
            SaveLog(stageno, "FOEQR2.2 IROCV  -> MES", _strLog);
        }
        public IROCVData ReadFOEQR2_2(int stageno)
        {
            //* Acknowledgement No
            _strLog = string.Empty;

            if (connection == false) return null;

            //* ERROR CODE
            UInt32 ecode = (UInt32)ReadValue("ns=2;s=Mes/ErrorCode", (int)EnumDataType.dtUInt32);
            int errorcode = (int)ecode;
            _strLog += ", ERROR CODE: " + errorcode.ToString();
            irocvdata[stageno].ERRORCODE = errorcode;

            //* ERROR MESSAGE
            string errormsg = (string)ReadValue("ns=2;s=Mes/ErrorMessage", (int)EnumDataType.dtString);
            _strLog += ", ERROR MESSAGE: " + errormsg;
            irocvdata[stageno].ERRORMESSAGE = errormsg;

            //* Save Log
            SaveLog(stageno, "FOEIR2.2 MES -> IROCV", _strLog);


            irocvdata[stageno].ERRORCODE = errorcode;
            irocvdata[stageno].ERRORMESSAGE = errormsg;
            irocvdata[stageno].LOG = _strLog;
            return irocvdata[stageno];
        }
        public void WritePLSInfo(int stageno, PLCSysInfo plcsysinfo)
        {
            _strLog = string.Empty;

            //* Interface Version Project
            string interfaceversionproject = plcsysinfo.INTERFACEVERSIONPROJECT;
            WriteValue("ns=2;s=PLC/InterfaceVersionProject", interfaceversionproject, (int)EnumDataType.dtString);
            _strLog += "Interface Version Project: " + interfaceversionproject;

            //* Equipment Name
            string equipmentname = plcsysinfo.EQUIPMENTNAME;
            WriteValue("ns=2;ns=2;s=PLC/EquipmentName", equipmentname, (int)EnumDataType.dtString);
            _strLog += ", Equipment Name: " + equipmentname;

            //* Equipment Type Id
            int equipmenttypeid = plcsysinfo.EQUIPMENTTYPEID;
            WriteValue("ns=2;s=PLC/EquipmentTypeID", equipmenttypeid.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Equipment Type ID: " + equipmenttypeid;

            //* Line ID
            int lineid = plcsysinfo.LINEID;
            WriteValue("ns=2;s=PLC/LineID", lineid.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Line ID: " + lineid;

            //* Area ID
            int areaid = plcsysinfo.AREAID;
            WriteValue("ns=2;s=PLC/AreaID", areaid.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Area ID: " + areaid;

            //* Vendor ID
            int vendorid = plcsysinfo.VENDORID;
            WriteValue("ns=2;s=PLC/VendorID", vendorid.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Vendor ID: " + vendorid;

            //* Equipment ID
            int equipmentid = plcsysinfo.EQUIPMENTID;
            WriteValue("ns=2;s=PLC/EquipmentID", equipmentid.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Equipment ID: " + equipmentid;

            //* STATE
            int state = plcsysinfo.STATE;
            WriteValue("ns=2;s=PLC/State", state.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", State: " + state;

            //* MODE
            int mode = plcsysinfo.MODE;
            WriteValue("ns=2;s=PLC/Mode", mode.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Mode:" + mode;

            //* Blocked
            bool blocked = plcsysinfo.BLOCKED;
            WriteValue("ns=2;s=PLC/Blocked", blocked.ToString(), (int)EnumDataType.dtBoolean);
            _strLog += ", Blocked:" + blocked;

            //* Starved
            bool starved = plcsysinfo.STARVED;
            WriteValue("ns=2;s=PLC/Starved", starved.ToString(), (int)EnumDataType.dtBoolean);
            _strLog += ", Starved:" + starved;

            //* Current Speed
            double currentspeed = plcsysinfo.CURRENTSPEED;
            WriteValue("ns=2:s=PLC/CurrentSpeed", currentspeed.ToString(), (int)EnumDataType.dtFloat);
            _strLog += ", Current Speed:" + currentspeed;

            //* Designed Speed
            double designspeed = plcsysinfo.DESIGNSPEED;
            WriteValue("ns=2:s=PLC/DesignSpeed", designspeed.ToString(), (int)EnumDataType.dtFloat);
            _strLog += ", Designed Spee:" + designspeed;

            //* Total Counter
            int totalcounter = plcsysinfo.TOTALCOUNTER;
            WriteValue("ns=2:s=PLC/TotalCounter", totalcounter.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Total Counter:" + totalcounter;

            //* STANDSTILLREASON
            int standstillreason = plcsysinfo.STANDSTILLREASON;
            WriteValue("ns=2:s=PLC/StandstillReason", standstillreason.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stand Still Reason:" + standstillreason;

            //* STACKLIGHT0COLOR
            int stacklight0color = plcsysinfo.STACKLIGHT0COLOR;
            WriteValue("ns=2:s=PLC/Stacklight0Color", stacklight0color.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stack Light Color 0:" + stacklight0color;

            //* STACKLIGHT0BEHAVIOR
            int stacklight0behavior = plcsysinfo.STACKLIGHT0BEHAVIOR;
            WriteValue("ns=2:s=PLC/Stacklight0Behavior", stacklight0behavior.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stack Light Behavior 0:" + stacklight0behavior;

            //* STACKLIGHT1COLOR
            int stacklight1color = plcsysinfo.STACKLIGHT1COLOR;
            WriteValue("ns=2:s=PLC/Stacklight1Color", stacklight1color.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stack Light Color 1:" + stacklight1color;

            //* STACKLIGHT1BEHAVIOR
            int stacklight1behavior = plcsysinfo.STACKLIGHT1BEHAVIOR;
            WriteValue("ns=2:s=PLC/Stacklight1Behavior", stacklight1behavior.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stack Light Behavior 1:" + stacklight1behavior;

            //* STACKLIGHT2COLOR
            int stacklight2color = plcsysinfo.STACKLIGHT2COLOR;
            WriteValue("ns=2:s=PLC/Stacklight2Color", stacklight2color.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stack Light Color 2:" + stacklight2color;

            //* STACKLIGHT2BEHAVIOR
            int stacklight2behavior = plcsysinfo.STACKLIGHT2BEHAVIOR;
            WriteValue("ns=2:s=PLC/Stacklight2Behavior", stacklight2behavior.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stack Light Behavior 2:" + stacklight2behavior;

            //* STACKLIGHT3COLOR
            int stacklight3color = plcsysinfo.STACKLIGHT3COLOR;
            WriteValue("ns=2:s=PLC/Stacklight3Color", stacklight3color.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stack Light Color 3:" + stacklight3color;

            //* STACKLIGHT3BEHAVIOR
            int stacklight3behavior = plcsysinfo.STACKLIGHT3BEHAVIOR;
            WriteValue("ns=2:s=PLC/Stacklight3Behavior", stacklight3behavior.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stack Light Behavior 3:" + stacklight3behavior;

            //* STACKLIGHT4COLOR
            int stacklight4color = plcsysinfo.STACKLIGHT4COLOR;
            WriteValue("ns=2:s=PLC/Stacklight4Color", stacklight4color.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stack Light Color 4:" + stacklight4color;

            //* STACKLIGHT4BEHAVIOR
            int stacklight4behavior = plcsysinfo.STACKLIGHT4BEHAVIOR;
            WriteValue("ns=2:s=PLC/Stacklight4Behavior", stacklight4behavior.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stack Light Behavior 4:" + stacklight4behavior;

            //* STACKLIGHT5COLOR
            int stacklight5color = plcsysinfo.STACKLIGHT5COLOR;
            WriteValue("ns=2:s=PLC/Stacklight5Color", stacklight5color.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stack Light Color 0:" + stacklight5color;

            //* STACKLIGHT5BEHAVIOR
            int stacklight5behavior = plcsysinfo.STACKLIGHT5BEHAVIOR;
            WriteValue("ns=2:s=PLC/Stacklight5Behavior", stacklight5behavior.ToString(), (int)EnumDataType.dtUInt32);
            _strLog += ", Stack Light Behavior 5:" + stacklight5behavior;

            //* Save Log
            SaveLog(stageno, "PLC SYS INFO. IROCV -> MES", _strLog);
        }

        public void WriteFORIR1_ForMes(int stageno, string[] cellid, string[] cellstatus, string traystatuscode, string errorcode, string errormessage)
        {
            _strLog = string.Empty;

            if (connection == false) return;

            //* CELL ID
            WriteValue("ns=2;s=Mes/CellID", cellid, (int)EnumDataType.dtStringArr);
            for (int nIndex = 0; nIndex < cellid.Length; nIndex++)
                _strLog += ", CELLNO" + (nIndex + 1).ToString("D2") + ":" + cellid[nIndex];

            //* CELL STATUS
            WriteValue("ns=2;s=Mes/CellStatus", cellstatus, (int)EnumDataType.dtStringArr);
            for (int nIndex = 0; nIndex < cellstatus.Length; nIndex++)
                _strLog += ", CELLSTATUS" + (nIndex + 1).ToString("D2") + ":" + cellstatus[nIndex];

            //* Tray Status Code
            WriteValue("ns=2;s=Mes/TrayStatusCode", traystatuscode, (int)EnumDataType.dtString);
            _strLog += ", TRAY STATUS CODE: " + traystatuscode;

            //* ErrorCode
            WriteValue("ns=2;s=Mes/ErrorCode", errorcode, (int)EnumDataType.dtUInt32);
            _strLog += ", ERROR CODE: " + errorcode;

            //* ErrorMessage
            WriteValue("ns=2;s=Mes/ErrorMessage", errormessage, (int)EnumDataType.dtString);
            _strLog += ", ERROR MESSAGE: " + errormessage;

            SaveLog(stageno, "FOEQR2.1 For MES", _strLog);
        }
        public void WriteFORIR2_ForMes(int stageno, string errorcode, string errormessage)
        {
            if (connection == false) return;
            //* ErrorCode
            WriteValue("ns=2;s=Mes/ErrorCode", errorcode, (int)EnumDataType.dtUInt32);

            //* ErrorMessage
            WriteValue("ns=2;s=Mes/ErrorMessage", errormessage, (int)EnumDataType.dtString);

            //* Save Log
            _strLog = "ErrorCode: " + errorcode + ", ErrorMessage: " + errormessage;
            SaveLog(stageno, "FOEQR2.2 For MES", _strLog);
        }
        #endregion

        #region Version 1. - MES와 IROCV간 주고 받는 Sequence 별로 구현 - MES Sequence Read/Write
        /// <summary>
        /// 1.12 Request Tray Information (Send Tray ID irocv -> mes)
        /// </summary>
        public bool ReadFOEQR1_12(int stageno)
        {
            //* Acknowlegement No
            _strLog = "Acknowledgement : " + _iMesAcknowledgeNo.ToString();
            
            //* Save Log
            SaveLog(stageno, "FOEQR1.12 MES -> IROCV", _strLog);
            
            bool bAck = _iMesAcknowledgeNo == 1 ? true : false;
            return bAck;
        }
        public void WriteFOEQR1_12(int stageno, string equipmentid, string trayid)
        {
            //* EQUIPMENT ID
            WriteValue("ns=2;s=Equipment/EquipmentID", equipmentid, (int)EnumDataType.dtString);

            //* TRAY ID
            WriteValue("ns=2;s=Equipment/TrayID", trayid, (int)EnumDataType.dtString);

            //* Save Log
            _strLog = "Equipment: " + equipmentid + ", Trayid: " + trayid;
            SaveLog(stageno, "FOEQR1.12 IROCV -> MES", _strLog);
        }
        /// <summary>
        /// 1.7 Request Reservation (Receive Tray Information mes -> irocv)
        /// </summary>
        public void WriteFOEQR1_7(int stageno, int iAck)
        {
            string strAck = iAck.ToString();

            //* Acknowlegement No
            WriteValue("ns=2;s=Equipment/AcknowledgeNo", strAck, (int)EnumDataType.dtUInt32);

            //* Save Log
            _strLog = "Acknowledgement: " + strAck;
            SaveLog(stageno, "FOEQR1.7 IROCV -> MES", _strLog);
        }
        public IROCVData ReadFOEQR1_7(int stageno)
        {
            _strLog = string.Empty;

            //* EQUIPMENT ID
            string equipid = (string)ReadValue("ns=2;s=Mes/EquipmentID", (int)EnumDataType.dtString);
            _strLog += "Equipment ID: " + equipid;
            
            //* TRAY ID
            string trayid = (string)ReadValue("ns=2;s=Mes/TrayID", (int)EnumDataType.dtString);
            _strLog += ", TRAY ID: " + trayid;
            
            //* RECIPE ID
            string recipeid = (string)ReadValue("ns=2;s=Mes/RecipeID", (int)EnumDataType.dtString);
            _strLog += ", RECIPE ID: " + recipeid;
            
            //* BYPASS
            var tmpBypass = ReadValue("ns=2;s=Mes/Bypass", (int)EnumDataType.dtBoolean);
            bool bypass = Convert.ToBoolean(tmpBypass.ToString());
            _strLog += ", BYPASS : " + tmpBypass.ToString();
            
            //* CELL ID
            string[] cellids = (string[])ReadValue("ns=2;s=Mes/CellID", (int)EnumDataType.dtStringArr);
            for (int nIndex = 0; nIndex < cellids.Length; nIndex++)
            {
                _strLog += ", CELLNO_" + (nIndex + 1).ToString("D2") + ": " + cellids[nIndex];
            }

            //* CELL STATUS
            string[] cellstatus = (string[])ReadValue("ns=2;s=Mes/CellStatus", (int)EnumDataType.dtStringArr);
            for (int nIndex = 0; nIndex < cellstatus.Length; nIndex++)
                _strLog += ", CELLNO_" + (nIndex + 1).ToString("D2") + ": " + cellstatus[nIndex];

            //* Write Log
            SaveLog(stageno, "FOEQR1.7 MES  -> IROCV", _strLog);

            //* irocvdata에 mes data 저장.
            //irocvdata[stageno].InitData();
            irocvdata[stageno].EQUIPMENTID = equipid; ;
            irocvdata[stageno].TRAYID = trayid;
            //irocvdata[stageno].RECIPEID = recipeid;
            //irocvdata[stageno].BYPASS = bypass;
            irocvdata[stageno].CELLID = cellids;
            for (int i = 0; i < cellids.Length; i++)
            {
                if (string.IsNullOrEmpty(cellids[i]) == true) irocvdata[stageno].CELL[i] = 0;
                else irocvdata[stageno].CELL[i] = 1;
            }
           // irocvdata[stageno].CELLSTATUS = cellstatus;
            irocvdata[stageno].TAGPATHNO = "FOEQR1.7";

            return irocvdata[stageno];
        }
        /// <summary>
        /// 1.1 Data Collection (Send IR, OCV Data irocv -> mes)
        /// </summary>
        public bool ReadFOEQR1_1(int stageno)
        {
            //* Acknowledgement No
            _strLog = "Acknowledgement : " + _iMesAcknowledgeNo.ToString();

            //* Save Log
            SaveLog(stageno, "FOEQR1.1 MES -> IROCV", _strLog);

            bool bAck = _iMesAcknowledgeNo == 1 ? true : false;
            return bAck;
        }
        public void WriteFOEQR1_1(int stageno, IROCVData irocvdata)
        {
            _strLog = string.Empty;

            //* EQUIPMENT ID
            string equipmentid = irocvdata.EQUIPMENTID;
            WriteValue("ns=2;s=Equipment/EquipmentID", equipmentid, (int)EnumDataType.dtString);
            _strLog += "Equipment ID: " + equipmentid;

            //* TRAY ID
            string trayid = irocvdata.TRAYID;
            WriteValue("ns=2;s=Equipment/TrayID", trayid, (int)EnumDataType.dtString);
            _strLog += ", TRAY ID: " + trayid;

            //* RECIPE ID
            //string recipeid = irocvdata.RECIPEID;
            //WriteValue("ns=2;s=Equipment/RecipeID", recipeid, (int)EnumDataType.dtString);
            //_strLog += ", RECIPE ID: " + recipeid;

            //* CELL ID
            string[] cellids = irocvdata.CELLID;
            WriteValue("ns=2;s=Equipment/CellID", cellids, (int)EnumDataType.dtStringArr);
            for (int nIndex = 0; nIndex < cellids.Length; nIndex++)
                _strLog += ", CELLNO_" + (nIndex + 1).ToString("D2") + ":" + cellids[nIndex];

            //* IR Values
            double[] irs = irocvdata.IR_AFTERVALUE;
            //string[] sIRs = irs.ToString().Split(',');
            string[] strIRs = Array.ConvertAll(irs, x => x.ToString());
            for (int nIndex = 0; nIndex < irs.Length; nIndex++)
                _strLog += ", CELLNO_" + (nIndex + 1).ToString("D2") + ":" + irs[nIndex].ToString();
            WriteValue("ns=2;s=Equipment/IR", strIRs, (int)EnumDataType.dtUInt32Arr);

            //* OCV Values
            double[] ocvs = irocvdata.OCV;
            string[] strOCVs = Array.ConvertAll(ocvs, x => x.ToString());
            WriteValue("ns=2;s=Equipment/OCV", strOCVs, (int)EnumDataType.dtUInt32Arr);
        }
        /// <summary>
        /// 1.13 Process Result (Receive Process Result mes -> irocv) 
        /// </summary>
        public void WriteFOEQR1_13(int stageno, int iAck)
        {
            string strAck = iAck.ToString();

            //* Acknowledgement No
            _strLog = "Acknowledgement: " + strAck;
            WriteValue("ns=2;s=Equipment/AcknowledgeNo", strAck, (int)EnumDataType.dtUInt32);

            //* Save Log
            SaveLog(stageno, "FOEQR1.13 IROCV -> MES", _strLog);
        }
        public IROCVData ReadFOEQR1_13(int stageno)
        {
            _strLog = string.Empty;

            //* EQUIPMENT ID
            string equipid = (string)ReadValue("ns=2;s=Mes/EquipmentID", (int)EnumDataType.dtString);
            _strLog += "Equipment ID: " + equipid;

            //* RESULT 1 -> Tray Emission, 2 -> Tray Retry
            int result = (int)ReadValue("ns=2;s=Mes/Result", (int)EnumDataType.dtUInt32);
            _strLog += ", RESULT: " + result.ToString();

            //* Save Log
            SaveLog(stageno, "FOEQR1.13 MES -> IROCV", _strLog );

            IROCVData irocvDatas = new IROCVData();
            irocvDatas.EQUIPMENTID = equipid;
            irocvDatas.MESRESULT = result;

            return irocvDatas;
        }
        #endregion

        #region MES LOG
        private void SaveLog(int stageno, string nType, string strLog)
        {
            string _strLogMessage = string.Empty;

            string strStageName = "IROCV" + (stageno + 1).ToString("D2");
            string strType = " OPCUA Tag(" + nType + ")"; 
            
            _strLogMessage = strStageName + "> " + strType + " - " + strLog; 
            RaiseOnSaveMesLog(_strLogMessage);
        }
        private void SaveLog(int stageno, string nType, IROCVData irocvdata)
        {
            string _strLogMessage = string.Empty;

            string strStageName = "IROCV" + (stageno + 1).ToString("D2");
            string strType = " OPCUA Tag(" + nType + ")";
            _strLogMessage = strStageName + "> OPCUA Tag(FOEQR1.12 MES -> IROCV) - Acknowledgement : " + _iMesAcknowledgeNo.ToString();

            //* ir, ocv, status, result list


            RaiseOnSaveMesLog(_strLogMessage);
        }
        #endregion

        #region OPC UA TAG
        public enum EnumDataType
        {
            dtString,
            dtStringArr,
            dtUInt32,
            dtUInt32Arr,
            dtFloat,
            dtFloatArr,
            dtBoolean
        }

        public struct Tag
        {
            public string tagName;
            public EnumDataType tagDataType;
        }

        public List<Tag> EquipTagList = new List<Tag>();
        public List<Tag> MesTagList = new List<Tag>();
        public List<Tag> PLCTagList = new List<Tag>();

        public void SetEquipmentTagList()
        {
            #region Equipment Tag List
            Tag tag;
            tag.tagName = "ns=2;s=Equipment/SequenceNo";
            tag.tagDataType = EnumDataType.dtUInt32;
            EquipTagList.Add(tag);

            tag.tagName = "ns=2;s=Equipment/AcknowledgeNo";
            tag.tagDataType = EnumDataType.dtUInt32;
            EquipTagList.Add(tag);

            tag.tagName = "ns=2;s=Equipment/EquipmentID";
            tag.tagDataType = EnumDataType.dtString;
            EquipTagList.Add(tag);

            tag.tagName = "ns=2;s=Equipment/TrayID";
            tag.tagDataType = EnumDataType.dtString;
            EquipTagList.Add(tag);

            tag.tagName = "ns=2;s=Equipment/CellID";
            tag.tagDataType = EnumDataType.dtStringArr;
            EquipTagList.Add(tag);

            tag.tagName = "ns=2;s=Equipment/IR";
            tag.tagDataType = EnumDataType.dtFloatArr;
            EquipTagList.Add(tag);

            tag.tagName = "ns=2;s=Equipment/OCV";
            tag.tagDataType = EnumDataType.dtFloatArr;
            EquipTagList.Add(tag);

            //tag.tagName = "ns=2;s=Equipment/IR";
            //tag.tagDataType = EnumDataType.dtUInt32Arr;
            //EquipTagList.Add(tag);

            //tag.tagName = "ns=2;s=Equipment/OCV";
            //tag.tagDataType = EnumDataType.dtUInt32Arr;
            //EquipTagList.Add(tag);

            tag.tagName = "ns=2;s=Equipment/CellStatus";
            tag.tagDataType = EnumDataType.dtStringArr;
            EquipTagList.Add(tag);
            #endregion
        }
        public void SetMesTagList()
        {
            #region MES Tag List
            Tag tag;
            tag.tagName = "ns=2;s=Mes/SequenceNo";
            tag.tagDataType = EnumDataType.dtUInt32;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/AcknowledgeNo";
            tag.tagDataType = EnumDataType.dtUInt32;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/ErrorCode";
            tag.tagDataType = EnumDataType.dtUInt32;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/ErrorMessage";
            tag.tagDataType = EnumDataType.dtString;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/EquipmentID";
            tag.tagDataType = EnumDataType.dtString;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/TrayID";
            tag.tagDataType = EnumDataType.dtString;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/TrayStatusCode";
            tag.tagDataType = EnumDataType.dtString;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/CellID";
            tag.tagDataType = EnumDataType.dtStringArr;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/CellStatus";
            tag.tagDataType = EnumDataType.dtStringArr;
            MesTagList.Add(tag);
            #endregion
        }
        public void SetPLCTagList()
        {
            #region Equipment Tag List
            Tag tag;
            tag.tagName = "ns=2;s=PLC/InterfaceVersionProject";
            tag.tagDataType = EnumDataType.dtString;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/EquipmentName";
            tag.tagDataType = EnumDataType.dtString;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/EquipmentTypeID";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/LineID";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/AreaID";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/VendorID";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/EquipmentID";
            tag.tagDataType = EnumDataType.dtUInt32;

            tag.tagName = "ns=2;s=PLC/State";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/Mode";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/Blocked";
            tag.tagDataType = EnumDataType.dtBoolean;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/Starved";
            tag.tagDataType = EnumDataType.dtBoolean;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/CurrentSpeed";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/DesignSpeed";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/TotalCounter";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/StanddstillReason";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/Stacklight0Color";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/Stacklight0Behavior";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/Stacklight1Color";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/Stacklight1Behavior";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/Stacklight2Color";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/Stacklight2Behavior";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/Stacklight3Color";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            tag.tagName = "ns=2;s=PLC/Stacklight3Behavior";
            tag.tagDataType = EnumDataType.dtUInt32;
            PLCTagList.Add(tag);

            #endregion
        }
        #endregion
    }
}


