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
    /// Interaction logic for Location.xaml
    /// </summary>
    public partial class Location : Page
    {
        public Location()
        {
            InitializeComponent();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            double latitude, longitude;

            if(double.TryParse(LatitudeTextBox.Text,out latitude) && double.TryParse(LongitudeTextBox.Text,out longitude))
            {
                Location location= new Location(latitude, longitude);
                TestLocation.Center = location;
                TestLocation.ZoomLevel = 15;
            }
        }
    }
}
