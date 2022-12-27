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
using System.Windows.Threading;

namespace Project
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            input_number.IsEnabled = true;
            input_password.IsEnabled = false;
            CodeBox.IsEnabled = false;
            RefreshBtn.IsEnabled = false;
            EnterBtn.IsEnabled = false;
            input_number.Focus();
        }

        DispatcherTimer timer = new DispatcherTimer();

        string code;

        private void gencode()
        {
            code = null;
            Random rnd = new Random();
            string[] MassiveCode = new string[] 
            { "Q", "W", "E", "R","T","Y","U","I","O","P" };
            for (int i = 0; i< 4; i++)
                code +=MassiveCode[rnd.Next(MassiveCode.Length)];
            if(MessageBox.Show(code.ToString(),"Code", MessageBoxButton.OK, MessageBoxImage.Warning) == MessageBoxResult.OK)
            {
                timer.Interval = TimeSpan.FromSeconds(10);
                timer.Tick += Timer_tick;
                timer.Start();
                input_password.IsEnabled = true;
                CodeBox.IsEnabled = true;
                RefreshBtn.IsEnabled = true;
            }
        }

        void Timer_tick(object sender, EventArgs e)
        {
            code = null;
            MessageBox.Show("Код сброшен");
            timer.Stop();
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            gencode();
            CodeBox.Focus();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void EnterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (code == CodeBox.Text)
            {
                timer.Stop();
                if(input_number.Text == "1")
                {
                    MessageBox.Show("Вы админ");
                }
                if (input_number.Text == "2")
                {
                    MessageBox.Show("Вы пользователь");
                }
            }
            else
            {
                timer.Stop();
                MessageBox.Show("Код неправильный");
            }
        }

        private void input_number_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                using(var db=new ProjectEntities())
                {
                    var number = db.Users.FirstOrDefault(u => u.Number == input_number.Text);
                    if (number == null)
                    {
                        MessageBox.Show("Пользователь не найден");
                    }
                    else
                    {
                        input_number.IsEnabled = false;
                        input_password.IsEnabled = true;
                        input_password.Focus();
                    }
                }
            }
        }

        private void input_password_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                using (var db = new ProjectEntities())
                {
                    var passwords = db.Users.FirstOrDefault(u => u.Number == input_number.Text & (u.Password == input_password.Password));
                    if (passwords == null)
                    {
                        MessageBox.Show("пароль не найден");
                    }
                    else
                    {
                        input_password.IsEnabled = false;
                        CodeBox.IsEnabled = true;
                        EnterBtn.IsEnabled = true;
                        gencode();
                        CodeBox.Focus();
                    }
                }
            }
        }
    }
}
