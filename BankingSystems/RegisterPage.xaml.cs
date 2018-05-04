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
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        private Window window;
        public RegisterPage(Window window)
        {
            InitializeComponent();
            this.window = window;
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new UserContext())
            {
                var login = context.Users.Where(u => u.Login == loginBox.Text);
                if (login != null)
                {
                    User user = new User() { FullName = fullNameBox.Text, Login = loginBox.Text, Password = passwordBox.Password, CreationDate = DateTime.Now };
                    context.Users.Add(user);
                    context.SaveChanges();

                    int id = context.Users.SingleOrDefault(u => u.Login == loginBox.Text).Id;
                    Wallet wallet = new Wallet() { Count = 0, CourseType = "KZT", UserId = id };
                    context.Wallets.Add(wallet);
                    context.SaveChanges();

                    window.Content = new LoginPage(window);
                }
                else
                {
                    MessageBox.Show("Такой логин уже существует ведите другой логин!!!");
                }
            }
        }
    }
}
