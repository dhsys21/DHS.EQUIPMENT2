using Opc.Ua;
using Opc.Ua.Configuration;
using OPCUASERVER;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DHS.EQUIPMENT
{
    class MesServer
    {
        public static bool connection = false;
        public bool isRead = false;

        public MesServer()
        {
            connection = false;
            MesServerStart();
        }

        public void MesServerStart()
        {
            connection = ConnectMESAsync().Result;
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

                bool certOk = application.CheckApplicationInstanceCertificate(false, 0).Result;
                if (!certOk)
                {
                    Console.WriteLine("Application instance certificate invalid!");
                }

                application.Start(new SharpNodeSettingsServer()).Wait();

                Console.WriteLine("Start the server...");

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
