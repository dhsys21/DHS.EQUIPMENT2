using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Microsoft.VisualBasic.FileIO;
using DHS.EQUIPMENT.Common;

namespace DHS.EQUIPMENT
{
    class Util
    {
        IniFile ini = new IniFile();
        string STX = string.Format("{0}", (char)2);
        string ETX = string.Format("{0}", (char)3);
        Form form = null;
        Panel panel = null;

        #region Add Form to Panel
        public void loadFormIntoPanel(Form childform, Panel parentpanel)
        {
            form = null;

            form = childform;
            panel = parentpanel;

            if (form != null && panel != null)
            {
                form.TopLevel = false;
                form.AutoScroll = true;
                InsertForm(form, panel);
                form.Dock = DockStyle.Fill;

                panel.Controls.Add(form);
                form.Show();
            }
        }

        private void InsertForm(Form f, Control c)
        {
            if (c != null)
            {
                f.TopLevel = false;
                f.Dock = DockStyle.Fill;
                f.FormBorderStyle = FormBorderStyle.None;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.ControlBox = false;

                c.Controls.Add(f);
                f.Show();
            }
        }

        static public void RemoveForm(Form f, Control c)
        {
            if (c != null)
            {
                f.TopLevel = false;
                f.Dock = DockStyle.None;
                f.FormBorderStyle = FormBorderStyle.None;
                f.MaximizeBox = false;
                f.MinimizeBox = false;
                f.ControlBox = false;

                c.Controls.Remove(f);
                f = null;
            }
        }
        #endregion

        public int TryParseInt(string text, int nDefaultValue)
        {
            int res;
            int value;
            if (Int32.TryParse(text,
                System.Globalization.NumberStyles.Integer,
                System.Globalization.CultureInfo.InvariantCulture,
                out res))
            {
                value = res;
                return value;
            }
            return nDefaultValue;
        }

        public double TryParseDouble(string text, double dDefaultValue)
        {
            double res;
            double value;
            if (Double.TryParse(text,
                System.Globalization.NumberStyles.Float,
                System.Globalization.CultureInfo.InvariantCulture,
                out res))
            {
                value = res;
                return value;
            }
            return dDefaultValue;
        }

        #region Config (ini/inf File)
        public void saveConfig(string filename, string main_title, string sub_title, string sValue)
        {

            // Network Setting
            ini[main_title][sub_title] = sValue;


            ini.Save(filename);
        }

        public string readConfig(string filename, string main_title, string sub_title)
        {
            string strValue = "";

            ini.Load(filename);
            strValue = ini[main_title][sub_title].ToString();

            return strValue;
        }
        #endregion

        #region CSV File
        public List<double[]> ReadCsvFile(string filename)
        {
            List<double[]> csvRecords = new List<double[]>();
            using (var parser = new TextFieldParser(filename))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(",");

                // 첫 라인은 title 이므로 skip 함.
                if (!parser.EndOfData)
                    parser.ReadLine();

                while (!parser.EndOfData)
                {
                    string[] fields = parser.ReadFields();
                    // 가장 앞 행은 채널 번호 (CHANNEL_01, CHANNEL_02, ...)
                    double[] values = new double[fields.Length - 1];
                    for (int nIndex = 1; nIndex < fields.Length; nIndex++)
                    {
                        values[nIndex - 1] = Convert.ToDouble(fields[nIndex]);
                    }
                    csvRecords.Add(values);
                }
            }
            return csvRecords;
        }
        public void WriteCsvFile(string filename, string[] strOffset)
        {
            using (var writer = new StreamWriter(filename))
            {
                // Write header row
                writer.WriteLine("CHANNEL,STANDARD,MEASURED,OFFSET"); // Replace with your column headers

                // Write data rows
                for (int nIndex = 0; nIndex < strOffset.Length; nIndex++)
                    writer.WriteLine("CH_" + (nIndex + 1).ToString() + "," + strOffset[nIndex]);
            }
        }
        #endregion

        #region File Write/ Read
        public void FileWrite(string filePath, string strData)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                {
                    streamWriter.Write(strData);

                    //다쓴 StreamWriter 와 FileStream 닫기
                    streamWriter.Flush();
                    streamWriter.Close();

                    fileStream.Close();
                }
            }
        }
        public void FileWrite_old(string filePath, string strData)
        {
            FileStream fileStream = new FileStream(
                filePath,              //저장경로
                FileMode.Create,       //파일스트림 모드
                FileAccess.Write       //접근 권한
                );

            StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8);
            streamWriter.Write(strData);

            //다쓴 StreamWriter 와 FileStream 닫기
            streamWriter.Close();
            fileStream.Close();
        }
        public void FileAppend(string filePath, string timeData, double[,] spData, string forceData)
        {
            string strData = "";
            using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fileStream))
                {
                    for (int i = 0; i < spData.GetLength(0); i++)
                    {
                        strData += spData[i, 0].ToString() + ";";
                    }
                    sw.WriteLine(timeData + ";" + strData + forceData);
                }
            }
        }

        public void FileAppend(string filePath, string strData)
        {
            try
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Append, FileAccess.Write))
                {
                    using (StreamWriter sw = new StreamWriter(fileStream))
                    {
                        sw.WriteLine(strData);
                    }
                }
            }
            catch (Exception ex) { }
        }
        #endregion


        public string MakeCMD(string cmd)
        {
            return STX + CheckSum(cmd) + ETX;
        }

        public string CheckSum(string strData)
        {
            string sRtnVal = "";

            if (strData != null)
            {
                byte checksum = 0x00;
                byte[] aa = new byte[strData.Length];

                for (int i = 0; i < strData.Length; i++)
                    aa[i] = (byte)strData[i];

                for (int i = 0; i < strData.Length; i++)
                    checksum += (byte)strData[i];

                string kwon = checksum.ToString("X2");

                sRtnVal = strData + kwon;
            }

            return sRtnVal;
        }

        public void DeleteOldFiles(string dirPath, string strDate)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                DateTime fileCreatedTime;
                DateTime compareTime = DateTime.ParseExact(strDate, "yyyyMMdd", null);
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    fileCreatedTime = file.CreationTime;
                    if (DateTime.Compare(fileCreatedTime, compareTime) < 0)
                    {
                        File.Delete(file.FullName);
                    }
                }
            }
            catch (Exception ex) { }
        }

        #region Precharger Result File
        public void SaveResultFile_Precharger(int stageno, PrechargerData CPreData)
        {
            string filename, dir, id, ir, ocv, ok_ng = string.Empty;

            string StageTitle = "STAGE " + (stageno + 1).ToString();
            dir = _Constant.DATA_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            filename = dir + "PreCharger_" + CPreData.TRAYID + "-" + System.DateTime.Now.ToString("yyMMddHHmmss") + ".csv";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            string file;
            file = "TRAY ID," + CPreData.TRAYID + Environment.NewLine;
            file = file + "CELL MODEL," + CPreData.CELLMODEL + Environment.NewLine;
            file = file + "LOT NUMBER," + CPreData.LOTNUMBER + Environment.NewLine;
            file = file + "ARRIVE TIME," + CPreData.ARRIVETIME.ToString("yyyy/MM/dd HH:mm:ss") + Environment.NewLine;
            file = file + "FINISH TIME," + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + Environment.NewLine;
            file = file + "VOLTAGE," + CPreData.SETVOLTAGE + Environment.NewLine;
            file = file + "CURRENT," + CPreData.SETCURRENT + Environment.NewLine;
            file = file + "TIME," + CPreData.SETTIME + Environment.NewLine;

            file += "CH,CELL ID,VOLT,CURR,RESULT\r\n";

            for (int i = 0; i < _Constant.ChannelCount; ++i)
            {
                //		ir = real_data.volt[i];
                //		ocv = real_data.curr[i];
                id = CPreData.CELLSERIAL[i];
                ir = CPreData.VOLT[i];
                ocv = CPreData.CURR[i];

                if (CPreData.CELL[i] == true)
                {
                    if (CPreData.MEASURERESULT[i] == true) ok_ng = "OK";
                    else ok_ng = "NG";
                }
                else if (CPreData.CELL[i] == false)
                {
                    if (CPreData.MEASURERESULT[i] == true) ok_ng = "No Cell";
                    else ok_ng = "NG(No Cell)";
                }

                file = file + (i + 1).ToString() + "," + id + "," + ir + "," + ocv + "," + ok_ng + Environment.NewLine;
            }

            FileWrite(filename, file);
        }
        #endregion Precharger Result File

        #region IROCV Result File
        public void SaveResultFile_IROCV(int stageno, IROCVData irocvdata, CEquipmentData systemconfig)
        {
            string filename, dir, id, ir, ir_offset, ocv, ok_ng = string.Empty;

            string StageTitle = "STAGE " + (stageno + 1).ToString();
            dir = _Constant.DATA_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            // for test 2023 05 18
            //filename = dir + "IROCV_" + irocvdata.TRAYID + "-" + System.DateTime.Now.ToString("yyMMddHHmmss") + ".csv";
            filename = dir + "IROCV_" + irocvdata.TRAYID.Replace("?", "TEST").Replace("\0", string.Empty) + "_" + System.DateTime.Now.ToString("yyMMddHHmmss") + ".csv";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            string file;
            file = "TRAY ID," + irocvdata.TRAYID.Replace("?", "TEST").Replace("\0", string.Empty) + Environment.NewLine;
            file += "EQUIPMENT ID," + irocvdata.EQUIPMENTID + Environment.NewLine;
            file += "ERROR CODE," + irocvdata.ERRORCODE.ToString() + Environment.NewLine;
            file += "ERROR MESSAGE," + irocvdata.ERRORMESSAGE + Environment.NewLine;
            //file = file + "CELL MODEL," + irocvdata.CELLMODEL + Environment.NewLine;
            //file = file + "LOT NUMBER," + irocvdata.LOTNUMBER + Environment.NewLine;
            file = file + "ARRIVE TIME," + irocvdata.ARRIVETIME.ToString("yyyy/MM/dd HH:mm:ss") + Environment.NewLine;
            file = file + "FINISH TIME," + irocvdata.FINISHTIME.ToString("yyyy/MM/dd HH:mm:ss") + Environment.NewLine;
            file = file + "IR RANGE," + systemconfig.IRMIN + "~" + systemconfig.IRMAX + Environment.NewLine;
            file = file + "IR REMEASURE RANGE," + systemconfig.IRREMEAMIN + "~" + systemconfig.IRREMEAMAX + Environment.NewLine;
            file = file + "OCV RANGE," + systemconfig.OCVMIN + "~" + systemconfig.OCVMAX + Environment.NewLine;
            file = file + "OCV REMEASURE RANGE," + systemconfig.OCVREMEAMIN + "~" + systemconfig.OCVREMEAMAX + Environment.NewLine;

            file += "CH,IR,IR OFFSET,OCV,RESULT" + Environment.NewLine;

            for (int i = 0; i < _Constant.ChannelCount; ++i)
            {
                ir = irocvdata.IR_AFTERVALUE[i].ToString("F4");
                ir_offset = irocvdata.IR_OFFSET[i].ToString("F4");
                ocv = irocvdata.OCV[i].ToString("F2");

                if (irocvdata.CELL[i] == 1)
                {
                    if (irocvdata.MEASURERESULT[i] == 0) ok_ng = "OK";
                    else ok_ng = "NG";
                }
                else 
                {
                    if (irocvdata.MEASURERESULT[i] == 1) ok_ng = "No Cell";
                    else ok_ng = "NG(No Cell)";
                }

                file = file + (i + 1).ToString() + "," + ir + "," + ir_offset + "," + ocv + "," + ok_ng + Environment.NewLine;
            }

            FileWrite(filename, file);
        }
        public void SaveManualResultFile_IROCV(int stageno, IROCVData irocvdata, CEquipmentData systemconfig)
        {
            string filename, dir, id, ir, ir_offset, ocv, ok_ng = string.Empty;

            string StageTitle = "STAGE " + (stageno + 1).ToString();
            dir = _Constant.DATA_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            filename = dir + "IROCV_" + irocvdata.TRAYID.Replace("?", "TEST").Replace("\0", string.Empty) + "_" + System.DateTime.Now.ToString("yyMMddHHmmss") + ".csv";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            string file;
            file = "TRAY ID," + irocvdata.TRAYID.Replace("?", "TEST").Replace("\0", string.Empty) + Environment.NewLine;
            file += "EQUIPMENT ID," + irocvdata.EQUIPMENTID + Environment.NewLine;
            file = file + "IR RANGE," + systemconfig.IRMIN + "~" + systemconfig.IRMAX + Environment.NewLine;
            file = file + "IR REMEASURE RANGE," + systemconfig.IRREMEAMIN + "~" + systemconfig.IRREMEAMAX + Environment.NewLine;
            file = file + "OCV RANGE," + systemconfig.OCVMIN + "~" + systemconfig.OCVMAX + Environment.NewLine;
            file = file + "OCV REMEASURE RANGE," + systemconfig.OCVREMEAMIN + "~" + systemconfig.OCVREMEAMAX + Environment.NewLine;

            file += "CH,IR,IR OFFSET,OCV,RESULT" + Environment.NewLine;

            for (int i = 0; i < _Constant.ChannelCount; ++i)
            {
                ir = irocvdata.IR_AFTERVALUE[i].ToString("F4");
                ir_offset = irocvdata.IR_OFFSET[i].ToString("F4");
                ocv = irocvdata.OCV[i].ToString("F2");

                if (irocvdata.MEASURERESULT[i] == 0) ok_ng = "OK";
                else ok_ng = "NG";

                file = file + (i + 1).ToString() + "," + ir + "," + ir_offset + "," + ocv + "," + ok_ng + Environment.NewLine;
            }

            FileWrite(filename, file);
        }
        public string SaveMsaResultFile_IROCV(int stageno, int nCount, IROCVData irocvdata)
        {
            string filename = string.Empty; 
            string dir, irOrigin, irAfter, ocv;

            string StageTitle = "STAGE " + (stageno + 1).ToString();
            dir = _Constant.MSA_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            filename = dir + "MSA_Result_" + System.DateTime.Now.ToString("yyMMddHHmmss") + "_" + nCount.ToString("D2") + ".csv";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            string file = string.Empty;
            file += "CH,IR Origin,IR After,OCV" + Environment.NewLine;

            for (int i = 0; i < _Constant.ChannelCount; ++i)
            {
                irOrigin = irocvdata.IR_ORIGINALVALUE[i].ToString("F4");
                irAfter = irocvdata.IR_AFTERVALUE[i].ToString("F4");
                ocv = irocvdata.OCV[i].ToString("F2");

                file = file + "Ch_" + (i + 1).ToString("D3") + "," + irOrigin + "," + irAfter + "," + ocv + Environment.NewLine;
            }

            FileWrite(filename, file);

            return filename;
        }
        public string SaveOffsetResultFile_IROCV(int stageno, int nCount, IROCVData irocvdata)
        {
            string filename = string.Empty;
            string dir, irOrigin;

            string StageTitle = "STAGE " + (stageno + 1).ToString();
            dir = _Constant.OFFSET_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            filename = dir + "OFFSET_Result_" + System.DateTime.Now.ToString("yyMMddHHmmss") + "_" + nCount.ToString("D2") + ".csv";

            if (File.Exists(filename))
            {
                File.Delete(filename);
            }

            string file = string.Empty;
            file += "CH,IR Origin" + Environment.NewLine;

            for (int i = 0; i < _Constant.ChannelCount; ++i)
            {
                irOrigin = irocvdata.IR_ORIGINALVALUE[i].ToString("F4");

                file = file + "Ch_" + (i + 1).ToString("D3") + "," + irOrigin + Environment.NewLine;
            }

            FileWrite(filename, file);

            return filename;
        }
        public void SaveReportFile_IROCV(int stageno, IROCVData irocvdata, string type, string reportFilename)
        {
            string ir_filename = "MSA_IR_" + reportFilename;
            SaveIRReportFile_IROCV(stageno, irocvdata, type, ir_filename);

            string ocv_filename = "MSA_OCV_" + reportFilename;
            if(type == "MSA")
                SaveOCVReportFile_IROCV(stageno, irocvdata, type, ocv_filename);
        }
        public string SaveIRReportFile_IROCV(int stageno, IROCVData irocvdata, string type, string reportFilename)
        {
            string filename = string.Empty;
            string dir = string.Empty;
            string StageTitle = "STAGE " + (stageno + 1).ToString();

            #region file create
            if (type == "MSA") dir = _Constant.MSA_PATH;
            else if (type == "OFFSET") dir = _Constant.OFFSET_PATH;

            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            filename = dir + reportFilename;
            #endregion

            string file = string.Empty;
            string title = string.Empty;
            file = "CHANNEL";

            //file += "CH,IR Origin,IR After,OCV" + Environment.NewLine;
            if (File.Exists(filename))
            {
                #region read and write msa / offset result file
                int linenumber = 0;
                //var reader = new StreamReader(File.OpenRead(filename));
                var reader = new StreamReader(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                while (!reader.EndOfStream)
                {
                    linenumber++;
                    //* title
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (linenumber == 1)
                    {
                        for (int i = 0; i < values.Length; i++)
                            title = title + ",IR_" + (i + 1).ToString("D3");
                        file = file + title + Environment.NewLine;
                    }

                    //* channel
                    if (linenumber > 1)
                    {
                        for (int i = 0; i < values.Length; i++)
                            file = file + values[i] + ",";

                        if (type == "MSA")
                            file = file + irocvdata.IR_AFTERVALUE[linenumber - 2].ToString("F4") + Environment.NewLine;
                        else if (type == "OFFSET")
                            file = file + irocvdata.IR_ORIGINALVALUE[linenumber - 2].ToString("F4") + Environment.NewLine;
                    }
                }
                #endregion
            }
            else
            {
                #region write new file
                file = "CHANNEL,IR_001" + Environment.NewLine;
                for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                {
                    if (type == "MSA")
                        file = file + "Ch_" + (nIndex + 1).ToString("D3") + ","
                            + irocvdata.IR_AFTERVALUE[nIndex].ToString("F4") + Environment.NewLine;
                    else if (type == "OFFSET")
                        file = file + "Ch_" + (nIndex + 1).ToString("D3") + ","
                            + irocvdata.IR_ORIGINALVALUE[nIndex].ToString("F4") + Environment.NewLine;
                }

                #endregion
            }

            FileWrite(filename, file);

            return filename;
        }
        public string SaveOCVReportFile_IROCV(int stageno, IROCVData irocvdata, string type, string reportFilename)
        {
            string filename = string.Empty;
            string dir = string.Empty;
            string StageTitle = "STAGE " + (stageno + 1).ToString();

            #region file create
            if (type == "MSA") dir = _Constant.MSA_PATH;
            else if (type == "OFFSET") dir = _Constant.OFFSET_PATH;

            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);

            filename = dir + reportFilename;
            #endregion

            string file = string.Empty;
            string title = string.Empty;
            file = "CHANNEL";

            //file += "CH,IR Origin,IR After,OCV" + Environment.NewLine;
            if (File.Exists(filename))
            {
                #region read and write msa / offset result file
                int linenumber = 0;
                //var reader = new StreamReader(File.OpenRead(filename));
                var reader = new StreamReader(File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                while (!reader.EndOfStream)
                {
                    linenumber++;
                    //* title
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    if (linenumber == 1)
                    {
                        for (int i = 0; i < values.Length; i++)
                            title = title + ",OCV_" + (i + 1).ToString("D3");
                        file = file + title + Environment.NewLine;
                    }

                    //* channel
                    if (linenumber > 1)
                    {
                        for (int i = 0; i < values.Length; i++)
                            file = file + values[i] + ",";

                        file = file + irocvdata.OCV[linenumber - 2].ToString("F2") + Environment.NewLine;
                    }
                }
                #endregion
            }
            else
            {
                #region write new file
                file = "CHANNEL,OCV_001" + Environment.NewLine;
                for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                {
                    file = file + "Ch_" + (nIndex + 1).ToString("D3") + ","
                                                + irocvdata.OCV[nIndex].ToString("F2") + Environment.NewLine;
                }
                #endregion
            }

            FileWrite(filename, file);

            return filename;
        }
        //* N회 측정한 offset 파일을 읽어 average를 구함
        public double[] ReadOffsetFile_IROCV(int stageno, string reportFilename)
        {
            string filename = string.Empty;
            string dir = string.Empty;
            string StageTitle = "STAGE " + (stageno + 1).ToString();
            double[] ir_offset_average = new double[_Constant.ChannelCount];
            #region file create
            dir = _Constant.OFFSET_PATH;

            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) return ir_offset_average;

            filename = dir + reportFilename;
            #endregion

            #region read msa / offset result file
            string file = string.Empty;
            int linenumber = 0;
            var reader = new StreamReader(File.OpenRead(filename));
            while (!reader.EndOfStream)
            {
                linenumber++;
                var line = reader.ReadLine();
                var values = line.Split(',');
                double sum = 0.0;
                if (linenumber > 1)
                {
                    for (int i = 1; i < values.Length; i++)
                        sum += Convert.ToDouble(values[i]);
                    ir_offset_average[linenumber - 2] = sum / (values.Length - 1);
                }
            }
            #endregion

            return ir_offset_average;
        }
        #endregion IROCV Result File

        #region IROCV Read Offset File
        #endregion

        #region Save Log
        string strPLCMessage = string.Empty;
        public void SaveLog(int nIndex, string strMessage, string type)
        {
            string dir = "";
            string StageTitle = "STAGE" + (nIndex + 1).ToString();
            dir = _Constant.LOG_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
            string filename = dir + StageTitle + "_" + DateTime.Now.ToString("yyMMdd-HH") + ".log";
            string strMonitoring = "";

            if (type == "RX" && strMessage.Contains("MON"))
            {
                strMonitoring = strMessage.Substring(0, 33) + Environment.NewLine;
                strMonitoring = strMonitoring + ParseMon2859(strMessage);
            }
            else
                strMonitoring = strMessage;

            strMonitoring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + type + "\t" + strMonitoring;
            if (System.IO.File.Exists(filename) == false) FileWrite(filename, strMonitoring);
            else FileAppend(filename, strMonitoring);

        }
        string oldIROCVMessage = string.Empty;
        public void SaveLog(int nIndex, string strMessage)
        {
            if (strMessage == oldIROCVMessage) return;
            oldIROCVMessage = strMessage;
            string dir = string.Empty;
            string StageTitle = "STAGE" + (nIndex + 1).ToString();
            dir = _Constant.LOG_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
            string filename = dir + StageTitle + "_" + DateTime.Now.ToString("yyMMdd-HH") + ".log";
            string strMonitoring = string.Empty;

            strMonitoring = strMessage;

            strMonitoring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\t" + strMonitoring;
            if (System.IO.File.Exists(filename) == false) FileWrite(filename, strMonitoring);
            else FileAppend(filename, strMonitoring);

        }
        public void SavePLCLog(int nIndex, string strMessage)
        {
            if (strMessage == strPLCMessage) return;
            strPLCMessage = strMessage;

            string dir = "";
            string StageTitle = "STAGE" + (nIndex + 1).ToString();
            dir = _Constant.LOG_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\" + StageTitle + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
            string filename = dir + StageTitle + "_PLC_" + DateTime.Now.ToString("yyMMdd-HH") + ".log";
            string strMonitoring = "";

            strMonitoring = strMessage;

            strMonitoring = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\t" + strMonitoring;
            if (System.IO.File.Exists(filename) == false) FileWrite(filename, strMonitoring);
            else FileAppend(filename, strMonitoring);

        }
        public void SaveMesLog(string strMessage)
        {
            if (strMessage == oldIROCVMessage) return;
            oldIROCVMessage = strMessage;
            string dir = string.Empty;
            dir = _Constant.LOG_PATH;
            dir += System.DateTime.Now.ToString("yyyyMMdd") + "\\";
            if (Directory.Exists(dir) == false) Directory.CreateDirectory(dir);
            string filename = dir + "MES_" + DateTime.Now.ToString("yyMMdd") + ".log";

            strMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "\t" + strMessage;
            if (System.IO.File.Exists(filename) == false) FileWrite(filename, strMessage);
            else FileAppend(filename, strMessage);

        }
        #endregion Save Log

        #region
        public void SaveNGInfo(int stageno, bool bIncrease, int[] ngUse, int[] ngCount)
        {
            string filename = _Constant.BIN_PATH + "RemeasureInfo_" + (stageno + 1) + ".inf";

            string ch_no = string.Empty;
            try
            {
                for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                {
                    ch_no = "CH_" + (nIndex + 1).ToString("D2");
                    if (bIncrease == true)
                    {
                        ngUse[nIndex] += 1;
                        ngCount[nIndex] += 1;
                    }

                    saveConfig(filename, ch_no, "USE", ngUse[nIndex].ToString());
                    saveConfig(filename, ch_no, "NG", ngCount[nIndex].ToString());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
        public void ReadNGInfo(int stageno, out int[] ngUse, out int[] ngCount)
        {
            ngUse = new int[_Constant.ChannelCount];
            ngCount = new int[_Constant.ChannelCount];
            string filename = _Constant.BIN_PATH + "RemeasureInfo_" + (stageno + 1) + ".inf";
            string ch_no = string.Empty;
            try
            {
                if (File.Exists(filename))
                {
                    for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                    {
                        ch_no = "CH_" + (nIndex + 1).ToString("D2");
                        ngUse[nIndex] = Convert.ToInt32(readConfig(filename, ch_no, "USE"));
                        ngCount[nIndex] = Convert.ToInt32(readConfig(filename, ch_no, "NG"));
                    }
                }
                else
                {
                    for (int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                    {
                        ngUse[nIndex] = 0;
                        ngCount[nIndex] = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
        #endregion

        //* MON 응답의 전체 길이가 2859
        private string ParseMon2859(string sMessage)
        {
            string sMon = "", code = "", tmpStr = "", msg = "";
            int nvolt = 0, ncurr = 0, ncapa = 0;
            sMon = sMessage.Substring(32);
            int iLength = sMon.Length - 10;
            int cnt = 1, nChannel = 1;
            while (iLength > cnt)
            {
                tmpStr = sMon.Substring(0, 11);
                sMon = sMon.Remove(0, 11);
                //sMon = sMon.Substring(11);
                cnt += 11;

                char[] charValues = tmpStr.ToCharArray();
                code = String.Format("{0:X}", Convert.ToInt32(charValues[0]));
                nvolt = (charValues[1] << 24) + (charValues[2] << 16) + (charValues[3] << 8) + charValues[4];
                ncurr = (charValues[5] << 24) + (charValues[6] << 16) + (charValues[7] << 8) + charValues[8];
                ncapa = (charValues[9] << 8) + (int)charValues[10];

                if (nChannel > 125)
                    code = code + "";

                msg = msg + "CH-" + nChannel.ToString() + "-" + code + "-" + nvolt + "-" + ncurr + "-" + ncapa + "\t";
                nChannel++;
            }


            return msg;
        }

        /// <summary>
		/// 셀의 위치 변경 - ChangeMap
		/// </summary>
		/// <param name="nIndex">셀 번호 0 - 255</param>
		/// <param name="startposition">시작점 (left top, right top, left bottom, right bottom</param>
		/// <param name="rowIndex">row 번호</param>
		/// <param name="colIndex">column 번호</param>
		public void ChangeMapToGridView(int nIndex, int startposition, out int rowIndex, out int colIndex)
        {
            rowIndex = 0;
            colIndex = 0;
            switch (startposition)
            {
                case 1: // start at left top to right top
                    rowIndex = nIndex / 16;
                    colIndex = nIndex % 16;
                    break;
                case 5: // start at left top to left bottom
                    rowIndex = nIndex % 16;
                    colIndex = nIndex / 16;
                    break;
                case 2: // start at right top to left top
                    rowIndex = nIndex / 16;
                    colIndex = 15 - nIndex % 16;
                    break;
                case 6: // start at right top to right bottom
                    rowIndex = nIndex % 16;
                    colIndex = 15 - nIndex / 16;
                    break;
                case 3: // start at left bottom to right bottom
                    rowIndex = 15 - nIndex / 16;
                    colIndex = nIndex % 16;
                    break;
                case 7: // start at left bottom to left top
                    rowIndex = 15 - nIndex % 16;
                    colIndex = nIndex / 16;
                    break;
                case 4: // start at right bottom to left bottom
                    rowIndex = 15 - nIndex / 16;
                    colIndex = 15 - nIndex % 16;
                    break;
                case 8: // start at right bottom to right top
                    rowIndex = 15 - nIndex % 16;
                    colIndex = 15 - nIndex / 16;
                    break;
                default:
                    break;
            }
        }

        public void ChangeMapToGridView(int nIndex, out int rowIndex, out int colIndex)
        {
            rowIndex = 0;
            colIndex = 0;
            int startposition = _Constant.StartPosition;
            switch (startposition)
            {
                case 1: // start at left top to right top
                    rowIndex = nIndex / 16;
                    colIndex = nIndex % 16;
                    break;
                case 5: // start at left top to left bottom
                    rowIndex = nIndex % 16;
                    colIndex = nIndex / 16;
                    break;
                case 2: // start at right top to left top
                    rowIndex = nIndex / 16;
                    colIndex = 15 - nIndex % 16;
                    break;
                case 6: // start at right top to right bottom
                    rowIndex = nIndex % 16;
                    colIndex = 15 - nIndex / 16;
                    break;
                case 3: // start at left bottom to right bottom
                    rowIndex = 15 - nIndex / 16;
                    colIndex = nIndex % 16;
                    break;
                case 7: // start at left bottom to left top
                    rowIndex = 15 - nIndex % 16;
                    colIndex = nIndex / 16;
                    break;
                case 4: // start at right bottom to left bottom
                    rowIndex = 15 - nIndex / 16;
                    colIndex = 15 - nIndex % 16;
                    break;
                case 8: // start at right bottom to right top
                    rowIndex = 15 - nIndex % 16;
                    colIndex = 15 - nIndex / 16;
                    break;
                default:
                    break;
            }
        }
    }

    public static class ExtensionMethods
    {
        public static void DoubleBuffered(this DataGridView dgv, bool setting)
        {
            Type dgvType = dgv.GetType();
            PropertyInfo pi = dgvType.GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            pi.SetValue(dgv, setting, null);
        }
    }
}
