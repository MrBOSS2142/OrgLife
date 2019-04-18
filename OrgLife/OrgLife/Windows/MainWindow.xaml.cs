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

using System.IO;
using System.Data.Entity;

namespace OrgLife
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Models.OrganizerDB ef = new Models.OrganizerDB();
        public MainWindow()
        {            
            InitializeComponent();
            ListViewMenu.SelectedIndex = 0;
            GridUp.MouseLeftButtonDown += new MouseButtonEventHandler(layoutApp);
            Classes.CheckEvent.Check();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            var user = ef.User.FirstOrDefault(s => s.UserID == Classes.SelectUser.SelectUserID);
            if (user != null)
            {
                var brush = new ImageBrush();
                brush.ImageSource = ImageFunc.ConvertImageFromBinary(user.UserPhoto);
                UserPhoto.Background = brush;
                LoginMain.Text = user.UserName;
            }
        }

        void layoutApp(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            LoginMain.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
            LoginMain.Visibility = Visibility.Hidden;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UserControl usc = null;
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemContacts":
                    usc = new Windows.Contacts();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemNotebook":
                    usc = new Windows.Notebook();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemBusiness":
                    usc = new Windows.Business();
                    GridMain.Children.Add(usc);
                    break;
                case "ItemEvent":
                    usc = new Windows.Event();
                    GridMain.Children.Add(usc);
                    break;
                default:
                    break;
            }
        }

        private void BtnMainWindowClose_CLick(object sender, RoutedEventArgs e)
        {
            base.OnClosed(e);
            App.Current.Shutdown();
            Environment.Exit(0);
        }

        private void BtnMainWindowFullSize_CLick(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Maximized;
            btnFullSize.Visibility = Visibility.Collapsed;
            btnNormalSize.Visibility = Visibility.Visible;
        }

        private void BtnNormalSize_CLick(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Normal;
            btnFullSize.Visibility = Visibility.Visible;
            btnNormalSize.Visibility = Visibility.Collapsed;
        }

        private void BtnСurtailWnd_CLick(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void BtnUserEdit_Click(object sender, RoutedEventArgs e)
        {
            Windows.ChangeUserInf win = new Windows.ChangeUserInf();
            win.Show();
        }
        private void BtnUserOut(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Reference_Click(object sender, RoutedEventArgs e)
        {
            Windows.Reference refer = new Windows.Reference();
            refer.Show();
        }
    }
}
