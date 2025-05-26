using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
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
    /// Interaction logic for GenreListWindow.xaml
    /// </summary>
    public partial class GenreListWindow : Window
    {
        private int loggedInUserId;
        private string connectionString = "server=localhost;user id=root;password=;database=könyvtár;SslMode=none;";

        public GenreListWindow(int userId)
        {
            loggedInUserId = userId;
            InitializeComponent();
        }

        public string SetTextProcessor() {
            TextProcessor process = new TextProcessor();
            process.SetOutputFormat(Strategy.md);
            process.AppendList(new[] {"Romance" , "Fantasy", "Crime" });
            return process.ToString();
            
        }

        private void search_Click(object sender, RoutedEventArgs e)
        {
            Genrefield.Text = SetTextProcessor();
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow addBook = new AddBookWindow(loggedInUserId);
            addBook.Show();
            this.Close();
        }

        }
    }
