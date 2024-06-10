using Opc.Ua;
using Opc.Ua.Configuration;
using OPCUASERVER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DHS.EQUIPMENT
{
    class MesServer
    {
        public static bool connection = false;
        private static string _sIpaddress;
        public bool isRead = false;

        public static string IPADDRESS { get => _sIpaddress; set => _sIpaddress = value; }

        public MesServer()
        {
            connection = false;
            //MesServerStartAsync();
        }

        public async Task MesServerStartAsync()
        {
            connection = await Task.FromResult<bool>(ConnectMESAsync().Result);
            if(connection == true)
            {

            }
        }

        public static async Task<bool> ConnectMESAsync()
        {
            try
            {
                var applicationName = "IROCV.OPCUA.SERVER";
                var configSectionName = "IROCV";

                bool autoAccept = false;
                string password = null;

                ApplicationInstance application = new ApplicationInstance();
                application.ApplicationType = ApplicationType.Server;
                application.ConfigSectionName = configSectionName;

                ApplicationConfiguration config = application.LoadApplicationConfiguration(false).Result;
                _sIpaddress = config.ServerConfiguration.BaseAddresses[1];

                bool certOk = application.CheckApplicationInstanceCertificate(false, 0).Result;
                if (!certOk)
                {
                    Console.WriteLine("Application instance certificate invalid!");
                }

                //* ReferenceNodeManager.cs -> CreateAddressSpace()로 넘어감
                application.Start(new SharpNodeSettingsServer()).Wait();

                Console.WriteLine("Start the server...");

                connection = true;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return false;
        }

        
    }
}
