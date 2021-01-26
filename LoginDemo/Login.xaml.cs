using LoginDemo.Servcices;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LoginDemo
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        static private IWebScrapingService _webScrapingService;

        public Login()
        {
            InitializeComponent();
            _webScrapingService = new WebScrapingService();
        }

        private void DoLogin(object sender, RoutedEventArgs e)
        {
            //TODO: sanatize and verefy inputed strings
            //validation and sanatize stuff right here            

            if (string.IsNullOrEmpty(userPasswordBox.Password) || string.IsNullOrEmpty(userTextBox.Text))
            {
                MessageBox.Show("Fill all the fields!", "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }

            _webScrapingService.Navigate(userTextBox.Text, userPasswordBox.Password);
            //GoToHomeWindow();
        }

        private void GoToHomeWindow()
        {
            var homeWindow = new Home();
            homeWindow.Show();
            this.Close();
        }
    }
}
