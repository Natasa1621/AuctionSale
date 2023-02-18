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
    /// Interaction logic for Offer_DeleteWindow.xaml
    /// </summary>
    public partial class BidOfferDeleteWindow : Window
    {
        public BidOfferDeleteWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SellBidOfferDeleteWindowVM viewModel = (SellBidOfferDeleteWindowVM)DataContext;
            viewModel.Done += ViewModel_Done;
        }

        private void ViewModel_Done(object sender, DonePropertyChangedEvents.DoneEventArgs e)
        {          
            MessageBox.Show(this, e.Message);
            this.Close();
        }
    }
}