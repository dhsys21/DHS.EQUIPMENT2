using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OPCUACLIENT;

namespace DHS.EQUIPMENT
{
    public class MesClient
    {
        public static bool connection = false;
        public bool isRead = false;
        OPCUACLIENT.OPCUACLIENT opcclient = null;

        static System.Windows.Forms.Timer _tmrMESRead = new System.Windows.Forms.Timer();
        public MesClient()
        {
            opcclient = new OPCUACLIENT.OPCUACLIENT();

            SetEquipmentTagList();
            SetMesTagList();

            MesClientStart();

            _tmrMESRead.Interval = 1000;
            _tmrMESRead.Tick += new EventHandler(MESReadTimer_Tick);
        }

        public void MesClientStart()
        {
            connection = opcclient.Connect("opc.tcp://192.168.0.14:48000/IROCV");
        }

        private void MESReadTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (connection == true)
                {
                    Stopwatch sw = new Stopwatch();
                    sw.Start();

                    #region Get Value from Equipment Tag
                    int nIndex = 0;
                    foreach (var tag in EquipTagList)
                    {
                        if (tag.tagDataType == MesClient.EnumDataType.dtUInt32)
                        {
                            var val = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(dgvPCTOMES, nIndex++, 1, val.ToString());
                        }
                        else if (tag.tagDataType == MesClient.EnumDataType.dtUInt32Arr)
                        {
                            var val = (UInt32[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            for (int i = 0; i < val.Length; i++)
                                SetValue(dgvPCTOMES, nIndex++, 1, val[i].ToString());
                        }
                        else if (tag.tagDataType == MesClient.EnumDataType.dtString)
                        {
                            var val = (string)ReadValue(tag.tagName, (int)tag.tagDataType);
                            if (val == null) val = string.Empty;
                            SetValue(dgvPCTOMES, nIndex++, 1, val.ToString());
                        }
                        else if (tag.tagDataType == MesClient.EnumDataType.dtStringArr)
                        {
                            var val = (string[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            for (int i = 0; i < val.Length; i++)
                                SetValue(dgvPCTOMES, nIndex++, 1, val[i]);
                        }
                    }
                    #endregion

                    #region Get Value from Mes Tag
                    nIndex = 0;
                    foreach (var tag in MesTagList)
                    {
                        if (tag.tagDataType == MesClient.EnumDataType.dtUInt32)
                        {
                            var val = (UInt32)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(dgvMESTOPC, nIndex++, 1, val.ToString());
                        }
                        else if (tag.tagDataType == MesClient.EnumDataType.dtUInt32Arr)
                        {
                            var val = (UInt32[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            for (int i = 0; i < val.Length; i++)
                                SetValue(dgvMESTOPC, nIndex++, 1, val[i].ToString());
                        }
                        else if (tag.tagDataType == MesClient.EnumDataType.dtString)
                        {
                            var val = (string)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(dgvMESTOPC, nIndex++, 1, val.ToString());
                        }
                        else if (tag.tagDataType == MesClient.EnumDataType.dtStringArr)
                        {
                            var val = (string[])ReadValue(tag.tagName, (int)tag.tagDataType);
                            for (int i = 0; i < val.Length; i++)
                                SetValue(dgvMESTOPC, nIndex++, 1, val[i]);
                        }
                        else if (tag.tagDataType == MesClient.EnumDataType.dtBoolean)
                        {
                            var val = ReadValue(tag.tagName, (int)tag.tagDataType);
                            //var val = (Boolean)ReadValue(tag.tagName, (int)tag.tagDataType);
                            SetValue(dgvMESTOPC, nIndex++, 1, val.ToString());
                        }
                    }
                    #endregion

                    sw.Stop();
                    //SetValue(sw.ElapsedMilliseconds.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        #region OPC UA Method
        private void SetValue(DataGridView dgv, int row, int column, string value)
        {
            dgv.Rows[row].Cells[column].Value = value;
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
