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

namespace Inventory
{

    public partial class LogIn : Window
    {
        public LogIn()
        {
            InitializeComponent();
        }

        private void closeApp(object sender, MouseButtonEventArgs e)
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

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            DragMove();
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            txtUsername.Text = "admin";
            txtPassword.Password = "1234";
            if (txtUsername.Text == "admin" && txtPassword.Password == "1234")
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
