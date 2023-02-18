using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VM_AP;

namespace View_AP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainWindowVM mainVM = new MainWindowVM(Mediator.Instance);
            this.DataContext = mainVM;           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        { 
            MainWindowVM viewModel = (MainWindowVM)DataContext;
            if (viewModel.CurrentUser == null)
            {
                sellBtn.IsEnabled = false;
                loginBtn.Visibility = Visibility.Visible;
                logoutBtn.Visibility = Visibility.Collapsed;
            }
            else
            {
                sellBtn.IsEnabled = true;
                logoutBtn.Visibility = Visibility.Visible;
                loginBtn.Visibility = Visibility.Collapsed;
            }                
        }

        private void BtnRegistration_Click(object sender, RoutedEventArgs e)
        {
            RegistrationWindow registrationWindow = new RegistrationWindow();
            registrationWindow.ShowDialog();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.DataContext = new LoginWindowVM();
            loginWindow.ShowDialog();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
           this.Close();
        }
        
        private void BtnProductName_Click(object sender, RoutedEventArgs e)
        {           
            ListBoxItem selectedItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromItem(((Button)sender).DataContext);
            selectedItem.IsSelected = false;
            selectedItem.IsSelected = true;

            MainWindowVM viewModel = (MainWindowVM)DataContext;
            BidOfferDeleteWindow bodWindow = new BidOfferDeleteWindow();
            if (viewModel.CurrentUser == null)
            {
                bodWindow.DataContext = new SellBidOfferDeleteWindowVM(viewModel.CurrentProduct, Mediator.Instance);
                bodWindow.btnPlaceBid.Visibility = Visibility.Collapsed;
            }
            else
                bodWindow.DataContext = new SellBidOfferDeleteWindowVM(viewModel.CurrentProduct, viewModel.CurrentUser, Mediator.Instance);
            bodWindow.adminButtons.Visibility = Visibility.Collapsed;
            bodWindow.Show();
        }

        private void BtnSell_Click(object sender, RoutedEventArgs e) 
        {
            MainWindowVM viewModel = (MainWindowVM)DataContext;
            SellWindow sellWindow = new SellWindow();
            sellWindow.DataContext = new SellBidOfferDeleteWindowVM(viewModel.CurrentUser, Mediator.Instance);
            sellWindow.ShowDialog();
        }
    }
}