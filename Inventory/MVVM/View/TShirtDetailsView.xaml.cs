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
            AutoComplete();
        }

        private void AutoComplete()
        {

            using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\user\\Source\\Repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))

           

            {
                SqlCommand cmd;
                SqlDataReader sdr;
                string q;

                // List for AutoSuggest
                List<int> TShirtIDs = new List<int>();
                List<string> TShirtBrands = new List<string>();
                List<string> TShirtNames = new List<string>();
                List<string> TShirtColors = new List<string>();
                List<string> TShirtSizes = new List<string>();

                // Reading Database Result per Column
                conn.Open();
                q = "SELECT TShirtID from TShirtDetails";
                cmd = new SqlCommand(q, conn);             
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    TShirtIDs.Add((int)sdr["TShirtID"]);
                }
                conn.Close();

                conn.Open();
                q = "SELECT TShirtBrand from TShirtDetails";
                cmd = new SqlCommand(q, conn);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    TShirtBrands.Add(sdr["TShirtBrand"].ToString());
                }
                conn.Close();

                conn.Open();
                q = "SELECT TShirtName from TShirtDetails";
                cmd = new SqlCommand(q, conn);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    TShirtNames.Add(sdr["TShirtName"].ToString());
                }
                conn.Close();

                conn.Open();
                q = "SELECT TShirtColor from TShirtDetails";
                cmd = new SqlCommand(q, conn);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    TShirtColors.Add(sdr["TShirtColor"].ToString());
                }
                conn.Close();

                conn.Open();
                q = "SELECT TShirtSize from TShirtDetails";
                cmd = new SqlCommand(q, conn);
                sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    TShirtSizes.Add(sdr["TShirtSize"].ToString());
                }
                conn.Close();

                // Setting Textbox AutoSuggest Sources
                TShirtDetailsID.ItemsSource = TShirtIDs;
                Brand.ItemsSource = TShirtBrands;
                Name.ItemsSource = TShirtNames;
                Color.ItemsSource = TShirtColors;
                Size.ItemsSource = TShirtSizes;
                conn.Close();
            }
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
            TShirtDetailsDataGrid.IsReadOnly = true;
            if (FilterBoxItem.Text == "")
            {
                try
                {

                    using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\user\\Source\\Repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
 
                     

                    {
                        SqlCommand cmd = new SqlCommand("SELECT TShirtID, TShirtBrand, TShirtName, TShirtColor, TShirtSize FROM TShirtDetails ;", conn);
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
            else
            {

                try
                {

                    using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\user\\Source\\Repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
 
                     

                    {
                        String queryString = "";

                        if (FilterBoxCateg.Text == "TShirtID")
                        {
                            queryString = "SELECT * FROM TShirtDetails WHERE TShirtID = @FilterItem";
                        }
                        else if (FilterBoxCateg.Text == "TShirtBrand" || FilterBoxCateg.Text == "TShirtQty")
                        {
                            queryString = "SELECT * FROM TShirtDetails WHERE TShirtBrand = @FilterItem";
                        }
                        else if (FilterBoxCateg.Text == "TShirtName")
                        {
                            queryString = "SELECT * FROM TShirtDetails WHERE TShirtName = @FilterItem";
                        }
                        else if (FilterBoxCateg.Text == "TShirtColor")
                        {
                            queryString = "SELECT * FROM TShirtDetails WHERE TShirtColor = @FilterItem";
                        }
                        else if (FilterBoxCateg.Text == "TShirtSize")
                        {
                            queryString = "SELECT * FROM TShirtDetails WHERE TShirtSize = @FilterItem";
                        }



                        SqlCommand cmd = new SqlCommand(queryString, conn);
                        cmd.Parameters.AddWithValue("@FilterCateg", FilterBoxCateg.Text);
                        cmd.Parameters.AddWithValue("@FilterItem", FilterBoxItem.Text);

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
        }

        // INSERT T SHIRT DETAILS
        private void InsertTShirtDetails_Click(object sender, RoutedEventArgs e)
        {
          
            try
            {

                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\user\\Source\\Repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
 
                 

                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO TShirtDetails (TShirtID, TShirtBrand, TShirtName, TShirtColor, TShirtSize, TShirtDirect) VALUES (@TShirtID, @Brand, @Name, @Color, @Size, @fileDirect);", conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@TShirtID", TShirtDetailsID.Text);
                    cmd.Parameters.AddWithValue("@Brand", Brand.Text);
                    cmd.Parameters.AddWithValue("@Name", Name.Text);
                    cmd.Parameters.AddWithValue("@Color", Color.Text);
                    cmd.Parameters.AddWithValue("@Size", Size.Text);
                    cmd.Parameters.AddWithValue("@fileDirect", fileDirect);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadGrid();
                    AutoComplete();
                    ClearData();
                    
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

                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\user\\Source\\Repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
 
                 

                {
                    SqlCommand cmd = new SqlCommand("UPDATE TShirtDetails set TShirtBrand = '" + Brand.Text + "', TShirtName = '" + Name.Text + "', TShirtColor = '" + Color.Text + "', TShirtSize = '" + Size.Text + "' WHERE TShirtID = '" + TShirtDetailsID.Text + "'", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadGrid();
                    AutoComplete();
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

                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\user\\Source\\Repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
 
                 

                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM TShirtDetails WHERE TShirtID = " + TShirtDetailsID.Text + " ", conn);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    LoadGrid();
                    AutoComplete();
                    ClearData();
                    
                    MessageBox.Show("T Shirt Details Deletion Successful", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Deletion Failed: \n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void TShirtDetailsID_TextChanged(object sender, RoutedEventArgs e)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\user\\Source\\Repos\\TShirtInventorySystem\\Inventory\\InventoryDB.mdf;Integrated Security=True"))
 
                 

                {
                    SqlCommand cmd = new SqlCommand("SELECT * FROM TShirtDetails WHERE TShirtID = '" + TShirtDetailsID.Text + "';", conn);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        Brand.Text = "";
                        Name.Text = "";
                        Color.Text = "";
                        Size.Text = "";
                        while (reader.Read())
                        {
                            Brand.Text = reader["TShirtBrand"].ToString();
                            Name.Text = reader["TShirtName"].ToString();
                            Color.Text = reader["TShirtColor"].ToString();
                            Size.Text = reader["TShirtSize"].ToString();
                        }
                    }
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

        string fileDirect = "";
        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog();
            


            dialog.Filter = "Image files (*.png)| *.png| (*.jpg)| *.jpg| (*.gif)| *.gif";
            

            bool? result = dialog.ShowDialog();

            if(result == true)
            {
                fileDirect = dialog.FileName;
                TShirtImage.Source = new BitmapImage(new Uri($@"{fileDirect}"));
            }

            
        }
    }
   }

