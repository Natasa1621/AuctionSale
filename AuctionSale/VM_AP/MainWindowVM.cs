using Model_AP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Threading;

namespace VM_AP
{
    public class MainWindowVM : INotifyPropertyChanged
    {
        #region Fields

        private User currentUser;
        private Product currentProduct;
        private ProductCollection productList;
        private ListCollectionView productListView;
        private string filteringText;
        private Mediator mediator;
        
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
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

        public ProductCollection ProductList
        {
            get { return productList; }
            set
            {
                if (productList == value)
                {
                    return;
                }
                productList = value;
                OnPropertyChanged("ProductList");
            }
        }

        public ListCollectionView ProductListView
        {
            get { return productListView; }
            set
            {
                if (productListView == value)
                {
                    return;
                }
                productListView = value;
                OnPropertyChanged("ProductListView");
            }
        }

        public String FilteringText
        {
            get { return filteringText; }
            set
            {
                if (filteringText == value)
                {
                    return;
                }
                filteringText = value;
                ProductListView.Refresh();
                OnPropertyChanged("FilteringText");               
            }
        }
        #endregion

        #region Constructors

        public MainWindowVM(Mediator mediator)
        {
            CurrentProduct = new Product();
            ProductList = ProductCollection.GetAllOfferedProducts();
            ProductListView = new ListCollectionView(ProductList);
            ProductListView.Filter = ProductFilter;

            this.mediator = mediator;
            mediator.Register("ProductAdditionByAdmin", ProductOffered);           
            mediator.Register("ProductRemoval", ProductGoneAway);           
            mediator.Register("ProductChange", ProductRefreshed);           
        }

        public MainWindowVM(User user, Mediator mediator)
        {
            CurrentUser = user;
            CurrentProduct = new Product();
            ProductList = ProductCollection.GetAllOfferedProducts();
            ProductListView = new ListCollectionView(ProductList);
            ProductListView.Filter = ProductFilter;

            this.mediator = mediator;
            mediator.Register("ProductAdditionByAdmin", ProductOffered); 
            mediator.Register("ProductRemoval", ProductGoneAway);
            mediator.Register("ProductChange", ProductRefreshed);
        }

        private bool ProductFilter(object obj)
        {
            if (FilteringText == null) return true;
            if (FilteringText.Equals("")) return true;

            Product product = obj as Product;
            return (product.ProductName.ToLower().StartsWith(FilteringText.ToLower()));
        }

        private void ProductOffered(object obj)
        {
            Product product = (Product)obj;
            ProductList.Add(product);        
        }
       
        private void ProductGoneAway(object obj)
        {
            Product product = (Product)obj;
            ProductList.Remove(product);
        }
       
        private void ProductRefreshed(object obj)
        {
            Product product = (Product)obj;
            
            for (int i = 0; i < ProductList.Count; i++)
            {
                if (product.Id == ProductList[i].Id)
                {
                   ProductList.Remove(ProductList[i]);
                    ProductList.Insert(i, product);
                }
            }
        }       
        #endregion
    }
}