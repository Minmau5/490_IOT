using GrpcClient;
using IOT.Monitoring;
using NSwag.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace IoT1.Model
{
    public class ConnectViewModel : INotifyPropertyChanged
    {
        public Client client = null;

        public PacketModel packetModel;

        public ConnectViewModel(PacketModel packetModel, string IpAddress)
        {
            this.IpAddress = IpAddress;
            _connectTogRPC = new Connection(this);
            _StartStopComm = new StreamPackets(this, packetModel);
            _StopEndpoint = new StopConnection(this);
            this.packetModel = packetModel;
            this.packetModel.PropertyChanged += PacketModel_PropertyChanged;
        }


        private void PacketModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //TODO
            //NotifyPropertyChanged("HEART");
        }

        public string HEART => packetModel.HEART;
        public string CO2 => packetModel.CO2;
        public string OXYGEN => packetModel.OXYGEN;
        public string TEMP => packetModel.TEMP;

        ~ConnectViewModel()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Calls any attached event handler and the respective UI component to update
        /// Note that the porpertyName is used to define what is the component name the change was made
        /// </summary>
        /// <param name="propertyName"></param>
        public void NotifyPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        

        public string IpAddress
        {
            get;
            set;
        }
        
        private ICommand _connectTogRPC;

        public ICommand ConnectTogRPC
        {
            get
            {
                return _connectTogRPC;
            }
        }

        private ICommand _StartStopComm;

        public ICommand StartStopComm
        {
            get
            {
                return _StartStopComm;
            }
        }

        private ICommand _StopEndpoint;

        public ICommand StopEndpoint
        {
            get
            {
                return _StopEndpoint;
            }
        }




    }
}
