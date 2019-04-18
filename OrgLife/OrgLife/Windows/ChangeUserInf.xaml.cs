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

using System.IO;
using Microsoft.Win32;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace OrgLife.Windows
{
    /// <summary>
    /// Логика взаимодействия для NewUser.xaml
    /// </summary>
    public partial class ChangeUserInf : Window
    {
        public ChangeUserInf()
        {
            InitializeComponent();
            MouseLeftButtonDown += new MouseButtonEventHandler(layoutApp);
        }

        void layoutApp(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnNewUserClose_CLick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnСurtailWnd_CLick(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        string ImgPath = "";
        private void ImageChange(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Jpeg|*.jpg| Png|*.png| Bmp|*.bmp| AllFiles|*.*", ValidateNames = true, Multiselect = false };
            if (openFileDialog.ShowDialog() == true)
            {
                var brush = new ImageBrush();
                string path = openFileDialog.FileName;
                brush.ImageSource = new BitmapImage(new Uri(path));
                UserPhoto.Background = brush;
                ImgPath = path;
            }
        }

        private void BtnUserChangeInf(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Models.OrganizerDB context = new Models.OrganizerDB())
                {
                    if (!Regex.IsMatch(Username.Text, Classes.ValidationRegex.Login))
                    {
                        MessageBox.Show("Логин не соответствует формату");
                        return;
                    }

                    if (!Regex.IsMatch(userNewPassword.Text, Classes.ValidationRegex.Password))
                    {
                        MessageBox.Show("Пароль не соответствует формату");
                        return;
                    }

                    Models.User user = context.User.Find(Classes.SelectUser.SelectUserID);

                    if (Crypter.Decrypt(user.Password) == userOldPassword.Text.Replace(" ", ""))
                    {
                        user.UserName = Username.Text; // Считываем логин

                        user.Password = (Crypter.Encrypt(userNewPassword.Text)).ToString(); // считываем и зашифровываем пароль

                        if (ImgPath != "")
                        {
                            user.UserPhoto = ImageFunc.ConvertImageToBinary(ImgPath);
                        }
                        context.SaveChanges();
                        MessageBox.Show("Информация успешно изменена");
                    }
                    else
                    {
                        MessageBox.Show("Введен неверный пароль.", "Проверьте правильность ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Изменить данные " + Username.Text + " не удалось, ошибка: " + ex.Message);
            }
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            using (Models.OrganizerDB context = new Models.OrganizerDB())
            {
                Models.User user = context.User.Find(Classes.SelectUser.SelectUserID);
                var brush = new ImageBrush();
                brush.ImageSource = ImageFunc.ConvertImageFromBinary(user.UserPhoto);
                UserPhoto.Background = brush;
                Username.Text = user.UserName;
            }
        }
    }
}
