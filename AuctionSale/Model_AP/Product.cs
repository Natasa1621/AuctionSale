using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Model_AP
{
    public class Product : INotifyPropertyChanged, INotifyDataErrorInfo
    {
        #region Fields 

        private int _id;
        private string _name;
        private byte[] _image;
        private string _description;
        private decimal _startingBid;
        private int _userSellerId;
        private int? _userBuyerId;
        private decimal? _finalBid;
        private bool _isOffered;
        private bool _isGone;
        private bool _isDeleted;
        private TimeSpan offerTime;
        private DispatcherTimer timer;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();
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

        public string ProductName
        {
            get { return _name; }
            set
            {
                if (_name == value)
                {
                    return;
                }
                _name = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value.Length > 50)
                {
                    errors.Add("Product name is too long, over 50 characters.");
                    SetErrors("ProductName", errors);
                    valid = false;
                }
                if (valid)
                {
                    ClearErrors("ProductName");
                }
                OnPropertyChanged(new PropertyChangedEventArgs("ProductName"));
            }
        }

        public byte[] Image
        {
            get { return _image; }
            set
            {
                if (_image == value)
                {
                    return;
                }
                _image = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Image"));
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description == value)
                {
                    return;
                }
                _description = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value.Length > 1000)
                {
                    errors.Add("Product description is too long, over 1000 characters.");
                    SetErrors("Description", errors);
                    valid = false;
                }
                if (valid)
                {
                    ClearErrors("Description");
                }
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }

        public decimal StartingBid
        {
            get { return _startingBid; }
            set
            {
                if (_startingBid == value)
                {
                    return;
                }
                _startingBid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartingBid"));
            }
        }

        public int UserSellerId
        {
            get { return _userSellerId; }
            set
            {
                if (_userSellerId == value)
                {
                    return;
                }
                _userSellerId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserSellerId"));
            }
        }

        public int? UserBuyerId
        {
            get { return _userBuyerId; }
            set
            {
                if (_userBuyerId == value)
                {
                    return;
                }
                _userBuyerId = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserBuyerId"));
            }
        }

        public decimal? FinalBid
        {
            get { return _finalBid; }
            set
            {
                if (_finalBid == value)
                {
                    return;
                }
                _finalBid = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FinalBid"));
            }
        }

        public bool IsOffered
        {
            get { return _isOffered; }
            set
            {
                if (_isOffered == value)
                {
                    return;
                }
                _isOffered = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsOffered"));
            }
        }

        public bool IsGone
        {
            get { return _isGone; }
            set
            {
                if (_isGone == value)
                {
                    return;
                }
                _isGone = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsGone"));
            }
        }

        public bool IsDeleted
        {
            get { return _isDeleted; }
            set
            {
                if (_isDeleted == value)
                {
                    return;
                }
                _isDeleted = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDeleted"));
            }
        }
        
        public TimeSpan OfferTime
        {
            get { return offerTime; }
            set
            {
                if (offerTime == value)
                {
                    return;
                }
                offerTime = value;
                OnPropertyChanged(new PropertyChangedEventArgs("OfferTime"));
            }
        }

        public DispatcherTimer Timer
        {
            get { return timer; }
            set
            {
                if (timer == value)
                {
                    return;
                }
                timer = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Timer"));
            }
        }

        public bool HasErrors
        {
            get
            {
                return (errors.Count > 0);
            }
        }
        #endregion

        #region Constructors

        public Product(string ProductName, byte[] Image, string Description, decimal StartingBid, int UserSellerId, int? UserBuyerId, decimal? FinalBid, bool IsOffered, bool IsGone, bool IsDeleted)
        {
            this.ProductName = ProductName;
            this.Image = Image;
            this.Description = Description;
            this.StartingBid = StartingBid;
            this.UserSellerId = UserSellerId;
            this.UserBuyerId = UserBuyerId;
            this.FinalBid = FinalBid;
            this.IsOffered = IsOffered;
            this.IsGone = this.IsGone;
            this.IsDeleted = IsDeleted;
        }

        public Product(int Id, string ProductName, byte[] Image, string Description, decimal StartingBid, int UserSellerId, int? UserBuyerId, decimal? FinalBid, bool IsOffered, bool IsGone, bool IsDeleted) : this(ProductName, Image, Description, StartingBid, UserSellerId, UserBuyerId, FinalBid, IsOffered, IsGone, IsDeleted)
        {
            this.Id = Id;
        }

        public Product(int Id, string ProductName, byte[] Image, string Description, decimal StartingBid, int UserSellerId, int? UserBuyerId, decimal? FinalBid, bool IsOffered, bool IsGone, bool IsDeleted, TimeSpan OfferTime, DispatcherTimer Timer) : this(Id, ProductName, Image, Description, StartingBid, UserSellerId, UserBuyerId, FinalBid, IsOffered, IsGone, IsDeleted)
        {
            this.OfferTime = OfferTime;
            this.Timer = Timer;
        }

        public Product(int UserSellerId)
        {
            ProductName = "";
            Image = Array.Empty<byte>();
            Description = "";
            StartingBid = 0;
            this.UserSellerId = UserSellerId;
            OfferTime = TimeSpan.Zero;
            Timer = null;
        }

        public Product()
        {
            ProductName = "";
            Image = Array.Empty<byte>();
            Description = "";
            StartingBid = 0;
            UserSellerId = 0;
            OfferTime = TimeSpan.Zero;
            Timer = null;
        }
        #endregion

        #region Data Access

        public static Product GetProductFromResultSet(SqlDataReader reader)
        {
            Product product;
            product = new Product((int)reader["ID"],
                (string)reader["productName"],
                (byte[])reader["image"],
                (string)reader["description"],
                (decimal)reader["startingBid"],
                (int)reader["sellerId"],
                reader["buyerId"] as int? ?? null,
                reader["finalBid"] as decimal? ?? null,
                (bool)reader["isOffered"],
                (bool)reader["isGone"],
                (bool)reader["isDeleted"]);
            return product;
        }

        public void Insert()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("INSERT INTO AuctionProduct(productName, description, image, startingBid, sellerId) VALUES(@ProductName, @Description, @Image, @StartingBid, @SellerId); SELECT IDENT_CURRENT('AuctionProduct');", conn);

                SqlParameter productNameParam = new SqlParameter("@ProductName", SqlDbType.NVarChar);
                productNameParam.Value = this.ProductName;

                SqlParameter imageParam = new SqlParameter("@Image", SqlDbType.VarBinary);
                imageParam.Value = this.Image;

                SqlParameter descriptionParam = new SqlParameter("@Description", SqlDbType.NVarChar);
                descriptionParam.Value = this.Description;

                SqlParameter startingBidParam = new SqlParameter("@StartingBid", SqlDbType.Money);
                startingBidParam.Value = this.StartingBid;

                SqlParameter sellerIdParam = new SqlParameter("@SellerId", SqlDbType.Int, 11);
                sellerIdParam.Value = this.UserSellerId;

                command.Parameters.Add(productNameParam);
                command.Parameters.Add(imageParam);
                command.Parameters.Add(descriptionParam);
                command.Parameters.Add(startingBidParam);
                command.Parameters.Add(sellerIdParam);

                var _id = command.ExecuteScalar();

                if (_id != null)
                {
                    this.Id = Convert.ToInt32(_id);

                }
            }
        }

        public void Offer()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE AuctionProduct SET isOffered=1 WHERE ID=@Id;", conn);

                SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int, 11);
                idParam.Value = this.Id;
                command.Parameters.Add(idParam);

                int rows = command.ExecuteNonQuery();
            }
        }

        public void Save()
        {
            if (Id == 0)
            {
                Insert();
            }
            else if (Id != 0 && !IsOffered)
            {
                Offer();
            }
        }

        public void Delete()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE AuctionProduct SET isDeleted=1 WHERE ID=@Id;", conn);

                SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int, 11);
                idParam.Value = this.Id;
                command.Parameters.Add(idParam);

                int rows = command.ExecuteNonQuery();
            }
        }

        public void Remove()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE AuctionProduct SET isGone=1 WHERE ID=@Id;", conn);

                SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int, 11);
                idParam.Value = this.Id;
                command.Parameters.Add(idParam);

                int rows = command.ExecuteNonQuery();
            }
        }

        public bool Buy()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["ConnString"].ToString();
                conn.Open();

                SqlCommand command = new SqlCommand("UPDATE AuctionProduct SET buyerId=@BuyerId, finalBid=@FinalBid WHERE ID=@Id;", conn);

                SqlParameter buyerIdParam = new SqlParameter("@BuyerId", SqlDbType.Int, 11);
                buyerIdParam.Value = this.UserBuyerId;

                SqlParameter finalBidParam = new SqlParameter("@FinalBid", SqlDbType.Money);
                finalBidParam.Value = this.FinalBid;

                SqlParameter idParam = new SqlParameter("@Id", SqlDbType.Int, 11);
                idParam.Value = this.Id;

                command.Parameters.Add(buyerIdParam);
                command.Parameters.Add(finalBidParam);
                command.Parameters.Add(idParam);

                int rows = command.ExecuteNonQuery();
            }
            return true;
        }

        public decimal IncreaseBid()
        {
            decimal amount;
            if (FinalBid == null)
            {
                amount = StartingBid;
                FinalBid = ++amount;
            }
            else
                ++FinalBid;
            return FinalBid.Value;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return (errors.Values);
            }
            else
            {
                if (errors.ContainsKey(propertyName))
                {
                    return (errors[propertyName]);
                }
                else
                {
                    return null;
                }
            }
        }

        private void SetErrors(string propertyName, List<string> propertyErrors)
        {
            errors.Remove(propertyName);
            errors.Add(propertyName, propertyErrors);
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }

        private void ClearErrors(string propertyName)
        {
            errors.Remove(propertyName);
            if (ErrorsChanged != null)
                ErrorsChanged(this, new DataErrorsChangedEventArgs(propertyName));
        }       
        #endregion
    }
}