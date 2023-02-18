using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Model_AP;

namespace VM_AP
{
    public class RegistrationWindowVM : DonePropertyChangedEvents, INotifyDataErrorInfo
    {
        #region Fields

        private User currentUser;
        private bool passwordLength;

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        private Dictionary<string, List<string>> errors = new Dictionary<string, List<string>>();

        #endregion

        #region Properties

        public User CurrentUser
        {
            get { return currentUser; }
            set
            {
                if (currentUser == value)
                {
                    return;
                }
                currentUser = value;
                OnPropertyChanged("CurrentUser");
                OnPropertyChanged("UserName");
                OnPropertyChanged("Password");
            }
        }

        public string UserName
        {
            get { return CurrentUser.UserName; }
            set
            {
                if (CurrentUser.UserName == value)
                {
                    return;
                }
                CurrentUser.UserName = value;

                List<string> errors = new List<string>();
                bool valid = true;

                if (value.Length > 50 || value.Length < 2)
                {
                    errors.Add("Username length is not appropriate.");
                    SetErrors("UserName", errors);
                    valid = false;
                }
                if (!ValidateUniqueName(value))
                {
                    errors.Add("This name has already been used.");
                    SetErrors("UserName", errors);
                    valid = false;
                }
                if (valid)
                {
                    ClearErrors("UserName");
                }
                OnPropertyChanged("UserName");
            }
        }

        public string Password
        {
            get { return CurrentUser.Password; }
            set
            {
                if (CurrentUser.Password == value)
                {
                    return;
                }
                CurrentUser.Password = value;                
                OnPropertyChanged("Password");
                PasswordLength = (value.Length > 7 && value.Length <= 50);
            }
        }

        public bool PasswordLength
        {
            get { return passwordLength; }
            set
            {
                passwordLength = value;
                List<string> errors = new List<string>();
                bool valid = true;

                if (value == false)
                {                   
                    errors.Add("Password length must be between 8 and 50 characters.");
                    SetErrors("PasswordLength", errors);
                    valid = false;
                }
                if (valid)
                {
                    ClearErrors("PasswordLength");
                }
                OnPropertyChanged("PasswordLength");
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

        #region Constructor

        public RegistrationWindowVM()
        {
            CurrentUser = new User();
            CurrentUser.User_Admin = false;
            SaveCommand = new RelayCommand(SaveExecute, CanSave);
        }
        #endregion

        #region ICommand

        private ICommand saveCommand;

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                if (saveCommand == value)
                {
                    return;
                }
                saveCommand = value;
                OnPropertyChanged("SaveCommand");
            }
        }

        void SaveExecute(object obj)
        {
            if (CurrentUser != null && !HasErrors)
            {
                CurrentUser.Insert();
                OnDone("You are registered. Please sign in.");        
            }
            else
            {
                OnDone("Check your input.");
            }
        }

        bool CanSave(object obj)
        {
            bool can = false;
            if (UserName?.Length > 0 && Password?.Length > 0)
            {
                can = true;
                passwordLength = false;
            }                
            return can;
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
           
        private bool ValidateUniqueName(string name)
        {
            int nameCount = 0;
            nameCount = UserCollection.GetAllUsers().Count(u => u.UserName.ToLower() == name.ToLower());
            if (nameCount > 0)
                return false;
            else
                return true;
        }
    }
    #endregion
}