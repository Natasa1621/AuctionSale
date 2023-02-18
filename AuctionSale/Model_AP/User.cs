using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model_AP
{
    public class User : INotifyPropertyChanged
    {
        #region Fields 

        private int _id;
        private string _name;
        private string _password;
        private bool _user_admin;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
        #endregion

        #region Properties

        public int Id
        {
            get { return _id; }
            set
            {
                if (_id == value)
                {
                    return;
                }
                _id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }

        public string UserName
        {
            get { return _name; }
            set
            {
                if (_name == value)
                {
                    return;
                }
                _name = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password == value)
                {
                    return;
                }
                _password = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Password"));
            }
        }

        public bool User_Admin
        {
            get { return _user_admin; }
            set
            {
                if (_user_admin == value)
                {
                    return;
                }
                _user_admin = value;
                OnPropertyChanged(new PropertyChangedEventArgs("User_Admin"));
            }
        }
        #endregion

        #region Constructors

        public User(string UserName, string Password, bool User_Admin)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.User_Admin = User_Admin;
        }

        public User(int Id, string UserName, string Password, bool User_Admin)
        {
            this.UserName = UserName;
            this.Password = Password;
            this.User_Admin = User_Admin;
            this.Id = Id;
        }

        public User()
        {
            UserName = "";
            Password = "";
            User_Admin = false;
        }

        public User(string UserName, string Password)
        {
            this.UserName = UserName;
            this.Password = Password;
            
        }
        #endregion

        #region Data Access

        public static User GetUserFromResultSet(SqlDataReader reader)
        {
            User user = new User((int)reader["ID"], (string)reader["userName"], (string)reader["password"], (bool)reader["IsAdmin"]);
            return user;
        }

        public void Insert()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("INSERT INTO AuctionUser(userName, password, IsAdmin) VALUES(@UserName, @UserPass, @IsAdmin); SELECT IDENT_CURRENT('AuctionUser');", conn);

                SqlParameter userNameParam = new SqlParameter("@UserName", SqlDbType.NVarChar);
                userNameParam.Value = this.UserName;

                SqlParameter userPassParam = new SqlParameter("@UserPass", SqlDbType.NVarChar);
                userPassParam.Value = this.Password;

                SqlParameter isAdminParam = new SqlParameter("@IsAdmin", SqlDbType.Bit);
                isAdminParam.Value = this.User_Admin;

                command.Parameters.Add(userNameParam);
                command.Parameters.Add(userPassParam);
                command.Parameters.Add(isAdminParam);

                var _id = command.ExecuteScalar();

                if (_id != null)
                {
                    this.Id = Convert.ToInt32(_id);

                }
            }
        }
        #endregion
    }
}