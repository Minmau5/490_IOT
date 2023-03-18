using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IoT1
{
    /// <summary>
    /// Interaction logic for Splash.xaml
    /// </summary>
    public partial class Splash : Window
    {
        public Splash()
        {
            InitializeComponent();
            loader.ValueChanged += Loader_ValueChanged;
        }

        private void Loader_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            percentage.Text = string.Format("{0:0.##}%", (sender as ProgressBar).Value);
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerAsync();
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            loader.Value = e.ProgressPercentage;
            if(loader.Value == 100)
            {
                MainWindow window = new MainWindow();
                this.Close();
                window.ShowDialog();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for(int i = 0; i<= 100; i+=4)
            {
                ((BackgroundWorker)sender).ReportProgress(i);
                Thread.Sleep(75);
            }
        }
    }
}
