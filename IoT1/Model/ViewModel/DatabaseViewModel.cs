using IoT1.Model.Command;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IoT1.Model.ViewModel
{
    public class DatabaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public IMongoCollection<PacketStreamModel> PacketStreamCollection;

        private Dictionary<string, int> sourceIds = new Dictionary<string, int>();


        public DatabaseViewModel(string databaseString, string period)
        {
            int _period;
            if (string.IsNullOrEmpty(period))
                _period = 48;
            else
                _period = int.Parse(period);

            DatabaseString = databaseString;
            sourceIds.Add("Heart Rate", 2);
            sourceIds.Add("Temperature", 0);
            sourceIds.Add("CO2", 1);
            sourceIds.Add("Blood Oxygen Level", 3);
            sourceIds.Add("GPS", 4);

            connectDatabase = new Database(this);

            getAllPackets = new GetPackets(this, _period);

        }

        public string DatabaseString
        {
            get;
            set;
        }



        //Notifies the UI
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ICommand connectDatabase;

        public ICommand ConnectDatabase
        {
            get
            {
                return connectDatabase;
            }
        }

        private ICommand getAllPackets;
        public ICommand GetAllPackets
        {
            get
            {
                return getAllPackets;
            }
        }


        public Dictionary<string, int> SourceIds { get => sourceIds; set => sourceIds = value; }
    }
}
