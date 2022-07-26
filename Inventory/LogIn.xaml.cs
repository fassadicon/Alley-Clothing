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
using System.Data.SqlClient;

namespace Inventory
{

    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void CloseApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MinimizeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void MaximizeApp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\source\\repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Accounts WHERE Username = '" + txtUsername.Text + "';", conn);
                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {                      
                        if (reader["Password"].ToString() == txtPassword.Password.ToString())
                        {
                           
                            new MainWindow().Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Incorrect Username/Password");
                            txtUsername.Clear();
                            txtPassword.Clear();
                            txtUsername.Focus();
                        }
                    }
                }
            }
        }
    }
}
