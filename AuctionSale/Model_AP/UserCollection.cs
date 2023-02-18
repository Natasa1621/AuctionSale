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
    public class UserCollection : ObservableCollection<User>
    {
        public static UserCollection GetAllUsers()
        {
            UserCollection users = new UserCollection();
            User user = null;

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("SELECT ID, userName, password, isAdmin FROM AuctionUser", conn);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        user = User.GetUserFromResultSet(reader);
                        users.Add(user);
                    }
                }
            }
            return users;
        }
    }
}