using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHS.EQUIPMENT.Common;
using static DHS.EQUIPMENT.IROCV;

namespace DHS.EQUIPMENT
{
    public class IROCV : CSocketDriver
    {
        string STX = string.Format("{0}", (char)2);
        string ETX = string.Format("{0}", (char)3);

        Util util = new Util();
        CEquipmentData _system;
        CheckSum cs = new CheckSum();
        public Timer _tmrSend = new Timer();

        bool bSendTimer = false;
        private int _iMeasureCount;
        private bool _bAMS;
        private bool _bAMF;
        
        //private bool _bAutoMode = false;
        private int _iStage = 0;
        private int _iMsaCount = 0;
        private int _iOffsetCount = 0;
        private string _sMsaFilename = string.Empty;
        private string _sOffsetFilename = string.Empty;
        protected enumEquipMode _EquipMode = enumEquipMode.AUTO;
        protected enumEquipStatus _EquipStatus = enumEquipStatus.StepVacancy;
        protected enumMsaStatus _MsaStatus = enumMsaStatus.StepTrayDown;

        string _strBuffData = string.Empty;

        //public bool AUTOMODE { get => _bAutoMode; set => _bAutoMode = value; }
        public bool AMS { get => _bAMS; set => _bAMS = value; }
        public bool AMF { get => _bAMF; set => _bAMF = value; }
        public int STAGENO { get => _iStage; set => _iStage = value; }
        public string MSAFILENAME { get => _sMsaFilename; set => _sMsaFilename = value; }
        public string OFFSETFILENAME { get => _sOffsetFilename; set => _sOffsetFilename = value; }
        public int MSACOUNT { get => _iMsaCount; set => _iMsaCount = value; }
        public int OFFSETCOUNT { get => _iOffsetCount; set => _iOffsetCount = value; }
        public enumEquipMode EQUIPMODE { get => _EquipMode; set => _EquipMode = value; }
        public enumEquipStatus EQUIPSTATUS { get => _EquipStatus; set => _EquipStatus = value; }
        public enumMsaStatus MSASTATUS { get => _MsaStatus; set => _MsaStatus = value; }
        public string StrBuffData { get => _strBuffData; set => _strBuffData = value; }
        

        private static IROCV[] IrocvDriver = new IROCV[_Constant.frmCount];
        public static IROCV GetInstance(int nIndex)
        {
            if (IrocvDriver[nIndex] == null) IrocvDriver[nIndex] = new IROCV();
            return IrocvDriver[nIndex];
        }

        #region delegate
        //Connection State는 공통적으로 사용
        public delegate void delegateDoWork();
        public delegate void delegateConnectionState(bool enumConnect);
        public event delegateConnectionState OnConnection = null;
        protected void RaiseOnConnectionState(bool enumConnect)
        {
            if (OnConnection != null)
            {
                new delegateDoWork(delegate ()
                {
                    OnConnection(enumConnect);
                }).BeginInvoke(null, null);
            }
        }

        public delegate void InitIROCV(int stageno);
        public event InitIROCV OnInitIROCV = null;
        protected void RaiseOnInitIROCV(int stageno)
        {
            if (OnInitIROCV != null)
            {
                OnInitIROCV(stageno);
            }
        }

        public delegate void IROCVError(int stageno, string param);
        public event IROCVError OnIROCVError = null;
        protected void RaiseOnIROCVError(int stageno, string param)
        {
            if (OnIROCVError != null)
            {
                OnIROCVError(stageno, param);
            }
        }

        public delegate void ProcessAms(int stageno);
        public event ProcessAms OnProcessAms = null;
        protected void RaiseOnProcessAms(int stageno)
        {
            if (OnProcessAms != null)
            {
                OnProcessAms(stageno);
            }
        }
        public delegate void ProcessAmf(int stageno);
        public event ProcessAmf OnProcessAmf = null;
        protected void RaiseOnProcessAmf(int stageno)
        {
            if (OnProcessAmf != null)
            {
                OnProcessAmf(stageno);
            }
        }
        public delegate void ProcessStop(int stageno);
        public event ProcessStop OnProcessStop = null;
        protected void RaiseOnProcessStop(int stageno)
        {
            if (OnProcessStop != null)
            {
                OnProcessStop(stageno);
            }
        }
        public delegate void ProcessIR(int stageno, string param);
        public event ProcessIR OnProcessIr = null;
        protected void RaiseOnProcessIr(int stageno, string param)
        {
            if (OnProcessIr != null)
            {
                OnProcessIr(stageno, param);
            }
        }
        public delegate void ProcessOCV(int stageno, string param);
        public event ProcessOCV OnProcessOcv = null;
        protected void RaiseOnProcessOcv(int stageno, string param)
        {
            if (OnProcessOcv != null)
            {
                OnProcessOcv(stageno, param);
            }
        }
        public delegate void ShowControlMessage(int stageno, string param);
        public event ShowControlMessage OnShowControlMessage = null;
        protected void RaiseOnShowControlMessage(int stageno, string param)
        {
            if (OnShowControlMessage != null)
            {
                OnShowControlMessage(stageno, param);
            }
        }
        #endregion

        public IROCV()
        {
            _system = CEquipmentData.GetInstance();

            _bAMS = false;
            _bAMF = false;

            _tmrSend = new Timer();
            _tmrSend.Interval = 500;
            _tmrSend.Tag = _iStage;
            _tmrSend.Tick += new EventHandler(SendTimer_Tick);
            _tmrSend.Enabled = true;
        }
        public IROCV(string _strIP, int _iPort, int stageno, string _strType)
            : base()
        {
            //* Send Timer => send command [SEN] to IROCV every one seconds
            
            Open(_strIP, _iPort, stageno, _strType);
        }
        ~IROCV()
        {
            CloseSocket();
        }

        private void SendTimer_Tick(object sender, EventArgs e)
        {
            if(bSendTimer == true && _bAMS == false)
            {
                CmdSEN();
            }
        }

        #region Connection
        public override int Send(string strMessage)
        {
            // _Logger.Log(Level.Info, "LabelPrint Message Send : " + strMessage);
            util.SaveLog(this._iStage, strMessage, "TX");
            return base.Send(strMessage);
        }

        public void StopTimer()
        {
            StopReconnectTimer();
        }

        public void Close()
        {
            CloseSocket();
        }

        public void Open(string _strIP, int _iPort, int stageno, string _strType)
        {
            try
            {
                InitConnectionString(_strIP, _iPort, stageno, _strType);
                int iRet = 0;
                iRet = OpenSocketPort();

                if (iRet == 0)
                {
                    bSendTimer = true;
                    util.SaveLog(_iStage, "IROCV is connected");
                    //RaiseOnConnectionState(true);
                }
                else
                {
                    //* 에러처리
                    //RaiseOnConnectionState(false);
                    util.SaveLog(_iStage, "IROCV is not connected");
                }
            }
            catch (Exception ex)
            {
                // _Logger.Log(Level.Exception, "Label Print Driver Open Fail!!! : " + ex.ToString());
            }
        }
        public void ChangeSetting(string _strIP, int _iPort, int stageno, string _strType)
        {
            try
            {
                InitConnectionString(_strIP, _iPort, stageno, _strType);
                int iRet = 0;
                iRet = OpenSocketPort();
            }
            catch (Exception ex)
            {
                // _Logger.Log(Level.Exception, "Label Print Driver Open Fail!!! : " + ex.ToString());
            }
        }
        #endregion

        #region IR/OCV Command
        public string MakeIROCVCommand(string cmd, string param)
        {
            string command = string.Empty;
            string STX = string.Format("{0}", (char)2);
            string ETX = string.Format("{0}", (char)3);

            string senddata = cmd;

            command = STX + senddata + param + cs.GetHexChecksum(senddata, param) + ETX;
            //command = senddata + cs.CheckSum_Cal(senddata) + ETX;

            return command;
        }
        public void CmdSEN()
        {
            string sendCommand = MakeIROCVCommand("SEN", "");
            Send(sendCommand);
        }
        public void CmdAMS()
        {
            bSendTimer = false;
            _bAMS = true;
            _bAMF = false;

            string sendCommand = MakeIROCVCommand("AMS", "");
            Send(sendCommand);
        }
        public void CmdAMF()
        {
            string sendCommand = MakeIROCVCommand("AMF", "");
            Send(sendCommand);

            bSendTimer = true;
        }
        public void CmdSTOP()
        {
            string sendCommand = MakeIROCVCommand("STP", "");
            Send(sendCommand);

            bSendTimer = true;
        }
        public void CmdRESET()
        {
            bSendTimer = false;

            string sendCommand = MakeIROCVCommand("RST", "");
            Send(sendCommand);
        }
        public void CmdIR(int iChannel)
        {
            bSendTimer = false;
            int mapping_channel = _system.CHANNELMAPPING[iChannel];
            string sendCommand = MakeIROCVCommand("IR*", mapping_channel.ToString("D3"));
            Send(sendCommand);
        }
        public void CmdOCV(int iChannel)
        {
            bSendTimer = false;
            int mapping_channel = _system.CHANNELMAPPING[iChannel];
            string sendCommand = MakeIROCVCommand("OCV", mapping_channel.ToString("D3"));
            Send(sendCommand);
        }
        string remainData = string.Empty;
        protected override int ParseMessage(string strMessage)
        {
            string a = string.Empty;
            a = strMessage;
            //Format: $,< UNo > (,< SeqNo >),< Sts >,< Ackcd >,< Command >,< Parameter >,< Value > (,< Sum >)< CR >
            try
            {
                int nlengthEtx = 0;
                int nlengthStx = 0;
                string strData = string.Empty;
                string _strError = string.Empty;

                _strBuffData = _strBuffData + strMessage;
                //util.SaveLog(_iStage, _strBuffData, "RX_Raw");

                if (_strBuffData.Contains(ETX) == true)
                {
                    string[] strbuffers = _strBuffData.Split('');
                    _strBuffData = strbuffers[0];

                    nlengthEtx = _strBuffData.IndexOf(""); //Etx = CR(\r)
                    nlengthStx = _strBuffData.IndexOf(""); //Stx = ID 수정필요

                    if (nlengthStx == 0) strData = _strBuffData.Remove(0,1);
                    else strData = remainData + _strBuffData;

                    remainData = strbuffers[strbuffers.Length - 1];
                    util.SaveLog(_iStage, strData, "RX");
                    _strBuffData = string.Empty;

                    OnReceiveStage(strData);
                }
                return 0;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
        private void OnReceiveStage(string msg)
        {
            string checksum = string.Empty;
            int datalength = msg.Length;
            
            string cmd = msg.Substring(0, 3);
            string param = msg.Substring(3, datalength - 3);
            checksum = msg.Substring(datalength - 2, 2);

            if (cmd != "SEN") RaiseOnShowControlMessage(this._iStage, msg);

            switch(cmd)
            {
                case "RST":
                    bSendTimer = true;
                    RaiseOnInitIROCV(this._iStage);
                    break;
                case "AMS":
                    _iMeasureCount = 0;
                    RaiseOnProcessAms(this._iStage);
                    break;
                case "AMF":
                    //CmdAMF();
                    RaiseOnProcessAmf(this._iStage);
                    break;
                case "STP":
                    RaiseOnProcessStop(this._iStage);
                    break;
                case "IR*":
                    _iMeasureCount++;
                    if (_bAMS == false) 
                        bSendTimer = true;
                    RaiseOnProcessIr(this._iStage, param);
                    break;
                case "OCV":
                    if(_bAMS == false) 
                        bSendTimer = true;
                    RaiseOnProcessOcv(this._iStage, param);

                    //* 마지막에 amf 안날라오는 경우가 있어서 추가함
                    //* amf 안날라오는 문제 해결됨.
                    //if (_iMeasureCount >= 32) CmdAMF();
                    break;
                case "SEN":
                    //RaiseOnSensorInputProcess(param);
                    //* error상태일 때 SENERR, error가 없을 때 SENIDL
                    RaiseOnIROCVError(this._iStage, param);
                    break;
                case "OUT":
                    //RaiseOnSensorOutputProcess(param);
                    break;
                case "ERR":
                    //ResponseError(param);
                    //RaiseOnIROCVError(this._iStage, param);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}
