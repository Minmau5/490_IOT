using IoT1.Model.ViewModel;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IoT1.Model.Command
{
    public class Database : ICommand
    {

        private static MongoClient mongoClient;


        public DatabaseViewModel ViewModel { get; }

        public event EventHandler CanExecuteChanged;

        public Database(DatabaseViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {
            return ViewModel.PacketStreamCollection == null;
        }

        //Passing paramters to Execute method directly:
        //CommandParameter="{Binding Text,ElementName=InputId}"  in XAML
        public void Execute(object parameter)
        {
            try
            {
                var camelCaseConvention = new ConventionPack { new CamelCaseElementNameConvention() };
                ConventionRegistry.Register("CamelCase", camelCaseConvention, type => true);


                mongoClient = new MongoClient(parameter.ToString());

                //ping database to ensure proper connection
                var db = mongoClient.GetDatabase("iot");
                ViewModel.PacketStreamCollection = db.GetCollection<PacketStreamModel>("packet_stream");
                
            }
            catch (Exception e)
            {
                _ = MessageBox.Show(e.Message, "Database error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                //Motifies back the ConnectDatabase bounded button
                CanExecuteChanged?.Invoke(this, new EventArgs());
                ViewModel.NotifyPropertyChanged(nameof(ViewModel.GetAllPackets));
            }
        }
    }

    public class GetPackets : ICommand, INotifyPropertyChanged
    {
        public event EventHandler CanExecuteChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly DatabaseViewModel viewModel;
        private readonly FilterDefinition<PacketStreamModel> filter;

        public ObservableCollection<PacketStreamModel> ListOfPackets { get; } = new ObservableCollection<PacketStreamModel>();

        private readonly int period;

        public GetPackets(DatabaseViewModel viewModel, int period, FilterDefinition<PacketStreamModel> filter = null)
        {

            this.period = period;

            this.viewModel = viewModel;

            // This is required to allow this ICommand to be notify when any viewModel property is modified or when ViewModel.PropertyChanged has been called
            this.viewModel.PropertyChanged += OnViewModelChanged;
            this.filter = filter;
        }

        private async Task<List<PacketStreamModel>> GetAll(int source)
        {
            var pastEpoch = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0);

            pastEpoch = pastEpoch.Subtract(TimeSpan.FromHours(this.period));

            var filter_1 = Builders<PacketStreamModel>.Filter.Gt(x => x.Timestamp, pastEpoch.Ticks);
            var filter_2 = Builders<PacketStreamModel>.Filter.Eq(x => x.IotId, (long)source);
            return await viewModel.PacketStreamCollection.Find(filter_1&filter_2).ToListAsync();
        }

        private void OnViewModelChanged(object sender, PropertyChangedEventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        public bool CanExecute(object parameter)
        {
            return viewModel.PacketStreamCollection != null;
        }


        public void Execute(object parameter)
        {
            try
            {
                if (parameter == null)
                    throw new Exception("Sensor must be selected");

                ListOfPackets.Clear();
                PropertyChanged?.Invoke(parameter, new PropertyChangedEventArgs(nameof(Execute)));
                _ = Task.Run(async () =>
                {
                    foreach (var res in await GetAll(viewModel.SourceIds[parameter.ToString()]))
                    {
                        ListOfPackets.Add(res);
                    }
                });

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Fetching from database", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
