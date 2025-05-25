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
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Prog_korny_wpf_beadando
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string connectionString = "server=localhost;user id=root;password=;database=könyvtár;SslMode=none;";

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Kérlek tölts ki minden mezőt!");
                return;
            }

            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT id FROM felhasználók WHERE felhasználónév = @username AND jelszó = @password";
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        int userId = reader.GetInt32("id");

                        MainWindow main = new MainWindow(userId); // csak akkor fog működni, ha MainWindow fogadja
                        main.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Hibás felhasználónév vagy jelszó!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba a bejelentkezés során: " + ex.Message);
                }
            }
        }

        private void OpenRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow reg = new RegisterWindow();
            reg.ShowDialog();
        }

    }
}
