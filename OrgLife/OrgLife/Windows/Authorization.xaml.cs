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
using System.Data.Entity;

namespace OrgLife.Windows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            base.OnClosed(e);
            App.Current.Shutdown();
            Environment.Exit(0);
        }

        private void BtnRegis_Click(object sender, RoutedEventArgs e)
        {
            Authorization LF = new Authorization();

            Windows.NewUser NU = new Windows.NewUser();
            NU.Show();
            LF.Close();
        }

        private void EnterApp_Click(object sender, RoutedEventArgs e)
        {
            EnterApp();
        }
        private void EnterKey_Click(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                EnterApp();
            }
        }
        private async void EnterApp()
        {
            try
            {
                using (Models.OrganizerDB context = new Models.OrganizerDB())
                {
                    var user = await context.User.FirstOrDefaultAsync(s => s.UserName == Username.Text.Replace(" ", ""));
                    if (user != null)
                    {
                        if (Crypter.Decrypt(user.Password) == UserPassword.Password.ToString().Replace(" ", ""))
                        {
                            Classes.SelectUser.SelectUserID = user.UserID;
                            MainWindow MainWnd = new MainWindow();
                            MainWnd.Show();
                            this.Close();
                        }
                        else
                            MessageBox.Show("Неправильный пароль");
                    }
                    else
                        MessageBox.Show("Такого пользователя не существует");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnСurtailWnd_CLick(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
