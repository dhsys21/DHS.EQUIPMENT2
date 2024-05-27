using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHS.EQUIPMENT.Common
{
    static class _Constant
    {
        public static readonly int frmCount = 1;
        public static readonly int StartPosition = 1;
        public static readonly int ChannelCount = 32;

        public static readonly double delDayInterval = 30;

        #region PATH
        public static readonly string APP_PATH = @"D:\IROCV\";
        public static readonly string BIN_PATH = APP_PATH + "Bin\\";
        public static readonly string DATA_PATH = APP_PATH + "Data\\";
        public static readonly string LOG_PATH = APP_PATH + "Log\\";
        public static readonly string MSA_PATH = APP_PATH + "MSA\\";
        public static readonly string OFFSET_PATH = APP_PATH + "OFFSET\\";
        public static readonly string TRAY_PATH = APP_PATH + "Tray\\";

        #endregion

        #region IR/OCV Equipment Status
        public static string STEPVACANCY = "IR/OCV is ready... ";
        public static string IROCVCONNECTION = "IR/OCV is not connected";
        public static string PLCCONNECTION = "PLC is not connected";
        #endregion

        #region Color Status
        //* 측정 color
        public static readonly Color ColorReady = Color.LightBlue;
        public static readonly Color ColorNoCell = Color.Silver;
        public static readonly Color ColorOutFlow = Color.Yellow;
        public static readonly Color ColorOCVNG = Color.FromArgb(0, 94, 255);
        public static readonly Color ColorIRNG = Color.Salmon;

        //* msa /offset color
        public static readonly Color ColorIR = Color.Wheat;//Color.FromArgb(187, 210, 251);
        public static readonly Color ColorOCV = Color.White;// Color.FromArgb(255, 205, 210);
        public static readonly Color ColorStandard = Color.FromArgb(144, 202, 249);
        public static readonly Color ColorMeasure = Color.GhostWhite;

        public static readonly Color ColorCharging = Color.Orange;
        public static readonly Color ColorFinish = Color.LimeGreen;
        public static readonly Color ColorVoltage = Color.LightSkyBlue;
        public static readonly Color ColorCurrent = Color.LightPink;
        #endregion

        #region (PreCharger, Channel) Status
        public static readonly int nNoAnswer = 0;
        public static readonly int nIdle = 1;
        public static readonly int nVacancy = 2;
        public static readonly int nIN = 3;
        public static readonly int nREADY = 4;
        public static readonly int nRUN = 5;
        public static readonly int nEND = 6;
        public static readonly int nFinish = 7;
        public static readonly int nManual = 8;
        public static readonly int nOpbox = 9;  // IMS 프로토콜 외 별도 생성
        public static readonly int nEmergency = 10;
        #endregion

        #region PLC - PC ADDRESS
        //public static readonly int DB_NUMBER = 85;
        public static readonly int PLC_DATA_LENGTH = 50;
        public static readonly int PC_DATA_LENGTH = 50;
        public static readonly int TRAY_ID_LENGTH = 20;

        //* FOR PLC
        public static readonly int PLC_HEART_BEAT = 0;
        public static readonly int PLC_ATUO_MANUAL = 1;
        public static readonly int PLC_ERROR = 2;
        public static readonly int PLC_TRAY_IN = 3;
        public static readonly int PLC_TRAY_DOWN = 4;
        public static readonly int PLC_TRAY_UP = 5;
        public static readonly int PLC_JOB_CHANGE = 6;
        public static readonly int PLC_READY_COMPLETE = 7;
        public static readonly int PLC_UNLOADING_COMPLETE = 8;
        public static readonly int PLC_TRAY_ID = 10;

        //* FOR PC
        public static readonly int PC_HEART_BEAT = 0;
        public static readonly int PC_AUTO_MANUAL = 1;
        public static readonly int PC_ERROR = 2;
        public static readonly int PC_TRAY_OUT = 3;
        public static readonly int PC_TRAY_DOWN = 4;
        public static readonly int PC_TRAY_UP = 5;
        public static readonly int PC_MEASUREMENT_WAIT = 6;
        public static readonly int PC_RUNNING = 7;
        public static readonly int PC_MEASUREMENT_COMPLETE = 8;

        //* PLC 데이터를 MES로 넘겨주기 위해 만듦 2024 05 27
        //public static readonly int DB_NUMBER_SYS = 66;
        public static readonly int PLC_DATA_LENGTH_SYS = 256;
        public static readonly int PLC_SYS_STRING_LENGTH = 98;

        public static readonly int PLC_INTERFACE_VERSION_PROJECT = 0;
        public static readonly int PLC_EQUIPMENT_NAME = 100;
        public static readonly int PLC_EQUIPMENT_TYPE_ID = 200;
        public static readonly int PLC_LINE_ID = 202;
        public static readonly int PLC_AREA_ID = 204;
        public static readonly int PLC_VENDOR_ID = 206;
        public static readonly int PLC_EQUIPMENT_ID = 208;
        public static readonly int PLC_STATE = 210;
        public static readonly int PLC_MODE = 212;
        public static readonly int PLC_BLOCKED = 214;
        public static readonly int PLC_STARVED = 215;
        public static readonly int PLC_CURRNET_SPEED = 216;
        public static readonly int PLC_DESIGNED_SPEED = 218;
        public static readonly int PLC_DEFECTOR_COUNTER = 220;
        public static readonly int PLC_GOOD_COUNTER = 220;
        public static readonly int PLC_TOTAL_COUNTER = 220;
        public static readonly int PLC_STAND_STILL_REASON = 222;
        public static readonly int PLC_STACK_LIGHT0_COLOR = 224;
        public static readonly int PLC_STACK_LIGHT0_BEHAVIOR = 226;
        public static readonly int PLC_STACK_LIGHT1_COLOR = 228;
        public static readonly int PLC_STACK_LIGHT1_BEHAVIOR = 230;
        public static readonly int PLC_STACK_LIGHT2_COLOR = 232;
        public static readonly int PLC_STACK_LIGHT2_BEHAVIOR = 234;
        public static readonly int PLC_STACK_LIGHT3_COLOR = 236;
        public static readonly int PLC_STACK_LIGHT3_BEHAVIOR = 238;
        public static readonly int PLC_STACK_LIGHT4_COLOR = 240;
        public static readonly int PLC_STACK_LIGHT4_BEHAVIOR = 242;
        public static readonly int PLC_STACK_LIGHT5_COLOR = 244;
        public static readonly int PLC_STACK_LIGHT5_BEHAVIOR = 246;
        #endregion

    }

    public enum enumEquipMode
    {
        AUTO = 0,
        MANUAL = 1, //* MANUAL == MSA
        OFFSET = 2
    }
    public enum enumMsaStatus
    {
        StepTrayDown = 0,
        StepTrayUp = 1,
        StepFinish = 3
    }
    public enum enumEquipStatus
    {
        StepVacancy = 0,
        StepTrayIn = 1,
        StepReady = 2,
        StepRun = 3,
        StepEnd = 4,
        StepTrayOut = 5,
        StepManual = 6,
        StepNoAnswer = 7,
        StepEmergency = 8
    }
    public enum enumSocketConnectionMode
    {
        // Server Style
        PASSIVE,
        // Client Style
        ACTIVE
    }

    public enum enumSocketMessageType
    {
        RECEIVE,
        SEND
    }
    public enum enumConnectionState
    {
        Disabled = 0,
        Enabled = 1,
        Disconnected = 2,
        Connected = 3,
        Retry = 4,
        TimeOut = 5
    }
    public enum enumStageError
    {
        NoError = 0,
        IROCVDisconnected = 1,
        PLCDisconnected = 2,
        MESDisconnected = 3,
        IROCVNoResponse = 4,
        IROCVNotRemote = 5
    }
    public enum enumLanguage
    {
        Kor = 0,
        Eng = 1,
        Nor = 2
    }
}
