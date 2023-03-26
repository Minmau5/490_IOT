using System;
using System.Windows;
using System.ComponentModel;
using IoT1.Model;
using System.Data.SqlClient;

namespace IoT1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotificationsViewModel _viewModel;
        private ConnectViewModel connectCtx;
        private PacketModel packets;
        private readonly SqlConnection connection;
        private SettingsViewModel settingsViewModel;
        public MainWindow(SqlConnection connection)
        {
            InitializeComponent();
            settingsViewModel = new SettingsViewModel();

            _viewModel = new NotificationsViewModel();
            DataContext = _viewModel;
            packets = new PacketModel();
            connectCtx = new ConnectViewModel(packets, settingsViewModel.PreviousGRPCIpAddress);
            redhat_id.Text = "Redhat Agent #12345";
            connectCtx.PropertyChanged += ConnectCtx_PropertyChanged;
            this.connection = connection;

        }

        /*
        // Overloaded constructor to accept the username and then display it at the top of the page
        
        public MainWindow() : this(string.Empty)
        {
        }

        // Overloaded constructor to accept the username
        public MainWindow(string username)
        {
            InitializeComponent();
            redhat_id.Text = username;

            _viewModel = new NotificationsViewModel();
            DataContext = _viewModel;
            packets = new PacketModel();
            connectCtx = new ConnectViewModel(packets);
        }
        */

        private void ConnectCtx_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "conn" && connectCtx.client != null)
            {
                _viewModel.AddNotification(new Notification
                {
                    Message = String.Format("Connection established with endpoint {0} ", connectCtx.IpAddress),
                    Timestamp = DateTime.Now
                });
            }

            else if (e.PropertyName == "stop" && connectCtx.client == null)
            {
                _viewModel.AddNotification(new Notification
                {
                    Message = String.Format("Connection has stopped"),
                    Timestamp = DateTime.Now
                });
            }

            else if (e.PropertyName == "start" && connectCtx.client != null)
            {
                _viewModel.AddNotification(new Notification
                {
                    Message = String.Format("Endpoint streaming started"),
                    Timestamp = DateTime.Now
                });
            }
        }

        private void Button_Click_Dashboard(object sender, RoutedEventArgs e)
        {

        }

        private void Minimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Maximize_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            connectCtx.IpAddress = settingsViewModel.PreviousGRPCIpAddress;
            MainFrame1.Content = new HomePage(connectCtx);
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame1.Content = new UserPage(connection);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame1.Content = new SettingsPage(settingsViewModel);
        }

        private void AddNotification_Click(object sender, RoutedEventArgs e)
        {
            var newNotification = new Notification { Message = "New notification received yay!", Timestamp = DateTime.Now };
            (DataContext as NotificationsViewModel).AddNotification(newNotification);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame1.Content = new Mission(settingsViewModel.DatabaseConnectionString, settingsViewModel.DatabasePeriod);
        }

        private void Grid_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
