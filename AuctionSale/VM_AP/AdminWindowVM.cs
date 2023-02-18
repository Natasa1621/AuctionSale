using Model_AP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace VM_AP
{
    public class AdminWindowVM : INotifyPropertyChanged
    {
        #region Fields

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

        public AdminWindowVM(Mediator mediator) 
        {            
            ProductList = ProductCollection.GetAllNewProducts();
            ProductListView = new ListCollectionView(ProductList);
            ProductListView.Filter = ProductFilter;

            this.mediator = mediator;           
            mediator.Register("ProductAdditionByUser", ProductAdded);           
            mediator.Register("ProductDeletion", ProductGoneAway);             
            mediator.Register("ProductRemovalByAdmin", ProductGoneAway);             
        }

        private bool ProductFilter(object obj)
        {
            if (FilteringText == null) return true;
            if (FilteringText.Equals("")) return true;

            Product product = obj as Product;
            return (product.ProductName.ToLower().StartsWith(FilteringText.ToLower()));
        }

        private void ProductAdded(object obj)
        {
            Product product = (Product)obj;
            ProductList.Add(product);
        }

        private void ProductGoneAway(object obj)
        {
            Product product = (Product)obj;
            ProductList.Remove(product);
        }       
        #endregion        
    }
}