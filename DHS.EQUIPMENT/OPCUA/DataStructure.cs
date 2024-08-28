using DHS.EQUIPMENT.Common;
using Opc.Ua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DHS.EQUIPMENT
{
    [DataContract(Namespace = "http://yourcompany.com/HeaderDataType")]
    public class HeaderDataType
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public DateTime Timestamp { get; set; }
    }
    [DataContract(Namespace = "http://yourcompany.com/ContentDataType")]
    public class TrayInfo
    {
        private static TrayInfo trayinfo = null;
        public static TrayInfo GetInstance()
        {
            if (trayinfo == null) trayinfo = new TrayInfo();
            return trayinfo;
        }

        public string TrayID { get; set; }
        public string EquipmentID { get; set; }
    }
    [DataContract(Namespace = "http://yourcompany.com/ContentDataType")]
    public class TrayRequestInfo : IEncodeable
    {
        public string[] CellID { get; set; }
        public string[] CellStatus { get; set; }
        public string TrayStatusCode { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        public ExpandedNodeId TypeId => throw new NotImplementedException();

        public ExpandedNodeId BinaryEncodingId => throw new NotImplementedException();

        public ExpandedNodeId XmlEncodingId => throw new NotImplementedException();

        public object Clone()
        {
            return new TrayRequestInfo
            {
                CellID = CellID,
                CellStatus = CellStatus,
                TrayStatusCode = TrayStatusCode,
                ErrorCode = ErrorCode,
                ErrorMessage = ErrorMessage
            };
        }

        public void Decode(IDecoder decoder)
        {
            CellID = decoder.ReadStringArray("CellID").Cast<string>().ToArray();
            CellStatus = decoder.ReadStringArray("CellStatus").Cast<string>().ToArray();
            TrayStatusCode = decoder.ReadString("TrayStatusCode");
            ErrorCode = decoder.ReadString("ErrorCode");
            ErrorMessage = decoder.ReadString("ErrorMessage");
        }

        public void Encode(IEncoder encoder)
        {
            encoder.WriteStringArray("CellID", CellID);
            encoder.WriteStringArray("CellStatus", CellStatus);
            encoder.WriteString("TrayStatusCode", TrayStatusCode);
            encoder.WriteString("ErrorCode", ErrorCode);
            encoder.WriteString("ErrorMessage", ErrorMessage);
        }

        public bool IsEqual(IEncodeable encodeable)
        {
            if (encodeable == null || !(encodeable is TrayRequestInfo))
            {
                return false;
            }

            var other = (TrayRequestInfo)encodeable;
            return CellID == other.CellID
                && CellStatus == other.CellStatus && TrayStatusCode == other.TrayStatusCode
                && ErrorCode == other.ErrorCode && ErrorMessage == other.ErrorMessage;
        }
    }
    public class IrocvDataCollection : IEncodeable
    {
        private static IrocvDataCollection trayrequestinfo = null;
        public static IrocvDataCollection GetInstance()
        {
            if (trayrequestinfo == null) trayrequestinfo = new IrocvDataCollection();
            return trayrequestinfo;
        }

        public string EquipmentID { get; set; }
        public string TrayID { get; set; }
        public string[] CellID { get; set; }
        public string[] CellStatus { get; set; }
        public double[] IR { get; set; }
        public double[] OCV { get; set; }

        ExpandedNodeId IEncodeable.TypeId => throw new NotImplementedException();

        ExpandedNodeId IEncodeable.BinaryEncodingId => throw new NotImplementedException();

        ExpandedNodeId IEncodeable.XmlEncodingId => throw new NotImplementedException();

        public ExpandedNodeId TypeId;

        public ExpandedNodeId BinaryEncodingId;

        public ExpandedNodeId XmlEncodingId;
        public void SetTypeId(NodeId nodeid)
        {
            TypeId = nodeid;
        }
        public object Clone()
        {
            return new IrocvDataCollection
            {
                EquipmentID = EquipmentID,
                TrayID = TrayID,
                CellID = CellID,
                CellStatus = CellStatus,
                IR = IR,
                OCV = OCV
            };
        }

        public void Decode(IDecoder decoder)
        {
            EquipmentID = decoder.ReadString("EquipmentID");
            TrayID = decoder.ReadString("TrayID");
            CellID = decoder.ReadStringArray("CellID").Cast<string>().ToArray();
            CellStatus = decoder.ReadStringArray("CellStatus").Cast<string>().ToArray();
            IR = decoder.ReadDoubleArray("IR").Cast<double>().ToArray();
            OCV = decoder.ReadDoubleArray("OCV").Cast<double>().ToArray();
        }

        public void Encode(IEncoder encoder)
        {
            encoder.WriteString("EquipmentID", EquipmentID);
            encoder.WriteString("TrayID", TrayID);
            encoder.WriteStringArray("CellID", CellID);
            encoder.WriteStringArray("CellStatus", CellStatus);
            encoder.WriteDoubleArray("IR", IR);
            encoder.WriteDoubleArray("OCV", OCV);
        }

        public bool IsEqual(IEncodeable encodeable)
        {
            if (encodeable == null || !(encodeable is IrocvDataCollection))
            {
                return false;
            }

            var other = (IrocvDataCollection)encodeable;
            return EquipmentID == other.EquipmentID && TrayID == other.TrayID
                && CellID == other.CellID && CellStatus == other.CellStatus
                && IR == other.IR && OCV == other.OCV;
        }
    }
    public class ReplyDataCollection : IEncodeable
    {
        public string ErrorCode { get; set; }
        public string ErrorMessage { get; set; }

        ExpandedNodeId IEncodeable.TypeId => throw new NotImplementedException();

        ExpandedNodeId IEncodeable.BinaryEncodingId => throw new NotImplementedException();

        ExpandedNodeId IEncodeable.XmlEncodingId => throw new NotImplementedException();

        public ExpandedNodeId TypeId;

        public ExpandedNodeId BinaryEncodingId;

        public ExpandedNodeId XmlEncodingId;

        public void SetTypeId(NodeId nodeid)
        {
            TypeId = nodeid;
        }
        public object Clone()
        {
            return new ReplyDataCollection
            {
                ErrorCode = ErrorCode,
                ErrorMessage = ErrorMessage
            };
        }

        public void Decode(IDecoder decoder)
        {
            ErrorCode = decoder.ReadString("ErrorCode");
            ErrorMessage = decoder.ReadString("ErrorMessage");
        }

        public void Encode(IEncoder encoder)
        {
            encoder.WriteString("ErrorCode", ErrorCode);
            encoder.WriteString("ErrorMessage", ErrorMessage);
        }

        public bool IsEqual(IEncodeable encodeable)
        {
            if (encodeable == null || !(encodeable is TrayRequestInfo))
            {
                return false;
            }

            var other = (TrayRequestInfo)encodeable;
            return ErrorCode == other.ErrorCode && ErrorMessage == other.ErrorMessage;
        }
    }
}
