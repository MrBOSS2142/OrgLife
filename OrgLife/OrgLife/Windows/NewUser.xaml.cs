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
using System.Data.Entity;

namespace OrgLife.Windows
{
    /// <summary>
    /// Логика взаимодействия для NewUser.xaml
    /// </summary>
    public partial class NewUser : Window
    {
        public NewUser()
        {
            InitializeComponent();
        }

        private void BtnNewUserClose_CLick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnUserAdd(object sender, RoutedEventArgs e)
        {
            int CheckingLoginExistance = 0;
            try
            {
                using (Models.OrganizerDB context = new Models.OrganizerDB())
                {
                    if (!Regex.IsMatch(Username.Text, Classes.ValidationRegex.Login))
                    {
                        MessageBox.Show("Логин не соответствует формату");
                        return;
                    }

                    if (!Regex.IsMatch(UserAddPassword.Text, Classes.ValidationRegex.Password))
                    {
                        MessageBox.Show("Пароль не соответствует формату");
                        return;
                    }

                    var user = context.User.FirstOrDefault(s => s.UserName == Username.Text.Replace(" ", ""));
                    if (user != null)    //проверка на существование пользователя
                    {
                        CheckingLoginExistance = 1; // пользователь существует
                    }
                    if (CheckingLoginExistance == 0)
                    {
                        Models.User Newuser = new Models.User();
                        Newuser.UserName = Username.Text; // Считываем логин
                        
                        Newuser.Password = (Crypter.Encrypt(UserAddPassword.Text)).ToString(); // считываем и зашифровываем пароль
                        
                        if (ImgPath != "")
                        {
                            Newuser.UserPhoto = ImageFunc.ConvertImageToBinary(ImgPath);
                        }
                        else
                        {
                            string fileName = "user3.png";
                            string fullPath;
                            fullPath = System.IO.Path.GetFullPath(fileName);
                            Newuser.UserPhoto = ImageFunc.ConvertImageToBinary(fullPath);
                        }
                        context.User.Add(Newuser);
                        context.SaveChanges();
                        MessageBox.Show("Новый пользователь " + Username.Text + " добавлен");
                        Username.Clear();
                        UserAddPassword.Clear();
                    }
                    else
                    {
                        MessageBox.Show("Пользователь " + Username.Text + " существует. Пожалуйста, придумайте другой логин.", "Повтор логина", MessageBoxButton.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Новый пользователь " + Username.Text + "не добавлен!!!б, ошибка: " + ex.Message);
            }
        }

        private void BtnСurtailWnd_CLick(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        string ImgPath = "";
        private void ImageChange(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog() { Filter = "Jpeg|*.jpg| Png|*.png| Bmp|*.bmp| AllFiles|*.*", ValidateNames=true, Multiselect=false };
            if (openFileDialog.ShowDialog() == true)
            {
                var brush = new ImageBrush();
                string path = openFileDialog.FileName;
                brush.ImageSource = new BitmapImage(new Uri(path));
                UserPh.Background = brush;
                ImgPath = path;
            }
        }
    }
}
