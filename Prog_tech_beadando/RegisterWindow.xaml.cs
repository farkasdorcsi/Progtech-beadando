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
    
    public partial class RegisterWindow : Window
    {

        private string connectionString = "server=localhost;user id=root;password=;database=könyvtár;SslMode=none;";

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            string email = EmailBox.Text.Trim();


            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Kérlek tölts ki minden mezőt!");
                return;
            }

            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Ellenőrzés: van-e már ilyen felhasználónév
                    string checkQuery = "SELECT COUNT(*) FROM felhasználók WHERE felhasználónév = @username";
                    var checkCmd = new MySqlCommand(checkQuery, conn);
                    checkCmd.Parameters.AddWithValue("@username", username);
                    int exists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (exists > 0)
                    {
                        MessageBox.Show("Ez a felhasználónév már foglalt!");
                        return;
                    }

                    // Új felhasználó mentése
                    string insertQuery = "INSERT INTO felhasználók (felhasználónév, jelszó, email) VALUES (@username, @password, @email)";
                    var insertCmd = new MySqlCommand(insertQuery, conn);
                    insertCmd.Parameters.AddWithValue("@username", username);
                    insertCmd.Parameters.AddWithValue("@password", password);
                    insertCmd.Parameters.AddWithValue("@email", email);
                    insertCmd.ExecuteNonQuery();

                    MessageBox.Show("Sikeres regisztráció!");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba a regisztráció során: " + ex.Message);
                }
            }
        }

    }
}
