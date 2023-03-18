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
        public SettingsPage()
        {
            InitializeComponent();
            _viewModel = new SettingsViewModel();
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




    }
}
