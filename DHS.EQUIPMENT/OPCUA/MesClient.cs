using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHS.EQUIPMENT.Common;
using Opc.Ua;
using OPCUACLIENT;
using static DHS.EQUIPMENT.MesClient;

namespace DHS.EQUIPMENT
{
    public class MesClient
    {
        public static bool connection = false;
        public bool isRead = false;
        OPCUACLIENT.OPCUACLIENT opcclient = null;
        IROCVData[] irocvdata = new IROCVData[_Constant.frmCount];

        private string _strLog;

        private string[] mesData = new string[70];
        private string[] pcData = new string[133];

        private int _iMesSequenceNo;
        private int _iMesAcknowledgeNo;
        private string _strMesEquipmentID;
        private string _strMesTrayID;
        private string _strMesRecipeID;
        private bool _bMesBypass;
        private string[] _strMesCellIDs;
        private string[] _strMesCellStats;

        private int _iPCSequenceNo;
        private int _iPCAcknowledgeNo;
        private string _strPCEquipmentID;
        private string _strPCTrayID;
        private string _strPCRecipeID;
        private string[] _strPCCellIDs;
        private int[] _iPCIRs;
        private int[] _iPCOCVs;
        private int[] _iPCResults;

        public int MESSEQUENCENO { get => _iMesSequenceNo; set => _iMesSequenceNo = value; }
        public int MESACKNOWLEDGENO { get => _iMesAcknowledgeNo; set => _iMesAcknowledgeNo = value; }
        public string MESEQUIPMENTID { get => _strMesEquipmentID; set => _strMesEquipmentID = value; }
        public string MESTRAYID { get => _strMesTrayID; set => _strMesTrayID = value; }
        public string MESRECIPEID { get => _strMesRecipeID; set => _strMesRecipeID = value; }
        public bool MESBYPASS { get => _bMesBypass; set => _bMesBypass = value; }
        public string[] MESCELLIDS { get => _strMesCellIDs; set => _strMesCellIDs = value; }
        public string[] MESCELLSTATS { get => _strMesCellStats; set => _strMesCellStats = value; }
        public int PCSEQUENCENO { get => _iPCSequenceNo; set => _iPCSequenceNo = value; }
        public int PCACKNOWLEDGENO { get => _iPCAcknowledgeNo; set => _iPCAcknowledgeNo = value; }
        public string PCEQUIPMENTID { get => _strPCEquipmentID; set => _strPCEquipmentID = value; }
        public string PCTRAYID { get => _strPCTrayID; set => _strPCTrayID = value; }
        public string PCRECIPEID { get => _strPCRecipeID; set => _strPCRecipeID = value; }
        public string[] PCCELLIDS { get => _strPCCellIDs; set => _strPCCellIDs = value; }
        public int[] PCIRS { get => _iPCIRs; set => _iPCIRs = value; }
        public int[] PCOCVS { get => _iPCOCVs; set => _iPCOCVs = value; }
        public int[] PCRESULTS { get => _iPCResults; set => _iPCResults = value; }

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
                connection = await Task.FromResult<bool>(opcclient.Connect("opc.tcp://192.168.0.13:48000/IROCV"));
                //opcclient.Connect("opc.tcp://192.168.0.14:48000/IROCV");
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
                        case "ns=2;s=Mes/RecipeID":
                            _strMesRecipeID = (string)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(4, _strMesRecipeID, "MES");
                            break;
                        case "ns=2;s=Mes/Bypass":
                            var tmp = ReadValue(tag.tagName, (int)tag.tagDataType);
                            _bMesBypass = Convert.ToBoolean(tmp.ToString());
                            SetValue(5, _bMesBypass, "MES");
                            break;
                        case "ns=2;s=Mes/CellID":
                            _strMesCellIDs = (string[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(6, _strMesCellIDs, "MES");
                            break;
                        case "ns=2;s=Mes/CellStatus":
                            _strMesCellStats = (string[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(38, _strMesCellStats, "MES");
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
                        case "ns=2;s=Equipment/RecipeID":
                            _strPCRecipeID = (string)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(4, _strPCRecipeID, "PC");
                            break;
                        case "ns=2;s=Equipment/CellID":
                            _strPCCellIDs = (string[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(5, _strPCCellIDs, "PC");
                            break;
                        case "ns=2;s=Equipment/IR":
                            iVals = (UInt32[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            _iPCIRs = iVals.Select(x => (int)x).ToArray();
                            SetValue(37, _iPCIRs, "PC");
                            break;
                        case "ns=2;s=Equipment/OCV":
                            iVals = (UInt32[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            _iPCOCVs = iVals.Select(x => (int)x).ToArray();
                            SetValue(69, _iPCOCVs, "PC");
                            break;
                        case "ns=2;s=Equipment/Result":
                            iVals = (UInt32[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            _iPCResults = iVals.Select(x => (int)x).ToArray();
                            SetValue(101, _iPCResults, "PC");
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
        #endregion

        #region MES와 IROCV간 주고 받는 Sequence 별로 구현 - MES Sequence Read/Write
        /// <summary>
        /// 1.12 Request Tray Information (Send Tray ID irocv -> mes)
        /// </summary>
        public bool ReadFOEQR1_12(int stageno)
        {
            _strLog = "Acknowledgement : " + _iMesAcknowledgeNo.ToString();
            SaveLog(stageno, "FOEQR1.12 MES -> IROCV", _strLog);
            
            bool bAck = _iMesAcknowledgeNo == 1 ? true : false;
            return bAck;
        }
        public void WriteFOEQR1_12(int stageno, string equipmentid, string trayid, IROCVData irocvdata)
        {
            WriteValue("ns=2;s=Equipment/EquipmentID", equipmentid, (int)EnumDataType.dtString);
            WriteValue("ns=2;s=Equipment/TrayID", trayid, (int)EnumDataType.dtString);

            _strLog = "Equipment: " + equipmentid + ", Trayid: " + trayid;
            SaveLog(stageno, "FOEQR1.12 IROCV -> MES", _strLog);
        }
        /// <summary>
        /// 1.7 Request Reservation (Receive Tray Information mes -> irocv)
        /// </summary>
        public void WriteFOEQR1_7(int stageno, int iAck)
        {
            string strAck = iAck.ToString();

            _strLog = "Acknowledgement: " + strAck;
            SaveLog(stageno, "FOEQR1.7 IROCV -> MES", _strLog);
            
            WriteValue("ns=2;s=Equipment/AcknowledgeNo", strAck, (int)EnumDataType.dtUInt32);
        }
        public void ReadFOEQR1_7(int stageno, IROCVData irocvdata)
        {
            string equipid = (string)ReadValue("ns=2;s=Mes/EquipmentID", (int)EnumDataType.dtString);
            string trayid = (string)ReadValue("ns=2;s=Mes/TrayID", (int)EnumDataType.dtString);
            string recipeid = (string)ReadValue("ns=2;s=Mes/RecipeID", (int)EnumDataType.dtString);
            var tmpBypass = ReadValue("ns=2;s=Mes/Bypass", (int)EnumDataType.dtBoolean);
            bool bypass = Convert.ToBoolean(tmpBypass.ToString());
            string[] cellids = (string[])ReadValue("ns=2;s=Mes/CellID", (int)EnumDataType.dtStringArr);

            string[] cellstatus = (string[])ReadValue("ns=2;s=Mes/CellStatus", (int)EnumDataType.dtStringArr);
        }
        /// <summary>
        /// 1.1 Data Collection (Send IR, OCV Data irocv -> mes)
        /// </summary>
        public bool ReadFOEQR1_1(int stageno)
        {
            _strLog = "Acknowledgement : " + _iMesAcknowledgeNo.ToString();
            SaveLog(stageno, "FOEQR1.1 MES -> IROCV", _strLog);

            bool bAck = _iMesAcknowledgeNo == 1 ? true : false;
            return bAck;
        }
        public void WriteFOEQR1_1(int stageno, IROCVData irocvdata)
        {
            string equipmentid = string.Empty;
            string trayid = string.Empty;
            string recipeid = string.Empty;
            string cellids = string.Empty;
            string irs = string.Empty;
            string ocvs = string.Empty;
            WriteValue("ns=2;s=Equipment/EquipmentID", equipmentid, (int)EnumDataType.dtString);
            WriteValue("ns=2;s=Equipment/TrayID", trayid, (int)EnumDataType.dtString);
            WriteValue("ns=2;s=Equipment/RecipeID", recipeid, (int)EnumDataType.dtString);
            WriteValue("ns=2;s=Equipment/CellID", cellids, (int)EnumDataType.dtStringArr);
            WriteValue("ns=2;s=Equipment/IR", irs, (int)EnumDataType.dtUInt32Arr);
            WriteValue("ns=2;s=Equipment/OCV", ocvs, (int)EnumDataType.dtUInt32Arr);
        }
        /// <summary>
        /// 1.13 Process Result (Receive Process Result mes -> irocv) 
        /// </summary>
        public void WriteFOEQR1_13(int stageno, int iAck)
        {
            string strAck = iAck.ToString();

            _strLog = "Acknowledgement: " + strAck;
            SaveLog(stageno, "FOEQR1.13 IROCV -> MES", _strLog);

            WriteValue("ns=2;s=Equipment/AcknowledgeNo", strAck, (int)EnumDataType.dtUInt32);
        }
        public void ReadFOEQR1_13(int stageno, IROCVData irocvdata)
        {
            string equipid = (string)ReadValue("ns=2;s=Mes/EquipmentID", (int)EnumDataType.dtString);
            int result = (int)ReadValue("ns=2;s=Mes/Result", (int)EnumDataType.dtUInt32);

            string strStageNo = (stageno + 1).ToString("D2");
            _strLog = "IROCV" + strStageNo + "> OPCUA Tag(FOEQR1.13 MES -> IROCV) - Acknowledgement : " + _iMesAcknowledgeNo.ToString();
            RaiseOnSaveMesLog(_strLog);
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
            dtBoolean
        }

        public struct Tag
        {
            public string tagName;
            public EnumDataType tagDataType;
        }

        public List<Tag> EquipTagList = new List<Tag>();
        public List<Tag> MesTagList = new List<Tag>();

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

            tag.tagName = "ns=2;s=Equipment/RecipeID";
            tag.tagDataType = EnumDataType.dtString;
            EquipTagList.Add(tag);

            tag.tagName = "ns=2;s=Equipment/CellID";
            tag.tagDataType = EnumDataType.dtStringArr;
            EquipTagList.Add(tag);

            tag.tagName = "ns=2;s=Equipment/IR";
            tag.tagDataType = EnumDataType.dtUInt32Arr;
            EquipTagList.Add(tag);

            tag.tagName = "ns=2;s=Equipment/OCV";
            tag.tagDataType = EnumDataType.dtUInt32Arr;
            EquipTagList.Add(tag);

            tag.tagName = "ns=2;s=Equipment/Result";
            tag.tagDataType = EnumDataType.dtUInt32Arr;
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

            tag.tagName = "ns=2;s=Mes/EquipmentID";
            tag.tagDataType = EnumDataType.dtString;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/TrayID";
            tag.tagDataType = EnumDataType.dtString;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/RecipeID";
            tag.tagDataType = EnumDataType.dtString;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/Bypass";
            tag.tagDataType = EnumDataType.dtBoolean;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/CellID";
            tag.tagDataType = EnumDataType.dtStringArr;
            MesTagList.Add(tag);

            tag.tagName = "ns=2;s=Mes/CellStatus";
            tag.tagDataType = EnumDataType.dtStringArr;
            MesTagList.Add(tag);
            #endregion
        }
        #endregion
    }
}
