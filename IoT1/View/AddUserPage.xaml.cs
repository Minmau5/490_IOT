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

namespace IoT1.View
{
    /// <summary>
    /// Interaction logic for AddUserPage.xaml
    /// </summary>
    public partial class AddUserPage : Page
    {
        public AddUserPage()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            int agentId = int.Parse(AgentIdTextBox.Text);
            int stationNumber = int.Parse(StationNumberTextBox.Text);
            bool isActiveInMission = ActiveInMissionCheckBox.IsChecked ?? false;

            User newUser = new User
            {
                FirstName = firstName,
                LastName = lastName,
                AgentId = agentId,
                StationNumber = stationNumber,
                IsActiveInMission = isActiveInMission
            };

            using (SqlConnection connection = new SqlConnection(@"Data Source=(localdb)\Local; Database = LoginDB;Integrated Security = True;"))
            {
                SqlCommand command = new SqlCommand("INSERT INTO Userspage (FirstName, LastName, AgentId, StationNumber, IsActive) " +
                                                     "VALUES (@FirstName, @LastName, @AgentId, @StationNumber, @IsActiveInMission)", connection);
                command.Parameters.AddWithValue("@FirstName", newUser.FirstName);
                command.Parameters.AddWithValue("@LastName", newUser.LastName);
                command.Parameters.AddWithValue("@AgentId", newUser.AgentId);
                command.Parameters.AddWithValue("@StationNumber", newUser.StationNumber);
                command.Parameters.AddWithValue("@IsActiveInMission", newUser.IsActiveInMission);

                connection.Open();
                int result = command.ExecuteNonQuery();
                if (result == 1)
                {
                    MessageBox.Show("User added successfully!");
                }
            }

        }
        private void ActiveInMissionCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            ActiveInMissionCheckBox.Background = Brushes.Green;
        }

        private void ActiveInMissionCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            ActiveInMissionCheckBox.Background = Brushes.Red;
        }
    }
}
