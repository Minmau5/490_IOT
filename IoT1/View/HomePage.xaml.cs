using IoT1.Model;
using Microsoft.Maps.MapControl.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace IoT1
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage(ConnectViewModel ctx)
        {
            InitializeComponent();

            DataContext = ctx;

            
            ctx.packetModel.PropertyChanged += new PropertyChangedEventHandler((x, y) =>
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {

                    var pack = y.PropertyName.Split(',');

                    switch ((int)x)
                    {
                        case 0:
                            heart_rate.Text = String.Format("{0:0.## BPM}", float.Parse(pack[0]));
                            break;
                        case 1:
                            co_2.Text = String.Format("{0:0.## ppm}", float.Parse(pack[0]));
                            break;
                        case 2:
                            temp.Text = String.Format("{0:0.## C}", float.Parse(pack[0]));
                            break;
                        case 4:
                            oxygen.Text = String.Format("{0:0.## mm}", float.Parse(pack[0]));
                            break;
                    }
                }));
                
            });

            ctx.packetModel.PropertyChanged += Packets_PropertyChanged;


            //heart_rate.Text = packet.GetPackets(0).Data.ToStringUtf8();
            //co_2.Text = "419.03 ppm";
            //temp.Text = "28 C";
            //oxygen.Text = "85 mm";

            // Subscribe to the TextChanged event of the heart rate TextBox
            heart_rate.TextChanged += HeartRate_TextChanged;
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

                        
                        Pushpin pushpin = new Pushpin
                        {
                            Background = Brushes.Black
                        };

                        pushpin.Background = Brushes.Black;
                        pushpin.Content = sender;

                        pushpin.Location = location;
                        TestLocation.Children.Add(pushpin);
                    }
                }
            }));
        }

        private void HeartRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Parse the heart rate value
            if (int.TryParse(heart_rate.Text.Replace("BPM", "").Trim(), out int heartRate))
            {
                // Check if heart rate is below 60 or above 140
                if (heartRate < 60 || heartRate > 140)
                {
                    // Set text color to red
                    heart_rate.Foreground = Brushes.Red;
                }
                else
                {
                    // Set text color to default
                    heart_rate.Foreground = Brushes.Black;
                }
            }
        }





    }
}
