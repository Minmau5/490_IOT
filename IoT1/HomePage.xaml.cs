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

namespace IoT1
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            heart_rate.Text = "70 BPM";
            co_2.Text = "419.03 ppm";
            temp.Text = "28 C";
            oxygen.Text = "85 mm";

            // Subscribe to the TextChanged event of the heart rate TextBox
            heart_rate.TextChanged += HeartRate_TextChanged;
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
