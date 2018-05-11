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

namespace BankingSystems
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        private Window window;
        private User user;
        private Wallet wallet;
        public LoginPage(Window window)
        {
            InitializeComponent();
            this.window = window;
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new UserContext())
            {
                user = context.Users.SingleOrDefault(u => u.Login == loginBox.Text && u.Password == passwordBox.Password);
                
                if (user != null)
                {
                    window.Content = new MainPage(window, user);
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль");
                }
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            window.Content = new RegisterPage(window);
        }
    }
}
