using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using IoT1.Model.Command;
using IoT1.Model.ViewModel;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Maps.MapControl.WPF;


namespace IoT1
{
    /// <summary>
    /// Interaction logic for Location.xaml
    /// </summary>
    public partial class Mission : Page
    {

        private readonly DatabaseViewModel databaseViewModel;
        public Mission(string databaseString, string period)
        {
            InitializeComponent();

            databaseViewModel = new DatabaseViewModel(databaseString, period);

            DataContext = databaseViewModel;

            (databaseViewModel.GetAllPackets as GetPackets).PropertyChanged += Location_PropertyChanged;

            (databaseViewModel.GetAllPackets as GetPackets).ListOfPackets.CollectionChanged += CollectionChanged;
        }

        
        private void Location_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {


            if(MainChart.SeriesCollection.Where(x => x.Title == sender.ToString()).Count() == 0)
            {
                MainChart.SeriesCollection.Add(
                     new LineSeries
                     {
                     Title = sender.ToString(),
                     Values = new ChartValues<float>(),
                     LineSmoothness = 1
                     }
                    );
            }
            
        }

        
        private void CollectionChanged(object sender, EventArgs e)
        {

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                var packets = (sender as ObservableCollection<PacketStreamModel>);
                if (packets.Count == 0)
                    return;
                var pastEpoch = new DateTime(1970,1,1,0,0,0).Add(new TimeSpan(packets.Last().Timestamp));
                
                MainChart.Labels.Add(pastEpoch.ToString());

                MainChart.SeriesCollection.Where(x => x.Title == packets.Last().GetIotIdString()).First().Values.Add(
                        packets.Last().Data.Value
                    );

            }), DispatcherPriority.Normal);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            MainChart.Labels.Clear();
            MainChart.SeriesCollection.Clear();
        }
    }
}
