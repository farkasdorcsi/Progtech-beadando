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
    
    public partial class AddBookWindow : Window
    {
        private int loggedInUserId;
        private string connectionString = "server=localhost;user id=root;password=;database=könyvtár;SslMode=none;";


        public AddBookWindow(int userId)
        {
            InitializeComponent();
            loggedInUserId= userId;
        }


        private void SaveBook_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleBox.Text.Trim();
            string author = AuthorBox.Text.Trim();
            string condition = (ConditionBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string description = DescriptionBox.Text.Trim();
            string imagePath = ImagePathBox.Text.Trim();

            if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author) || condition == null || string.IsNullOrWhiteSpace(description) || string.IsNullOrWhiteSpace(imagePath))
            {
                MessageBox.Show("Kérlek, tölts ki minden mezőt.");
                return;
            }

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO könyvek ( title, author, book_condition, description, image_path user_id) VALUES ( @title, @author, @cond, @desc, @img, @user)";
                    MySqlCommand cmd = new MySqlCommand(query, conn);                    
                    cmd.Parameters.AddWithValue("@title", title);
                    cmd.Parameters.AddWithValue("@author", author);
                    cmd.Parameters.AddWithValue("@cond", condition);
                    cmd.Parameters.AddWithValue("@desc", description);
                    cmd.Parameters.AddWithValue("@img", imagePath);
                    cmd.Parameters.AddWithValue("@user", loggedInUserId);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("A könyv sikeresen hozzá lett adva!");
                    this.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Adatbázis hiba: " + ex.Message);
                }
            }
        }




    }
}
