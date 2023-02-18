using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_AP
{
    public class ProductCollection : ObservableCollection<Product>
    {
        public static ProductCollection GetAllNewProducts()
        {
            ProductCollection productsNew = new ProductCollection();
            Product product = null;

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();
                {
                    SqlCommand command = new SqlCommand("SELECT ID, productName, image, description, startingBid, sellerId, buyerId, finalBid, isOffered, isGone, isDeleted FROM AuctionProduct WHERE isOffered=0 AND isDeleted=0", conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            product = Product.GetProductFromResultSet(reader);
                            productsNew.Add(product);
                        }
                    }
                }
                return productsNew;
            }
        }

        public static ProductCollection GetAllOfferedProducts()
        {
            ProductCollection productsOffered = new ProductCollection();
            Product product = null;

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();
                {
                    SqlCommand command = new SqlCommand("SELECT ID, productName, image, description, startingBid, sellerId, buyerId, finalBid, isOffered, isGone, isDeleted FROM AuctionProduct WHERE isOffered=1 AND isGone=0", conn);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            product = Product.GetProductFromResultSet(reader);
                            productsOffered.Add(product);
                        }
                    }
                }
                return productsOffered;
            }
        }
    }
}