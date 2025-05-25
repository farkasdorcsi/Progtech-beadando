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
using System.Data;

namespace Prog_korny_wpf_beadando
{
    /// <summary>
    /// Interaction logic for IncomingRequestsWindow.xaml
    /// </summary>
    public partial class IncomingRequestsWindow : Window
    {

        private string connectionString = "server=localhost;user id=root;password=;database=könyvtár;SslMode=none;";
        private int loggedInUserId = 1; // Teszthez beállított user

        public IncomingRequestsWindow()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = @"
                    SELECT csere_kérések.id, 
                           felhasználók.felhasználónév AS Küldő,
                           k1.title AS FelajánlottKönyv,
                           k2.title AS SajátKönyv,
                           csere_kérések.status
                    FROM csere_kérések
                    JOIN felhasználók ON csere_kérések.from_user_id = felhasználók.id
                    JOIN könyvek k1 ON csere_kérések.book_id = k1.id
                    JOIN könyvek k2 ON k2.user_id = csere_kérések.to_user_id
                    WHERE csere_kérések.to_user_id = @userId AND csere_kérések.status = 'Pending'
                    GROUP BY csere_kérések.id";

                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", loggedInUserId);
                var adapter = new MySqlDataAdapter(cmd);
                var dt = new DataTable();
                adapter.Fill(dt);
                RequestsGrid.ItemsSource = dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba betöltéskor: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatus("Matched");
        }

        private void Decline_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatus("Declined");
        }

        private void ChangeStatus(string newStatus)
        {
            if (RequestsGrid.SelectedItem == null)
            {
                MessageBox.Show("Válassz ki egy ajánlatot!");
                return;
            }

            var row = (DataRowView)RequestsGrid.SelectedItem;
            int requestId = Convert.ToInt32(row["id"]);

            var conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                string update = "UPDATE csere_kérések SET status = @status WHERE id = @id";
                var cmd = new MySqlCommand(update, conn);
                cmd.Parameters.AddWithValue("@status", newStatus);
                cmd.Parameters.AddWithValue("@id", requestId);
                cmd.ExecuteNonQuery();

                MessageBox.Show($"Ajánlat {newStatus} státuszba került.");
                LoadRequests(); // frissít a táblázat
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba mentéskor: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }



    }
}
