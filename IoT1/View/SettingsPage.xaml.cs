using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {

        /*
         * private int _pollingFrequency;
        private string _gRpcIPAddress;
        private string _cameraApiServer;
        private string _databaseConnectionString;
        */

        private readonly SettingsViewModel _viewModel;
        public SettingsPage(SettingsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                textBox.GotFocus -= TextBox_GotFocus;
                string propertyName = "";
                if (textBox.Name == "PollingFreqTextBox") propertyName = "PollingFrequency";
                if (textBox.Name == "GRPCIpAddressTextBox") propertyName = "GRPCIpAddress";
                if (textBox.Name == "CameraApiServerTextBox") propertyName = "CameraApiServer";
                if (textBox.Name == "DatabaseConnectionStringTextBox") propertyName = "DatabaseConnectionString";
                if (textBox.Name == "DatabasePeriod") propertyName = "DatabasePeriod";

                if (!string.IsNullOrEmpty(propertyName))
                {
                    Binding binding = new Binding(propertyName)
                    {
                        Source = _viewModel,
                        Mode = BindingMode.TwoWay,
                        UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    };
                    BindingOperations.SetBinding(textBox, TextBox.TextProperty, binding);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) //check NET connection
            {
                string localIP;
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    localIP = endPoint.Address.ToString();
                }

                GRPCIpAddressTextBox.Text = localIP;
                
            }
            else
            {
                MessageBox.Show("Unable to find your local address", "Settings", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
