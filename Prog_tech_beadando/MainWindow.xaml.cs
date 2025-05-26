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
using MySql.Data.MySqlClient;
using System.Data;
using Prog_korny_wpf_beadando;


namespace Prog_korny_wpf_beadando
{
    
    public partial class MainWindow : Window
    {
        private int loggedInUserId;
        private string connectionString = "server=localhost;user id=root;password=;database=könyvtár;SslMode=none;";
        

        public MainWindow(int userId)
        {
            InitializeComponent();
            loggedInUserId = userId;
            LoadBooks();

        }

        private void AddBookButton_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addWindow = new AddBookWindow(loggedInUserId);
            addWindow.ShowDialog(); // várakozik, amíg bezárják
            LoadBooks(); // újratölti a könyvlistát a mentés után
        }


        private void LoadBooks()
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"
                SELECT 
                    könyvek.id AS ID,
                    title AS Cím,
                    author AS Szerző,
                    book_condition AS Állapot,
                    description AS Leírás,
                    image_path,
                    felhasználók.felhasználónév AS Tulajdonos
                FROM könyvek
                JOIN felhasználók ON könyvek.user_id = felhasználók.id";

                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    BooksGrid.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hiba: " + ex.Message);
                }
            }
            
            if (!BooksGrid.Columns.OfType<DataGridTemplateColumn>().Any(col => col.Header.ToString() == "Borítókép"))
            {
                var imageColumn = new DataGridTemplateColumn
                {
                    Header = "Borítókép",
                    CellTemplate = (DataTemplate)this.Resources["ImageCellTemplate"]
                };

                BooksGrid.Columns.Add(imageColumn);
            }



        }




        private void SendExchangeRequest_Click(object sender, RoutedEventArgs e)
        {
            if (BooksGrid.SelectedItem == null)
            {
                MessageBox.Show("Válassz ki egy könyvet a listából.");
                return;
            }

            DataRowView selectedRow = (DataRowView)BooksGrid.SelectedItem;
            int selectedBookId = Convert.ToInt32(selectedRow["ID"]);
            string selectedTitle = selectedRow["Cím"].ToString();

            ExchangeRequestWindow window = new ExchangeRequestWindow(selectedBookId, selectedTitle);
            window.ShowDialog();
        }


        private void OpenRequests_Click(object sender, RoutedEventArgs e)
        {
            IncomingRequestsWindow win = new IncomingRequestsWindow();
            win.ShowDialog();
        }


        private void PrototypeButton_Click(object sender, RoutedEventArgs e)
        {
            Book prototype = new Book
            {
                Title = "Prototípus könyv",
                Author = "Ismeretlen",
                Condition = "Új",
                Description = "Ez egy minta könyv.",
                ImagePath = "img/default.jpg",
                UserId = loggedInUserId
            };

            Book ujKonyv = prototype.Clone();
            ujKonyv.Title = "Másolt könyv – új cím";
            ujKonyv.UserId = loggedInUserId;

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = @"INSERT INTO könyvek (title, author, book_condition, description, image_path, user_id)
                             VALUES (@title, @auth, @cond, @desc, @img, @user)";

                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@title", ujKonyv.Title);
                    cmd.Parameters.AddWithValue("@auth", ujKonyv.Author);
                    cmd.Parameters.AddWithValue("@cond", ujKonyv.Condition);
                    cmd.Parameters.AddWithValue("@desc", ujKonyv.Description);
                    cmd.Parameters.AddWithValue("@img", ujKonyv.ImagePath);
                    cmd.Parameters.AddWithValue("@user", ujKonyv.UserId);

                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Könyv (Prototype alapján) sikeresen mentve!");
                    LoadBooks();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Adatbázis hiba: " + ex.Message);
                }
            }
        }


    }
}

