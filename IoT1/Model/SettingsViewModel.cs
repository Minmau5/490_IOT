using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IoT1
{
   


    public class SettingsViewModel : INotifyPropertyChanged
    {
        private int _pollingFrequency;
        private string _gRPCIpAddress;
        private string _cameraApiServer;
        private string _databaseConnectionString;
        private int _previousPollingFrequency;
        private string _previousGRPCIpAddress;
        private string _previousCameraApiServer;
        private string _previousDatabaseConnectionString;

        public int PreviousPollingFrequency => _previousPollingFrequency;
        public string PreviousGRPCIpAddress => _previousGRPCIpAddress;
        public string PreviousCameraApiServer => _previousCameraApiServer;
        public string PreviousDatabaseConnectionString => _previousDatabaseConnectionString;

        private ICommand _saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new RelayCommand(param => SaveSettings(), param => CanSaveSettings());
                return _saveCommand;
            }
        }

        private const string SettingsFileName = "settings.json";
        public SettingsViewModel()
        {
            LoadSettings();
        }
        private void LoadSettings()
        {
            if (File.Exists(SettingsFileName))
            {
                var json = File.ReadAllText(SettingsFileName);
                var settings = JsonConvert.DeserializeObject<SettingsModel>(json);

                _previousPollingFrequency = settings.PollingFrequency;
                _previousGRPCIpAddress = settings.GRPCIpAddress;
                _previousCameraApiServer = settings.CameraApiServer;
                _previousDatabaseConnectionString = settings.DatabaseConnectionString;
            }

            PollingFrequency = PreviousPollingFrequency;
            GRPCIpAddress = PreviousGRPCIpAddress;
            CameraApiServer = PreviousCameraApiServer;
            DatabaseConnectionString = PreviousDatabaseConnectionString;
        }

        private void SaveSettings()
        {
            var settings = new SettingsModel
            {
                PollingFrequency = PollingFrequency,
                GRPCIpAddress = GRPCIpAddress,
                CameraApiServer = CameraApiServer,
                DatabaseConnectionString = DatabaseConnectionString
            };

            var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
            File.WriteAllText(SettingsFileName, json);

            _previousPollingFrequency = PollingFrequency;
            _previousGRPCIpAddress = GRPCIpAddress;
            _previousCameraApiServer = CameraApiServer;
            _previousDatabaseConnectionString = DatabaseConnectionString;

            OnPropertyChanged(nameof(PreviousPollingFrequency));
            OnPropertyChanged(nameof(PreviousGRPCIpAddress));
            OnPropertyChanged(nameof(PreviousCameraApiServer));
            OnPropertyChanged(nameof(PreviousDatabaseConnectionString));
        }


        /*
        private void LoadSettings()
        {
            // Load previous values from storage (e.g., file, database, etc.)
            _previousPollingFrequency = 30; // Example value
            _previousGRPCIpAddress = "192.168.1.100"; // Example value
            _previousCameraApiServer = "192.168.1.200"; // Example value
            _previousDatabaseConnectionString = "Server=myServer;Database=myDatabase;User Id=myUsername;Password=myPassword;"; // Example value

            PollingFrequency = PreviousPollingFrequency;
            GRPCIpAddress = PreviousGRPCIpAddress;
            CameraApiServer = PreviousCameraApiServer;
            DatabaseConnectionString = PreviousDatabaseConnectionString;
        }
        */

        private bool CanSaveSettings()
        {
            return true; // You can implement validation logic here if needed
        }

        /*
        private void SaveSettings()
        {
            // Save the current values to storage (e.g., file, database, etc.)

            _previousPollingFrequency = PollingFrequency;
            _previousGRPCIpAddress = GRPCIpAddress;
            _previousCameraApiServer = CameraApiServer;
            _previousDatabaseConnectionString = DatabaseConnectionString;

            OnPropertyChanged(nameof(PreviousPollingFrequency));
            OnPropertyChanged(nameof(PreviousGRPCIpAddress));
            OnPropertyChanged(nameof(PreviousCameraApiServer));
            OnPropertyChanged(nameof(PreviousDatabaseConnectionString));
        }
        */

        public int PollingFrequency
        {
            get => _pollingFrequency;
            set
            {
                _pollingFrequency = value;
                OnPropertyChanged(nameof(PollingFrequency));
            }
        }

        public string GRPCIpAddress
        {
            get => _gRPCIpAddress;
            set
            {
                _gRPCIpAddress = value;
                OnPropertyChanged(nameof(GRPCIpAddress));
            }
        }

        public string CameraApiServer
        {
            get => _cameraApiServer;
            set
            {
                _cameraApiServer = value;
                OnPropertyChanged(nameof(CameraApiServer));
            }
        }

        public string DatabaseConnectionString
        {
            get => _databaseConnectionString;
            set
            {
                _databaseConnectionString = value;
                OnPropertyChanged(nameof(DatabaseConnectionString));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
