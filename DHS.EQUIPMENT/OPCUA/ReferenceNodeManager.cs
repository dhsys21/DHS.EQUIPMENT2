﻿/* ========================================================================
 * Copyright (c) 2005-2016 The OPC Foundation, Inc. All rights reserved.
 *
 * OPC Foundation MIT License 1.00
 * 
 * Permission is hereby granted, free of charge, to any person
 * obtaining a copy of this software and associated documentation
 * files (the "Software"), to deal in the Software without
 * restriction, including without limitation the rights to use,
 * copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following
 * conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
 * OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
 * WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
 * FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 *
 * The complete license agreement can be found here:
 * http://opcfoundation.org/License/MIT/1.00/
 * ======================================================================*/

using System;
using System.Collections.Generic;
using System.Xml;
using System.Threading;
using Opc.Ua;
using Opc.Ua.Server;
using System.IO;
using Telerik.WinControls.UI.TaskBoard;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.ServiceModel.Channels;
using Microsoft.Extensions.Primitives;
using System.Linq;
using DHS.EQUIPMENT;
using DHS.EQUIPMENT.Common;
using S7.Net;

namespace OPCUASERVER
{
    /// <summary>
    /// A node manager for a server that exposes several variables.
    /// </summary>
    public class EmptyNodeManager : CustomNodeManager2
    {
        IrocvProcess irocvprocess = IrocvProcess.GetInstance();
        IROCVData[] irocvdata = new IROCVData[_Constant.frmCount];
        #region Constructors
        /// <summary>
        /// Initializes the node manager.
        /// </summary>
        public EmptyNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        :
            base(server, configuration, Namespaces.ReferenceApplications)
        {
            SystemContext.NodeIdFactory = this;

            // get the configuration for the node manager.
            m_configuration = configuration.ParseExtension<ReferenceServerConfiguration>();

            // use suitable defaults if no configuration exists.
            if (m_configuration == null)
            {
                m_configuration = new ReferenceServerConfiguration();
            }


            timer1 = new System.Timers.Timer(500);
            timer1.Elapsed += Timer1_Elapsed;
            timer1.Start();
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// An overrideable version of the Dispose.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // TBD
            }
        }
        #endregion

        #region INodeIdFactory Members
        /// <summary>
        /// Creates the NodeId for the specified node.
        /// </summary>
        private long m_lastUsedId;
        private ushort m_namespaceIndex;
        public NodeId New2(ISystemContext context, NodeState node)
        {
            uint id = Utils.IncrementIdentifier(ref m_lastUsedId);
            return new NodeId(id, m_namespaceIndex);
        }
        public override NodeId New(ISystemContext context, NodeState node)
        {
            BaseInstanceState instance = node as BaseInstanceState;

            if (instance != null && instance.Parent != null)
            {
                string id = instance.Parent.NodeId.Identifier as string;

                if (id != null)
                {
                    return new NodeId(id + "_" + instance.SymbolicName, instance.Parent.NodeId.NamespaceIndex);
                }
            }

            return node.NodeId;
        }
        #endregion

        #region Private Helper Functions
        private static bool IsUnsignedAnalogType(BuiltInType builtInType)
        {
            if (builtInType == BuiltInType.Byte ||
                builtInType == BuiltInType.UInt16 ||
                builtInType == BuiltInType.UInt32 ||
                builtInType == BuiltInType.UInt64)
            {
                return true;
            }
            return false;
        }

        private static bool IsAnalogType(BuiltInType builtInType)
        {
            switch (builtInType)
            {
                case BuiltInType.Byte:
                case BuiltInType.UInt16:
                case BuiltInType.UInt32:
                case BuiltInType.UInt64:
                case BuiltInType.SByte:
                case BuiltInType.Int16:
                case BuiltInType.Int32:
                case BuiltInType.Int64:
                case BuiltInType.Float:
                case BuiltInType.Double:
                    return true;
            }
            return false;
        }

        private static Opc.Ua.Range GetAnalogRange(BuiltInType builtInType)
        {
            switch (builtInType)
            {
                case BuiltInType.UInt16:
                    return new Range(System.UInt16.MaxValue, System.UInt16.MinValue);
                case BuiltInType.UInt32:
                    return new Range(System.UInt32.MaxValue, System.UInt32.MinValue);
                case BuiltInType.UInt64:
                    return new Range(System.UInt64.MaxValue, System.UInt64.MinValue);
                case BuiltInType.SByte:
                    return new Range(System.SByte.MaxValue, System.SByte.MinValue);
                case BuiltInType.Int16:
                    return new Range(System.Int16.MaxValue, System.Int16.MinValue);
                case BuiltInType.Int32:
                    return new Range(System.Int32.MaxValue, System.Int32.MinValue);
                case BuiltInType.Int64:
                    return new Range(System.Int64.MaxValue, System.Int64.MinValue);
                case BuiltInType.Float:
                    return new Range(System.Single.MaxValue, System.Single.MinValue);
                case BuiltInType.Double:
                    return new Range(System.Double.MaxValue, System.Double.MinValue);
                case BuiltInType.Byte:
                    return new Range(System.Byte.MaxValue, System.Byte.MinValue);
                default:
                    return new Range(System.SByte.MaxValue, System.SByte.MinValue);
            }
        }
        #endregion

        #region Timer Tick

        private System.Timers.Timer timer1 = null;

        private void Timer1_Elapsed( object sender, System.Timers.ElapsedEventArgs e )
        {
            if (list != null)
            {
                lock (Lock)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        list[i].Value = list[i].Value + 1;
                        // 下面这行代码非常的关键，涉及到更改之后会不会通知到客户端
                        list[i].ClearChangeMasks( SystemContext, false );
                    }
                }
            }
        }

        #endregion

        #region INodeManager Members
        /// <summary>
        /// Does any initialization required before the address space can be used.
        /// </summary>
        /// <remarks>
        /// The externalReferences is an out parameter that allows the node manager to link to nodes
        /// in other node managers. For example, the 'Objects' node is managed by the CoreNodeManager and
        /// should have a reference to the root folder node(s) exposed by this node manager.  
        /// </remarks>
        public override void CreateAddressSpace( IDictionary<NodeId, IList<IReference>> externalReferences )
        {
            lock (Lock)
            {
                LoadPredefinedNodes(SystemContext, externalReferences);

                IList<IReference> references = null;

                if (!externalReferences.TryGetValue(ObjectIds.ObjectsFolder, out references))
                {
                    externalReferences[ObjectIds.ObjectsFolder] = references = new List<IReference>();
                }

                string resourcepath = "IROCV2.Config.xml";
                ImportXml(externalReferences, resourcepath);

                ushort namespaceIndex2 = SystemContext.NamespaceUris.GetIndexOrAppend("urn:KitInformationmodel.Siemens.com");
                CreateObjectInNamespace("urn:KitInformationmodel.Siemens.com", namespaceIndex2);
                // NodeState deviceSetNode = PredefinedNodes.Values.First(x => x.BrowseName.Name == "Battery Standard Interface");
                //base.CreateAddressSpace(externalReferences);

                /* for test 2024 07 09 

                #region MES TAG LIST
                FolderState rootMy = CreateFolder(null, "Mes");
                rootMy.AddReference(ReferenceTypes.Organizes, true, ObjectIds.ObjectsFolder);
                references.Add(new NodeStateReference(ReferenceTypes.Organizes, false, rootMy.NodeId));
                rootMy.EventNotifier = EventNotifiers.SubscribeToEvents;
                AddRootNotifier(rootMy);

                string[] strCellStatus = new string[channelcount];
                string[] strCellID = new string[channelcount];
                for (int i = 0; i < channelcount; i++)
                {
                    strCellStatus[i] = "";
                    strCellID[i] = "";
                }

                CreateVariable(rootMy, "SequenceNo", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy, "AcknowledgeNo", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy, "EquipmentID", DataTypeIds.String, ValueRanks.Scalar, "");
                CreateVariable(rootMy, "TrayID", DataTypeIds.String, ValueRanks.Scalar, "");
                CreateVariable(rootMy, "TrayStatusCode", DataTypeIds.String, ValueRanks.Scalar, "");
                CreateVariable(rootMy, "ErrorCode", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy, "ErrorMessage", DataTypeIds.String, ValueRanks.Scalar, "");

                CreateVariable(rootMy, "CellID", DataTypeIds.String, ValueRanks.OneDimension, strCellID);
                CreateVariable(rootMy, "CellStatus", DataTypeIds.String, ValueRanks.OneDimension, strCellStatus);

                AddPredefinedNode(SystemContext, rootMy);
                #endregion

                #region PC TAG LIST
                FolderState rootMy2 = CreateFolder(null, "Equipment");
                rootMy2.AddReference(ReferenceTypes.Organizes, true, ObjectIds.ObjectsFolder);
                references.Add(new NodeStateReference(ReferenceTypes.Organizes, false, rootMy2.NodeId));
                rootMy2.EventNotifier = EventNotifiers.SubscribeToEvents;
                AddRootNotifier(rootMy2);

                //UInt32[] uiIR = new UInt32[channelcount];
                //UInt32[] uiOCV = new uint[channelcount];
                float[] fIR = new float[channelcount];
                float[] fOCV = new float[channelcount];
                string[] strCellStatus2 = new string[channelcount];
                string[] strCellID2 = new string[channelcount];
                for (int i = 0; i < channelcount; i++)
                {
                    //uiIR[i] = (UInt32)0;
                    //uiOCV[i] = (UInt32)0;
                    fIR[i] = (float)0.0;
                    fOCV[i] = (float)0.0;
                    strCellStatus2[i] = "";
                    strCellID2[i] = "";
                }

                //* IR/OCV 데이터
                CreateVariable(rootMy2, "SequenceNo", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy2, "AcknowledgeNo", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy2, "EquipmentID", DataTypeIds.String, ValueRanks.Scalar, "");
                CreateVariable(rootMy2, "TrayID", DataTypeIds.String, ValueRanks.Scalar, "");

                CreateVariable(rootMy2, "CellID", DataTypeIds.String, ValueRanks.OneDimension, strCellID2);
                CreateVariable(rootMy2, "CellStatus", DataTypeIds.String, ValueRanks.OneDimension, strCellStatus2);
                //CreateVariable(rootMy2, "IR", DataTypeIds.UInt32, ValueRanks.OneDimension, uiIR);
                //CreateVariable(rootMy2, "OCV", DataTypeIds.UInt32, ValueRanks.OneDimension, uiOCV);
                CreateVariable(rootMy2, "IR", DataTypeIds.Float, ValueRanks.OneDimension, fIR);
                CreateVariable(rootMy2, "OCV", DataTypeIds.Float, ValueRanks.OneDimension, fOCV);

                AddPredefinedNode(SystemContext, rootMy2);
                #endregion

                #region PLC TAG LIST
                FolderState rootMy3 = CreateFolder(null, "PLC");
                rootMy3.AddReference(ReferenceTypes.Organizes, true, ObjectIds.ObjectsFolder);
                references.Add(new NodeStateReference(ReferenceTypes.Organizes, false, rootMy3.NodeId));
                rootMy3.EventNotifier = EventNotifiers.SubscribeToEvents;
                AddRootNotifier(rootMy3);

                //* PLC System 정보
                bool bValue = false;
                float fValue = 0.0f;
                CreateVariable(rootMy3, "InterfaceVersionProject", DataTypeIds.String, ValueRanks.Scalar, "");
                CreateVariable(rootMy3, "EquipmentName", DataTypeIds.String, ValueRanks.Scalar, "");
                CreateVariable(rootMy3, "EquipmentTypeID", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "LineID", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "AreaID", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "VendorID", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "EquipmentID", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "State", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "Mode", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                //CreateVariable(rootMy3, "Blocked", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                //CreateVariable(rootMy3, "Starved", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "Blocked", DataTypeIds.Boolean, ValueRanks.Scalar, bValue);
                CreateVariable(rootMy3, "Starved", DataTypeIds.Boolean, ValueRanks.Scalar, bValue);
                CreateVariable(rootMy3, "CurrentSpeed", DataTypeIds.Float, ValueRanks.Scalar, fValue);
                CreateVariable(rootMy3, "DesignSpeed", DataTypeIds.Float, ValueRanks.Scalar, fValue);
                CreateVariable(rootMy3, "DefectCounter", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "GoodCounter", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "TotalCounter", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "StandStillReason", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);

                CreateVariable(rootMy3, "StackLight0Color", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "StackLight0Behavior", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "StackLight1Color", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "StackLight1Behavior", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "StackLight2Color", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "StackLight2Behavior", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "StackLight3Color", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);
                CreateVariable(rootMy3, "StackLight3Behavior", DataTypeIds.UInt32, ValueRanks.Scalar, (UInt32)0);

                AddPredefinedNode(SystemContext, rootMy3);
                #endregion

                */
            }
        }
        private void CreateObjectInNamespace(string objectName, ushort namespaceIndex)
        {
            // Create a new object
            NodeId objectId = new NodeId(objectName, namespaceIndex);
            BaseObjectState newObject = new BaseObjectState(null)
            {
                NodeId = objectId,
                BrowseName = new QualifiedName(objectName, namespaceIndex),
                DisplayName = objectName,
            };

            // Add the object to the address space
            AddPredefinedNode(SystemContext, newObject);
        }

        #region Import XML
        private void ImportXml(IDictionary<NodeId, IList<IReference>> externalReferences, string resourcepath)
        {
            NodeStateCollection predefinedNodes = new NodeStateCollection();

            Stream stream = new FileStream(resourcepath, FileMode.Open);
            Opc.Ua.Export.UANodeSet nodeSet = Opc.Ua.Export.UANodeSet.Read(stream);

            //* System.String[]이 NameSpaceUris에 들어감.
            //SystemContext.NamespaceUris.GetIndexOrAppend(nodeSet.NamespaceUris.ToString());

            for (int i = 0; i < nodeSet.NamespaceUris.Length; i++)
                SystemContext.NamespaceUris.GetIndexOrAppend(nodeSet.NamespaceUris[i].ToString());

            nodeSet.Import(SystemContext, predefinedNodes);

            for (int ii = 0; ii < predefinedNodes.Count; ii++)
            {
                AddPredefinedNode(SystemContext, predefinedNodes[ii]);
            }
            // ensure the reverse refernces exist.
            AddReverseReferences(externalReferences);
        }
        protected override NodeState AddBehaviourToPredefinedNode(ISystemContext context, NodeState predefinedNode)
        {
            if (predefinedNode is BaseInstanceState passiveNode)
            {
                NodeId typeId = passiveNode.TypeDefinitionId;

                if (!IsNodeIdInNamespace(typeId) || typeId.IdType != IdType.Numeric)
                {
                    return predefinedNode;
                }

                //switch ((uint)typeId.Identifier)
                //{
                //    case ObjectTypes.head:
                //        return ReplaceNodeWithType(passiveNode, context, (p) => new Custom2State(p));
                //        //case ObjectTypes.CustomType1:
                //        //    return ReplaceNodeWithType(passiveNode, context, (p) => new Custom1State(p));
                //}
            }

            return predefinedNode;
        }

        protected BaseInstanceState ReplaceNodeWithType<T>(BaseInstanceState passiveNode, ISystemContext context, Func<NodeState, T> create)
            where T : BaseInstanceState
        {
            if (passiveNode is T)
            {
                return passiveNode;
            }

            T activeNode = create(passiveNode.Parent);
            activeNode.Create(context, passiveNode);

            if (passiveNode.Parent != null)
            {
                passiveNode.Parent.ReplaceChild(context, activeNode);
            }

            return activeNode;
        }
        #endregion

        #region Call Event Handler
        private BaseDataVariableState<bool> SystemState = null;
        protected override ServiceResult Call(ISystemContext context, 
            CallMethodRequest methodToCall, 
            MethodState method, 
            CallMethodResult result)
        {
            return DispatchControllerMethod(context, methodToCall, 
                methodToCall.InputArguments, result.InputArgumentResults, result.OutputArguments);
        }
        private ServiceResult DispatchControllerMethod(
            ISystemContext context,
            CallMethodRequest methodToCall,
            IList<Variant> inputArguments,
            List<StatusCode> inputArgumentResults,
            List<Variant> outputArguments)
        {
            /// outputArguments의 header
            /// 공통으로 사용
            
            HeaderDataType header = new HeaderDataType
            {
                Id = 1,
                Type = "1",
                Timestamp = DateTime.Now
            };

            NodeId nodeid = methodToCall.MethodId;
            string strNodeId = nodeid.Identifier.ToString();

            if(strNodeId == "7004")
            {
                /// [GetEnvelope (FORIR_2_1_RequestTrayInformation)]
                /// inputArguments : null
                /// outputArguments(return value) : TrayInfo(EquipmentID, TrayID)
                
                TrayInfo trayInfo = new TrayInfo
                {
                    EquipmentID = "IRCOV0002",
                    TrayID = "Test1234"
                };

                ExtensionObject extensionObject1 = CreateExtensionObject(NodeId.Parse("ns=0;i=5000"), header);
                ExtensionObject extensionObject2 = CreateExtensionObject(NodeId.Parse("ns=0;i=5032"), trayInfo);

                outputArguments.Add(new Variant(extensionObject1));
                outputArguments.Add(new Variant(extensionObject2));

                /// ProSys Library에서는 아래 코드를 사용
                /// outputArguments[0] = new Variant(extensionObject1);
                /// outputArguments[1] = new Variant(extensionObject2);

                //* Write Tray Info
                irocvprocess.MESWRITETRAYINFO = true;
                
                return StatusCodes.Good;
            }
            else if (strNodeId == "7012")
            {
                /// SetEnvelope (FORIR_2_1_RequestTrayInformation)
                /// inputArguments : TrayRequestInfo (CellID, CellStatus, TrayStatusCode, ErrorCode, ErrorMessage)
                /// outputArguments : null
                
                if (inputArguments != null)
                {
                    foreach (Variant value1 in inputArguments)
                    {
                        ExtensionObject extObj = value1.Value as ExtensionObject;
                        
                        if (extObj != null)
                            Console.WriteLine(extObj.ToString());

                        TrayRequestInfo trayinfo;
                        if(extObj.TypeId.Identifier.ToString() == "5034")
                        {
                            trayinfo = ConvertExtensionObjectTrayInfo(extObj);

                            //* Read Tray Info
                            irocvprocess.MESREADTRAYINFO = true;
                            irocvprocess.SetTrayInfo(0, trayinfo);

                            //* for test
                            irocvdata[0] = IROCVData.GetInstance(0);
                            irocvprocess.DisplayTrayInfo(0, irocvdata[0]);
                        }
                    }
                }

                return StatusCodes.Good;
            }
            else if (strNodeId == "7013")
            {
                /// GetEnvelope (FORIR_2_2_DataCollection)
                /// inputArguments : null
                /// outputArguments(return value) : Data Collection(EquipmentID, TayID, CellID, CellStatus, IR, OCV)

                #region for test
                string equipmentid = "IROCV0002";
                string trayid = "Test0001";
                string[] cellid = new string[32];
                string[] cellstatus = new string[32];
                double[] ir = new double[32];
                double[] ocv = new double[32];

                for(int i = 0; i < 32;i++)
                {
                    cellid[i] = "test" + (i + 1).ToString("D3");
                    cellstatus[i] = "0";
                    
                    ir[i] = 0.3832 + (double)(i % 8 + i) / 10000.0;
                    ocv[i] = 3721.21 + (double)(i % 8 + i) / 100.0;
                }
                irocvdata[0] = IROCVData.GetInstance(0);
                irocvdata[0].InitData();
                irocvdata[0].EQUIPMENTID = equipmentid;
                irocvdata[0].TRAYID = trayid;
                irocvdata[0].CELLID = cellid;
                irocvdata[0].CELLSTATUSIROCV = cellstatus;
                irocvdata[0].IR_AFTERVALUE = ir;
                irocvdata[0].OCV = ocv;
                #endregion for test

                IrocvDataCollection irocvData = new IrocvDataCollection();
                irocvData = irocvprocess.GetIrocvDataCollection(NodeId.Parse("ns=0;i=5041"), irocvdata[0]);
                ExtensionObject extensionObject2 = ConvertDataCollectionExtensionObject(irocvData);
                ExtensionObject extensionObject1 = CreateExtensionObject(NodeId.Parse("ns=0;i=5000"), header);
                //ExtensionObject extensionObject2 = CreateExtensionObject(NodeId.Parse("ns=0;i=5041"), irocvData);

                outputArguments.Add(new Variant(extensionObject1));
                outputArguments.Add(new Variant(extensionObject2));

                //* Write Data Collection
                irocvprocess.MESWRITEDATACOLLECTION = true;

                return StatusCodes.Good;
            }
            else if(strNodeId == "7016")
            {
                /// SetEnvelope (FORIR_2_2_DataCollection)
                /// inputArguments : Data Collection Reply (ErrorCode, ErrorMessage)
                /// outputArguments : null
                if (inputArguments != null)
                {
                    foreach (Variant value1 in inputArguments)
                    {
                        ExtensionObject extObj = value1.Value as ExtensionObject;

                        if (extObj != null)
                            Console.WriteLine(extObj.ToString());

                        ReplyDataCollection replyDataCollection;
                        if (extObj.TypeId.Identifier.ToString() == "5045")
                        {
                            replyDataCollection = ConvertExtensionObjectDataCollection(extObj);

                            //* Read Tray Info
                            irocvprocess.MESREADDATACOLLECTION = true;
                            irocvprocess.SetReplyDataCollection(0, replyDataCollection);
                        }
                    }
                }

                return StatusCodes.Good;
            }

            return StatusCodes.Bad;
        }

        #region ExtensionObject
        public static TrayRequestInfo ConvertExtensionObjectTrayInfo(ExtensionObject extObj)
        {
            if(extObj.Body is TrayRequestInfo)
                return (TrayRequestInfo)extObj.Body;

            if(extObj.Body is byte[])
            {
                var decoder = new BinaryDecoder((byte[])extObj.Body, new ServiceMessageContext());
                var data = new TrayRequestInfo();
                data.Decode(decoder);
                return data;
            }

            throw new InvalidCastException("ExtensionObject cannot be cast to TrayRequestInfo");
        }
        public static ReplyDataCollection ConvertExtensionObjectDataCollection(ExtensionObject extObj)
        {
            if (extObj.Body is ReplyDataCollection)
                return (ReplyDataCollection)extObj.Body;

            if (extObj.Body is byte[])
            {
                var decoder = new BinaryDecoder((byte[])extObj.Body, new ServiceMessageContext());
                var data = new ReplyDataCollection();
                data.Decode(decoder);
                return data;
            }

            throw new InvalidCastException("ExtensionObject cannot be cast to TrayRequestInfo");
        }
        public static ExtensionObject ConvertDataCollectionExtensionObject(IrocvDataCollection irocvDC)
        {
            // Create a binary encoder
            byte[] buffer;
            using (var stream = new System.IO.MemoryStream())
            {
                using (var encoder = new BinaryEncoder(stream, new ServiceMessageContext(), false))
                {
                    irocvDC.Encode(encoder);
                    //encoder.Flush();
                    buffer = stream.ToArray();
                }
            }

            // Create an ExtensionObject with the encoded data
            var extensionObject = new ExtensionObject(irocvDC.TypeId, buffer);
            return extensionObject;
        }
        public static ExtensionObject CreateExtensionObject(NodeId nodeid, HeaderDataType header)
        {
            List<byte> headerBytes = new List<byte>();

            headerBytes.AddRange(IntToBytes(header.Id));
            headerBytes.AddRange(IntToBytes(header.Type.Length));
            headerBytes.AddRange(StringToBytes(header.Type));

            /// DataTime 형식이 오류가 남. 수정 필요
            /// headerBytes.AddRange(DateTimeToByteArray(DateTime.Now));
            uint identifier = Convert.ToUInt32(nodeid.Identifier);
            var typeid = new ExpandedNodeId(identifier, "urn:KitInformationmodel.Siemens.com");
            return new ExtensionObject(typeid, headerBytes.ToArray());
        }

        public static ExtensionObject CreateExtensionObject(NodeId nodeid, TrayInfo content)
        {
            List<byte> contentBytes = new List<byte>();

            contentBytes.AddRange(IntToBytes(content.EquipmentID.Length));
            contentBytes.AddRange(StringToBytes(content.EquipmentID));

            contentBytes.AddRange(IntToBytes(content.TrayID.Length));
            contentBytes.AddRange(StringToBytes(content.TrayID));

            uint identifier = Convert.ToUInt32(nodeid.Identifier);
            var typeid = new ExpandedNodeId(identifier, "http://StandardBatteryInterface");
            return new ExtensionObject(typeid, contentBytes.ToArray());
        }
        public static ExtensionObject CreateExtensionObject(NodeId nodeid, IrocvDataCollection content)
        {
            List<byte> contentBytes = new List<byte>();

            contentBytes.AddRange(IntToBytes(content.EquipmentID.Length));
            contentBytes.AddRange(StringToBytes(content.EquipmentID));

            contentBytes.AddRange(IntToBytes(content.TrayID.Length));
            contentBytes.AddRange(StringToBytes(content.TrayID));

            contentBytes.AddRange(IntToBytes(content.CellID.Length));
            contentBytes.AddRange(StringToBytes(content.CellID));

            contentBytes.AddRange(IntToBytes(content.CellStatus.Length));
            contentBytes.AddRange(StringToBytes(content.CellStatus));

            contentBytes.AddRange(IntToBytes(content.IR.Length));
            contentBytes.AddRange(DoubleToBytes(content.IR));

            contentBytes.AddRange(IntToBytes(content.OCV.Length));
            contentBytes.AddRange(DoubleToBytes(content.OCV));

            uint identifier = Convert.ToUInt32(nodeid.Identifier);
            var typeid = new ExpandedNodeId(identifier, "http://StandardBatteryInterface");
            return new ExtensionObject(typeid, contentBytes.ToArray());
        }
        public static byte[] IntToBytes(int value)
        {
            byte[] byteArray = BitConverter.GetBytes(value);
            return byteArray;
        }
        public static byte[] StringToBytes(string value)
        {
            byte[] asciiBytes = Encoding.ASCII.GetBytes(value);
            return asciiBytes;
        }
        public static byte[] StringToBytes(string[] values)
        {
            List<byte> byteList = new List<byte>();

            foreach (var str in values)
            {
                byte[] stringBytes = Encoding.UTF8.GetBytes(str);
                byte[] lengthBytes = BitConverter.GetBytes(stringBytes.Length);

                byteList.AddRange(lengthBytes);
                byteList.AddRange(stringBytes);
            }

            return byteList.ToArray();
        }
        public static byte[] DoubleToBytes(double value)
        {
            byte[] doublegBytes = BitConverter.GetBytes(value);
            return doublegBytes;
        }
        public static byte[] DoubleToBytes(double[] values)
        {
            List<byte> byteList = new List<byte>();
            foreach(var dValue in values)
            {
                byte[] doublegBytes = BitConverter.GetBytes(dValue);
                byte[] lengthBytes = BitConverter.GetBytes(doublegBytes.Length);

                byteList.AddRange(lengthBytes);
                byteList.AddRange(doublegBytes);
            }

            return byteList.ToArray();
        }
        public static byte[] DateTimeToByteArray2(DateTime dateTime)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(ms, dateTime);
                return ms.ToArray();
            }
        }
        public static byte[] DateTimeToByteArray(DateTime dateTime)
        {
            long ticks = dateTime.Ticks;
            return BitConverter.GetBytes(ticks);
        }
        #endregion
        
        protected ServiceResult Call2(ISystemContext context,
            NodeId objectId,
            IList<Variant> inputArguments,
            IList<ServiceResult> argumentErrors,
            IList<Variant> outputArguments)
        {
            return ServiceResult.Good;
        }
        private ServiceResult OnAddCall(
            ISystemContext context,
            MethodState method,
            IList<object> inputArguments,
            IList<object> outputArguments )
        {

            // all arguments must be provided.
            if (inputArguments.Count < 2)
            {
                return StatusCodes.BadArgumentsMissing;
            }

            try
            {
                Int32 floatValue = (Int32)inputArguments[0];
                Int32 uintValue = (Int32)inputArguments[1];

                // set output parameter
                outputArguments[0] = "我也不知道刚刚发生了什么，调用设备为：" + method.Parent.DisplayName;
                return ServiceResult.Good;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show( ex.Message );
                return new ServiceResult( StatusCodes.BadInvalidArgument );
            }
        }

        #endregion

        #region Create Variable and Method

        #region Create Folder
        private List<BaseDataVariableState<int>> list = null;

        /// <summary>
        /// 创建一个新的节点，节点名称为字符串
        /// </summary>
        protected FolderState CreateFolder( NodeState parent, string name )
        {
            return CreateFolder( parent, name, string.Empty );
        }
        
        /// <summary>
        /// 创建一个新的节点，节点名称为字符串
        /// </summary>
        protected FolderState CreateFolder( NodeState parent, string name, string description )
        {
            FolderState folder = new FolderState( parent );

            folder.SymbolicName = name;
            folder.ReferenceTypeId = ReferenceTypes.Organizes;
            folder.TypeDefinitionId = ObjectTypeIds.FolderType;
            folder.Description = description;
            if (parent == null)
            {
                folder.NodeId = new NodeId( name, NamespaceIndex );
            }
            else
            {
                folder.NodeId = new NodeId( parent.NodeId.ToString( ) + "/" + name );
            }
            folder.BrowseName = new QualifiedName( name, NamespaceIndex );
            folder.DisplayName = new LocalizedText( name );
            folder.WriteMask = AttributeWriteMask.None;
            folder.UserWriteMask = AttributeWriteMask.None;
            folder.EventNotifier = EventNotifiers.None;

            if (parent != null)
            {
                parent.AddChild( folder );
            }

            return folder;
        }

        /// <summary>
        /// Creates a new variable.
        /// </summary>
        private BaseDataVariableState<T> CreateVariable<T>( NodeState parent, string name, NodeId dataType, int valueRank, T defaultValue )
        {
            BaseDataVariableState<T> variable = new BaseDataVariableState<T>( parent );

            variable.SymbolicName = name;
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.TypeDefinitionId = VariableTypeIds.BaseDataVariableType;
            if (parent == null)
            {
                variable.NodeId = new NodeId( name, NamespaceIndex );
            }
            else
            {
                variable.NodeId = new NodeId( parent.NodeId.ToString( ) + "/" + name );
            }
            variable.BrowseName = new QualifiedName( name, NamespaceIndex );
            variable.DisplayName = new LocalizedText( name );
            variable.WriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description;
            variable.UserWriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description;
            variable.DataType = dataType;
            variable.ValueRank = valueRank;
            variable.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.Historizing = false;
            variable.Value = defaultValue;
            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.Now;


            if (parent != null)
            {
                parent.AddChild( variable );
            }

            return variable;
        }

        /// <summary>
        /// 创建一个值节点，类型需要在创建的时候指定
        /// </summary>
        protected BaseDataVariableState CreateBaseVariable( NodeState parent, string name, string description, NodeId dataType, int valueRank, object defaultValue )
        {
            BaseDataVariableState variable = new BaseDataVariableState( parent );

            variable.SymbolicName = name;
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.TypeDefinitionId = VariableTypeIds.BaseDataVariableType;
            if (parent == null)
            {
                variable.NodeId = new NodeId( name, NamespaceIndex );
            }
            else
            {
                variable.NodeId = new NodeId( parent.NodeId.ToString( ) + "/" + name );
            }
            variable.Description = description;
            variable.BrowseName = new QualifiedName( name, NamespaceIndex );
            variable.DisplayName = new LocalizedText( name );
            variable.WriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description;
            variable.UserWriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description;
            variable.DataType = dataType;
            variable.ValueRank = valueRank;
            variable.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.Historizing = false;
            variable.Value = defaultValue;
            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.Now;

            if (parent != null)
            {
                parent.AddChild( variable );
            }

            return variable;
        }

        /// <summary>
        /// Creates a new variable.
        /// </summary>
        private DataItemState CreateDataItemVariable(NodeState parent, string path, string name, BuiltInType dataType, int valueRank)
        {
            DataItemState variable = new DataItemState(parent);
            variable.ValuePrecision = new PropertyState<double>(variable);
            variable.Definition = new PropertyState<string>(variable);

            variable.Create(
                SystemContext,
                null,
                variable.BrowseName,
                null,
                true);

            variable.SymbolicName = name;
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.NodeId = new NodeId(path, NamespaceIndex);
            variable.BrowseName = new QualifiedName(path, NamespaceIndex);
            variable.DisplayName = new LocalizedText("en", name);
            variable.WriteMask = AttributeWriteMask.None;
            variable.UserWriteMask = AttributeWriteMask.None;
            variable.DataType = (uint)dataType;
            variable.ValueRank = valueRank;
            variable.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.Historizing = false;
            variable.Value = Opc.Ua.TypeInfo.GetDefaultValue((uint)dataType, valueRank, Server.TypeTree);
            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.UtcNow;

            if (valueRank == ValueRanks.OneDimension)
            {
                variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0 });
            }
            else if (valueRank == ValueRanks.TwoDimensions)
            {
                variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0, 0 });
            }

            variable.ValuePrecision.Value = 2;
            variable.ValuePrecision.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.ValuePrecision.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.Definition.Value = String.Empty;
            variable.Definition.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.Definition.UserAccessLevel = AccessLevels.CurrentReadOrWrite;

            if (parent != null)
            {
                parent.AddChild(variable);
            }

            return variable;
        }

        private DataItemState[] CreateDataItemVariables(NodeState parent, string path, string name, BuiltInType dataType, int valueRank, UInt16 numVariables)
        {
            List<DataItemState> itemsCreated = new List<DataItemState>();
            // create the default name first:
            itemsCreated.Add(CreateDataItemVariable(parent, path, name, dataType, valueRank));
            // now to create the remaining NUMBERED items
            for (uint i = 0; i < numVariables; i++)
            {
                string newName = string.Format("{0}{1}", name, i.ToString("000"));
                string newPath = string.Format("{0}/Mass/{1}", path, newName);
                itemsCreated.Add(CreateDataItemVariable(parent, newPath, newName, dataType, valueRank));
            }//for i
            return (itemsCreated.ToArray());
        }

        private ServiceResult OnWriteDataItem(
            ISystemContext context,
            NodeState node,
            NumericRange indexRange,
            QualifiedName dataEncoding,
            ref object value,
            ref StatusCode statusCode,
            ref DateTime timestamp)
        {
            DataItemState variable = node as DataItemState;

            // verify data type.
            Opc.Ua.TypeInfo typeInfo = Opc.Ua.TypeInfo.IsInstanceOfDataType(
                value,
                variable.DataType,
                variable.ValueRank,
                context.NamespaceUris,
                context.TypeTable);

            if (typeInfo == null || typeInfo == Opc.Ua.TypeInfo.Unknown)
            {
                return StatusCodes.BadTypeMismatch;
            }

            if (typeInfo.BuiltInType != BuiltInType.DateTime)
            {
                double number = Convert.ToDouble(value);
                number = Math.Round(number, (int)variable.ValuePrecision.Value);
                value = Opc.Ua.TypeInfo.Cast(number, typeInfo.BuiltInType);
            }

            return ServiceResult.Good;
        }
        #endregion

        /// <summary>
        /// Creates a new method.
        /// </summary>
        private MethodState CreateMethod( NodeState parent, string name )
        {
            MethodState method = new MethodState( parent );

            method.SymbolicName = name;
            method.ReferenceTypeId = ReferenceTypeIds.HasComponent;
            if (parent == null)
            {
                method.NodeId = new NodeId( name, NamespaceIndex );
            }
            else
            {
                method.NodeId = new NodeId( parent.NodeId.ToString( ) + "/" + name );
            }
            method.BrowseName = new QualifiedName( name, NamespaceIndex );
            method.DisplayName = new LocalizedText( name );
            method.WriteMask = AttributeWriteMask.None;
            method.UserWriteMask = AttributeWriteMask.None;
            method.Executable = true;
            method.UserExecutable = true;

            if (parent != null)
            {
                parent.AddChild( method );
            }

            return method;
        }

        /// <summary>
        /// Creates a new variable.
        /// </summary>
        private AnalogItemState CreateAnalogItemVariable(NodeState parent, string path, string name, BuiltInType dataType, int valueRank)
        {
            return (CreateAnalogItemVariable(parent, path, name, dataType, valueRank, null));
        }

        private AnalogItemState CreateAnalogItemVariable(NodeState parent, string path, string name, BuiltInType dataType, int valueRank, object initialValues)
        {
            return (CreateAnalogItemVariable(parent, path, name, dataType, valueRank, initialValues, null));
        }

        private AnalogItemState CreateAnalogItemVariable(NodeState parent, string path, string name, BuiltInType dataType, int valueRank, object initialValues, Range customRange)
        {
            return CreateAnalogItemVariable(parent, path, name, (uint)dataType, valueRank, initialValues, customRange);
        }

        private AnalogItemState CreateAnalogItemVariable(NodeState parent, string path, string name, NodeId dataType, int valueRank, object initialValues, Range customRange)
        {
            AnalogItemState variable = new AnalogItemState(parent);
            variable.BrowseName = new QualifiedName(path, NamespaceIndex);
            variable.EngineeringUnits = new PropertyState<EUInformation>(variable);
            variable.InstrumentRange = new PropertyState<Range>(variable);

            variable.Create(
                SystemContext,
                new NodeId(path, NamespaceIndex),
                variable.BrowseName,
                null,
                true);

            variable.NodeId = new NodeId(path, NamespaceIndex);
            variable.SymbolicName = name;
            variable.DisplayName = new LocalizedText("en", name);
            variable.WriteMask = AttributeWriteMask.None;
            variable.UserWriteMask = AttributeWriteMask.None;
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.DataType = dataType;
            variable.ValueRank = valueRank;
            variable.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.Historizing = false;

            if (valueRank == ValueRanks.OneDimension)
            {
                variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0 });
            }
            else if (valueRank == ValueRanks.TwoDimensions)
            {
                variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0, 0 });
            }

            BuiltInType builtInType = Opc.Ua.TypeInfo.GetBuiltInType(dataType, Server.TypeTree);

            // Simulate a mV Voltmeter
            Range newRange = GetAnalogRange(builtInType);
            // Using anything but 120,-10 fails a few tests
            newRange.High = Math.Min(newRange.High, 120);
            newRange.Low = Math.Max(newRange.Low, -10);
            variable.InstrumentRange.Value = newRange;

            if (customRange != null)
            {
                variable.EURange.Value = customRange;
            }
            else
            {
                variable.EURange.Value = new Range(100, 0);
            }

            if (initialValues == null)
            {
                variable.Value = Opc.Ua.TypeInfo.GetDefaultValue(dataType, valueRank, Server.TypeTree);
            }
            else
            {
                variable.Value = initialValues;
            }

            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.UtcNow;
            // The latest UNECE version (Rev 11, published in 2015) is available here:
            // http://www.opcfoundation.org/UA/EngineeringUnits/UNECE/rec20_latest_08052015.zip
            variable.EngineeringUnits.Value = new EUInformation("mV", "millivolt", "http://www.opcfoundation.org/UA/units/un/cefact");
            // The mapping of the UNECE codes to OPC UA(EUInformation.unitId) is available here:
            // http://www.opcfoundation.org/UA/EngineeringUnits/UNECE/UNECE_to_OPCUA.csv
            variable.EngineeringUnits.Value.UnitId = 12890; // "2Z"
            variable.OnWriteValue = OnWriteAnalog;
            variable.EURange.OnWriteValue = OnWriteAnalogRange;
            variable.EURange.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.EURange.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.EngineeringUnits.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.EngineeringUnits.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.InstrumentRange.OnWriteValue = OnWriteAnalogRange;
            variable.InstrumentRange.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.InstrumentRange.UserAccessLevel = AccessLevels.CurrentReadOrWrite;

            if (parent != null)
            {
                parent.AddChild(variable);
            }

            return variable;
        }

        /// <summary>
        /// Creates a new variable.
        /// </summary>
        private DataItemState CreateTwoStateDiscreteItemVariable(NodeState parent, string path, string name, string trueState, string falseState)
        {
            TwoStateDiscreteState variable = new TwoStateDiscreteState(parent);

            variable.NodeId = new NodeId(path, NamespaceIndex);
            variable.BrowseName = new QualifiedName(path, NamespaceIndex);
            variable.DisplayName = new LocalizedText("en", name);
            variable.WriteMask = AttributeWriteMask.None;
            variable.UserWriteMask = AttributeWriteMask.None;

            variable.Create(
                SystemContext,
                null,
                variable.BrowseName,
                null,
                true);

            variable.SymbolicName = name;
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.DataType = DataTypeIds.Boolean;
            variable.ValueRank = ValueRanks.Scalar;
            variable.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.Historizing = false;
            variable.Value = (bool)GetNewValue(variable);
            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.UtcNow;

            variable.TrueState.Value = trueState;
            variable.TrueState.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.TrueState.UserAccessLevel = AccessLevels.CurrentReadOrWrite;

            variable.FalseState.Value = falseState;
            variable.FalseState.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.FalseState.UserAccessLevel = AccessLevels.CurrentReadOrWrite;

            if (parent != null)
            {
                parent.AddChild(variable);
            }

            return variable;
        }

        /// <summary>
        /// Creates a new variable.
        /// </summary>
        private DataItemState CreateMultiStateDiscreteItemVariable(NodeState parent, string path, string name, params string[] values)
        {
            MultiStateDiscreteState variable = new MultiStateDiscreteState(parent);

            variable.NodeId = new NodeId(path, NamespaceIndex);
            variable.BrowseName = new QualifiedName(path, NamespaceIndex);
            variable.DisplayName = new LocalizedText("en", name);
            variable.WriteMask = AttributeWriteMask.None;
            variable.UserWriteMask = AttributeWriteMask.None;

            variable.Create(
                SystemContext,
                null,
                variable.BrowseName,
                null,
                true);

            variable.SymbolicName = name;
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.DataType = DataTypeIds.UInt32;
            variable.ValueRank = ValueRanks.Scalar;
            variable.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.Historizing = false;
            variable.Value = (uint)0;
            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.UtcNow;
            variable.OnWriteValue = OnWriteDiscrete;

            LocalizedText[] strings = new LocalizedText[values.Length];

            for (int ii = 0; ii < strings.Length; ii++)
            {
                strings[ii] = values[ii];
            }

            variable.EnumStrings.Value = strings;
            variable.EnumStrings.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.EnumStrings.UserAccessLevel = AccessLevels.CurrentReadOrWrite;

            if (parent != null)
            {
                parent.AddChild(variable);
            }

            return variable;
        }

        /// <summary>
        /// Creates a new UInt32 variable.
        /// </summary>
        private DataItemState CreateMultiStateValueDiscreteItemVariable(NodeState parent, string path, string name, params string[] enumNames)
        {
            return CreateMultiStateValueDiscreteItemVariable(parent, path, name, null, enumNames);
        }

        /// <summary>
        /// Creates a new variable.
        /// </summary>
        private DataItemState CreateMultiStateValueDiscreteItemVariable(NodeState parent, string path, string name, NodeId nodeId, params string[] enumNames)
        {
            MultiStateValueDiscreteState variable = new MultiStateValueDiscreteState(parent);

            variable.NodeId = new NodeId(path, NamespaceIndex);
            variable.BrowseName = new QualifiedName(path, NamespaceIndex);
            variable.DisplayName = new LocalizedText("en", name);
            variable.WriteMask = AttributeWriteMask.None;
            variable.UserWriteMask = AttributeWriteMask.None;

            variable.Create(
                SystemContext,
                null,
                variable.BrowseName,
                null,
                true);

            variable.SymbolicName = name;
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.DataType = (nodeId == null) ? DataTypeIds.UInt32 : nodeId;
            variable.ValueRank = ValueRanks.Scalar;
            variable.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.Historizing = false;
            variable.Value = (uint)0;
            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.UtcNow;
            variable.OnWriteValue = OnWriteValueDiscrete;

            // there are two enumerations for this type:
            // EnumStrings = the string representations for enumerated values
            // ValueAsText = the actual enumerated value

            // set the enumerated strings
            LocalizedText[] strings = new LocalizedText[enumNames.Length];
            for (int ii = 0; ii < strings.Length; ii++)
            {
                strings[ii] = enumNames[ii];
            }

            // set the enumerated values
            EnumValueType[] values = new EnumValueType[enumNames.Length];
            for (int ii = 0; ii < values.Length; ii++)
            {
                values[ii] = new EnumValueType();
                values[ii].Value = ii;
                values[ii].Description = strings[ii];
                values[ii].DisplayName = strings[ii];
            }
            variable.EnumValues.Value = values;
            variable.EnumValues.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.EnumValues.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.ValueAsText.Value = variable.EnumValues.Value[0].DisplayName;

            if (parent != null)
            {
                parent.AddChild(variable);
            }

            return variable;
        }

        private ServiceResult OnWriteDiscrete(
            ISystemContext context,
            NodeState node,
            NumericRange indexRange,
            QualifiedName dataEncoding,
            ref object value,
            ref StatusCode statusCode,
            ref DateTime timestamp)
        {
            MultiStateDiscreteState variable = node as MultiStateDiscreteState;

            // verify data type.
            Opc.Ua.TypeInfo typeInfo = Opc.Ua.TypeInfo.IsInstanceOfDataType(
                value,
                variable.DataType,
                variable.ValueRank,
                context.NamespaceUris,
                context.TypeTable);

            if (typeInfo == null || typeInfo == Opc.Ua.TypeInfo.Unknown)
            {
                return StatusCodes.BadTypeMismatch;
            }

            if (indexRange != NumericRange.Empty)
            {
                return StatusCodes.BadIndexRangeInvalid;
            }

            double number = Convert.ToDouble(value);

            if (number >= variable.EnumStrings.Value.Length | number < 0)
            {
                return StatusCodes.BadOutOfRange;
            }

            return ServiceResult.Good;
        }

        private ServiceResult OnWriteValueDiscrete(
            ISystemContext context,
            NodeState node,
            NumericRange indexRange,
            QualifiedName dataEncoding,
            ref object value,
            ref StatusCode statusCode,
            ref DateTime timestamp)
        {
            MultiStateValueDiscreteState variable = node as MultiStateValueDiscreteState;

            TypeInfo typeInfo = TypeInfo.Construct(value);

            if (variable == null ||
                typeInfo == null ||
                typeInfo == Opc.Ua.TypeInfo.Unknown ||
                !TypeInfo.IsNumericType(typeInfo.BuiltInType))
            {
                return StatusCodes.BadTypeMismatch;
            }

            if (indexRange != NumericRange.Empty)
            {
                return StatusCodes.BadIndexRangeInvalid;
            }

            Int32 number = Convert.ToInt32(value);
            if (number >= variable.EnumValues.Value.Length || number < 0)
            {
                return StatusCodes.BadOutOfRange;
            }

            if (!node.SetChildValue(context, BrowseNames.ValueAsText, variable.EnumValues.Value[number].DisplayName, true))
            {
                return StatusCodes.BadOutOfRange;
            }

            node.ClearChangeMasks(context, true);

            return ServiceResult.Good;
        }

        private ServiceResult OnWriteAnalog(
            ISystemContext context,
            NodeState node,
            NumericRange indexRange,
            QualifiedName dataEncoding,
            ref object value,
            ref StatusCode statusCode,
            ref DateTime timestamp)
        {
            AnalogItemState variable = node as AnalogItemState;

            // verify data type.
            Opc.Ua.TypeInfo typeInfo = Opc.Ua.TypeInfo.IsInstanceOfDataType(
                value,
                variable.DataType,
                variable.ValueRank,
                context.NamespaceUris,
                context.TypeTable);

            if (typeInfo == null || typeInfo == Opc.Ua.TypeInfo.Unknown)
            {
                return StatusCodes.BadTypeMismatch;
            }

            // check index range.
            if (variable.ValueRank >= 0)
            {
                if (indexRange != NumericRange.Empty)
                {
                    object target = variable.Value;
                    ServiceResult result = indexRange.UpdateRange(ref target, value);

                    if (ServiceResult.IsBad(result))
                    {
                        return result;
                    }

                    value = target;
                }
            }

            // check instrument range.
            else
            {
                if (indexRange != NumericRange.Empty)
                {
                    return StatusCodes.BadIndexRangeInvalid;
                }

                double number = Convert.ToDouble(value);

                if (variable.InstrumentRange != null && (number < variable.InstrumentRange.Value.Low || number > variable.InstrumentRange.Value.High))
                {
                    return StatusCodes.BadOutOfRange;
                }
            }

            return ServiceResult.Good;
        }

        private ServiceResult OnWriteAnalogRange(
            ISystemContext context,
            NodeState node,
            NumericRange indexRange,
            QualifiedName dataEncoding,
            ref object value,
            ref StatusCode statusCode,
            ref DateTime timestamp)
        {
            PropertyState<Range> variable = node as PropertyState<Range>;
            ExtensionObject extensionObject = value as ExtensionObject;
            TypeInfo typeInfo = TypeInfo.Construct(value);

            if (variable == null ||
                extensionObject == null ||
                typeInfo == null ||
                typeInfo == Opc.Ua.TypeInfo.Unknown)
            {
                return StatusCodes.BadTypeMismatch;
            }

            Range newRange = extensionObject.Body as Range;
            AnalogItemState parent = variable.Parent as AnalogItemState;
            if (newRange == null ||
                parent == null)
            {
                return StatusCodes.BadTypeMismatch;
            }

            if (indexRange != NumericRange.Empty)
            {
                return StatusCodes.BadIndexRangeInvalid;
            }

            TypeInfo parentTypeInfo = TypeInfo.Construct(parent.Value);
            Range parentRange = GetAnalogRange(parentTypeInfo.BuiltInType);
            if (parentRange.High < newRange.High ||
                parentRange.Low > newRange.Low)
            {
                return StatusCodes.BadOutOfRange;
            }

            value = newRange;

            return ServiceResult.Good;
        }

        /// <summary>
        /// Creates a new variable.
        /// </summary>
        private BaseDataVariableState CreateVariable(NodeState parent, string path, string name, BuiltInType dataType, int valueRank)
        {
            return CreateVariable(parent, path, name, (uint)dataType, valueRank);
        }

        /// <summary>
        /// Creates a new variable.
        /// </summary>
        private BaseDataVariableState CreateVariable(NodeState parent, string path, string name, NodeId dataType, int valueRank)
        {
            BaseDataVariableState variable = new BaseDataVariableState(parent);

            variable.SymbolicName = name;
            variable.ReferenceTypeId = ReferenceTypes.Organizes;
            variable.TypeDefinitionId = VariableTypeIds.BaseDataVariableType;
            variable.NodeId = new NodeId(path, NamespaceIndex);
            variable.BrowseName = new QualifiedName(path, NamespaceIndex);
            variable.DisplayName = new LocalizedText("en", name);
            variable.WriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description;
            variable.UserWriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description;
            variable.DataType = dataType;
            variable.ValueRank = valueRank;
            variable.AccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.UserAccessLevel = AccessLevels.CurrentReadOrWrite;
            variable.Historizing = false;
            variable.Value = GetNewValue(variable);
            variable.StatusCode = StatusCodes.Good;
            variable.Timestamp = DateTime.UtcNow;

            if (valueRank == ValueRanks.OneDimension)
            {
                variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0 });
            }
            else if (valueRank == ValueRanks.TwoDimensions)
            {
                variable.ArrayDimensions = new ReadOnlyList<uint>(new List<uint> { 0, 0 });
            }

            if (parent != null)
            {
                parent.AddChild(variable);
            }

            return variable;
        }

        private object GetNewValue(BaseVariableState variable)
        {
            if (m_generator == null)
            {
                m_generator = new Opc.Ua.Test.DataGenerator(null);
                m_generator.BoundaryValueFrequency = 0;
            }

            object value = null;

            while (value == null)
            {
                value = m_generator.GetRandom(variable.DataType, variable.ValueRank, new uint[] { 10 }, Server.TypeTree);
            }

            return value;
        }

        /// <summary>
        /// Frees any resources allocated for the address space.
        /// </summary>
        public override void DeleteAddressSpace()
        {
            lock (Lock)
            {
                // TBD
            }
        }

        /// <summary>
        /// Returns a unique handle for the node.
        /// </summary>
        protected override NodeHandle GetManagerHandle(ServerSystemContext context, NodeId nodeId, IDictionary<NodeId, NodeState> cache)
        {
            lock (Lock)
            {
                // quickly exclude nodes that are not in the namespace. 
                if (!IsNodeIdInNamespace(nodeId))
                {
                    return null;
                }

                NodeState node = null;

                if (!PredefinedNodes.TryGetValue(nodeId, out node))
                {
                    return null;
                }

                NodeHandle handle = new NodeHandle();

                handle.NodeId = nodeId;
                handle.Node = node;
                handle.Validated = true;

                return handle;
            }
        }

        /// <summary>
        /// Verifies that the specified node exists.
        /// </summary>
        protected override NodeState ValidateNode(
           ServerSystemContext context,
           NodeHandle handle,
           IDictionary<NodeId, NodeState> cache)
        {
            // not valid if no root.
            if (handle == null)
            {
                return null;
            }

            // check if previously validated.
            if (handle.Validated)
            {
                return handle.Node;
            }

            // TBD

            return null;
        }
        #endregion Create Variable and Method

        #endregion INodeManager Members

        #region Overrides
        #endregion

        #region Private Fields
        private ReferenceServerConfiguration m_configuration;
        private Opc.Ua.Test.DataGenerator m_generator;
        private Timer m_simulationTimer;
        private UInt16 m_simulationInterval = 1000;
        private bool m_simulationEnabled = true;
        #endregion
    }

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
            if(encodeable == null || !(encodeable is  TrayRequestInfo))
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
        public string EquipmentID { get; set; }
        public string TrayID { get; set; }
        public string[] CellID { get; set; }
        public string[] CellStatus { get; set;}
        public double[] IR {  get; set; }
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
