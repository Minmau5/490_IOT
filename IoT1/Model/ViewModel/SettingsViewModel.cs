using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IoT1
{
   


    public class SettingsViewModel : INotifyPropertyChanged
    {
        private int _pollingFrequency;
        private string _gRPCIpAddress;
        private string _cameraApiServer;
        private string _databaseConnectionString;
        private string _databasePeriod;
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
                DatabasePeriod = settings.DatabasePeriod;
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
                DatabaseConnectionString = DatabaseConnectionString,
                DatabasePeriod = DatabasePeriod
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
            OnPropertyChanged(nameof(DatabasePeriod));


            MessageBox.Show("Settings have been saved", "Settings", MessageBoxButton.OK, MessageBoxImage.Information);
        }



        private bool CanSaveSettings()
        {
            return true; // You can implement validation logic here if needed
        }



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

        public string DatabasePeriod
        {
            get => _databasePeriod;
            set
            {
                _databasePeriod = value;
                OnPropertyChanged(nameof(DatabasePeriod));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
