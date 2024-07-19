using Opc.Ua;
using Opc.Ua.Configuration;
using Opc.Ua.Server;
using Opc.Ua.Export;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OPCUASERVER;

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
            //connection = await Task.FromResult<bool>(StartMES().Result);
            if (connection == true)
            {

            }
        }
        public static async Task<bool> StartMES()
        {
            ApplicationInstance application = new ApplicationInstance();
            application.ApplicationName = "UA Sample Server";
            application.ApplicationType = ApplicationType.Server;
            application.ConfigSectionName = "IROCV";

            try
            {
                application.LoadApplicationConfiguration(false).Wait();

                // check the application certificate.
                bool certOK = application.CheckApplicationInstanceCertificate(false, 0).Result;
                if (!certOK)
                {
                    throw new Exception("Application instance certificate invalid!");
                }

                // start the server.
                application.Start(new SharpNodeSettingsServer()).Wait();

                connection = true;
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return false;
        }
        public static async Task<bool> ConnectMESAsync()
        {
            try
            {
                var applicationName = "IROCV";
                var configSectionName = "IROCV";

                bool autoAccept = false;
                string password = null;

                ApplicationInstance application = new ApplicationInstance();
                application.ApplicationType = ApplicationType.Server;
                application.ConfigSectionName = configSectionName;

                ApplicationConfiguration config = application.LoadApplicationConfiguration(false).Result;
                // Modify the server configuration to change the port
                var endpointConfig = config.ServerConfiguration.BaseAddresses[1];
                UriBuilder uriBuilder = new UriBuilder(endpointConfig);
                uriBuilder.Host = "herald";
                uriBuilder.Port = 4841; // Set the new port number here
                config.ServerConfiguration.BaseAddresses[1] = uriBuilder.ToString();
                _sIpaddress = uriBuilder.ToString();

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
