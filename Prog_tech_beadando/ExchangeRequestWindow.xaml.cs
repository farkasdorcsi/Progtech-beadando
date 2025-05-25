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
    /// Interaction logic for ExchangeRequestWindow.xaml
    /// </summary>
    public partial class ExchangeRequestWindow : Window
    {

        private string connectionString = "server=localhost;user id=root;password=;database=könyvtár;SslMode=none;";
        private int targetBookId;
        private int loggedInUserId = 1; // TESZTHEZ: legyen 1-es felhasználó

        public ExchangeRequestWindow(int selectedBookId, string selectedBookTitle)
        {
            InitializeComponent();
            targetBookId = selectedBookId;
            SelectedBookText.Text = $"Ajánlat erre a könyvre: {selectedBookTitle}";
            LoadUserBooks();
        }

        private void LoadUserBooks()
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "SELECT id, title FROM könyvek WHERE user_id = @userId";
                MySqlCommand cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", loggedInUserId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                OfferBookComboBox.ItemsSource = dt.DefaultView;
                OfferBookComboBox.DisplayMemberPath = "title";
                OfferBookComboBox.SelectedValuePath = "id";
            }
            catch(Exception ex)
            {
                MessageBox.Show("Hiba a könyvek betöltésekor: " + ex.Message);
            }
            finally
            {
                conn.Dispose();
            }
        }

        private void SendRequest_Click(object sender, RoutedEventArgs e)
        {
            if (OfferBookComboBox.SelectedValue == null)
            {
                MessageBox.Show("Válassz egy könyvet az ajánlathoz!");
                return;
            }

            int offeredBookId = Convert.ToInt32(OfferBookComboBox.SelectedValue);

            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                string insert = "INSERT INTO csere_kérések (from_user_id, to_user_id, book_id, status) " +
                                "VALUES (@fromUser, (SELECT user_id FROM könyvek WHERE id = @targetBook), @offeredBook, 'Pending')";
                MySqlCommand cmd = new MySqlCommand(insert, conn);
                cmd.Parameters.AddWithValue("@fromUser", loggedInUserId);
                cmd.Parameters.AddWithValue("@targetBook", targetBookId);
                cmd.Parameters.AddWithValue("@offeredBook", offeredBookId);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Csereajánlat elküldve!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba az ajánlat küldésekor: " + ex.Message);
            }
            finally
            {
                conn.Dispose();
            }
        }





    }
}
