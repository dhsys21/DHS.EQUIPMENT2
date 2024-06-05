using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHS.EQUIPMENT.Common;
using Telerik.Collections.Generic;


namespace DHS.EQUIPMENT
{
    class IrocvProcess
    {
        public static IrocvProcess irocvprocess = null;
        Util util;
        SIEMENSS7LIB siemensplc;
        OPENUACLIENT opcuames;
        MesServer messerver;
        MesClient mesclient;
        CEquipmentData _system;
        PLCINTERFACE plcinterface = null;
        MESINTERFACE mesinterface = null;
        IROCV[] irocv = new IROCV[_Constant.frmCount];
        IROCVData[] irocvdata = new IROCVData[_Constant.frmCount];
        IROCVForm[] irocvform = new IROCVForm[_Constant.frmCount];
        IROCVConfig[] irocvconfig = new IROCVConfig[_Constant.frmCount];
        NGInfo nginfo;
        PasswordForm pwdForm;
        
        IROCVMeasureInfoForm measureinfo;
        IROCVRemeasureInfo remeasureinfo;
        //* Error Form
        private ErrorForm _errorForm;

        public Timer[] _tmrEquipStatus = new Timer[_Constant.frmCount];
        public Timer[] _tmrAutoInspection = new Timer[_Constant.frmCount];
        public Timer[] _tmrConnectionChange = new Timer[_Constant.frmCount];
        public Timer[] _tmrMsaInspection = new Timer[_Constant.frmCount];
        public Timer _tmrGetPlcData = new Timer();
        public Timer _tmrGetMesData = new Timer();
        public Timer DeleteFileTimer = null;

        private int msacount = 0;
        private int offsetcount = 0;
        private int nInspectionStep = 0;

        #region Connection Status 변수
        private bool[] _bIrocvConnected = new bool[_Constant.frmCount];
        private bool _bPlcConnected = false;
        private bool _bMesConnected = false;

        public bool[] IROCVCONNECTED { get => _bIrocvConnected; set => _bIrocvConnected = value; }
        public bool PLCCONNECTED { get => _bPlcConnected; set => _bPlcConnected = value; }
        public bool MESCONNECTED { get => _bMesConnected; set => _bMesConnected = value; }
        #endregion

        public static IrocvProcess GetInstance()
        {
            if (irocvprocess == null) irocvprocess = new IrocvProcess();
            return irocvprocess;
        }
        public IrocvProcess()
        {
            irocvprocess = this;
            util = new Util();
            _system = CEquipmentData.GetInstance();

            //* Make Folder
            MakeFolder();

            #region PLC
            _bPlcConnected = false;
            plcinterface = PLCINTERFACE.GetInstance();
            plcinterface.OnWritePLCClick += _PLCINTERFACE_WritePLC;

            //* PLC Connection
            try
            {
                //siemensplc = new SIEMENSS7LIB("192.168.10.1");
                siemensplc = new SIEMENSS7LIB(_system.PLCIPADDRESS, _system.PLCDBNUMBER, _system.PLCDBNUMBERSYS);
                _bPlcConnected = SIEMENSS7LIB.connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //* PLC Timer
            _tmrGetPlcData.Interval = 1000;
            _tmrGetPlcData.Tick += new EventHandler(GetPlcDataTimer_Tick);
            _tmrGetPlcData.Enabled = true;
            #endregion

            #region MES
            _bMesConnected = false;
            mesinterface = MESINTERFACE.GetInstance();
            mesinterface.OnWriteButtonClick += _MesClient_WriteMesValue;
            //* MES 시뮬레이션
            mesinterface.OnWriteForIR1 += _MesClient_WriteForIR1;
            mesinterface.OnWriteForIR2 += _MeSClient_WriteForIR2;
            mesinterface.OnReadForIR1 += _MesClient_ReadForIR1;
            mesinterface.OnReadForIR2 += _MesClient_ReadForIR2;
            mesinterface.OnWritePLCSysInfo += _MesClient_WritePLCSysInfo;

            //* MES Connection
            try
            {
                messerver = new MesServer();
                Task.Run(() => messerver.MesServerStartAsync());

                mesclient = new MesClient();
                mesclient.OnSetDataToDgv += _MesClient_SetDataToDgv;
                mesclient.OnSaveMesLog += _MesClient_SaveMesLog;

                //_bMesConnected = MesClient.connection;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            //* MES Timer
            _tmrGetMesData.Interval = 1000;
            _tmrGetMesData.Tick += new EventHandler(GetMesDataTimer_TickAsync);
            //_tmrGetMesData.Enabled = true;
            #endregion

            ReadChannelMapping();

            //* Delete File Timer
            DeleteFileTimer = new Timer();
            DeleteFileTimer.Interval = 60 * 60 * 1000; //1시간에 한번씩 데이터 삭제
            DeleteFileTimer.Tick += new EventHandler(DeleteFileTimer_Tick);
            DeleteFileTimer.Enabled = true;

            #region Measure Info Form
            //* IROCV MEASURE INFO FORM
            measureinfo = IROCVMeasureInfoForm.GetInstance();
            measureinfo.OnProbeOpenClick += _MEASUREINFOFORM_ProbeOpen;
            measureinfo.OnProbeCloseClick += _MEASUREINFOFORM_ProbeClose;
            measureinfo.OnMsaStartClick += _MEASUREINFOFORM_MsaStart;
            measureinfo.OnMsaStopClick += _MEASUREINFOFORM_MsaStop;
            measureinfo.OnOffsetStartClick += _MEASUREINFOFORM_OffsetStart;
            measureinfo.OnOffsetStopClick += _MEASUREINFOFORM_OffsetStop;
            measureinfo.OnOffsetApplyClick += _MEASUREINFOFORM_OffsetApply;
            measureinfo.OnOffsetCmdIrClick += _MEASUREINFOFORM_OffsetCmdIr;
            measureinfo.OnOffsetOpenClick += _MEASUREINFOFORM_OffsetOpen;
            measureinfo.OnOffsetSaveClick += _MEASUREINFOFORM_OffsetSave;
            measureinfo.OnManualSaveClick += _MEASUREINFOFORM_ManualSave;
            measureinfo.OnIRFetchClick += _MEASUREINFOFORM_IRFetch;
            measureinfo.OnOCVFetchClick += _MEASUREINFOFORM_OCVFetch;
            measureinfo.OnAmsStartClick += _MEASUREINFOFORM_AmsStart;
            measureinfo.OnAmsStopClick += _MEASUREINFOFORM_AmsStop;
            measureinfo.OnInitDataClick += _MEASUREINFOFORM_InitData;
            #endregion

            #region Remeasure Info Form
            remeasureinfo = IROCVRemeasureInfo.GetInstance();
            remeasureinfo.OnRemeasureAllClick += _REMEASUREINFOFORM_RemeasureAll;
            remeasureinfo.OnTrayOutClick += _REMEASUREINFOFORM_TrayOut;
            #endregion

            //* Password Form
            pwdForm = new PasswordForm();

            //* NG INFO
            nginfo = NGInfo.GetInstance();

            //* Error Form
            _errorForm = ErrorForm.GetInstance();
            _errorForm.StartPosition = FormStartPosition.CenterScreen;

            #region IR/OCV
            //* IROCV 
            for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
            {
                //* IROCV Socekt
                #region IR/OCV
                string config_fn;
                string HOST;
                int PORT;

                //HOST = "192.168.100.101";
                PORT = 45000;

                //* IROCV FORM
                irocvform[nIndex] = IROCVForm.GetInstance(nIndex);
                irocvform[nIndex].OnGridViewCellMouseEnter += _IROCVFORM_SetValueToLabel;
                irocvform[nIndex].OnOperationMode += _IROCVFORM_SetOperationMode;
                irocvform[nIndex].OnIROCVReset += _IROCVFORM_IROCVReset;
                irocvform[nIndex].OnNGInfo += _IROCVFORM_NGInfo;
                irocvform[nIndex].OnConfigForm += _IROCVFORM_ConfigForm;
                irocvform[nIndex].OnRemeasureInfo += _IROCVFORM_REMEASUREInfo;

                //* IROCV CONFIG
                irocvconfig[nIndex] = IROCVConfig.GetInstance(nIndex);
                irocvconfig[nIndex].OnSaveButtonClick += _IROCVCONFIG_SaveConfig;
                irocvconfig[nIndex].STAGENO = nIndex;
                
                //* IROCV
                irocv[nIndex] = IROCV.GetInstance(nIndex);
                ReadConfigFile(nIndex);
                irocv[nIndex].Open(_system.IPADDRESS[nIndex], PORT, nIndex, "ACTIVE");
                irocv[nIndex].STAGENO = nIndex;
                irocv[nIndex].EQUIPMODE = enumEquipMode.AUTO;
                irocv[nIndex].EQUIPSTATUS = enumEquipStatus.StepVacancy;

                irocv[nIndex].OnInitIROCV += _IROCV_Initialize;
                irocv[nIndex].OnProcessAms += _IROCV_ProcessAms;
                irocv[nIndex].OnProcessAmf += _IROCV_ProcessAmf;
                irocv[nIndex].OnProcessStop += _IROCV_ProcessStop;
                irocv[nIndex].OnProcessIr += _IROCV_ProcessIr;
                irocv[nIndex].OnProcessOcv += _IROCV_ProcessOcv;
                irocv[nIndex].OnIROCVError += _IROCV_Error;
                irocv[nIndex].OnShowControlMessage += _IROCV_ShowControlMessage;

                _bIrocvConnected[nIndex] = false;
                #endregion

                //* IROCV DATA
                irocvdata[nIndex] = IROCVData.GetInstance(nIndex);
                irocvdata[nIndex].InitData();
                ReadOffsetFile(nIndex);

                int[] ngUse, ngCount;
                util.ReadNGInfo(nIndex, out ngUse, out ngCount);
                _system.REMEASUREUSE = ngUse;
                _system.REMEASURENG = ngCount;
                nginfo.SetNGInfo(nIndex);

                //* Auto Inspection Timer
                _tmrAutoInspection[nIndex] = new Timer();
                _tmrAutoInspection[nIndex].Interval = 1000;
                _tmrAutoInspection[nIndex].Tag = nIndex;
                _tmrAutoInspection[nIndex].Tick += new EventHandler(AutoInspectionTimer_Tick);

                //* MSA Timer
                _tmrMsaInspection[nIndex] = new Timer();
                _tmrMsaInspection[nIndex].Interval = 1000;
                _tmrMsaInspection[nIndex].Tag = nIndex;
                _tmrMsaInspection[nIndex].Tick += new EventHandler(MsaInspectionTimer_Tick);

                SetOperationMode(nIndex, true);

                //* Connection Status
                _tmrConnectionChange[nIndex] = new Timer();
                _tmrConnectionChange[nIndex].Interval = 1000;
                _tmrConnectionChange[nIndex].Tag = nIndex;
                _tmrConnectionChange[nIndex].Tick += new EventHandler(ConnectionChangeTimer_Tick);

                //* Equip Status
                _tmrEquipStatus[nIndex] = new Timer();
                _tmrEquipStatus[nIndex].Interval = 1000;
                _tmrEquipStatus[nIndex].Tag = nIndex;
                _tmrEquipStatus[nIndex].Tick += new EventHandler(EquipStatusTimer_Tick);
                _tmrEquipStatus[nIndex].Enabled = true;
            }
            #endregion

            _tmrGetMesData.Enabled = true;
        }

        
        #region MES Method
        private void _MesClient_WriteMesValue(string node, string value, int nDataType)
        {
            mesclient.WriteValue(node, value);
        }
        private void _MesClient_SetDataToDgv(string[] pcValues, string[] mesValues)
        {
            mesinterface.SetDataToGrid(pcValues, mesValues);
        }
        private void _MesClient_SaveMesLog(string mesLog)
        {
            util.SaveMesLog(mesLog);
        }
        //* MES 시뮬레이션
        private void _MesClient_WriteForIR1(string equipmentid, string trayid)
        {
            mesclient.WriteFOEQR2_1(0, equipmentid, trayid);
        }
        IROCVData irocvdataTest = new IROCVData();
        private void _MeSClient_WriteForIR2(string equipmentid, string trayid, string[] cellid, string[] cellstatus, double[] ir, double[] ocv)
        {

            irocvdataTest.EQUIPMENTID = equipmentid;
            irocvdataTest.TRAYID = trayid;
            irocvdataTest.CELLID = cellid;
            irocvdataTest.CELLSTATUSIROCV = cellstatus;
            irocvdataTest.IR_AFTERVALUE = ir;
            irocvdataTest.OCV = ocv;

            mesclient.WriteFOEQR2_2(0, irocvdataTest);
        }

        private void _MesClient_ReadForIR1(string[] cellid, string[] cellstatus, string traystatuscode, string errorcode, string errormessage)
        {
            mesclient.WriteFORIR1_ForMes(0, cellid, cellstatus, traystatuscode, errorcode, errormessage);
            mesclient.ReadFOEQR2_1(0);

            mesinterface.ShowReadMesValues(irocvdata[0].LOG);
        }
        private void _MesClient_ReadForIR2(string errorcode, string errormessage)
        {
            mesclient.WriteFORIR2_ForMes(0, errorcode, errormessage);
            mesclient.ReadFOEQR2_1(0);

            mesinterface.ShowReadMesValues(irocvdata[0].LOG);
        }

        private void _MesClient_WritePLCSysInfo()
        {
            mesclient.WritePLSInfo(0);
        }
        #endregion
        private void _PLCINTERFACE_WritePLC(int stageno, string tagname, int nValue)
        {
            if (tagname == "PC Heart Beat") siemensplc.SetHeartBeat(stageno, nValue);
            else if (tagname == "PC Auto Manual") siemensplc.SetAutoMode(stageno, nValue);
            else if (tagname == "PC Error") siemensplc.SetPCError(stageno, nValue);
            else if (tagname == "Tray Out") siemensplc.SetTrayOut(stageno, nValue);
            else if (tagname == "Tray Down") siemensplc.SetTrayDown(stageno, nValue);
            else if (tagname == "Tray Up") siemensplc.SetTrayUp(stageno, nValue);
            else if (tagname == "Measurement Wait") siemensplc.SetMeasurementWait(stageno, nValue);
            else if (tagname == "Measurement Run") siemensplc.SetMeasurementRunning(stageno, nValue);
            else if (tagname == "Measurement Complete") siemensplc.SetMeasurementComplete(stageno, nValue);
        }

        #region FILE MANAGE
        private void DeleteFileTimer_Tick(object sender, EventArgs e)
        {
            DeleteOldFiles();
        }
        private void DeleteOldFiles()
        {
            DeleteFileTimer.Enabled = false;
            try
            {
                util.DeleteOldFiles(_Constant.DATA_PATH, DateTime.Now.AddDays(_Constant.delDayInterval).ToString("yyyyMMdd"));
                util.DeleteOldFiles(_Constant.LOG_PATH, DateTime.Now.AddDays(_Constant.delDayInterval).ToString("yyyyMMdd"));
                util.DeleteOldFiles(_Constant.MSA_PATH, DateTime.Now.AddDays(_Constant.delDayInterval).ToString("yyyyMMdd"));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Delete old files error : " + ex.Message);
            }
            DeleteFileTimer.Enabled = true;
        }

        private void MakeFolder()
        {
            if (Directory.Exists(_Constant.APP_PATH) == false) Directory.CreateDirectory(_Constant.APP_PATH);
            if (Directory.Exists(_Constant.BIN_PATH) == false) Directory.CreateDirectory(_Constant.BIN_PATH);
            if (Directory.Exists(_Constant.DATA_PATH) == false) Directory.CreateDirectory(_Constant.DATA_PATH);
            if (Directory.Exists(_Constant.LOG_PATH) == false) Directory.CreateDirectory(_Constant.LOG_PATH);
            if (Directory.Exists(_Constant.MSA_PATH) == false) Directory.CreateDirectory(_Constant.MSA_PATH);
            if (Directory.Exists(_Constant.OFFSET_PATH) == false) Directory.CreateDirectory(_Constant.OFFSET_PATH);
        }
        #endregion

        #region CONFIG/ RESULT FILE/ NG INFO

        #region Channel Mapping
        private void ReadChannelMapping()
        {
            int linenumber = 0;
            string filename = _Constant.BIN_PATH + "mapping.csv";
            if (File.Exists(filename))
            {
                try
                {
                    var reader = new StreamReader(File.OpenRead(filename));
                    while (!reader.EndOfStream)
                    {
                        linenumber++;
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if (linenumber != 1)
                        {
                            _system.CHANNELMAPPING.Add(Convert.ToInt32(values[0]), Convert.ToInt32(values[1]));
                        }

                        Console.WriteLine("Key => " + values[0] + " , Value => " + values[1]);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            }
            else
            {
                string strChannelMapping = "before,after";
                for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                {
                    _system.CHANNELMAPPING.Add(nIndex + 1, nIndex + 1);
                    strChannelMapping += (nIndex + 1).ToString() + "," + (nIndex + 1).ToString() + Environment.NewLine;
                }

                util.FileWrite(filename, strChannelMapping);
            }

        }
        #endregion

        #region Read Config / Offset
        private void SaveConfigFile(int stageno)
        {
            string filename = _Constant.BIN_PATH + "SystemInfo_" + (stageno + 1) + ".inf";

            util.saveConfig(filename, "EQUIPMENT_ID", "ID", _system.EQUIPMENTID);
            util.saveConfig(filename, "IROCV", "IPADDRESS", _system.IPADDRESS[stageno]);
            util.saveConfig(filename, "OUTFLOW", "OCV_MIN", _system.OCVMINVALUE.ToString());
            util.saveConfig(filename, "REMEASURE", "REMEASURE_COUNT", _system.REMEASURECOUNT.ToString());
            util.saveConfig(filename, "REMEASURE", "REMEASURE_MAX_COUNT", _system.REMEASUREMAXCOUNT.ToString());
            util.saveConfig(filename, "REMEASURE", "REMEASURE_PERCENT", _system.REMEASUREPERCENT.ToString());

            //* IR SPEC
            util.saveConfig(filename, "SPEC", "IRMIN", _system.IRMIN.ToString());
            util.saveConfig(filename, "SPEC", "IRMAX", _system.IRMAX.ToString());
            util.saveConfig(filename, "SPEC", "IRREMEAMIN", _system.IRREMEAMIN.ToString());
            util.saveConfig(filename, "SPEC", "IRREMEAMAX", _system.IRREMEAMAX.ToString());

            //* OCV SPEC
            util.saveConfig(filename, "SPEC", "OCVMIN", _system.OCVMIN.ToString());
            util.saveConfig(filename, "SPEC", "OCVMAX", _system.OCVMAX.ToString());
            util.saveConfig(filename, "SPEC", "OCVREMEAMIN", _system.OCVREMEAMIN.ToString());
            util.saveConfig(filename, "SPEC", "OCVREMEAMAX", _system.OCVREMEAMAX.ToString());
        }
        private void ReadConfigFile(int stageno)
        {
            string filename = _Constant.BIN_PATH + "SystemInfo_" + (stageno + 1) + ".inf";
            try
            {
                if (File.Exists(filename))
                {
                    _system.EQUIPMENTID = util.readConfig(filename, "EQUIPMENT_ID", "ID");
                    irocvform[stageno].SetStageTitle(_system.EQUIPMENTID);
                    _system.IPADDRESS[stageno] = util.readConfig(filename, "IROCV", "IPADDRESS");

                    int remeasurecount = 0;
                    _system.REMEASURECOUNT = util.TryParseInt(util.readConfig(filename, "REMEASURE", "REMEASURE_COUNT"), remeasurecount);
                    int remeasuremaxcount = 0;
                    _system.REMEASUREMAXCOUNT = util.TryParseInt(util.readConfig(filename, "REMEASURE", "REMEASURE_MAX_COUNT"), remeasuremaxcount);
                    int remeasurepercent = 0;
                    _system.REMEASUREPERCENT = util.TryParseInt(util.readConfig(filename, "REMEASURE", "REMEASURE_PERCENT"), remeasurepercent);

                    //HOST, PORT, nIndex, "ACTIVE");
                    irocv[stageno].ChangeSetting(_system.IPADDRESS[stageno], 45000, stageno, "ACTIVE");

                    int ocvminvalue = 0;
                    _system.OCVMINVALUE = util.TryParseInt(util.readConfig(filename, "OUTFLOW", "OCV_MIN"), ocvminvalue);

                    //* IR SPEC
                    double irmin = 0.0;
                    _system.IRMIN = util.TryParseDouble(util.readConfig(filename, "SPEC", "IRMIN"), irmin);
                    double irmax = 0.0;
                    _system.IRMAX = util.TryParseDouble(util.readConfig(filename, "SPEC", "IRMAX"), irmax);
                    double irremeamin = 0.0;
                    _system.IRREMEAMIN = util.TryParseDouble(util.readConfig(filename, "SPEC", "IRREMEAMIN"), irremeamin);
                    double irremeamax = 0.0;
                    _system.IRREMEAMAX = util.TryParseDouble(util.readConfig(filename, "SPEC", "IRREMEAMAX"), irremeamax);

                    measureinfo.SetIRMinMax(_system.IRMIN, _system.IRMAX, _system.IRREMEAMIN, _system.IRREMEAMAX);

                    //* OCV SPEC
                    double ocvmin = 0.0;
                    _system.OCVMIN = util.TryParseDouble(util.readConfig(filename, "SPEC", "OCVMIN"), ocvmin);
                    double ocvmax = 0.0;
                    _system.OCVMAX = util.TryParseDouble(util.readConfig(filename, "SPEC", "OCVMAX"), ocvmax);
                    double ocvremeamin = 0.0;
                    _system.OCVREMEAMIN = util.TryParseDouble(util.readConfig(filename, "SPEC", "OCVREMEAMIN"), ocvremeamin);
                    double ocvremeamax = 0.0;
                    _system.OCVREMEAMAX = util.TryParseDouble(util.readConfig(filename, "SPEC", "OCVREMEAMAX"), ocvremeamax);

                    measureinfo.SetOCVMinMax(_system.OCVMIN, _system.OCVMAX, _system.OCVREMEAMIN, _system.OCVREMEAMAX);

                    measureinfo.SetChartMinMax();

                    //* IROCV CONFIG FORM에 설정값 표시
                    irocvconfig[stageno].SetStageSystemValue();

                    //* IROCV FORM에 SPEC 표시
                    DisplaySpec(stageno);
                }
                else
                {
                    irocvconfig[stageno].Left = 100 + (stageno * irocvform[stageno].Width);
                    irocvconfig[stageno].Show();
                }
            }
            catch(Exception ex)
            {
                irocvconfig[stageno].Left = 100 + (stageno * irocvform[stageno].Width);
                irocvconfig[stageno].Show();
                Console.WriteLine(ex.ToString());
            }
            
        }
        private void DisplaySpec(int stageno)
        {
            irocvform[stageno].SetIrSpec(_system.IRMIN, _system.IRMAX, _system.IRREMEAMIN, _system.IRREMEAMAX);
            irocvform[stageno].SetOcvSpec(_system.OCVMIN, _system.OCVMAX, _system.OCVREMEAMIN, _system.OCVREMEAMAX);
        }
        private void OpenOffsetFile(int stageno)
        {
            string filename = string.Empty;
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = "D:\\IROCV\\BIN";
                openFileDialog.InitialDirectory = _Constant.BIN_PATH;
                openFileDialog.Filter = "csv files (*.csv)|";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filename = openFileDialog.FileName;
                    if (ReadOffsetFile(stageno, filename) == false)
                        MessageBox.Show("Open offset file Error. Please check offset file.");
                }
            }
        }
        private bool ApplyOffset(int stageno, string[] stroffset, double[] offset)
        {
            irocvdata[stageno].IR_OFFSET = offset;
            return SaveOffsetFile(stageno, stroffset);
        }
        private bool SaveOffsetFile(int stageno, string[] strOffset)
        {
            string filename = _Constant.BIN_PATH + "Calibration_Stage" + (stageno + 1) + ".csv";
            try
            {
                if (File.Exists(filename)) File.Delete(filename);

                util.WriteCsvFile(filename, strOffset);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            return true;
        }
        private bool ReadOffsetFile(int stageno, string filename)
        {
            try
            {
                List<double[]> offsetvalues = util.ReadCsvFile(filename);
                if (offsetvalues.Count != _Constant.ChannelCount) return false;
                for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                {
                    double[] channelvalues = offsetvalues[nIndex];
                    if (channelvalues.Length != 3) return false;
                    //* 0 => standard, 1 => measured by irocv, 2 => offset (measured - standard)
                    irocvdata[stageno].IR_STANDARD[nIndex] = channelvalues[0];
                    irocvdata[stageno].IR_MEASURE[nIndex] = channelvalues[1];
                    irocvdata[stageno].IR_OFFSET[nIndex] = channelvalues[2];
                }

                measureinfo.SetOffsetValueToLabel(irocvdata[stageno]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

            return true;
        }
        private bool ReadOffsetFile(int stageno)
        {
            string filename = _Constant.BIN_PATH + "Calibration_Stage" + (stageno + 1) + ".csv";
            try
            {
                if (File.Exists(filename))
                {
                    List<double[]> offsetvalues = util.ReadCsvFile(filename);
                    if (offsetvalues.Count != _Constant.ChannelCount) return false;
                    for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                    {
                        double[] channelvalues = offsetvalues[nIndex];
                        if (channelvalues.Length != 3) return false;
                        //* 0 => standard, 1 => measured by irocv, 2 => offset (measured - standard)
                        irocvdata[stageno].IR_STANDARD[nIndex] = channelvalues[0];
                        irocvdata[stageno].IR_MEASURE[nIndex] = channelvalues[1];
                        irocvdata[stageno].IR_OFFSET[nIndex] = channelvalues[2];
                    }

                    measureinfo.SetOffsetValueToLabel(irocvdata[stageno]);
                }
                else
                {
                    for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                        irocvdata[stageno].IR_OFFSET[nIndex] = 0.0;
                    return false;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            
            return true;
        }
        #endregion Read Config / Offset

        #region Result / MES / OFFSET File
        private void SaveResultFile(int stageno)
        {
            irocvdata[stageno].SetFinishTime();
            util.SaveResultFile_IROCV(stageno, irocvdata[stageno], _system);
        }
        private void SaveManualResultFile(int stageno)
        {
            util.SaveManualResultFile_IROCV(stageno, irocvdata[stageno], _system);
        }
        private void SaveMsaResultFile(int stageno, int nCount)
        {
            string msaReportFile = string.Empty;
            msaReportFile = util.SaveMsaResultFile_IROCV(stageno, nCount, irocvdata[stageno]);
            util.SaveReportFile_IROCV(stageno, irocvdata[stageno], "MSA", irocv[stageno].MSAFILENAME);
        }
        private void SaveOffsetResultFile(int stageno, int nCount)
        {
            string offsetReportFile = string.Empty;
            offsetReportFile = util.SaveOffsetResultFile_IROCV(stageno, nCount, irocvdata[stageno]);
            util.SaveReportFile_IROCV(stageno, irocvdata[stageno], "OFFSET", irocv[stageno].OFFSETFILENAME);

            //* N회 측정이 끝나면 마지막으로 offset 평균값 계산 및 measure info form에 표시
            SetOffsetAverage(stageno);
        }
        private void SetOffsetAverage(int stageno)
        {
            //* average 계산
            if(File.Exists(irocv[stageno].OFFSETFILENAME) == true)
            {
                irocvdata[stageno].IR_OFFSETMEASUREAVERAGE = util.ReadOffsetFile_IROCV(stageno, irocv[stageno].OFFSETFILENAME);
                //* measure info form 에 표시
                measureinfo.DisplayOffsetAverage(stageno, irocvdata[stageno].IR_OFFSETMEASUREAVERAGE);
            }
        }
        #endregion

        #region NG Info
        /*
        private void SaveNGInfo(int stageno, bool bIncrease)
        {
            string filename = _Constant.BIN_PATH + "RemeasureInfo_" + (stageno + 1) + ".inf";

            string ch_no = string.Empty;
            for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                ch_no = "CH_" + (nIndex + 1).ToString("D2");
                if (bIncrease == true)
                {
                    _system.REMEASUREUSE[nIndex] += 1;
                    _system.REMEASURENG[nIndex] += 1;
                }

                util.saveConfig(filename, ch_no, "USE", _system.REMEASUREUSE[nIndex].ToString());
                util.saveConfig(filename, ch_no, "NG", _system.REMEASURENG[nIndex].ToString());
            }

            nginfo.SetNGInfo(stageno);
        }
        private void ReadNGInfo(int stageno)
        {
            string filename = _Constant.BIN_PATH + "RemeasureInfo_" + (stageno + 1) + ".inf";
            string ch_no = string.Empty;
            try
            {
                if (File.Exists(filename))
                {
                    for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                    {
                        ch_no = "CH_" + (nIndex + 1).ToString("D2");
                        _system.REMEASUREUSE[nIndex] = Convert.ToInt32(util.readConfig(filename, ch_no, "USE"));
                        _system.REMEASURENG[nIndex] = Convert.ToInt32(util.readConfig(filename, ch_no, "NG"));
                    }
                }
                else
                {
                    for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                    {
                        _system.REMEASUREUSE[nIndex] = 0;
                        _system.REMEASURENG[nIndex] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        */
        #endregion

        #endregion

        #region Timer
        private async void GetMesDataTimer_TickAsync(object sender, EventArgs e)
        {
            if (MesServer.connection == true && _bMesConnected == false)
            {
                try
                {
                    _bMesConnected = await Task.Run(() => mesclient.MesClientStartAsync());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        private void GetPlcDataTimer_Tick(object sender, EventArgs e)
        {
            _bPlcConnected = SIEMENSS7LIB.connection;
            
            if(_bPlcConnected == true)
            {
                try
                {
                    //foreach (var tagname in siemensplc.PLCTAGNAMES)
                    //{
                    //    if (siemensplc.PLCVALUES.ContainsKey(tagname).Equals(true))
                    //        plcinterface.SetDataToGrid(tagname, siemensplc.PLCVALUES[tagname].ToString(), 0);
                    //}
                    //plcinterface.SetDataToGrid("MB10020", siemensplc.PLCTRAYID, 0);
                    plcinterface.SetDataToGrid(siemensplc.PCVALUES, siemensplc.PLCVALUES, siemensplc.PLCVALUESSYS, 0);
                }catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
        }
        private void EquipStatusTimer_Tick(object sender, EventArgs e)
        {
            int stageno = int.Parse(((Timer)sender).Tag.ToString());

            irocvform[stageno].SetIROCVConnectionStatus(irocv[stageno].ConnectionState);
            if (irocv[stageno].ConnectionState == enumConnectionState.Connected)
            {
                if (_bIrocvConnected[stageno] == false) 
                    SetOperationMode(stageno, true);
                _bIrocvConnected[stageno] = true;

                //* 2023 07 25 mainform ConnectionChangeTimer_Tick으로 옮김
                //PLC_SETERROR(stageno, 0);
            }
            else
            {
                if (_bIrocvConnected[stageno] == true)
                    SetOperationMode(stageno, false);
                _bIrocvConnected[stageno] = false;

                PLC_SETAUTOMODE(stageno, 0);

                //* 2023 07 25 mainform ConnectionChangeTimer_Tick으로 옮김
                //PLC_SETERROR(stageno, 1);

                irocv[stageno].EQUIPMODE = enumEquipMode.MANUAL;
                irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepNoAnswer;
            }

            if (irocv[stageno].EQUIPMODE != enumEquipMode.AUTO && irocv[stageno].EQUIPSTATUS != enumEquipStatus.StepNoAnswer)
                irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepManual;

            irocvform[stageno].SetStageStatus(irocv[stageno].EQUIPSTATUS, _bPlcConnected, siemensplc.PLCAUTOMANUAL, siemensplc.PLCERROR, _bMesConnected);
        }
        private void ConnectionChangeTimer_Tick(object sender, EventArgs e)
        {
            //int stageno = int.Parse(((Timer)sender).Tag.ToString());

            //irocvform[stageno].SetIROCVConnectionStatus(irocv[stageno].ConnectionState);
            //if (irocv[stageno].ConnectionState == enumConnectionState.Connected)
            //{
            //    _bIrocvConnected[stageno] = true;
            //    _tmrAutoInspection[stageno].Enabled = true;

            //    util.SaveLog(stageno, "IROCV is connected");
            //}
            //else
            //{
            //    PLC_SETAUTOMODE(stageno, 0);
            //    _bIrocvConnected[stageno] = false;
            //    _tmrAutoInspection[stageno].Enabled = false;
            //}
        }
        private void AutoInspectionTimer_Tick(object sender, EventArgs e)
        {
            int stageno = int.Parse(((Timer)sender).Tag.ToString());
            
            //if (irocv[stageno].AUTOMODE == false || siemensplc.PLCAUTOMANUAL == 0) return;
            //if (irocv[stageno].AUTOMODE == false) return;
            if (irocv[stageno].EQUIPMODE != enumEquipMode.AUTO) return;

            //* 2024 06 05 추가 - plc manual mode에서는 실행하지 않음.
            if (siemensplc.PLCAUTOMANUAL == 0) return;

            if (_bMesConnected == false) return;

            //* 2023 07 25 PLC 에러 발생시 StepVacancy 상태가 아니면 IR/OCV 초기화 한다.
            if (siemensplc.PLCERROR == 1 && irocv[stageno].EQUIPSTATUS != enumEquipStatus.StepVacancy)
                IROCV_Initialize(stageno);

            switch (irocv[stageno].EQUIPSTATUS)
            {
                case enumEquipStatus.StepVacancy:
                    SetProcessStatus(stageno, enumProcess.pReady);
                    AutoInspection_StepTrayInCheck(stageno);
                    //* Measurement Status
                    PLC_MEASUREMENT_WAIT(stageno, 1);
                    break;
                case enumEquipStatus.StepTrayIn:
                    AutoInspection_StepTrayIdCheck(stageno);
                    break;
                case enumEquipStatus.StepReady:
                    AutoInspection_StepAutoStart(stageno);
                    break;
                case enumEquipStatus.StepRun:
                    SetProcessStatus(stageno, enumProcess.pMeasure);
                    //* 측정이 끝나면 case "AMF": 에서 AutoTestFinish() 실행
                    PLC_MEASUREMENT_RUNNING(stageno, 1);
                    break;
                case enumEquipStatus.StepEnd:
                    PLC_MEASUREMENT_COMPLETE(stageno, 1);
                    AutoInspection_StepEnd(stageno);
                    break;
                case enumEquipStatus.StepTrayOut:
                    AutoInspection_StepFinish(stageno);
                    break;
                default:
                    break;
            }
        }
        private void MsaInspectionTimer_Tick(object sender, EventArgs e)
        {
            int stageno = int.Parse(((Timer)sender).Tag.ToString());

            switch(irocv[stageno].MSASTATUS)
            {
                case enumMsaStatus.StepTrayDown:
                    MsaInspection_StepTrayDown(stageno);
                    break;
                case enumMsaStatus.StepTrayUp:
                    MsaInspection_StepTrayUp(stageno);
                    break;
                case enumMsaStatus.StepFinish:
                    MsaInspection_StepFinish(stageno);
                    break;
                default:
                    break;
            }
        }

        #region MSA / OFFSET Inspection Timer
        int nStep = 0;
        private void MsaInspection_Start(int stageno, int count, string type)
        {
            //* cell info for manual
            irocvdata[stageno].SetTrayInfoForManual();

            if (type == "MSA")
            {
                irocv[stageno].EQUIPMODE = enumEquipMode.MANUAL;
                irocv[stageno].MSASTATUS = enumMsaStatus.StepTrayDown;
                nStep = 0;
                irocv[stageno].MSACOUNT = count;
                irocv[stageno].MSAFILENAME = "Report_" + System.DateTime.Now.ToString("yyMMddHHmmss") + ".csv";
                msacount = 0;
                measureinfo.SetMsaCurrentCount(msacount);
                _tmrMsaInspection[stageno].Enabled = true;
            }
            else if(type == "OFFSET")
            {
                irocv[stageno].EQUIPMODE = enumEquipMode.OFFSET;
                irocv[stageno].MSASTATUS = enumMsaStatus.StepTrayDown;
                nStep = 0;
                irocv[stageno].OFFSETCOUNT = count;
                irocv[stageno].OFFSETFILENAME = "OFFSET_Report_" + System.DateTime.Now.ToString("yyMMddHHmmss") + ".csv";
                offsetcount = 0;
                measureinfo.SetOffsetCurrentCount(offsetcount);
                _tmrMsaInspection[stageno].Enabled = true;
            }
            
        }
        private void MsaInspection_Stop(int stageno)
        {
            irocv[stageno].CmdSTOP();

            PLC_TRAYDOWN(stageno);
            _tmrMsaInspection[stageno].Enabled = false;
        }

        private void MsaInspection_StepTrayDown(int stageno)
        {
            if(siemensplc.PLCTRAYDOWN == 1)
            {
                PLC_TRAYUP(stageno);
                IROCV_Initialize(stageno);

                irocv[stageno].MSASTATUS = enumMsaStatus.StepTrayUp;
            }
        }
        private void MsaInspection_StepTrayDown2(int stageno)
        {
            switch(nStep)
            {
                case 0:
                    IROCV_Initialize(stageno);
                    nStep = 1;
                    break;
                case 1:
                    if (siemensplc.PLCTRAYDOWN == 1)
                        PLC_TRAYUP(stageno);
                    
                    if (siemensplc.PCTRAYUP == 1)
                        nStep = 2;
                    break;
                case 2:
                    irocv[stageno].MSASTATUS = enumMsaStatus.StepTrayUp;
                    nStep = 0;
                    break;
                default:
                    break;
            }
            
        }
        private void MsaInspection_StepTrayUp(int stageno)
        {
            if (siemensplc.PLCTRAYUP == 1)
            {
                siemensplc.SetTrayUp(stageno, 0);
                CmdAutoStart(stageno);

                irocv[stageno].MSASTATUS = enumMsaStatus.StepFinish;
            }
        }
        private void MsaInspection_StepFinish(int stageno)
        {
            if(siemensplc.PLCTRAYDOWN == 1)
            {
                siemensplc.SetTrayDown(stageno, 0);

                if(irocv[stageno].EQUIPMODE == enumEquipMode.MANUAL)
                {
                    msacount += 1;
                    measureinfo.SetMsaCurrentCount(msacount);
                    SaveMsaResultFile(stageno, msacount);

                    if (msacount >= irocv[stageno].MSACOUNT)
                    {
                        _tmrMsaInspection[stageno].Enabled = false;
                        MessageBox.Show("MSA complete!");
                    }
                    else
                        irocv[stageno].MSASTATUS = enumMsaStatus.StepTrayDown;
                }
                else if(irocv[stageno].EQUIPMODE == enumEquipMode.OFFSET)
                {
                    offsetcount += 1;
                    measureinfo.SetOffsetCurrentCount(offsetcount);
                    SaveOffsetResultFile(stageno, offsetcount);

                    if (offsetcount >= irocv[stageno].OFFSETCOUNT)
                    {
                        _tmrMsaInspection[stageno].Enabled = false;
                        MessageBox.Show("OFFSET complete!");
                    }
                    else
                        irocv[stageno].MSASTATUS = enumMsaStatus.StepTrayDown;
                }
            }
        }
        #endregion MSA / OFFSET Inspection Timer

        #region Auto Inspection Timer
        private void SetProcessStatus(int stageno, enumProcess enumprocess)
        {
            irocvform[stageno].SetProcessStatus(enumprocess);
        }
        private void AutoInspection_StepTrayInCheck(int stageno)
        {
            if (siemensplc.PLCTRAYIN == 1)
            {
                PLC_Initialize(stageno);
                IROCV_Initialize(stageno);
                SetProcessStatus(stageno, enumProcess.pTrayIn);

                irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepTrayIn;
                nInspectionStep = 0;

                irocvdata[stageno].SetArriveTime();
                //irocvdata[stageno].ARRIVETIME = DateTime.Now;

                SaveLog(stageno, "TRAY IN ...");
            }
            else
            {
                //irocvform[stageno].SetStageStatus(enumEquipStatus.StepVacancy);
            }
        }
        private void AutoInspection_StepTrayIdCheck(int stageno)
        {
            string trayid = siemensplc.PLCTRAYID;
            string equipid = _system.EQUIPMENTID;
            bool bAck = false;
            //trayid = SIEMENSS7LIB.ReadString("MB10020", 20);

            switch(nInspectionStep)
            {
                case 0:
                    //* PLC - Read Tray ID
                    if(string.IsNullOrEmpty(trayid) == false)
                    {
                        irocvform[stageno].SetTrayId(trayid);
                        irocvdata[stageno].TRAYID = trayid;
                        irocvdata[stageno].EQUIPMENTID = equipid;
                        SetProcessStatus(stageno, enumProcess.pBarcode);

                        SaveLog(stageno, "TRAY ID : " + trayid);

                        //* MES사용확인 - 사용하지 않으면 5로 넘어감.
                        if (_system.UNUSEMES == true)
                            nInspectionStep = 5;
                        else
                            nInspectionStep = 1;
                    }
                    break;
                case 1:
                    //* MES - Request Tray Information
                    //* IROCV -> MES FOEQR1.12 : equipment id, tray id 쓰기
                    //* FORIR 2.1 2024 05 28 수정
                    //mesclient.WriteFOEQR1_12(stageno, equipid, trayid);
                    mesclient.WriteFOEQR2_1(stageno, equipid, trayid);
                    SetProcessStatus(stageno, enumProcess.pRequestTrayInfo);
                    nInspectionStep = 3;
                    break;
                case 3:
                    //* MES - Request Reservation (트레이 정보)
                    //* MES -> IROCV FOEQR1.7 (tray information)
                    //* FORIR 2.1 2024 05 28 수정
                    //irocvdata[stageno] = mesclient.ReadFOEQR1_7(stageno);
                    irocvdata[stageno] = mesclient.ReadFOEQR2_1(stageno);
                    SetProcessStatus(stageno, enumProcess.pReplyTrayInfo);
                    //irocvdata[stageno].TRAYSTATUSCODE = "CN";
                    if (irocvdata[stageno].TRAYSTATUSCODE == "CN")
                    {
                        //* MES - Display Tray Info.
                        DisplayTrayInfo(stageno, irocvdata[stageno]);

                        nInspectionStep = 5;
                    }
                    else if (irocvdata[stageno].TRAYSTATUSCODE == "DT" || irocvdata[stageno].TRAYSTATUSCODE == "NT")
                    {
                        //* PLC - Request Tray Out
                        PLC_TRAYOUT(stageno, 1);
                        SetProcessStatus(stageno, enumProcess.pTrayOut);
                    }
                    break;
                case 5:
                    //* PLC - Read Tray Ready Complete
                    if (siemensplc.PLCREADYCOMPLETE == 1)
                    {
                        //* PLC - Request Tray Up
                        //PLC_TRAYUP(stageno); ==> 2024 05 21 StepReady 상태에서 수행하도록 변경
                        irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepReady;
                        nInspectionStep = 0;
                    }
                    break;
                default:
                    break;
            }
        }
        //* MES 적용 전 버전
        private void AutoInspection_StepTrayIdCheck2(int stageno)
        {
            string trayid = siemensplc.PLCTRAYID;
            bool bAck = false;
            //trayid = SIEMENSS7LIB.ReadString("MB10020", 20);
            if (trayid != "")
            {
                irocvform[stageno].SetTrayId(trayid);
                irocvdata[stageno].TRAYID = trayid;

                SaveLog(stageno, "TRAY ID : " + trayid);

                if (siemensplc.PLCREADYCOMPLETE == 1)
                {
                    PLC_TRAYUP(stageno);
                    irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepReady;
                }
            }
        }
        private void AutoInspection_StepAutoStart(int stageno)
        {
            switch(nInspectionStep)
            {
                case 0:
                    PLC_TRAYUP(stageno);
                    nInspectionStep = 1;
                    break;
                case 1:
                    if (siemensplc.PCTRAYUP == 1)
                        nInspectionStep = 2;
                    else
                        PLC_TRAYUP(stageno);
                    break;
                case 2:
                    //* PLC Tray Up 확인
                    if (siemensplc.PLCTRAYUP == 1)
                    {
                        SetProcessStatus(stageno, enumProcess.pTrayUp);
                        CmdAutoStart(stageno);
                        irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepRun;
                    }
                    break;
                default:
                    break;
            }
        }
        private void AutoInspection_StepAutoStart2(int stageno)
        {
            //* PLC Tray Up 확인
            if (siemensplc.PLCTRAYUP == 1)
            {
                if (irocvdata[stageno].REMEASURE == false)
                    CmdAutoStart(stageno);
                else if (irocvdata[stageno].REMEASURE == true)
                    CmdRemeasureExcute(stageno);
                irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepRun;
            }
        }
        private void AutoInspection_StepEnd(int stageno)
        {
            bool bAck = false;
            int remeasurecellpercent = irocvdata[stageno].CELLCOUNT == 0 ? 0 : (irocvdata[stageno].REMEASURECELLCOUNT / irocvdata[stageno].CELLCOUNT) * 100;
            switch (nInspectionStep)
            {
                case 0:
                    //* Remeasure check
                    //* irocvdata[stageno].REMEASURECELLCOUNT > 0 config에서 설정한 %를 넘어가는 지 확인
                    //* remeasure count확인
                    if (siemensplc.PLCTRAYDOWN == 1)
                    {
                        SetProcessStatus(stageno, enumProcess.pTrayDown);
                        if (remeasurecellpercent > _system.REMEASUREPERCENT && irocvdata[stageno].REMEASURECOUNT < _system.REMEASURECOUNT)
                        {
                            measureinfo.InitDisplayMesChannelInfo(stageno, irocvdata[stageno], irocv[stageno].EQUIPMODE);
                            PLC_TRAYUP(stageno);
                            irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepReady;
                            irocvdata[stageno].REMEASURECOUNT += 1;
                            nInspectionStep = 0;
                        } else
                        {
                            //* MES사용확인 - 사용하지 않으면 5로 넘어감.
                            if (_system.UNUSEMES == true)
                                nInspectionStep = 4;
                            else
                                nInspectionStep = 1;
                        }
                    }
                    else
                    {
                        PLC_TRAYDOWN(stageno);
                    }
                    break;
                case 1:
                    //* MES - Data Collection
                    //* IROCV -> MES FOEQR1.1 (send ir, ocv data to mes)
                    //* FORIR2.2 2024 05 28 수정
                    //mesclient.WriteFOEQR1_1(stageno, irocvdata[stageno]);
                    mesclient.WriteFOEQR2_2(stageno, irocvdata[stageno]);
                    SetProcessStatus(stageno, enumProcess.pDataUpload);
                    nInspectionStep = 3;
                    break;
                case 3:
                    //* MES - Request Process Result (트레이 배출 또는 재측정)
                    //* MES -> IROCV FOEQR1.13 (Process Result) 1 : Tray Emission  2: Tray Retry
                    //* 20240521 Process Result 삭제. IR/OCV에서 자체 판단하여 재측정 후 내보냄.
                    //* FORIR2.2 2024 05 28 수정
                    //irocvdata[stageno] = mesclient.ReadFOEQR1_13(stageno);
                    irocvdata[stageno] = mesclient.ReadFOEQR2_2(stageno);
                    SetProcessStatus(stageno, enumProcess.pDataReply);
                    nInspectionStep = 4;
                    break;
                case 4:
                    SetProcessStatus(stageno, enumProcess.pFinish);
                    PLC_TRAYOUT(stageno, 1);
                    irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepTrayOut;
                    SaveResultFile(stageno);

                    nInspectionStep = 0;
                    break;
                default:
                    break;
            }
        }
        //* MES 적용 전 버전
        private void AutoInspection_StepTrayOut(int stageno)
        {
            if (siemensplc.PLCTRAYDOWN == 1)
            {
                PLC_TRAYOUT(stageno, 1);
                irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepTrayOut;

                SaveResultFile(stageno);
            }
        }
        private void AutoInspection_StepFinish(int stageno)
        {
            if (siemensplc.PLCTRAYIN == 0)
            {
                SetProcessStatus(stageno, enumProcess.pTrayOut);
                PLC_TRAYOUT(stageno, 0);
                irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepVacancy;
            }
        }
        #endregion Auto Inspection Timer

        #endregion Timer

        #region IR/OCV
        public void close()
        {
            for (int nIndex = 0; nIndex < _Constant.frmCount; nIndex++)
                if (irocv[nIndex] != null) irocv[nIndex].Close();
        }

        #region IR/OCV Method
        public void IROCV_Error(int stageno, string param)
        {
            //* 2023 07 25 컨트롤러 에러 추가
            string statusCode = param.Substring(0, 3);
            if(statusCode == "ERR")
            {
                SetOperationMode(stageno, false);
                _errorForm.ShowMessage(enumStageError.IROCVNoResponse, stageno);
            }
            else if(statusCode == "IDL")
            {
                //* error 해제
                _errorForm.HideMessage2(false);
            }
        }
        public void IROCV_Initialize(int stageno)
        {
            util.SaveLog(stageno, "IROCV Initialize ...");
            irocvdata[stageno].InitData();
            measureinfo.InitData(stageno);
            //measureinfo.InitChart();

            measureinfo.InitDisplayChannelInfo(stageno, irocvdata[stageno], irocv[stageno].EQUIPMODE);

            InitEquipStatus(stageno);
            SetProcessStatus(stageno, enumProcess.pReady);
        }
        public void IROCV_Refresh(int stageno)
        {
            //* mes에서 데이터 받아서 화면 refresh
            util.SaveLog(stageno, "IROCV Data Initialize ...");
            measureinfo.InitData(stageno);
            //measureinfo.InitChart();

            measureinfo.InitDisplayMesChannelInfo(stageno, irocvdata[stageno], irocv[stageno].EQUIPMODE);
        }
        private void InitEquipStatus(int stageno)
        {
            irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepVacancy;
            irocv[stageno].MSASTATUS = enumMsaStatus.StepTrayDown;
        }
        private void SetOperationMode(int stageno, bool bAuto)
        {
            //* display Local Mode - picture in stage status
            if (bAuto)
            {
                util.SaveLog(stageno, "Set AUTO MODE");
                IROCV_Initialize(stageno);
                PLC_Initialize(stageno);

                irocv[stageno].EQUIPMODE = enumEquipMode.AUTO;
                irocv[stageno].CmdSTOP();
                irocv[stageno].CmdRESET();

                PLC_SETAUTOMODE(stageno, 1);
                if (_tmrAutoInspection[stageno].Enabled == false)
                    _tmrAutoInspection[stageno].Enabled = true;
            }
            else if (bAuto == false)
            {
                util.SaveLog(stageno, "Set MANUAL MODE");
                IROCV_Initialize(stageno);
                PLC_Initialize(stageno);

                irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepManual;
                irocv[stageno].EQUIPMODE = enumEquipMode.MANUAL;
                irocv[stageno].CmdSTOP();

                PLC_SETAUTOMODE(stageno, 0);
                if (_tmrAutoInspection[stageno].Enabled == true)
                    _tmrAutoInspection[stageno].Enabled = false;
            }

            //* display [manual mode] in measure info form
            measureinfo.SetOperationMode(bAuto);
        }
        private void ShowControlMessage(int stageno, string param)
        {
            irocvform[stageno].SetControlMessage(param);
        }
        private void ProcessIr(int stageno, string param)
        {
            int channel = Convert.ToInt32(param.Substring(0, 3));
            int channel_no = _system.CHANNELMAPPING[channel] - 1;

            irocvdata[stageno].SetValue(param, "IR", irocv[stageno].EQUIPMODE);
            //* for test 2024 06 03 임시로 랜덤값 저장
            //irocvdata[stageno].SetValue(param, "IR", irocv[stageno].EQUIPMODE, irocvdataTest);
            measureinfo.DisplayChannelInfo(channel_no, stageno, irocvdata[stageno], irocv[stageno].EQUIPMODE);
        }
        private void ProcessOcv(int stageno, string param)
        {
            int channel = Convert.ToInt32(param.Substring(0, 3));
            int channel_no = _system.CHANNELMAPPING[channel] - 1;

            irocvdata[stageno].SetValue(param, "OCV", irocv[stageno].EQUIPMODE);
            //* for test 2024 06 03 임시로 랜덤값 저장
            //irocvdata[stageno].SetValue(param, "OCV", irocv[stageno].EQUIPMODE, irocvdataTest);
            measureinfo.DisplayChannelInfo(channel_no, stageno, irocvdata[stageno], irocv[stageno].EQUIPMODE);
        }
        private void AutoTestStart(int stageno)
        {
            irocv[stageno].AMS = true;
            irocv[stageno].AMF = false;
            //irocvform[stageno].SetStageStatus(enumEquipStatus.StepRun);
        }
        private void AutoTestFinish(int stageno)
        {
            //irocv[stageno].AMS = false;
            //irocv[stageno].AMF = true;
            //irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepEnd;

            if (irocv[stageno].EQUIPMODE == enumEquipMode.AUTO)
            {
                SetRemeasureList(stageno);
            }
            else
            {
                AutoTestStop(stageno);
            }
        }
        private void AutoTestStop(int stageno)
        {
            //* STP to IROCV
            irocv[stageno].AMS = false;
            irocv[stageno].AMF = true;

            //* 
            irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepEnd;
            nInspectionStep = 0;

            //* Tray Down to PLC
            PLC_TRAYDOWN(stageno);
        }
        /// <summary>
        /// MEASURERESULT : 0 -> OK, 
        /// 2 -> IR NG, 3 -> OCV NG, 
        /// 4 -> IR REMEASURE NG, 5-> OCV REMEASURE NG
        /// </summary>
        private async Task SetRemeasureList(int stageno)
        {
            bool bRemeasure = false;
            //int iRemeasureCount = 0;
            double irvalue = 0.0, ocvvalue = 0.0;

            if (irocv[stageno].EQUIPMODE == enumEquipMode.AUTO)
            {
                irocvdata[stageno].REMEASURECELLCOUNT = 0;

                #region IR/ OCV Error 처리
                for (int index = 0; index < _Constant.ChannelCount; ++index)
                {
                    irvalue = irocvdata[stageno].IR_AFTERVALUE[index];
                    ocvvalue = irocvdata[stageno].OCV[index];

                    if (irocvdata[stageno].CELL[index] == 1)
                    {
                        //* IR Remeasure Error
                        if (irvalue < _system.IRREMEAMIN || irvalue > _system.IRREMEAMAX)
                        {
                            irocvdata[stageno].MEASURERESULT[index] = 4;
                            irocvdata[stageno].REMEASURECELLCOUNT++;
                        }
                        //* IR Spec Error
                        else if (irvalue < _system.IRMIN || irvalue > _system.IRMAX)
                        {
                            irocvdata[stageno].MEASURERESULT[index] = 2;
                        }

                        //* OCV Remeasure Error
                        else if (ocvvalue < _system.OCVREMEAMIN || ocvvalue > _system.OCVREMEAMAX)
                        {
                            if (irocvdata[stageno].MEASURERESULT[index] != 2 && irocvdata[stageno].MEASURERESULT[index] != 4)
                            {
                                irocvdata[stageno].MEASURERESULT[index] = 5;
                                irocvdata[stageno].REMEASURECELLCOUNT++;
                            }
                        }
                        //* OCV Spec Error
                        else if (ocvvalue < _system.OCVMIN || ocvvalue > _system.OCVMAX)
                        {
                            if (irocvdata[stageno].MEASURERESULT[index] != 2 && irocvdata[stageno].MEASURERESULT[index] != 4)
                            {
                                irocvdata[stageno].MEASURERESULT[index] = 3;
                            }
                        }
                        else
                        {
                            irocvdata[stageno].MEASURERESULT[index] = 0;
                        }
                    }
                    else
                        irocvdata[stageno].MEASURERESULT[index] = 0;
                }
                #endregion

                AddRemeaseList(stageno);

                //* AMF 이거나 재측정 수가 일정 갯수가 넘으면 트레이 다운 후 [트레이 배출 또는 전체 재측정]
                //* AMF 상태가 아니거나 재측정 수가 일정 갯수(5개) 이하면 현재 컨택 상태에서 에러난 채널만 부분 재측정
                //*irocvdata[stageno].REMEASURECELLCOUNT > _system.REMEASUREMAXCOUNT
                if (irocv[stageno].AMF == true || (irocvdata[stageno].REMEASURECELLCOUNT == 0 || irocvdata[stageno].REMEASURECELLCOUNT > 5))
                {
                    AutoTestStop(stageno);
                }
                else
                {
                    await RemeasureExcute(stageno);
                }
            }

            if (irocv[stageno].AMF == false)
            {
                irocv[stageno].AMF = true;
                SetRemeasureList(stageno);
                //CmdAmf(stageno);
            }

            //WriteCommLog("IR/OCV STOP", "SetRemeasureList()");
        }
        private void SetRemeasureList_Demo(int stageno)
        {
            bool bRemeasure = false;
            //int iRemeasureCount = 0;
            double irvalue = 0.0, ocvvalue = 0.0;

            if (irocv[stageno].EQUIPMODE == enumEquipMode.AUTO)
            {
                irocvdata[stageno].REMEASURECELLCOUNT = 0;
                irocvdata[stageno] = irocvdataTest;

                #region IR/ OCV Error 처리
                for (int index = 0; index < _Constant.ChannelCount; ++index)
                {
                    irvalue = irocvdata[stageno].IR_AFTERVALUE[index];
                    ocvvalue = irocvdata[stageno].OCV[index];

                    if (irocvdata[stageno].CELL[index] == 1)
                    {
                        //* IR Remeasure Error
                        if (irvalue < _system.IRREMEAMIN || irvalue > _system.IRREMEAMAX)
                        {
                            irocvdata[stageno].MEASURERESULT[index] = 4;
                            irocvdata[stageno].REMEASURECELLCOUNT++;
                        }
                        //* IR Spec Error
                        else if (irvalue < _system.IRMIN || irvalue > _system.IRMAX)
                        {
                            irocvdata[stageno].MEASURERESULT[index] = 2;
                        }

                        //* OCV Remeasure Error
                        else if (ocvvalue < _system.OCVREMEAMIN || ocvvalue > _system.OCVREMEAMAX)
                        {
                            if (irocvdata[stageno].MEASURERESULT[index] != 2 && irocvdata[stageno].MEASURERESULT[index] != 4)
                            {
                                irocvdata[stageno].MEASURERESULT[index] = 5;
                                irocvdata[stageno].REMEASURECELLCOUNT++;
                            }
                        }
                        //* OCV Spec Error
                        else if (ocvvalue < _system.OCVMIN || ocvvalue > _system.OCVMAX)
                        {
                            if (irocvdata[stageno].MEASURERESULT[index] != 2 && irocvdata[stageno].MEASURERESULT[index] != 4)
                            {
                                irocvdata[stageno].MEASURERESULT[index] = 3;
                            }
                        }
                        else
                        {
                            irocvdata[stageno].MEASURERESULT[index] = 0;
                        }
                    }
                    else
                        irocvdata[stageno].MEASURERESULT[index] = 0;
                }
                #endregion

                AddRemeaseList(stageno);

                //* AMF 이거나 재측정 수가 일정 갯수가 넘으면 트레이 다운 후 [트레이 배출 또는 전체 재측정]
                //* AMF 상태가 아니거나 재측정 수가 일정 갯수(5개) 이하면 현재 컨택 상태에서 에러난 채널만 부분 재측정
                //*irocvdata[stageno].REMEASURECELLCOUNT > _system.REMEASUREMAXCOUNT
                if (irocv[stageno].AMF = true || irocvdata[stageno].REMEASURECELLCOUNT > 5)
                {
                    AutoTestStop(stageno);
                }
                else
                {
                    RemeasureExcute(stageno);
                }
            }

            if (irocv[stageno].AMF == false)
            {
                irocv[stageno].AMF = true;
                SetRemeasureList_Demo(stageno);
                //CmdAmf(stageno);
            }

            //WriteCommLog("IR/OCV STOP", "SetRemeasureList()");
        }
        private async Task RemeasureExcute(int stageno)
        {
            for (int nChannel = 0; nChannel < _Constant.ChannelCount; ++nChannel)
            {
                if (irocvdata[stageno].MEASURERESULT[nChannel] != 0)
                {
                    CmdFetchIr(stageno, nChannel + 1);
                    await Task.Delay(1000);
                    CmdFetchOcv(stageno, nChannel + 1);
                    await Task.Delay(1000);
                }
            }

        }
        private void AddRemeaseList(int stageno)
        {
            int nRow = 0;
            int mResult = 0;
            double ir = 0.0, ocv = 0.0;

            remeasureinfo.InitData(stageno, irocvdata[stageno].REMEASURECELLCOUNT);
            for (int nChannel = 0; nChannel < _Constant.ChannelCount; ++nChannel)
            {
                mResult = irocvdata[stageno].MEASURERESULT[nChannel];
                ir = irocvdata[stageno].IR_AFTERVALUE[nChannel];
                ocv = irocvdata[stageno].OCV[nChannel];
                if ( mResult != 0)
                {
                    remeasureinfo.AddRemeasureList(nRow++, nChannel, mResult, ir, ocv);
                }
            }
        }
        #endregion IR/OCV Method

        #region IR/OCV Command
        private void CmdAutoStart(int stageno)
        {
            irocv[stageno].CmdAMS();
        }
        private void CmdAutoStop(int stageno)
        {
            irocv[stageno].CmdSTOP();
        }
        private void CmdFetchIr(int stageno, int channel)
        {
            irocv[stageno].CmdIR(channel);
        }
        private void CmdFetchOcv(int stageno, int channel)
        {
            irocv[stageno].CmdOCV(channel);
        }
        private void CmdAmf(int stageno)
        {
            irocv[stageno].CmdAMF();
        }
        private void CmdReset(int stageno)
        {
            irocv[stageno].CmdRESET();
        }
        private void CmdRemeasureExcute(int stageno)
        {

        }
        private void CmdRemeasureAll(int stageno)
        {
            //* auto inspection - tray up 으로 이동
            //* mode - remeasure all
            irocvdata[stageno].REMEASURE = false;
            irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepTrayIn;
        }
        private void CmdRemeasure(int stageno)
        {
            //* auto inspection - tray up 으로 이동
            //* mode - remeasure
            irocvdata[stageno].REMEASURE = true;
            irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepTrayIn;
        }
        private void CmdTrayOut(int stageno)
        {
            //* MES 보고? 후 트레이 배출
            //* auto inspection - tray out 으로 이동
            irocv[stageno].EQUIPSTATUS = enumEquipStatus.StepTrayOut;
        }
        #endregion IR/OCV Command

        #endregion

        #region PLC Action
        private void SaveLog(int stageno, string strMessage)
        {
            util.SaveLog(stageno, strMessage);
            util.SavePLCLog(stageno, strMessage);
        }
        public void PLC_Initialize(int stageno)
        {
            siemensplc.PLC_DBInitialize(stageno);

            SaveLog(stageno, "PLC Initialize ...");
        }
        private void PLC_SETAUTOMODE(int stageno, int nValue)
        {
            siemensplc.SetAutoMode(stageno, nValue);

            if (nValue == 1) SaveLog(stageno, "PC SET AUTOMODE ON");
            else SaveLog(stageno, "PC SET AUTOMODE OFF");
        }
        public void PLC_SETERROR(int stageno, int nValue, string msg, enumStageError _StageError)
        {
            if (nValue == 1 && siemensplc.PCERROR == 0)
            {
                SaveLog(stageno, "PC SET ERROR ON : " + msg);
            }
            else if (nValue == 0 && siemensplc.PCERROR == 1)
            {
                SaveLog(stageno, "PC SET ERROR OFF : " + msg);
            }

            siemensplc.SetPCError(stageno, nValue);

            if(nValue == 1) _errorForm.ShowMessage(_StageError, stageno);
            else _errorForm.HideMessage2(false);
        }
        private void PLC_TRAYDOWN(int stageno)
        {
            siemensplc.SetTrayUp(stageno, 0);
            siemensplc.SetTrayDown(stageno, 1);

            SaveLog(stageno, "TRAY DOWN ON");
        }
        private void PLC_TRAYUP(int stageno)
        {
            //* 2024 06 05 조건을 추가 함.
            if(siemensplc.PLCREADYCOMPLETE == 1 && siemensplc.PLCTRAYIN == 1)
            {
                siemensplc.SetTrayDown(stageno, 0);
                siemensplc.SetTrayUp(stageno, 1);

                SaveLog(stageno, "TRAY UP ON");
            }
            else
            {
                SaveLog(stageno, "waiting for [TRAY READY COMPLETE] signal");
            }
        }
        private void PLC_TRAYOUT(int stageno, int nValue)
        {
            siemensplc.SetTrayOut(stageno, nValue);

            if (nValue == 1) SaveLog(stageno, "TRAY OUT ON");
            else SaveLog(stageno, "TRAY OUT OFF");
        }
        private void PLC_MEASUREMENT_WAIT(int stageno, int nValue)
        {
            if (siemensplc.PCMEASUREMENTWAIT == 0)
                SaveLog(stageno, "SET MEASUREMENT WAIT ON");

            siemensplc.SetMeasurementWait(stageno, 1);
            siemensplc.SetMeasurementRunning(stageno, 0);
            siemensplc.SetMeasurementComplete(stageno, 0);
        }
        private void PLC_MEASUREMENT_RUNNING(int stageno, int nValue)
        {
            siemensplc.SetMeasurementWait(stageno, 0);
            siemensplc.SetMeasurementRunning(stageno, 1);
            siemensplc.SetMeasurementComplete(stageno, 0);

            SaveLog(stageno, "SET MEASUREMENT RUNNING ON");
        }
        private void PLC_MEASUREMENT_COMPLETE(int stageno, int nValue)
        {
            siemensplc.SetMeasurementWait(stageno, 0);
            siemensplc.SetMeasurementRunning(stageno, 0);
            siemensplc.SetMeasurementComplete(stageno, 1);

            SaveLog(stageno, "SET MEASUREMENT COMPLETE ON");
        }
        #endregion PLC Action

        #region MES Action
        private void DisplayTrayInfo(int stageno, IROCVData irocvData)
        {
            irocvform[stageno].SetTrayId(irocvData.TRAYID);
            irocvform[stageno].SetMesInfo(irocvData.TRAYSTATUSCODE, irocvData.ERRORCODE, irocvData.ERRORMESSAGE);
            //irocvform[stageno].SetRecipeId(irocvData.RECIPEID);

            IROCV_Refresh(stageno);
            irocvdata[stageno].SetStartTime();
        }
        #endregion

        #region DELEGATE

        #region Delegate Event IROCV Equipment (Socket)
        private void _IROCV_ShowControlMessage(int stageno, string param)
        {
            ShowControlMessage(stageno, param);
        }
        private void _IROCV_ProcessOcv(int stageno, string param)
        {
            ProcessOcv(stageno, param);
        }

        private void _IROCV_ProcessIr(int stageno, string param)
        {
            ProcessIr(stageno, param);
        }

        private void _IROCV_ProcessAmf(int stageno)
        {
            AutoTestFinish(stageno);
        }
        private void _IROCV_ProcessStop(int stageno)
        {
            AutoTestStop(stageno);
        }
        private void _IROCV_ProcessAms(int stageno)
        {
            if(irocv[stageno].AMF == false)
                AutoTestStart(stageno);
        }
        private void _IROCV_Error(int stageno, string param)
        {
            IROCV_Error(stageno, param);
        }
        private void _IROCV_Initialize(int stageno)
        {
            IROCV_Initialize(stageno);
        }
        #endregion Delegate Event IROCV Equipment (Socket)

        #region Delegate Event IROCV Form Event
        private void _IROCVFORM_SetValueToLabel(int channelno, int stageno)
        {
            double ir = irocvdata[stageno].IR_AFTERVALUE[channelno];
            double ocv = irocvdata[stageno].OCV[channelno];
            irocvform[stageno].SetValueToLabel(ir, ocv);
        }
        private void _IROCVFORM_SetOperationMode(int stageno, bool bAuto)
        {
            SetOperationMode(stageno, bAuto);
        }
        private void _IROCVFORM_IROCVReset(int stageno)
        {
            //* 계측기 리셋
            CmdReset(stageno);

            //* IROCV step 초기화 - 2024 05 10
            IROCV_Initialize(stageno);
        }
        private void _IROCVFORM_NGInfo(int stageno)
        {
            nginfo.SetStageNo(stageno);

            int[] ngUse, ngCount;
            util.ReadNGInfo(stageno, out ngUse, out ngCount);
            _system.REMEASUREUSE = ngUse;
            _system.REMEASURENG = ngCount;
            nginfo.SetNGInfo(stageno);
            nginfo.Show();
        }
        private void _IROCVFORM_REMEASUREInfo(int stageno)
        {
            //* test
            //irocvdata[stageno].REMEASURECELLCOUNT = 4;
            //irocvdata[stageno].MEASURERESULT[2] = 2;
            //irocvdata[stageno].IR_AFTERVALUE[2] = 0.322;
            //irocvdata[stageno].OCV[2] = 3672.21;

            //irocvdata[stageno].MEASURERESULT[12] = 3;
            //irocvdata[stageno].IR_AFTERVALUE[12] = 0.302;
            //irocvdata[stageno].OCV[12] = 3676.11;

            //irocvdata[stageno].MEASURERESULT[23] = 4;
            //irocvdata[stageno].IR_AFTERVALUE[23] = 0.312;
            //irocvdata[stageno].OCV[23] = 3662.31;

            //irocvdata[stageno].MEASURERESULT[29] = 5;
            //irocvdata[stageno].IR_AFTERVALUE[29] = 0.352;
            //irocvdata[stageno].OCV[29] = 3652.31;

            //AddRemeaseList(stageno);

            SetRemeasureList(stageno);

            remeasureinfo.Show();
        }
        private void _IROCVFORM_ConfigForm(int stageno)
        {
            using (var f = new PasswordForm())
            {
                f.StartPosition = FormStartPosition.Manual;
                f.Location = new Point(300, 300);
                DialogResult dr = f.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    irocvconfig[stageno].Show();
                    irocvconfig[stageno].STAGENO = stageno;
                }
            }
        }

        #endregion Delegate Event IROCV Form Event

        #region Delegate Event Measure Info Form Event
        private void _MEASUREINFOFORM_InitData(int stageno)
        {
            IROCV_Initialize(stageno);
        }
        private void _MEASUREINFOFORM_AmsStop(int stageno)
        {
            CmdAutoStop(stageno);
        }
        private void _MEASUREINFOFORM_AmsStart(int stageno)
        {
            CmdAutoStart(stageno);
        }
        private void _MEASUREINFOFORM_OCVFetch(int stageno, int channel)
        {
            CmdFetchOcv(stageno, channel);
        }
        private void _MEASUREINFOFORM_IRFetch(int stageno, int channel)
        {
            CmdFetchIr(stageno, channel);
        }

        //* PLC
        private void _MEASUREINFOFORM_ProbeClose(int stageno)
        {
            PLC_TRAYUP(stageno);
        }

        private void _MEASUREINFOFORM_ProbeOpen(int stageno)
        {
            PLC_TRAYDOWN(stageno);
        }

        //* MSA
        private void _MEASUREINFOFORM_MsaStop(int stageno)
        {
            MsaInspection_Stop(stageno);
        }

        private void _MEASUREINFOFORM_MsaStart(int stageno, int count)
        {
            MsaInspection_Start(stageno, count, "MSA");
        }
        private void _MEASUREINFOFORM_ManualSave(int stageno)
        {
            SaveManualResultFile(stageno);
        }
        //* OFFSET
        private void _MEASUREINFOFORM_OffsetSave(int stageno, string[] strOffset)
        {
            if(SaveOffsetFile(stageno, strOffset) == true)
                MessageBox.Show("Save Offset file at IROCV Bin folder.");
            else
                MessageBox.Show("Save Offset file Error!");
        }

        private void _MEASUREINFOFORM_OffsetOpen(int stageno)
        {
            OpenOffsetFile(stageno);
        }

        private void _MEASUREINFOFORM_OffsetCmdIr(int stageno, int channel)
        {
            CmdFetchIr(stageno, channel);
        }

        private void _MEASUREINFOFORM_OffsetApply(int stageno, string[] strOffset, double[] offset)
        {
            if (ApplyOffset(stageno, strOffset, offset) == true)
                MessageBox.Show("Apply IR/OCV offset success.");
            else
                MessageBox.Show("Apply IR/OCV offset Error!");
        }

        private void _MEASUREINFOFORM_OffsetStop(int stageno)
        {
            //* Msa Inspection과 같은 timer를 사용한다. (공유)
            MsaInspection_Stop(stageno);
        }

        private void _MEASUREINFOFORM_OffsetStart(int stageno, int count)
        {
            MsaInspection_Start(stageno, count, "OFFSET");
        }
        #endregion Delegate Event Measure Info Form Event

        #region Delegate Event Remeasure Info Form Event
        private void _REMEASUREINFOFORM_TrayOut(int stageno)
        {
            CmdTrayOut(stageno);
        }
        private void _REMEASUREINFOFORM_Remeasure(int stageno)
        {
            CmdRemeasure(stageno);
        }
        private void _REMEASUREINFOFORM_RemeasureAll(int stageno)
        {
            CmdRemeasureAll(stageno);
        }
        #endregion

        #region Delegate IROCV Config Form Event
        private void _IROCVCONFIG_SaveConfig(int stageno)
        {
            SaveConfigFile(stageno);
            ReadConfigFile(stageno);
        }
        #endregion

        #endregion
    }
}