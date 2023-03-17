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
using System.Windows.Threading;
using IoT1.Model;
using Microsoft.Maps.MapControl.WPF;

namespace IoT1
{
    /// <summary>
    /// Interaction logic for Location.xaml
    /// </summary>
    public partial class Location : Page
    {
        private PacketModel packets;
        public Location(PacketModel model)
        {
            InitializeComponent();

            this.packets = model;


            
        }



    }
}
