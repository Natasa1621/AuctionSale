using Model_AP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace VM_AP
{
    public class SellBidOfferDeleteWindowVM : DonePropertyChangedEvents
    {
        #region Fields

        private User currentUser;
        private Product currentProduct;
        private Mediator mediator;
        private string windowTitle;      
        private bool newCountdown;

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
            }
        }

        public Product CurrentProduct
        {
            get { return currentProduct; }
            set
            {
                if (currentProduct == value)
                {
                    return;
                }
                currentProduct = value;
                OnPropertyChanged("CurrentProduct");
            }
        }

        public string WindowTitle
        {
            get { return windowTitle; }
            set
            {
                if (windowTitle == value)
                {
                    return;
                }
                windowTitle = value;
                OnPropertyChanged("WindowTitle");
            }
        }       
        #endregion

        #region Constructors
        
        public SellBidOfferDeleteWindowVM(Product product, User user, Mediator mediator)
        {
            CurrentProduct = product;
            CurrentUser = user;
            this.mediator = mediator;            
            WindowTitle = "Bidding Process";
          
            BidCommand = new RelayCommand(BidExecute, CanBid);           
            newCountdown = true;
        }

        public SellBidOfferDeleteWindowVM(Product product, Mediator mediator)
        {
            CurrentProduct = product;
            this.mediator = mediator;          
            WindowTitle = "Product Information";

            SaveCommand = new RelayCommand(SaveExecute, CanSave);
            DeleteCommand = new RelayCommand(DeleteExecute, CanDelete);           
        }

        public SellBidOfferDeleteWindowVM(User user, Mediator mediator)
        {
            CurrentUser = user;
            CurrentProduct = new Product(user.Id);
            this.mediator = mediator;
            WindowTitle = "New Product";

            SaveCommand = new RelayCommand(SaveExecute, CanSave);
        }
        #endregion

        #region ICommand

        private ICommand bidCommand;

        public ICommand BidCommand
        {
            get { return bidCommand; }
            set
            {
                if (bidCommand == value)
                {
                    return;
                }
                bidCommand = value;
                OnPropertyChanged("BidCommand");               
            }
        }

        void BidExecute(object obj)
        {
            if (CurrentProduct != null)
            {
                CurrentProduct.FinalBid = CurrentProduct.IncreaseBid();
                CurrentProduct.UserBuyerId = CurrentUser.Id;
                CurrentProduct.Buy();
                mediator.Notify("ProductChange", CurrentProduct);
                CallTimer(CurrentProduct);              
                newCountdown = false;               
            }
        }

        bool CanBid(object obj)
        {
            bool can = true;
            if (CurrentProduct.UserSellerId == CurrentUser.Id)
                can = false;
            return can;
        }

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
            if (CurrentProduct != null)
            {
                CurrentProduct.Save();
                if (CurrentUser != null)
                {
                    OnDone("Product will be posted after passing verification.");
                    mediator.Notify("ProductAdditionByUser", CurrentProduct);
                }
                else
                {
                    mediator.Notify("ProductRemovalByAdmin", CurrentProduct);                  
                    OnDone("Product Offered.");                   
                    mediator.Notify("ProductAdditionByAdmin", CurrentProduct);
                    CallTimer(CurrentProduct);
                }
            }
        }

        bool CanSave(object obj)
        {
            bool can = false;
            if (CurrentProduct.ProductName.Length > 0 && CurrentProduct.Description.Length > 0 && CurrentProduct.StartingBid > 0.0m)
                can = true;
            return can;
        }

        void CallTimer(Product product)
        {
            if (product.Timer != null)
            {
                product.Timer.Stop();
                product.Timer.Tick -= Tmr_Tick;
                product.Timer = null;              
            }
            product.Timer = new DispatcherTimer();
            product.Timer.Tick += new EventHandler(Tmr_Tick);
            product.Timer.Interval = new TimeSpan(0, 0, 1);
            product.OfferTime = TimeSpan.FromSeconds(120);           
            product.Timer.Start();    
        }
       
        private void Tmr_Tick(object sender, EventArgs e)
        {
            if (CurrentProduct.OfferTime > TimeSpan.Zero)
            {
                CurrentProduct.OfferTime = CurrentProduct.OfferTime.Subtract(TimeSpan.FromSeconds(1));
                mediator.Notify("ProductChange", CurrentProduct);
            }
            else
            {
                CurrentProduct.Timer.Stop();
                CurrentProduct.Timer = null;
                CurrentProduct.Remove();
                mediator.Notify("ProductRemoval", CurrentProduct);
                if (!newCountdown)
                {
                    if (CurrentUser != null)
                    {
                        if (CurrentUser.Id == CurrentProduct.UserBuyerId)
                            OnDone(string.Format("Product {0}, auction no: {1}\nGoing, going, gone to {2}.\nCongratulations!", CurrentProduct.ProductName, CurrentProduct.Id, CurrentUser.UserName));
                    }
                }
            }      
        }

        private ICommand deleteCommand;

        public ICommand DeleteCommand
        {
            get { return deleteCommand; }
            set
            {
                if (deleteCommand == value)
                {
                    return;
                }
                deleteCommand = value;
                OnPropertyChanged("DeleteCommand");
            }
        }

        void DeleteExecute(object obj)
        {
            CurrentProduct.Delete();
            OnDone("Product Deleted.");
            mediator.Notify("ProductDeletion", CurrentProduct);
        }

        bool CanDelete(object obj)
        {
            if (CurrentProduct == null) return false;
            return true;
        }       
        #endregion
    }
}