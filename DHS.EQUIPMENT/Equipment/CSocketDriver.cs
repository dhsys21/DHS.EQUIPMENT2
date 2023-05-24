using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using DHS.EQUIPMENT.Common;

namespace DHS.EQUIPMENT
{
    public partial class CSocketDriver
    {
        Util util = new Util();
        string STX = string.Format("{0}", (char)2);
        string ETX = string.Format("{0}", (char)3);

        public delegate void delegateConnectionState(int stageno, enumConnectionState enumConnection);
        public event delegateConnectionState OnConnectionStateChanged = null;
        protected void RaiseOnConnection(int stageno, enumConnectionState enumConnection)
        {
            if (OnConnectionStateChanged != null)
            {
                OnConnectionStateChanged(stageno, enumConnection);
            }
        }

        #region Message Queue
        public class CSocketMessage
        {
            private bool _bIsByteData = false;
            public bool IsByteData
            {
                get { return _bIsByteData; }
            }

            public CSocketMessage(enumSocketMessageType enType, string strMessage)
            {
                _enType = enType;
                _strMessage = strMessage;

                _bIsByteData = false;
            }

            public CSocketMessage(enumSocketMessageType enType, byte[] byMessage)
            {
                _enType = enType;
                _byteMessage = byMessage;

                _bIsByteData = true;
            }

            private enumSocketMessageType _enType = enumSocketMessageType.SEND;
            public enumSocketMessageType Type
            {
                get { return _enType; }
                set { _enType = value; }
            }

            private string _strMessage = string.Empty;
            public string Message
            {
                get { return _strMessage; }
                set { _strMessage = value; }
            }

            private byte[] _byteMessage = null;
            public byte[] ByteMessage
            {
                get { return _byteMessage; }
                set { _byteMessage = value; }
            }
        }
        #endregion

        #region connection mode (Server / Client)
        protected enumSocketConnectionMode _enConnectionMode = enumSocketConnectionMode.ACTIVE;
        public enumSocketConnectionMode ConnectionMode
        {
            get { return _enConnectionMode; }
        }

        protected enumConnectionState _ConnectionState = enumConnectionState.Disconnected;
        public enumConnectionState ConnectionState
        {
            get { return _ConnectionState; }
        }
        #endregion

        string _DriverName = string.Empty;
        public string DriverName
        {
            get { return _DriverName; }
            set { _DriverName = value; }
        }
        protected DateTime _Alivetime;


        protected IPAddress _IP = IPAddress.Parse("192.168.0.1");
        protected int _iPort = 10000;
        protected int _iStage = 0;
        protected TcpListener _socketServer = null;
        protected TcpClient _socketClient = null;

        protected Thread _threadReceiveData = null;
        protected Thread _threadMessageQueue = null;
        protected Queue<CSocketMessage> _queueMessage = new Queue<CSocketMessage>();
        protected bool _bMessageQueueThreadRunning = false;

        protected System.Timers.Timer _tmrReconnect = new System.Timers.Timer(2000);

        public CSocketDriver()
        {
            _tmrReconnect.Elapsed += new System.Timers.ElapsedEventHandler(_tmrReconnect_Elapsed);
        }

        protected void InitConnectionString(string strIP, int iPort, int iStage, string strMode)
        {
            _IP = IPAddress.Parse(strIP);
            _iPort = iPort;
            _iStage = iStage;
            _enConnectionMode = (enumSocketConnectionMode)Enum.Parse(typeof(enumSocketConnectionMode), strMode);
        }

        protected void StartReceiveDataThread()
        {
            ThreadStart ts = new ThreadStart(DoRecieveData);

            _threadReceiveData = null;
            _threadReceiveData = new System.Threading.Thread(ts);
            _threadReceiveData.Start();
        }

        protected int RequestConnection()
        {
            util.SaveLog(this._iStage, "IROCV is reconnecting...");

            IPEndPoint remote = null;
            try
            {
                CloseClientSocket();
                int timeout = 1000;
                _socketClient = new TcpClient();
                //_socketClient.Connect(_IP, _iPort);
                Task result = _socketClient.ConnectAsync(_IP, _iPort);
                int index = Task.WaitAny(new[] { result }, timeout);

                if (_socketClient.Connected)
                {
                    remote = (IPEndPoint)_socketClient.Client.RemoteEndPoint;

                    // SystemLogger.Log(string.Format("Driver Connected with {0} : {1}, Mode : {2}", remote.Address, remote.Port.ToString(), _enConnectionMode), this);
                    StartMessageQueueThread();
                    StartReceiveDataThread();
                    _ConnectionState = enumConnectionState.Connected;
                    RaiseOnConnection(_iStage, enumConnectionState.Connected);
                    return 0;
                }
                else
                {
                    RaiseOnConnection(_iStage, enumConnectionState.Disconnected);
                    return 1;
                }


            }
            catch (Exception ex)
            {
                if (_socketClient != null)
                {
                    _socketClient.Close();
                    _socketClient = null;
                }

                return -1;
            }
        }

        void _tmrReconnect_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                //RequestConnection();

                _tmrReconnect.Stop();
                if (RequestConnection() != 0)
                    _tmrReconnect.Start();
                else
                    _tmrReconnect.Stop();
            }
            catch (Exception ex)
            {
            }
        }

        protected int OpenSocketPort()
        {
            try
            {
                CloseSocket();

                if (_enConnectionMode == enumSocketConnectionMode.ACTIVE)
                {
                    if (RequestConnection() != 0) // 연결이 되지 않았으면
                    {
                        _tmrReconnect.Start();
                        return 1;
                    }
                }
                else if (_enConnectionMode == enumSocketConnectionMode.PASSIVE)
                {
                    StartReceiveDataThread();
                }
            }
            catch (Exception ex)
            {
            }

            return 0; // 연결이 되었으면
        }

        protected void CloseClientSocket()
        {
            try
            {
                if (_socketClient != null && _socketClient.Client != null && _socketClient.Connected)
                {
                    _socketClient.Client.Shutdown(SocketShutdown.Both);
                    _socketClient.Client.Disconnect(false);

                    System.Threading.Thread.Sleep(10);
                    _socketClient.Close();
                }
                _socketClient = null;

                if (_ConnectionState == enumConnectionState.Connected)
                {
                    // SystemLogger.Log("Driver Disconnected", this);
                    _ConnectionState = enumConnectionState.Disconnected;

                    if (_enConnectionMode == enumSocketConnectionMode.ACTIVE)
                    {
                        if (!_tmrReconnect.Enabled)
                            _tmrReconnect.Enabled = true;
                    }
                }
                else
                {
                    _ConnectionState = enumConnectionState.Disconnected;
                }
            }
            catch (Exception ex)
            {
                if (!_tmrReconnect.Enabled)
                    _tmrReconnect.Enabled = true;
                //SystemLogger.Log(ex);
            }
        }

        protected void StopReconnectTimer()
        {
            _tmrReconnect.Enabled = false;
        }

        protected void CloseSocket()
        {
            if (_socketClient != null)
            {
                CloseClientSocket();
                _socketClient = null;
            }

            if (_socketServer != null)
            {
                _socketServer.Stop();
                _socketServer.Server.Close();
                _socketServer = null;
            }

            StopMessageQueueThread();
            StopReceiveDataThread();
        }

        void StartMessageQueueThread()
        {
            StopMessageQueueThread();

            _queueMessage.Clear();
            System.Threading.ThreadStart sTh = new System.Threading.ThreadStart(DoMessageQueue);

            if (_threadMessageQueue == null)
            {
                _threadMessageQueue = new System.Threading.Thread(sTh);
                _bMessageQueueThreadRunning = true;
                _threadMessageQueue.Start();
            }
        }

        void StopMessageQueueThread()
        {
            _bMessageQueueThreadRunning = false;
            if (_threadMessageQueue != null)
            {
                _threadMessageQueue.Abort();
                _threadMessageQueue = null;
            }
             
            _queueMessage.Clear();
        }

        void StopReceiveDataThread()
        {
            if (_threadReceiveData != null)
            {
                _threadReceiveData.Abort();
                _threadReceiveData = null;
            }
        }

        /// <summary>
        /// String 으로 부터 Encoding된 Byte Array를 Return한다.
        /// </summary>
        /// <param name="strData"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        protected virtual byte[] GetEncodedBytes(string strData, int iCount)
        {
            return Encoding.ASCII.GetBytes(strData.ToCharArray(), 0, iCount);
        }

        /// <summary>
        /// Byte Array 으로 부터 Encoding된 String를 Return한다.
        /// </summary>
        /// <param name="byData"></param>
        /// <param name="iCount"></param>
        /// <returns></returns>
        //* Encoding.ASCII.GetSTring()은 127 이상을 ?로 대체 함.
        protected virtual string GetEncodedString(byte[] byData, int iCount)
        {
            //return Encoding.ASCII.GetString(byData, 0, iCount);
            //return Encoding.GetEncoding(437).GetString(byData);
            Encoding enc = Encoding.GetEncoding("iso-8859-1");
            return enc.GetString(byData, 0, iCount);
        }

        void DoMessageQueue()
        {
            CSocketMessage item = null;
            Byte[] byteMessage = null;
            while (_bMessageQueueThreadRunning)
            {
                try
                {
                    if (_queueMessage != null && _queueMessage.Count > 0)
                    {
                        item = _queueMessage.Dequeue();
                        if (item == null) return;
                        switch (item.Type)
                        {
                            case enumSocketMessageType.RECEIVE:
                                ParseMessage(item.Message);
                                break;
                            case enumSocketMessageType.SEND:
                                if (_ConnectionState == enumConnectionState.Connected)
                                {
                                    if (_socketClient != null && _socketClient.Client != null && _socketClient.Client.Connected)
                                    {
                                        if (item.IsByteData)
                                        {
                                            //SystemLogger.Log(Level.Info, string.Format("SEND : {0}", GetEncodedString(item.ByteMessage, item.ByteMessage.Length)), this);
                                            byteMessage = item.ByteMessage;
                                        }
                                        else
                                        {
                                            //SystemLogger.Log(Level.Info, string.Format("SEND : {0}", item.Message), this);
                                            byteMessage = GetEncodedBytes(item.Message, item.Message.Length);
                                        }

                                        _socketClient.GetStream().Write(byteMessage, 0, byteMessage.Length);
                                    }
                                }
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                }

                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strMessage"></param>
        /// <returns></returns>
        virtual protected int ParseMessage(string strMessage)
        {
            //SystemLogger.Log(string.Format("RECV : {0}", strMessage), Name);

            return 0;
        }

        virtual public int Send(string strMessage)
        {
            _queueMessage.Enqueue(new CSocketMessage(enumSocketMessageType.SEND, strMessage));
            return 0;
        }

        virtual public int Send(byte[] byteMessage)
        {
            _queueMessage.Enqueue(new CSocketMessage(enumSocketMessageType.SEND, byteMessage));
            return 0;
        }

        protected byte[] _byBuffer = new byte[8192];
        protected int _iBufferSize = 8192;
        /// <summary>
        /// 소켓 스트림으로부터 메세지를 읽어들입니다.
        /// </summary>
        /// <param name="sck"> 데이터를 읽을 소켓입니다.</param>
        /// <param name="rTimeOut"> Inter Charactor Timeout (T8)을 체크할 값입니다. </param>
        /// <returns> 읽은 바이트 수 입니다.</returns>
        protected int ReadStream2(TcpClient socket, int rTimeOut)
        {
            string _msgString = "";
            int socket_len = 0;
            Stream stream = socket.GetStream();
            try
            {
                int iRead = stream.Read(_byBuffer, 0, _iBufferSize);
                
                //_msgString += GetEncodedString(_byBuffer, iRead);
                if (_msgString.Substring(0, 1) == STX)
                {
                    socket_len = Convert.ToInt32(_msgString.Substring(8, 5)) + 14;
                    if(_msgString.Substring(socket_len - 1, 1) == ETX)
                    {
                        _Alivetime = DateTime.Now;
                        //LogRawReceivedData(iRead);
                        _queueMessage.Enqueue(new CSocketMessage(enumSocketMessageType.RECEIVE, _msgString));
                        //_queueMessage.Enqueue(new CSocketMessage(enumSocketMessageType.RECEIVE, GetEncodedString(_byBuffer, iRead)));
                    }
                }
            }
            catch (Exception ex)
            {
                //SystemLogger.Log(ex);
                CloseClientSocket();
                return 1;
            }
            return 0;
        }

        protected int ReadStream(TcpClient socket, int rTimeOut)
        {
            Stream stream = socket.GetStream();
            try
            {
                int iRead = stream.Read(_byBuffer, 0, _iBufferSize);
                if (iRead > 0)
                {
                    _Alivetime = DateTime.Now;
                    _queueMessage.Enqueue(new CSocketMessage(enumSocketMessageType.RECEIVE, GetEncodedString(_byBuffer, iRead)));
                }
            }
            catch (Exception ex)
            {
                //SystemLogger.Log(ex);
                CloseClientSocket();
                return 1;
            }
            return 0;
        }

        protected void LogRawReceivedData(int iCount)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("RECV : ");
            for (int i = 0; i < iCount; i++)
            {
                sb.AppendFormat("{0:X2} ", _byBuffer[i]);
            }
            //SystemLogger.Log(Level.Debug, sb.ToString(), this);
        }

        void DoRecieveData()
        {
            IPEndPoint local = null;
            IPEndPoint remote = null;

            while (true)
            {
                try
                {
                    bool bAcceptError = true;
                    if (_enConnectionMode == enumSocketConnectionMode.PASSIVE)
                    {
                        if (_socketServer != null && _socketServer.Server != null)
                        {
                            _socketServer.Stop();
                            _ConnectionState = enumConnectionState.Disconnected;
                            RaiseOnConnection(_iStage, enumConnectionState.Disconnected);
                            _Alivetime = DateTime.Now;

                        }

                        local = new IPEndPoint(_IP, _iPort);

                        if (_socketServer == null)
                        {
                            _socketServer = new TcpListener(local);
                            _socketServer.Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 5000);

                            int size = sizeof(UInt32);
                            UInt32 on = 1;
                            UInt32 keepAliveInterval = 1000;   // Send a packet once every 10 seconds.
                            UInt32 retryInterval = 500;        // If no response, resend every second.
                            byte[] inArray = new byte[size * 3];
                            Array.Copy(BitConverter.GetBytes(on), 0, inArray, 0, size);
                            Array.Copy(BitConverter.GetBytes(keepAliveInterval), 0, inArray, size, size);
                            Array.Copy(BitConverter.GetBytes(retryInterval), 0, inArray, size * 2, size);
                            _socketServer.Server.IOControl(IOControlCode.KeepAliveValues, inArray, null);
                        }
                        try
                        {
                            _socketServer.Start();

                            try
                            {
                                _socketClient = _socketServer.AcceptTcpClient();
                                bAcceptError = false;
                            }
                            catch (Exception ex)
                            {
                                //SystemLogger.Log(ex);
                                //OnDriverMessage(DriverMessageLevel.ERROR, "Error while accepting socket " + ex.Message + "\r\n" + ex.StackTrace);

                                bAcceptError = true;
                            }

                        }
                        catch (SocketException sckEx)
                        {
                            if (sckEx.SocketErrorCode == SocketError.AddressNotAvailable)
                            {
                                // 이 경우 네트워크에 문제 있어 소켓을 쓸 수 없다. 
                                // 정상화 될때까지 재 시도를 많이 하므로 별도로 파일 로그는 찍지 않는다.
                                //OnDriverMessage(DriverMessageLevel.ERROR, "Error while starting server socket " + sckEx.Message + "\r\n" + sckEx.StackTrace);
                            }
                        }
                        catch (Exception ex)
                        {
                            //SystemLogger.Log(ex);
                            //OnDriverMessage(DriverMessageLevel.ERROR, "Error while starting server socket " + ex.Message + "\r\n" + ex.StackTrace);

                            bAcceptError = true;
                        }

                        if (!bAcceptError)
                        {
                            remote = (IPEndPoint)_socketClient.Client.RemoteEndPoint;
                            // SystemLogger.Log(string.Format("Driver Connected with {0} : {1}, Mode : {2}", remote.Address, remote.Port.ToString(), _enConnectionMode), this);
                            StartMessageQueueThread();
                            _ConnectionState = enumConnectionState.Connected;
                            RaiseOnConnection(_iStage, enumConnectionState.Connected);
                        }
                    }
                    else
                    {
                        bAcceptError = false;
                    }

                    while (!bAcceptError)
                    {
                        if (_socketClient == null)
                            break;
                        else if (_socketClient.Client == null)
                            break;
                        else if (!_socketClient.Client.Connected)
                            break;

                        if (ReadStream(_socketClient, 0) != 0)
                        {
                        }

                        if (_socketClient == null || !_socketClient.Connected)
                        {
                            if (_ConnectionState == enumConnectionState.Disconnected)
                            {
                                // SystemLogger.Log("Driver Disconnected", this);
                                _ConnectionState = enumConnectionState.Disconnected;
                                RaiseOnConnection(_iStage, enumConnectionState.Disconnected);

                                if (_enConnectionMode == enumSocketConnectionMode.ACTIVE)
                                {
                                    if (!_tmrReconnect.Enabled)
                                        _tmrReconnect.Enabled = true;
                                }
                                break;
                            }
                        }

                        //if (((TimeSpan)(DateTime.Now - _Alivetime)).Minutes > 1)
                        if (((TimeSpan)(DateTime.Now - _Alivetime)).Seconds > 15)
                        {
                            //Client가 연결이 종료된 것을 확인 할 방법이 없는 경우 처리
                            bAcceptError = true;
                            //SystemLogger.Log("Driver Disconnected", this);
                            _ConnectionState = enumConnectionState.Disconnected;
                            RaiseOnConnection(_iStage, enumConnectionState.Disconnected);
                        }
                    }
                }
                catch (Exception ex)
                {
                    //SystemLogger.Log(ex);
                    //OnDriverMessage(DriverMessageLevel.ERROR, ex.Message + "\r\n" + ex.StackTrace);
                }

                if (_enConnectionMode == enumSocketConnectionMode.ACTIVE)
                {
                    if (_socketClient == null)
                        break;
                    else if (_socketClient.Client == null)
                        break;
                    else if (_socketClient.Connected == false)
                        break;

                    if (!_tmrReconnect.Enabled)
                        _tmrReconnect.Enabled = true;

                }

                Thread.Sleep(1);
            }
        }


    }
}
