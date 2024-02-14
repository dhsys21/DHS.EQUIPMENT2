using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Opc.Ua;
using OpcUaHelper;
using Org.BouncyCastle.Asn1.X509.Qualified;

namespace OPCUACLIENT
{
    public class OPCUACLIENT
    {
        OpcUaClient client = null;
        public OPCUACLIENT()
        {
            client = new OpcUaClient();
            client.UserIdentity = new UserIdentity(new AnonymousIdentityToken());
        }
        public bool Connect(string serverurl)
        {
            bool bConnected = false;
            try
            {
                client.ConnectServer(serverurl);
                bConnected = client.Connected;
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString()); 
            }
            
            return bConnected;
        }
        public void Disconnect()
        {
            client.Disconnect();
        }
        public string ReadString(string nodename)
        {
            string val = string.Empty;
            try
            {
                val = client.ReadNode<string>(nodename);
            }catch(Exception ex) { 
                Console.WriteLine(ex.ToString()); 
            }

            return val;
        }
        public object Read<T>(string nodename)
        {
            object objval = null;
            try
            {
                //if(nodename == "ns=2;s=Equipment/CellID")
                //    objval = client.ReadNode<T>(nodename);
                //else
                    objval = client.ReadNode<T>(nodename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return objval;
        }
        public UInt32[] ReadUInt32Arrs(string nodename)
        {
            try
            {
                var val = client.ReadNode<UInt32[]>(nodename);
                return val;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return null;
        }
        public void Write<T>(string nodename, T value)
        {
            try
            {
                client.WriteNode<T>(nodename, value);
            }
            catch (Exception ex) { 
                Console.WriteLine(ex.ToString()); 
            }
        }
        public void WriteString(string nodename, string value)
        {
            try
            {
                client.WriteNode<string>(nodename, value);
                //client.WriteNode<UInt32>(nodename, Convert.ToUInt32(value));
            }catch(Exception ex) { Console.WriteLine(ex.ToString()) ; }
        }
        public void WriteUInt32Arr(string nodename, UInt32[] values)
        {
            try
            {
                client.WriteNode<UInt32[]>(nodename, values);
                //client.WriteNode<UInt32>(nodename, Convert.ToUInt32(value));
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
        }
    }
}
