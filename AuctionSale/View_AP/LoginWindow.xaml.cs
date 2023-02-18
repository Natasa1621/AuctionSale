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
using System.Windows.Shapes;
using VM_AP;

namespace View_AP
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginWindowVM viewModel = (LoginWindowVM)DataContext;
            viewModel.Done += ViewModel_Done;
        }

        private void ViewModel_Done(object sender, LoginWindowVM.DoneEventArgs e)
        {
            MessageBox.Show(e.Message);

            if (e.Message == (String.Format("Welcome admin {0}.", ((LoginWindowVM)this.DataContext).UserName)))
            {
                AdminWindow adminWindow = new AdminWindow();
                adminWindow.Show();
                this.Close();
            }               
            else if (e.Message == (String.Format("Welcome {0}.", ((LoginWindowVM)this.DataContext).UserName)))
            {
                LoginWindowVM viewModel = (LoginWindowVM)DataContext;
                MainWindow mainWindow = new MainWindow();
                mainWindow.DataContext = new MainWindowVM(viewModel.GetUser(), Mediator.Instance);
                mainWindow.ShowDialog();
                this.Close();
            }               
            else
            {
                passwordPassBox.Password = "";
                userNameTxtBox.Text = "";
            }
        }
        
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            { ((LoginWindowVM)this.DataContext).Password = ((PasswordBox)sender).Password; }
        }
    }
}