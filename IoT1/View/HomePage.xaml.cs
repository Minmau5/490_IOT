using IOT.Monitoring;
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

                    switch (((Packet)x).Id)
                    {
                        case 0:
                            temp.Text = String.Format("{0:0.## C}", float.Parse(pack[0]));
                            break;
                        case 1:
                            co_2.Text = String.Format("{0:0.## ppm}", float.Parse(pack[0]));
                            break;
                        case 2:
                            heart_rate.Text = String.Format("{0:0.## BPM}", float.Parse(pack[0]));
                            break;
                        case 3:
                            oxygen.Text = String.Format("{0:0.## mm}", float.Parse(pack[0]));
                            break;
                    }
                }));
                
            });

            ctx.packetModel.PropertyChanged += Packets_PropertyChanged;


            //heart_rate.Text = packet.GetPackets(0).Data.ToStringUtf8();
            heart_rate.Text = "69 BPM";
            co_2.Text = "40 ppm";
            temp.Text = "28 C";
            oxygen.Text = "85 mm";

            // Subscribe to the TextChanged event of the heart rate TextBox
            heart_rate.TextChanged += HeartRate_TextChanged;
            co_2.TextChanged += CO_TextChanged;
            oxygen.TextChanged += Oxygen_TextChanged;
            temp.TextChanged += Temp_TextChanged;
        }

        private void Packets_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var packet = (Packet)sender;
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                if (packet.Id == 4 && packet.Type != TYPE.Err)
                {
                    var coords = e.PropertyName.Split(',');

                    if (coords.Length != 2)
                        return;

                    if (double.TryParse(coords[0], out double longitude) && double.TryParse(coords[1], out double latitude))
                    {
                        Microsoft.Maps.MapControl.WPF.Location location = new Microsoft.Maps.MapControl.WPF.Location(latitude, longitude);
                        TestLocation.Center = location;

                        
                        Pushpin pushpin = new Pushpin
                        {
                            Background = Brushes.Black
                        };

                        pushpin.Background = Brushes.Black;
                        pushpin.Content = packet.Id;

                        pushpin.Location = location;
                        TestLocation.Children.Add(pushpin);
                    }
                }
            }));
        }

        //
        //Heart Rate Text Change Method
        private void HeartRate_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Parse the heart rate value
            if (int.TryParse(heart_rate.Text.Replace("BPM", "").Trim(), out int heartRate))
            {
                // Check if heart rate is below 60 or above 140
                if (heartRate < 60 || heartRate > 140)
                {
                    // Set text color to red
                    heart_rate.Foreground = Brushes.White;
                    heart_rate.Background = Brushes.Red;
                }
                else
                {
                    // Set text color to default
                    heart_rate.Foreground = Brushes.White;
                    heart_rate.Background = Brushes.Green;
                }
            }
        }

        //
        //CO Text Change Method
        private void CO_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Parse the co_2 value
            if (int.TryParse(co_2.Text.Replace("ppm", "").Trim(), out int co2))
            {
                // Check if co_2 is above 50
                if (co2 > 50)
                {
                    // Set text color to red
                    co_2.Foreground = Brushes.White;
                    co_2.Background = Brushes.Red;
                }
                else
                {
                    // Set text color to default
                    co_2.Foreground = Brushes.White;
                    co_2.Background = Brushes.Green;
                }
            }
        }

        //
        //Oxygen Text Change Method
        private void Oxygen_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Parse the oxygen value
            if (int.TryParse(oxygen.Text.Replace("mm", "").Trim(), out int oxygenValue))
            {
                // Check if oxygen is below 80 or above 100
                if (oxygenValue < 80 || oxygenValue > 100)
                {
                    // Set text color to red
                    oxygen.Foreground = Brushes.White;
                    oxygen.Background = Brushes.Red;
                }
                else
                {
                    // Set text color to default
                    oxygen.Foreground = Brushes.White;
                    oxygen.Background = Brushes.Green;
                    //oxygen.Foreground = Brushes.Black;
                }
            }
        }


        //
        //Temperature Text Change Method
        private void Temp_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Parse the temperature value
            if (double.TryParse(temp.Text.Replace("C", "").Trim(), out double temperature))
            {
                // Check if temperature is above 65 C
                if (temperature > 65)
                {
                    // Set text color to red
                   temp.Foreground = Brushes.White;
                   temp.Background= Brushes.Red;
                   //temp.Foreground = Brushes.Red;
                }
                else
                {
                    // Set text color to default
                    temp.Foreground = Brushes.White;
                    temp.Background = Brushes.Green;

                    //temp.Foreground = Brushes.Black;
                }
            }
        }




    }
}
