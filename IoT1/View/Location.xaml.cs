using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using IoT1.Model;
using Microsoft.Maps.MapControl.WPF;

namespace IoT1
{
    /// <summary>
    /// Interaction logic for Location.xaml
    /// </summary>
    public partial class Location : Page
    {
        private PacketModel packets;
        public Location(PacketModel model)
        {
            InitializeComponent();

            this.packets = model;

            this.packets.PropertyChanged += Packets_PropertyChanged;

            
        }

        private void Packets_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                if ((int)sender == 4)
                {
                    var coords = e.PropertyName.Split(',');

                    if (coords.Length != 2)
                        return;

                    if (double.TryParse(coords[0], out double latitude) && double.TryParse(coords[1], out double longitude))
                    {
                        Microsoft.Maps.MapControl.WPF.Location location = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
                        TestLocation.Center = location;
                        TestLocation.ZoomLevel = 18;

                        Pushpin pushpin = new Pushpin();
                        pushpin.Location = location;
                        TestLocation.Children.Add(pushpin);
                    }
                }
            }));
        }

    }
}
