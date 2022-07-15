using System;
using System.Collections.Generic;
using System.Data;
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

namespace Inventory.MVVM.View
{
    /// <summary>
    /// Interaction logic for LogView.xaml
    /// </summary>
    public partial class LogView : UserControl
    {
        public LogView()
        {
            InitializeComponent();
            LoadGrid();
            StartDate.SelectedDate = DateTime.Today;
            EndDate.SelectedDate = DateTime.Today;
        }
        // LOG DETAILS TABLE
        public void LoadGrid()
        {
            try
            {
                // Labels
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    int count = 0;
                    using (SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM DeliveryDetails WHERE DeliveryType = 'In' AND DateReceived BETWEEN '" + StartDate.Text + "' AND '" + EndDate.Text + "';", conn))
                    {
                        conn.Open();
                        count = (int)cmdCount.ExecuteScalar();
                        DeliveryIn.Content = count.ToString();
                        conn.Close();
                    }

                    count = 0;
                    using (SqlCommand cmdCount = new SqlCommand("SELECT COUNT(*) FROM DeliveryDetails WHERE DeliveryType = 'Out' AND DateReceived BETWEEN '" + StartDate.Text + "' AND '" + EndDate.Text + "';", conn))
                    {
                        conn.Open();
                        count = (int)cmdCount.ExecuteScalar();
                        DeliveryOut.Content = count.ToString();
                        conn.Close();
                    }

                    using (SqlCommand cmdCount = new SqlCommand("SELECT SUM (Quantity) FROM DeliveryDetails WHERE DeliveryType = 'In' AND DateReceived BETWEEN '" + StartDate.Text + "' AND '" + EndDate.Text + "';", conn))
                    {
                        conn.Open();
                        TShirtIn.Content = Convert.ToString(cmdCount.ExecuteScalar());
                        conn.Close();
                    }

                    using (SqlCommand cmdCount = new SqlCommand("SELECT SUM (Quantity) FROM DeliveryDetails WHERE DeliveryType = 'Out' AND DateReceived BETWEEN '" + StartDate.Text + "' AND '" + EndDate.Text + "';", conn))
                    {
                        conn.Open();
                        TShirtOut.Content = Convert.ToString(cmdCount.ExecuteScalar());
                        conn.Close();
                    }
                }

                // Stock Log Table
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Stocks WHERE Date BETWEEN '" + StartDate.Text + "' AND '" + EndDate.Text + "';", conn);
                    DataTable dt = new DataTable();
                    conn.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                    conn.Close();
                    StockSummary.ItemsSource = dt.DefaultView;
                }
                // Delivery Log Table
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM DeliveryDetails WHERE DateReceived BETWEEN '"+ StartDate.Text+ "' AND '" + EndDate.Text + "';", conn);
                    DataTable dt = new DataTable();
                    conn.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                    conn.Close();
                    Deliveries.ItemsSource = dt.DefaultView;
                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Reading Failed: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void StartDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadGrid();
        }

        private void EndDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadGrid();
        }

        private void printBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.IsEnabled = false;
                PrintDialog printDialog = new PrintDialog();                         
                if (printDialog.ShowDialog() == true)
                {
                    printDialog.PrintVisual(PrintArea, "Log");
                }
            }
            finally
            {
                this.IsEnabled = true;    
            }
        }
    }
}
