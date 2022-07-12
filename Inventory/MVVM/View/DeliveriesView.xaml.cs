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
        }

        private void ClearData()
        {
            DeliveryID.Text = string.Empty;
            DeliveryType.Text = string.Empty;
            TShirtID.Text = string.Empty;
            Quantity.Text = string.Empty;
            DateReceived.Text = string.Empty;
            DateDelivered.Text = string.Empty;
        }
        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            ClearData();
        }

        // UPDATE ON STOCKS IF DELIVERY TYPE IS "OUT"
        private void UpdateOnStocks()
        {
            int stocks, newStocks;
           

            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\Source\\Repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT TShirtQty FROM Stocks WHERE TShirtID = '" + TShirtID.Text + "'", conn);
                    SqlCommand cmd2 = new SqlCommand("UPDATE Stocks SET TShirtQty = @newStocks WHERE TShirtID = '" + TShirtID.Text + "'", conn);
                    conn.Open();

                    // GETTING THE QTY AND DEFECTS ON THE DATABASE
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // DIRETSO NA INT FOR COMPUTATION
                            stocks = Int32.Parse(reader["TShirtQty"].ToString());
                           

                            // COMPUTATION
                            newStocks = stocks - Int32.Parse(Quantity.Text);
                          
                            //ADDING NEW VALUES TO DATABASE
                            cmd2.Parameters.AddWithValue("@newStocks", newStocks);
 

                        }

                    }

                    cmd2.ExecuteNonQuery();
                    conn.Close();
                    MessageBox.Show("Stocks Details Updated Successfully", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);

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
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\Source\\Repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM DeliveryDetails;", conn);
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

        // INSERT T SHIRT DETAILS
        private void InsertTShirtDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\Source\\Repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO Deliveries (DeliveryID, TransferType, TShirtID, Quantity, DateReceived, DateDelivered) VALUES (@DeliveryID, @DeliveryType, @TShirtID, @Quantity, @DateReceived, @DateDelivered);", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@DeliveryID", DeliveryID.Text);
                    cmd.Parameters.AddWithValue("@DeliveryType", DeliveryType.Text);
                    cmd.Parameters.AddWithValue("@TShirtID", TShirtID.Text);
                    cmd.Parameters.AddWithValue("@Quantity", Quantity.Text);
                    cmd.Parameters.AddWithValue("@DateReceived", DateReceived.Text);
                    cmd.Parameters.AddWithValue("@DateDelivered", DateDelivered.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();

                    if(DeliveryType.Text == "Out" || DeliveryType.Text == "OUT" || DeliveryType.Text == "out")
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
        private void UpdateTShirtDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\Source\\Repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE Deliveries set TransferType = '" + DeliveryType.Text + "', TShirtID = '" + TShirtID.Text + "', Quantity = '" + Quantity.Text + "', DateReceived = '" + DateReceived.Text + "' DataDelivered = '" + DateDelivered.Text + "' WHERE IdTShirt = '" + DeliveryID.Text + "'", conn);
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
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Acer\\Source\\Repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM Deliveries WHERE DeliveryID = " + DeliveryID.Text + " ", conn);
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



    }
    
}
