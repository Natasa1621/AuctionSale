using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SellWindow.xaml
    /// </summary>
    public partial class SellWindow : Window
    {
        public SellWindow()
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
            MessageBox.Show(e.Message);
            this.Close();
        }

        private void BtnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                Uri fileUri = new Uri(openFileDialog.FileName);                                     
                BitmapImage imgSource = new BitmapImage(fileUri);              
                img.Source = imgSource;

                byte[] buffer = File.ReadAllBytes(openFileDialog.FileName);
                if (this.DataContext != null)
                    ((SellBidOfferDeleteWindowVM)this.DataContext).CurrentProduct.Image = buffer;
            }
        }
    }
}