using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Threading;

namespace BankingSystems
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public delegate void AccountStateHandler(string message);
        public delegate void Result();

        AccountStateHandler _del;
        public void RegisterHandler(AccountStateHandler del)
        {
            _del = del;
        }

        private Window window;
        private User user;
        private Wallet wallet;

        private string output;
        private string replenish;

        public MainPage(Window window, User user)
        {
            InitializeComponent();
            this.window = window;
            this.user = user;
            using (var context = new UserContext())
            {
                wallet = context.Wallets.SingleOrDefault(w => w.UserId == user.Id);
                nameBlock.Text = user.FullName;
                countManeys.Text = wallet.Count + " " + wallet.CourseType;
            }
        }

        private void ReplenishButton_Click(object sender, RoutedEventArgs e)
        {
            replenish = replenishBox.Text;
            Result result = new Result(Replenish);
            IAsyncResult resultObj = result.BeginInvoke(null, null);
        }
        private void OutputButton_Click(object sender, RoutedEventArgs e)
        {
            output = outputBox.Text;
            Result result = new Result(Output);
            IAsyncResult resultObj = result.BeginInvoke(null, null);
        }

        private void Replenish()
        {
            int countMoneyReplenish;
            RegisterHandler(new AccountStateHandler(Show_Message));
            using (var context = new UserContext())
            {
                int.TryParse(replenish, out countMoneyReplenish);
                wallet.Count += countMoneyReplenish;

                if (_del != null)
                    _del($"Сумма {countMoneyReplenish} пополнино на счет");

                context.Entry(wallet).State = EntityState.Modified;
                context.SaveChanges();
            }
        }
        private void Output()
        {
            int countMoneyOutput;
            RegisterHandler(new AccountStateHandler(Show_Message));
            using (var context = new UserContext())
            {
                int.TryParse(output, out countMoneyOutput);
                if (countMoneyOutput <= wallet.Count)
                {
                    wallet.Count -= countMoneyOutput;

                    if (_del != null)
                        _del($"Сумма {countMoneyOutput} снято с счета");

                    context.Entry(wallet).State = EntityState.Modified;
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
