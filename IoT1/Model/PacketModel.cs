using IOT.Monitoring;
using NSwag.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IoT1.Model
{
    public class PacketModel : INotifyPropertyChanged
    {
        public ObservableDictionary<int, Packet> _packets = new ObservableDictionary<int, Packet>();
        
        public string HEART
        {
            get
            {
                if (this._packets.Contains(2))
                    return this._packets[2].Data.ToStringUtf8() + " BPM";
                else
                    return "";
            }
        }

        public string CO2
        {
            get
            {
                if (this._packets.Contains(1))
                    return this._packets[1].Data.ToStringUtf8() + " ppm";
                else
                    return "";
            }
        }

        public string OXYGEN
        {
            get
            {
                if (this._packets.Contains(3))
                    return this._packets[3].Data.ToStringUtf8() + " mm";
                else
                    return "";
            }
        }

                public string TEMP
        {
            get
            {
                if (this._packets.Contains(0))
                    return this._packets[0].Data.ToStringUtf8() + " C";
                else
                    return "";
            }
        }


        public PacketModel()
        {
        }

        public Packet GetPackets(int id)
        {
            if (this._packets.ContainsKey(id))
                return this._packets[id];
            return new Packet();
        }

        public void AddPacket(int id, Packet packet)
        {
            this._packets[id] = packet;

            PropertyChanged?.Invoke(packet, new PropertyChangedEventArgs(packet.Data.ToStringUtf8()));
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
