using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT1
{
    public class SettingsModel
    {
        public int PollingFrequency { get; set; }
        public string GRPCIpAddress { get; set; }
        public string CameraApiServer { get; set; }
        public string DatabaseConnectionString { get; set; }

        public SettingsModel()
        {
        }

        public SettingsModel(int pollingFrequency, string gRPCIpAddress, string cameraApiServer, string databaseConnectionString)
        {
            PollingFrequency = pollingFrequency;
            GRPCIpAddress = gRPCIpAddress;
            CameraApiServer = cameraApiServer;
            DatabaseConnectionString = databaseConnectionString;
        }
    }
}

