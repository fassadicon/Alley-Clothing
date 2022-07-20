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
    public partial class StocksView : UserControl
    {
        public StocksView()
        {
            InitializeComponent();
            LoadGrid();
            AutoComplete();
        }

        private void ClearData()
        {
            TShirtID.Text = string.Empty;
            TShirtQty.Text = string.Empty;
            TShirtDefect.Text = string.Empty;
            StockDate.Text = string.Empty;
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        // AUTO SUGGEST
        private void AutoComplete()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\source\\repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
            {
                SqlCommand cmd;
                SqlDataReader sdr;
                string q;

                // List for AutoSuggest
                List<int> StockIDs = new List<int>();
                List<int> TShirtIDs = new List<int>();

                // Reading Database Result per Column
                conn.Open();
                q = "SELECT StockID from Stocks";
                cmd = new SqlCommand(q, conn);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                   StockIDs.Add((int)sdr["StockID"]);
                }
                conn.Close();

                conn.Open();
                q = "SELECT TShirtID from TShirtDetails";
                cmd = new SqlCommand(q, conn);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    TShirtIDs.Add((int)sdr["TShirtID"]);
                }
                conn.Close();

                // Setting Textbox AutoSuggest Sources
                StockID.ItemsSource = StockIDs;
                TShirtID.ItemsSource = TShirtIDs;
            }
        }

        //INVENTORY DATABASE T SHIRT DETAILS TABLE
        public void LoadGrid()
        {
            if (FilterBoxItem.Text == "")
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\source\\repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM Stocks ;", conn);
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
            else
            {

                try
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\source\\repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                    {
                        String queryString = "";

                        if (FilterBoxCateg.Text == "TShirtID")
                        {
                            queryString = "SELECT * FROM Stocks WHERE TShirtID = @FilterItem";
                        }
                        else if (FilterBoxCateg.Text == "TShirtQuantity" || FilterBoxCateg.Text == "TShirtQty")
                        {
                            queryString = "SELECT * FROM Stocks WHERE TShirtQty = @FilterItem";
                        }
                        else if (FilterBoxCateg.Text == "TShirtDefect")
                        {
                            queryString = "SELECT * FROM Stocks WHERE TShirtDefect = @FilterItem";
                        }
                        else if (FilterBoxCateg.Text == "StockID")
                        {
                            queryString = "SELECT * FROM Stocks WHERE StockID = @FilterItem";
                        }



                        SqlCommand cmd = new SqlCommand(queryString, conn);
                        cmd.Parameters.AddWithValue("@FilterCateg", FilterBoxCateg.Text);
                        cmd.Parameters.AddWithValue("@FilterItem", FilterBoxItem.Text);

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
        }

        // INSERT T SHIRT DETAILS
        private void InsertTShirtDetails_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\source\\repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                    {
                        SqlCommand cmd = new SqlCommand("INSERT INTO Stocks (StockID, TShirtID, TShirtQty, TShirtDefect, Date) VALUES (@StockID, @TShirtID, @TShirtQty, @TShirtDefect, @Date);", conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("@StockID", StockID.Text);
                        cmd.Parameters.AddWithValue("@TShirtID", TShirtID.Text);
                        cmd.Parameters.AddWithValue("@TShirtQty", TShirtQty.Text);
                        cmd.Parameters.AddWithValue("@TShirtDefect", TShirtDefect.Text);
                        cmd.Parameters.AddWithValue("@Date", StockDate.Text);
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
        private void UpdateStockDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\source\\repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Stocks set TShirtID = '" + TShirtID.Text + "', TShirtQty = '" + TShirtQty.Text + "', TShirtDefect = '" + TShirtDefect.Text + "', Date = '" + StockDate.Text + "' WHERE StockID = '" + StockID.Text + "'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadGrid();
                    //AutoComplete();
                    ClearData();

                    MessageBox.Show("Stock Transaction Details Update Successful", "Updated", MessageBoxButton.OK, MessageBoxImage.Information);
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
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\source\\repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Stocks WHERE StockID = " + StockID.Text + " ", conn);
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
        private void TShirtID_TextChanged_1(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\source\\repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    // GETTING THE TSHIRT DETAILS
                    if (TShirtID.Text == "")
                    {
                        TShirtBrandPreview.Content = "";
                        TShirtNamePreview.Content = "";
                        TShirtColorPreview.Content = "";
                        TShirtSizePreview.Content = "";
                        TShirtQtyPreview.Content = "";
                    }
                    else
                    {
                        String TShirtIDTextBoxContent = TShirtID.Text;
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TShirtDetails INNER JOIN Stocks ON TShirtDetails.TShirtID = " + TShirtIDTextBoxContent, conn);
                        //SqlCommand cmd = new SqlCommand("SELECT * FROM TShirtDetails WHERE TShirtID = '" + TShirtIDTextBoxContent + "';", conn);
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

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            // GETTING THE TSHIRT DETAILS
            if (FilterBoxItem.Text == "")
            {
                LoadGrid();
            }
            else
            {
                String FilterBoxCategContent = FilterBoxCateg.Text;
                String FilterBoxItemContent = FilterBoxItem.Text;
                LoadGrid();
            }

        }

        private void StockID_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\source\\repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    // GETTING THE TSHIRT DETAILS                  
                    SqlCommand cmd = new SqlCommand("SELECT * FROM Stocks WHERE StockID = '" + StockID.Text + "';", conn);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        TShirtID.Text = "";
                        TShirtQty.Text = "";
                        TShirtDefect.Text = "";
                        StockDate.Text = "";
                        while (reader.Read())
                        {
                            TShirtID.Text = reader["TShirtID"].ToString();
                            TShirtQty.Text = reader["TShirtQty"].ToString();
                            TShirtDefect.Text = reader["TShirtDefect"].ToString();
                            StockDate.Text = reader["Date"].ToString();
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

