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
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace OrgLife.Windows
{
    /// <summary>
    /// Логика взаимодействия для NewContact.xaml
    /// </summary>
    public partial class NewContact : Window
    {
        public NewContact()
        {
            InitializeComponent();
            DOB.SelectedDate = DateTime.Today;
            GridUp.MouseLeftButtonDown += new MouseButtonEventHandler(layoutApp);
        }

        void layoutApp(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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
                PersonPhoto.Background = brush;
                ImgPath = path;
            }
        }

        private void BtnContactAdd(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Models.OrganizerDB context = new Models.OrganizerDB())
                {
                    Models.Person pers = new Models.Person();

                    if (ImgPath != "")
                    { 
                        pers.PersonPhoto = ImageFunc.ConvertImageToBinary(ImgPath);
                    }
                    else
                    {
                        string fileName = "user3.png";
                        string fullPath;
                        fullPath = System.IO.Path.GetFullPath(fileName);
                        pers.PersonPhoto = ImageFunc.ConvertImageToBinary(fullPath);                       
                    }

                    if (Name.Text != "")
                    {
                        if (Regex.IsMatch(Name.Text, Classes.ValidationRegex.NameRegex))
                        {
                            pers.Name = Name.Text;
                        }
                        else
                        {
                            MessageBox.Show("Неверный формат имени");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Поле Имя должно быть заполнено");
                        return;
                    }

                    if (LastName.Text != "")
                    {
                        if (Regex.IsMatch(LastName.Text, Classes.ValidationRegex.NameRegex))
                        {
                            pers.Lastname = LastName.Text;
                        }
                        else
                        {
                            MessageBox.Show("Неверный формат фамилии");
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Поле Фамилия должно быть заполнено");
                        return;
                    }

                    if (Patronymic.Text != "")
                        pers.Patronymic = Patronymic.Text;
                    else
                        pers.Patronymic = "";

                    pers.Gender = Gender.Text.Remove(1);

                    pers.DOB = DOB.SelectedDate.ToString().Remove(10, 8);

                    if (NickName.Text != "")
                        pers.NickName = NickName.Text;
                    else
                        pers.NickName = "";

                    if (PhoneNumber.Text != "")
                    {
                        if (Regex.IsMatch(PhoneNumber.Text, Classes.ValidationRegex.PhoneRegex))
                        {
                            pers.Phone_number = PhoneNumber.Text;
                        }
                        else
                        {
                            MessageBox.Show("Неверный формат номера телефона");
                            return;
                        }
                    }
                    else
                        pers.Phone_number = "";

                    if (Position.Text != "")
                        pers.Position = Position.Text;
                    else
                        pers.Position = "";
                    if (Email.Text != "")
                    {
                        if (Regex.IsMatch(Email.Text, Classes.ValidationRegex.EmailRegex))
                            pers.Email = Email.Text;
                        else
                        {
                            MessageBox.Show("Неверный формат Email");
                            return;
                        }
                    }
                    else
                        pers.Email = "";
                    if (Adress.Text != "")
                        pers.AdressPerson = Adress.Text;
                    else
                        pers.AdressPerson = "";
                    pers.User = Classes.SelectUser.SelectUserID;
                    context.Person.Add(pers);
                    context.SaveChanges();
                    MessageBox.Show("Контакт " + Name.Text + " " + LastName.Text + " добавлен");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnСurtailWnd_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
