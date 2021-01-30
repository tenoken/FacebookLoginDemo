using LoginDemo.Servcices;
using System.Windows;

namespace LoginDemo
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        static private IWebScrapingService _webScrapingService;

        public Login(IWebScrapingService webScrapingService)
        {
            InitializeComponent();
            _webScrapingService = webScrapingService;
        }

        private void DoLogin(object sender, RoutedEventArgs e)
        {
            //TODO: sanatize and verefy inputed strings
            //validation and sanatize stuff right here       

            if (!IsInputsFilled())
               return;

            var homePageDocument = _webScrapingService.GetHomePageDocument(userTextBox.Text, userPasswordBox.Password);

            MessageBox.Show("The application successfully logged into facebook.", "Login Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool IsInputsFilled()
        {
            if (string.IsNullOrEmpty(userPasswordBox.Password) || string.IsNullOrEmpty(userTextBox.Text))
            {
                MessageBox.Show("It is necessary fill all fields!", "Validation Issues", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }

            return true;
        }
    }
}
