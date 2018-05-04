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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public delegate void AccountStateHandler(string message);
        AccountStateHandler _del;
        public void RegisterHandler(AccountStateHandler del)
        {
            _del = del;
        }


        private Window window;
        private User user;
        private Wallet wallet;
        private int countMoney;
        public MainPage(Window window, User user)
        {
            InitializeComponent();
            this.window = window;
            this.user = user;
            nameBlock.Text = user.FullName;
            using (var context = new UserContext())
            {
                countMoney = context.Wallets.SingleOrDefault(w => w.UserId == user.Id).Count;
                countManeys.Text = countMoney + " " + ;
            }
        }

        private void ReplenishButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterHandler(new AccountStateHandler(Show_Message));
            using (var context = new UserContext())
            {
                countMoney += int.Parse(replenishBox.Text);

                 if (_del != null)
                    _del($"Сумма {replenishBox.Text} пополнино на счета");

                 wallet = context.Wallets.SingleOrDefault(w => w.UserId == user.Id);
                 wallet.Count = countMoney;
                 context.Wallets.Add(wallet);
                 context.SaveChanges();
            }
        }

        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            RegisterHandler(new AccountStateHandler(Show_Message));
            using (var context = new UserContext())
            {
                if (int.Parse(replenishBox.Text) <= countMoney)
                {
                    countMoney -= int.Parse(replenishBox.Text);

                    if (_del != null)
                        _del($"Сумма {replenishBox.Text} снято с счета");

                    wallet = context.Wallets.SingleOrDefault(w => w.UserId == user.Id);
                    wallet.Count = countMoney;
                    context.Wallets.Add(wallet);
                    context.SaveChanges();
                }
                else
                {
                    if (_del != null)
                        _del("Недостаточно денег на счете");
                }
            }
        }

        private void replenishBox_GotFocus(object sender, RoutedEventArgs e)
        {
            replenishBox.Text = "";
        }

        private void outputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            outputBox.Text = "";
        }
        private static void Show_Message(String message)
        {
            MessageBox.Show(message);
        }
    }
}
