using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DHS.EQUIPMENT.Common;

namespace DHS.EQUIPMENT
{
    class OPENUACLIENT
    {
        IROCVData[] irocvdata = new IROCVData[_Constant.frmCount];

        public static bool connection = false;
        private bool _bconnected = false;

        public static string _strHost;
        public static int _iPort;
        public bool isRead = false;
        public bool isWrite = false;

        public bool CONNECTED { get => _bconnected; set => _bconnected = value; }

        private static OPENUACLIENT openuaclient = new OPENUACLIENT();
        public static OPENUACLIENT GetInstance()
        {
            if (openuaclient == null) openuaclient = new OPENUACLIENT();
            return openuaclient;
        }

        public OPENUACLIENT(string HOST, int PORT)
        {
            _strHost = HOST;
            _iPort = PORT;

            //* connection

            //* IROCV DATA
            for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
                irocvdata[nIndex] = IROCVData.GetInstance(nIndex);

            if (connection == true)
            {
                _bconnected = true;
                isRead = true;
                isWrite = false;
            }
            else
                _bconnected = false;
        }
        public OPENUACLIENT()
        {

        }

        private async void Connect()
        {
            try
            {

            }
            catch(Exception ex)
            {

            }
        }
        private void OnNotification()
        {

        }
        
        //* send tray id to MES
        private void NotifyTrayInfo(int stageno)
        {
            
        }
        //* receive tray info from MES
        private void OnNotifyTrayInfo(int stageno)
        {
            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                irocvdata[stageno].CELL[nIndex] = 1;
                irocvdata[stageno].IR_CHANNELOFFSET[nIndex] = 0;
            }

            #region 채널 offset - 앞뒤로 셀이 없을 때 추가 offset 설정
            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                if(irocvdata[stageno].CELL[nIndex] == 1)
                {
                    switch (nIndex)
                    {
                        case 0:
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.009;
                            if (irocvdata[stageno].CELL[nIndex + 2] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.002;
                            break;
                        case 1:
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.001;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.003;
                            break;
                        case 2:
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.003;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.004;
                            break;
                        case 3:
                            if (irocvdata[stageno].CELL[nIndex - 2] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.002;
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.009;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.001;
                            break;
                        case 4:
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.001;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.009;
                            if (irocvdata[stageno].CELL[nIndex + 2] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.002;
                            break;
                        case 5:
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.001;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.003;
                            break;
                        case 6:
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.003;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.004;
                            break;
                        case 7:
                            if (irocvdata[stageno].CELL[nIndex - 2] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.002;
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.009;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.001;
                            break;
                        case 8:
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.001;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.009;
                            if (irocvdata[stageno].CELL[nIndex + 2] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.002;
                            break;
                        case 9:
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.001;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.003;
                            break;
                        case 10:
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.003;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.004;
                            break;
                        case 11:
                            if (irocvdata[stageno].CELL[nIndex - 2] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.002;
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.009;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.001;
                            break;
                        case 12:
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.001;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.009;
                            if (irocvdata[stageno].CELL[nIndex + 2] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.002;
                            break;
                        case 13:
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.001;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.003;
                            break;
                        case 14:
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.003;
                            if (irocvdata[stageno].CELL[nIndex + 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.004;
                            break;
                        case 15:
                            if (irocvdata[stageno].CELL[nIndex - 2] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.002;
                            if (irocvdata[stageno].CELL[nIndex - 1] == 0) irocvdata[stageno].IR_CHANNELOFFSET[nIndex] -= 0.009;
                            break;
                        default:
                            break;
                    }
                }
            }
            #endregion
        }
        //* send IR/OCV result value to MES
        private void NotifyResultInfo(int stageno)
        {

        }
        //* receive IR/OCV result info(ok/ng tray-out/remeasure) from MES
        private void OnNotifyResultInfo(int stageno)
        {

        }
        public void ReadTrayInfo(int stageno)
        {
            OnNotifyTrayInfo(stageno);
        }
        public void WriteTrayInfo(int stageno)
        {
            NotifyTrayInfo(stageno);
        }
        public void ReadResultInfo(int stageno)
        {
            OnNotifyResultInfo(stageno);
        }
        public void WriteResultInfo(int stageno)
        {
            NotifyResultInfo(stageno);
        }
    }
}
