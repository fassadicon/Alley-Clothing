using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for StocksView.xaml
    /// </summary>
    public partial class StocksView : UserControl
    {
        public StocksView()
        {
            InitializeComponent();
            LoadGrid();
        }

        private void ClearData()
        {
            TShirtID.Text = string.Empty;
            TShirtQty.Text = string.Empty;
            TShirtDefect.Text = string.Empty;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        // INVENTORY DATABASE T SHIRT DETAILS TABLE
        public void LoadGrid()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Stocks;", conn);
                    DataTable dt = new DataTable();
                    conn.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                    conn.Close();
                    StocksDetailsGrid.ItemsSource = dt.DefaultView;
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Reading Failed: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // INSERT T SHIRT DETAILS
        private void InsertTShirtDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Stocks (TShirtID, TShirtQty, TShirtDefect) VALUES (@TShirtID, @TShirtQty, @TShirtDefect);", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@TShirtID", TShirtID.Text);
                    cmd.Parameters.AddWithValue("@TShirtQty", TShirtQty.Text);
                    cmd.Parameters.AddWithValue("@TShirtDefect", TShirtDefect.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadGrid();
                    ClearData();

                    MessageBox.Show("Stocks Details Input Successful", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Insertion Failed: \n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Format Exception: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // UPDATE T SHIRT DETAILS
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            int stocks, defect;
            int newStocks, newDefect;

            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT TShirtQty, TShirtDefect FROM Stocks WHERE TShirtID = '" + TShirtID.Text + "'", conn);
                    SqlCommand cmd2 = new SqlCommand("UPDATE Stocks SET TShirtQty = @newStocks, TShirtDefect = @newDefect WHERE TShirtID = '" + TShirtID.Text + "'", conn);
                    conn.Open();

                    // GETTING THE QTY AND DEFECTS ON THE DATABASE
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // DIRETSO NA INT FOR COMPUTATION
                            stocks = Int32.Parse(reader["TShirtQty"].ToString());
                            defect = Int32.Parse(reader["TShirtDefect"].ToString());

                            // COMPUTATION
                            newStocks = stocks + Int32.Parse(TShirtQty.Text);
                            newDefect = defect + Int32.Parse(TShirtDefect.Text);

                            //ADDING NEW VALUES TO DATABASE
                            cmd2.Parameters.AddWithValue("@newStocks", newStocks);
                            cmd2.Parameters.AddWithValue("@newDefect", newDefect);

                        }
                        
                    }

                    cmd2.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Stocks Details Updated Successfully", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);

                    conn.Close();
                    LoadGrid();
                    ClearData();

                    MessageBox.Show("T Shirt Details Update Successful", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Update Failed: \n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Format Exception: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // DELETE STOCKS
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Stocks WHERE TShirtID = " + TShirtID.Text + " ", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadGrid();
                    ClearData();

                    MessageBox.Show("T Shirt Details Deletion Successful", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Deletion Failed: \n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //TSHIRT DETALS PREVIEW
        private void SeePrevButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    // GETTING THE TSHIRT DETAILS
                    SqlCommand cmd = new SqlCommand("SELECT * FROM TShirtDetails INNER JOIN Stocks ON TShirtDetails.TShirtID = Stocks.TShirtID", conn);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            TShirtBrandPreview.Content = reader["TShirtBrand"].ToString();
                            TShirtNamePreview.Content = reader["TShirtName"].ToString();
                            TShirtColorPreview.Content = reader["TShirtColor"].ToString();
                            TShirtSizePreview.Content = reader["TShirtSize"].ToString();
                            TShirtQtyPreview.Content = reader["TShirtQty"].ToString();
                        }
                        
                    }
                    
                    conn.Close();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Preview Failed: \n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException ex)
            {
                MessageBox.Show("Format Exception: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


    }
}
