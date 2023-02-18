using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using VM_AP;

namespace View_AP
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            AdminWindowVM adminVM = new AdminWindowVM(Mediator.Instance);
            this.DataContext = adminVM;
        }
      
        private void BtnLogout_Click(object sender, RoutedEventArgs e) //???
        {          
            this.Close();
        }
        
        private void BtnProductName_Click(object sender, RoutedEventArgs e)
        {
            ListBoxItem selectedItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromItem(((Button)sender).DataContext);
            selectedItem.IsSelected = true;

            AdminWindowVM viewModel = (AdminWindowVM)DataContext;
            BidOfferDeleteWindow bodWindow = new BidOfferDeleteWindow();
            bodWindow.DataContext = new SellBidOfferDeleteWindowVM(viewModel.CurrentProduct, Mediator.Instance);
            bodWindow.bidderButtons.Visibility = Visibility.Collapsed;
            bodWindow.ShowDialog();
            
        }
    }
}