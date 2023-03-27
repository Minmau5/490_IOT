using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
    /// Interaction logic for RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=DEOLIVEK2;Initial Catalog=LoginDB;Integrated Security=True");

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txt_Username.Text.Length == 0 || txt_Password.Password.Length == 0)
            {
                MessageBox.Show("Empty field", "Login/Registration", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                

                SqlCommand cmd = new SqlCommand("INSERT INTO tblUser VALUES (@Username, @Password)", conn);
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@Username", txt_Username.Text);
                cmd.Parameters.AddWithValue("@Password", txt_Password.Password);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Registration Completed", "Login/Registration", MessageBoxButton.OK);
                MainWindow window = new MainWindow(conn, txt_Username.Text);
                Application.Current.MainWindow.Close();
                window.ShowDialog();
            }

            catch (SqlException Ex)
            {

                MessageBox.Show(Ex.Message);

            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
