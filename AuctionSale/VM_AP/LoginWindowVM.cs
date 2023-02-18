using Model_AP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VM_AP
{
    public class LoginWindowVM : DonePropertyChangedEvents
    {
        #region Fields

        private string name;
        private string password;

        #endregion

        #region Properties

        public string UserName
        {
            get { return name; }
            set
            {
                if (name == value)
                {
                    return;
                }
                name = value;
                OnPropertyChanged("UserName");                
            }
        }

        public string Password
        {
            get { return password; }
            set
            {
                if (password == value)
                {
                    return;
                }
                password = value;
                OnPropertyChanged("Password");
            }
        }
        #endregion

        #region Constructors

        public LoginWindowVM()
        {
            LoginCommand = new RelayCommand(LoginExecute, CanLogin);
        }

        public LoginWindowVM(User user)
        {
            LoginCommand = new RelayCommand(LoginExecute, CanLogin);
            UserName = user.UserName;
        }
        #endregion

        #region ICommand

        private ICommand loginCommand;

        public ICommand LoginCommand
        {
            get { return loginCommand; }
            set
            {
                if (loginCommand == value)
                {
                    return;
                }
                loginCommand = value;
                OnPropertyChanged("LoginCommand");
            }
        }

        void LoginExecute(object obj)
        {
            if (UserName != null && Password != null)
            {
                if (GetUser() != null)
                {
                    if (GetUser().User_Admin)
                        OnDone(String.Format("Welcome admin {0}.", UserName));
                    else if (!GetUser().User_Admin)
                        OnDone(String.Format("Welcome {0}.", UserName));
                }
                else
                    OnDone("Login failed. Please try again or sign up.");
            }
        }

        bool CanLogin(object obj)
        {
            bool can = false;
            if (UserName?.Length > 0 && Password?.Length > 7)
                can = true;
            return can;
        }

        private User GetUser()
        {
            return UserCollection.GetAllUsers().FirstOrDefault(u => u.UserName == UserName && u.Password == Password);
        }
        #endregion
    }
}