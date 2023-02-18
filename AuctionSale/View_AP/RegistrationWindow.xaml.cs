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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class RegistrationWindow : Window
    {
        public RegistrationWindow()
        {
            InitializeComponent();

            RegistrationWindowVM registrationVM = new RegistrationWindowVM();
            this.DataContext = registrationVM;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            RegistrationWindowVM viewModel = (RegistrationWindowVM)DataContext;
            viewModel.Done += ViewModel_Done;
        }

        private void ViewModel_Done(object sender, DonePropertyChangedEvents.DoneEventArgs e)
        {
            MessageBox.Show(e.Message);
            if (e.Message == "You are registered. Please sign in.")
            {
                RegistrationWindowVM viewModel = (RegistrationWindowVM)DataContext;
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.DataContext = new LoginWindowVM(viewModel.CurrentUser);
                loginWindow.passwordPassBox.Password = viewModel.CurrentUser.Password;
                loginWindow.ShowDialog();
                this.Close();
            }            
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
            ((RegistrationWindowVM)this.DataContext).Password = ((PasswordBox)sender).Password; 
        }
    }
}