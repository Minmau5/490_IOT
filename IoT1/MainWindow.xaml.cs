using System;
using System.Collections.ObjectModel;
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
using GrpcClient;
using System.ComponentModel;
using System.Windows.Threading;
using IOT.Monitoring;
using System.Threading.Channels;
using System.Threading;
using IoT1.Model;
using System.Net.PeerToPeer;

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

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new NotificationsViewModel();
            DataContext = _viewModel;
            packets = new PacketModel();
            connectCtx = new ConnectViewModel(packets);
            redhat_id.Text = "Redhat Agent #12345";
            connectCtx.PropertyChanged += ConnectCtx_PropertyChanged;
           
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
            if (e.PropertyName == "start" && connectCtx.client != null)
            {
                _viewModel.AddNotification(new Notification
                {
                    Message = String.Format("New client has been connected to {0} ", connectCtx.IpAddress),
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
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame1.Content = new LoginPage();
        }
        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame1.Content = new HomePage(connectCtx);
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame1.Content = new UserPage();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame1.Content = new SettingsPage();
        }

        private void AddNotification_Click(object sender, RoutedEventArgs e)
        {
            var newNotification = new Notification { Message = "New notification received yay!", Timestamp = DateTime.Now };
            (DataContext as NotificationsViewModel).AddNotification(newNotification);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainFrame1.Content = new Location(packets);
        }



    }
}
