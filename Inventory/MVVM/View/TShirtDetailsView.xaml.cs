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
    public partial class TShirtDetailsView : UserControl
    {
        public TShirtDetailsView()
        {
            InitializeComponent();
            LoadGrid();
            TShirtDetailsDropDisplay();
        }

        private void ClearData()
        {
            TShirtDetailsID.Text = string.Empty;
            Brand.Text = string.Empty;
            Name.Text = string.Empty;
            Color.Text = string.Empty;
            Size.Text = string.Empty;
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
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\Inventory\\Inventory\\InventoryDatabase.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM TShirtDetails;", conn);
                    DataTable dt = new DataTable();
                    conn.Open();
                    SqlDataReader sdr = cmd.ExecuteReader();
                    dt.Load(sdr);
                    conn.Close();
                    TShirtDetailsDataGrid.ItemsSource = dt.DefaultView;
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
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\Inventory\\Inventory\\InventoryDatabase.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO TShirtDetails (IdTShirt, Brand, Name, Color, Size) VALUES (@IdTShirt, @Brand, @Name, @Color, @Size);", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@IdTShirt", TShirtDetailsID.Text);
                    cmd.Parameters.AddWithValue("@Brand", Brand.Text);
                    cmd.Parameters.AddWithValue("@Name", Name.Text);
                    cmd.Parameters.AddWithValue("@Color", Color.Text);
                    cmd.Parameters.AddWithValue("@Size", Size.Text);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadGrid();
                    ClearData();
                    TShirtDetailsDropDisplay();
                    MessageBox.Show("T Shirt Details Input Successful", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
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
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\Inventory\\Inventory\\InventoryDatabase.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE TShirtDetails set Brand = '" + Brand.Text + "', Name = '" + Name.Text + "', Color = '" + Color.Text + "', Size = '" + Size.Text + "' WHERE IdTShirt = '" + TShirtDetailsID.Text + "'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadGrid();
                    ClearData();
                    TShirtDetailsDropDisplay();
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
                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\Inventory\\Inventory\\InventoryDatabase.mdf;Integrated Security=True"))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM TShirtDetails WHERE IdTShirt = " + TShirtDetailsID.Text + " ", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadGrid();
                    ClearData();
                    TShirtDetailsDropDisplay();
                    MessageBox.Show("T Shirt Details Deletion Successful", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Deletion Failed: \n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // DROP DOWN
        private void TShirtIDDrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (TShirtIDDrop.SelectedValue == "ADD NEW")
                {
                    ClearData();
                }
                else
                {
                    using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\Inventory\\Inventory\\InventoryDatabase.mdf;Integrated Security=True"))
                    {
                        SqlCommand cmd = new SqlCommand("SELECT * FROM TShirtDetails WHERE IdTShirt = '" + TShirtIDDrop.SelectedValue + "'", conn);
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        SqlDataReader sdr = cmd.ExecuteReader();
                        while (sdr.Read())
                        {
                            string sIdTShirt = sdr["IdTShirt"].ToString();
                            TShirtDetailsID.Text = sIdTShirt;
                            string sBrand = sdr["Brand"].ToString();
                            Brand.Text = sBrand;
                            string sName = sdr["Name"].ToString();
                            Name.Text = sName;
                            string sColor = sdr["Color"].ToString();
                            Color.Text = sColor;
                            string sSize = sdr["Size"].ToString();
                            Size.Text = sSize;
                        }
                        conn.Close();
                        LoadGrid();
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("TShirtID Dropdown Failed: \n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TShirtDetailsDropDisplay()
        {
            TShirtIDDrop.Items.Clear();
            BrandDrop.Items.Clear();
            NameDrop.Items.Clear();
            ColorDrop.Items.Clear();
            SizeDrop.Items.Clear();

            TShirtIDDrop.Items.Add("ADD NEW");
            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\Inventory\\Inventory\\InventoryDatabase.mdf;Integrated Security=True"))
            {
                string q = "SELECT * from TShirtDetails";
                SqlCommand cmd = new SqlCommand(q, conn);
                conn.Open();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    TShirtIDDrop.Items.Add(sdr["IdTShirt"].ToString());
                    BrandDrop.Items.Add(sdr["Brand"].ToString());
                    NameDrop.Items.Add(sdr["Name"].ToString());
                    ColorDrop.Items.Add(sdr["Color"].ToString());
                    SizeDrop.Items.Add(sdr["Size"].ToString());
                }
                conn.Close();
            }
        }

        private void BrandDrop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\FAsad\\source\\repos\\Inventory\\Inventory\\InventoryDatabase.mdf;Integrated Security=True"))
            {
                SqlCommand cmd = new SqlCommand("SELECT Brand FROM TShirtDetails", conn);
                conn.Open();
                cmd.ExecuteNonQuery();
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    string sBrand = sdr["Brand"].ToString();
                    Brand.Text = sBrand;
                }
                conn.Close();
                LoadGrid();
            }
        }
    }
}
