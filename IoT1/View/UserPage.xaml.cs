using IoT1.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        

        public UserPage()
        {
            InitializeComponent();
            List<User> users = GetUsersFromDatabase();
            UserGrid.ItemsSource = users;
        }
        private List<User> GetUsersFromDatabase()
        {
            List<User> users = new List<User>();
            try
            {
                string connectionString = @"Data Source=(localdb)\Local; Database = LoginDB;Integrated Security = True;";
                string sql = "SELECT * FROM Userspage";

                using (SqlConnection connection = new SqlConnection("Server=localhost;Database=master;Trusted_Connection=True;"))
                {
                    SqlCommand command = new SqlCommand(sql, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        User user = new User();
                        user.FirstName = reader["FirstName"].ToString();
                        user.LastName = reader["LastName"].ToString();
                        user.AgentId = (int)reader["AgentId"];
                        user.StationNumber = (int)reader["StationNumber"];
                        user.IsActiveInMission = (bool)reader["IsActive"];
                        users.Add(user);
                    }
                }
            }catch(Exception e)
            {
                _ = MessageBox.Show(e.Message.ToString(), "Error connecting to database", MessageBoxButton.OK, MessageBoxImage.Error);
            }


            return users;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
             AddUserPage addUserPage= new AddUserPage();
            this.NavigationService.Navigate(addUserPage);
        }

        private void UserGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserGrid.SelectedItem != null)
            {
                    User selectedUser = (User)UserGrid.SelectedItem;

            }
        }

    }

}
