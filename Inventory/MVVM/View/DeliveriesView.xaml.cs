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
    /// Interaction logic for DeliveriesView.xaml
    /// </summary>
    public partial class DeliveriesView : UserControl
    {
        public DeliveriesView()
        {
            InitializeComponent();
            LoadGrid();
            AutoComplete();
        }

        private void ClearData()
        {
            DeliveryID.Text = string.Empty;
            In.IsChecked = false;
            Out.IsChecked = false;
            TShirtID.Text = string.Empty;
            Quantity.Text = string.Empty;
            DateReceived.Text = string.Empty;
            DateDelivered.Text = string.Empty;
        }
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        // AUTO SUGGEST
        private void AutoComplete()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
            {
                SqlCommand cmd;
                SqlDataReader sdr;
                string q;

                // List for AutoSuggest
                List<int> DeliveryIDs = new List<int>();
                List<int> TShirtIDs = new List<int>();

                // Reading Database Result per Column
                conn.Open();
                q = "SELECT DeliveryID from DeliveryDetails";
                cmd = new SqlCommand(q, conn);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    DeliveryIDs.Add((int)sdr["DeliveryID"]);
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
                DeliveryID.ItemsSource = DeliveryIDs;
                TShirtID.ItemsSource = TShirtIDs;
            }
        }

        // UPDATE ON STOCKS IF DELIVERY TYPE IS "OUT"
        private void UpdateOnStocks()
        {
            int stocks;

            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT TotalQty FROM Quantity WHERE TShirtID = '" + TShirtID.Text + "'", conn);
                    SqlCommand cmd2 = new SqlCommand("UPDATE Quantity SET TotalQty = @stocks WHERE TShirtID = '" + TShirtID.Text + "'", conn);
                    conn.Open();

                    // GETTING THE QTY AND DEFECTS ON THE DATABASE
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // DIRETSO NA INT FOR COMPUTATION
                            stocks = Int32.Parse(reader["TotalQty"].ToString()) - Int32.Parse(Quantity.Text);




                            //ADDING NEW VALUES TO DATABASE
                            cmd2.CommandType = CommandType.Text;
                            cmd2.Parameters.AddWithValue("@stocks", stocks);
 

                        }

                    }

                    cmd2.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Quantity Details Updated Successfully", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);

                    conn.Close();
   
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

        // INVENTORY DATABASE T SHIRT DETAILS TABLE
        public void LoadGrid()
        {
            DeliveryDetailsGrid.IsReadOnly = true;
            DeliveryDetailsGrid.CanUserResizeColumns = false;
            if (FilterBoxItem.Text == "")
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT DeliveryID, DeliveryType as Type, TShirtID, Quantity, DateReceived, DateDelivered AS DateReleased FROM DeliveryDetails ;", conn);
                        DataTable dt = new DataTable();
                        conn.Open();
                        SqlDataReader sdr = cmd.ExecuteReader();
                        dt.Load(sdr);
                        conn.Close();
                        DeliveryDetailsGrid.ItemsSource = dt.DefaultView;
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
                    using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                    {
                        String queryString = "";

                        if (FilterBoxCateg.Text == "TShirtID")
                        {
                            queryString = "SELECT * FROM DeliveryDetails WHERE TShirtID = @FilterItem";
                        }
                        else if (FilterBoxCateg.Text == "DeliveryType")
                        {
                            queryString = "SELECT * FROM DeliveryDetails WHERE DeliveryType = @FilterItem";
                        }
                        else if (FilterBoxCateg.Text == "Quantity")
                        {
                            queryString = "SELECT * FROM DeliveryDetails WHERE Quantity = @FilterItem";
                        }
                        else if (FilterBoxCateg.Text == "DateReceived")
                        {
                            queryString = "SELECT * FROM DeliveryDetails WHERE DateReceived = @FilterItem";
                        }
                        else if (FilterBoxCateg.Text == "DateDelivered")
                        {
                            queryString = "SELECT * FROM DeliveryDetails WHERE DateDelivered = @FilterItem";
                        }


                        SqlCommand cmd = new SqlCommand(queryString, conn);
                        cmd.Parameters.AddWithValue("@FilterCateg", FilterBoxCateg.Text);
                        cmd.Parameters.AddWithValue("@FilterItem", FilterBoxItem.Text);

                        DataTable dt = new DataTable();
                        conn.Open();
                        SqlDataReader sdr = cmd.ExecuteReader();
                        dt.Load(sdr);
                        conn.Close();
                        DeliveryDetailsGrid.ItemsSource = dt.DefaultView;
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Reading Failed: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }


        }

        // INSERT T SHIRT DETAILS
        private void InsertDeliveryDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    String DeliveryType;
                    if (Out.IsChecked==true)
                    {
                        DeliveryType = "Out";
                    }
                    else if (In.IsChecked == true)
                    {
                        DeliveryType = "In";
                    }
                    else
                    {
                        DeliveryType = "";
                    }
                    SqlCommand cmd = new SqlCommand("INSERT INTO DeliveryDetails  (DeliveryID, DeliveryType, TShirtID, Quantity, DateReceived, DateDelivered) VALUES (@DeliveryID, @DeliveryType, @TShirtID, @Quantity, @DateReceived, @DateDelivered);", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@DeliveryID", DeliveryID.Text);
                    cmd.Parameters.AddWithValue("@DeliveryType", DeliveryType);
                    cmd.Parameters.AddWithValue("@TShirtID", TShirtID.Text);
                    cmd.Parameters.AddWithValue("@Quantity", Quantity.Text);
                    cmd.Parameters.AddWithValue("@DateReceived", DateReceived.Text);
                    cmd.Parameters.AddWithValue("@DateDelivered", DateDelivered.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    if(DeliveryType == "Out" || DeliveryType == "OUT" || DeliveryType == "out")
                    {
                        UpdateOnStocks();
                    }

                    conn.Close();
                    LoadGrid();
                    ClearData();
                   
                    MessageBox.Show("Delivery Details Input Successful", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
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
        private void UpdateDeliveryDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    String DeliveryType;
                    if (Out.IsChecked == true)
                    {
                        DeliveryType = "Out";
                    }
                    else if (In.IsChecked == true)
                    {
                        DeliveryType = "In";
                    }
                    else
                    {
                        DeliveryType = "";
                    }
                    SqlCommand cmd = new SqlCommand("UPDATE DeliveryDetails  set DeliveryType = '" + DeliveryType + "', TShirtID = '" + TShirtID.Text + "', Quantity = '" + Quantity.Text + "', DateReceived = '" + DateReceived.Text + "' DataDelivered = '" + DateDelivered.Text + "' WHERE IdTShirt = '" + DeliveryID.Text + "'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
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
        // DELETE T SHIRT DETAILS
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM DeliveryDetails  WHERE DeliveryID = " + DeliveryID.Text + " ", conn);
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

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

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

        // PREVIEW
        private void TShirtID_TextChanged(object sender, RoutedEventArgs e)
        {
            String direct = "";
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    // GETTING THE TSHIRT DETAILS
                    if (TShirtID.Text == "")
                    {
                        TShirtBrandPreview.Content = "Brand:";
                        TShirtNamePreview.Content = "Name:";
                        TShirtColorPreview.Content = "Color:";
                        TShirtSizePreview.Content = "Size:";
                        TShirtQtyPreview.Content = "Quantity:";
                        TShirtImage.Source = new BitmapImage(new Uri(@"C:\Users\FAsad\source\repos\NewRepo\Inventory\MVVM\previewImages\white-tshirt.jpg"));
                    }
                    else
                    {
                        String TShirtIDTextBoxContent = TShirtID.Text;
                        
                        SqlCommand cmd1 = new SqlCommand("SELECT SUM(TShirtQty) FROM Stocks WHERE TShirtID = " + TShirtIDTextBoxContent, conn);
                        conn.Open();
                        // int TotalQuantity = (int)cmd1.ExecuteScalar();
                        conn.Close();

                        SqlCommand cmd = new SqlCommand("SELECT * FROM TShirtDetails INNER JOIN Stocks ON TShirtDetails.TShirtID = " + TShirtIDTextBoxContent, conn);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TShirtBrandPreview.Content = "Brand: " + reader["TShirtBrand"].ToString();
                                TShirtNamePreview.Content = "Name: " + reader["TShirtName"].ToString();
                                TShirtColorPreview.Content = "Color: " + reader["TShirtColor"].ToString();
                                TShirtSizePreview.Content = "Size: " + reader["TShirtSize"].ToString();
                                // TShirtQtyPreview.Content = "Quantity: " + TotalQuantity;
                                direct = reader["TShirtDirect"].ToString();
                                TShirtImage.Source = new BitmapImage(new Uri($@"{direct}"));
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

        private void DeliveryID_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\NewRepo\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM DeliveryDetails WHERE DeliveryID = '" + DeliveryID.Text + "';", conn);
                    conn.Open();
                    String DeliveryType = "";
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        TShirtID.Text = "";
                        Quantity.Text = "";
                        DateReceived.Text = "";
                        DateDelivered.Text = "";
                        while (reader.Read())
                        {
                            DeliveryType = reader["DeliveryType"].ToString();
                            TShirtID.Text = reader["TShirtID"].ToString();
                            Quantity.Text = reader["Quantity"].ToString();
                            DateReceived.Text = reader["DateReceived"].ToString();
                            DateDelivered.Text = reader["DateDelivered"].ToString();
                        }
                    }
                    if (DeliveryType == "In")
                    {
                        In.IsChecked = true;
                    }
                    else if (DeliveryType == "Out")
                    {
                        Out.IsChecked = true;
                    }
                    else
                    {
                        In.IsChecked = false;
                        Out.IsChecked = false;
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
