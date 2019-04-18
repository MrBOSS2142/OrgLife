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
using System.Data;
using System.Data.Entity;

namespace OrgLife.Windows
{
    /// <summary>
    /// Логика взаимодействия для Business.xaml
    /// </summary>
    public partial class Business : UserControl
    {
        Models.OrganizerDB context = new Models.OrganizerDB();
        public Business()
        {
            InitializeComponent();
            BusinessDatePicker.SelectedDate = DateTime.Today;
            FillTable();
        }

        private void FillTable()
        {
            context.Business.Load();
            businessDataGrid.ItemsSource = context.Business.Local.ToBindingList().Where(p => p.User == Classes.SelectUser.SelectUserID);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {

            // Не загружайте свои данные во время разработки.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Загрузите свои данные здесь и присвойте результат CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

        private void AddBusiness_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string emptyRichTxtBox = new TextRange(BusinessTextWork.Document.ContentStart, BusinessTextWork.Document.ContentEnd).Text;

                if (emptyRichTxtBox.Length - 2 == 0)
                {
                    MessageBox.Show("Текстовое поле пусто!");
                    return;
                }

                Models.Business business = new Models.Business();
                business.State = StateBox.Text;
                business.Date = BusinessDatePicker.SelectedDate.ToString().Remove(10, 8);
                business.Person = BusinessPerson.Text;
                business.TextWork = new TextRange(BusinessTextWork.Document.ContentStart, BusinessTextWork.Document.ContentEnd).Text;
                business.User = Classes.SelectUser.SelectUserID;
                context.Business.Add(business);
                context.SaveChanges();
                FillTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangeBusiness_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.Business business = context.Business.Find(ind);
                string emptyRichTxtBox = new TextRange(BusinessTextWork.Document.ContentStart, BusinessTextWork.Document.ContentEnd).Text;

                if (emptyRichTxtBox.Length - 2 == 0)
                {
                    MessageBox.Show("Текстовое поле пусто!");
                    return;
                }
                business.State = StateBox.Text;
                business.Date = BusinessDatePicker.SelectedDate.ToString().Remove(10, 8);
                business.Person = BusinessPerson.Text;
                business.TextWork = new TextRange(BusinessTextWork.Document.ContentStart, BusinessTextWork.Document.ContentEnd).Text;
                context.SaveChanges();
                FillTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteBusiness_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.Business bus = context.Business.Find(ind);
                context.Business.Remove(bus);
                context.SaveChanges();
                FillTable();
                DeleteBusiness.IsEnabled = false;
                BusinessPerson.Text = "";
                FlowDocument flow = new FlowDocument(new Paragraph(new Run("")));
                BusinessTextWork.Document = flow;
                FillTable();
                DeleteBusiness.IsEnabled = false;
                ChangeBusiness.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MaxRichTxtBoxLenght(object sender, KeyEventArgs e)
        {
            TextRange tr = new TextRange(BusinessTextWork.Document.ContentStart, BusinessTextWork.Document.ContentEnd);
            if (tr.Text.Length > 60)
            {
                e.Handled = true;
                return;
            }
        }

        int ind = -1;
        private void BusinessRecord(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGrid dg = sender as DataGrid;
                Models.Business row = (Models.Business)dg.SelectedItems[0];
                ind = Convert.ToInt32(row.ID_Business.ToString());
                using (Models.OrganizerDB context = new Models.OrganizerDB())
                {
                    var busin = context.Business.FirstOrDefault(s => s.ID_Business == ind && s.User == Classes.SelectUser.SelectUserID);
                    if (busin != null)
                    {
                        StateBox.Text = busin.State;
                        BusinessDatePicker.SelectedDate = DateTime.Parse(busin.Date);
                        BusinessPerson.Text = busin.Person;
                        FlowDocument flow = new FlowDocument(new Paragraph(new Run(busin.TextWork)));
                        BusinessTextWork.Document = flow;
                        DeleteBusiness.IsEnabled = true;
                        ChangeBusiness.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        DateTime days;
        private void BusinessTime_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (StateBox.Text == "Выполняется")
                {
                    days = (DateTime)BusinessDatePicker.SelectedDate;
                    if ((DateTime.Today - days).Days != 0)
                    {
                        MessageBox.Show("До дедлайна осталось " + (days - DateTime.Today).Days.ToString() + " дней");
                    }
                    else
                        MessageBox.Show("Дедлайн сегодня!!!");
                }
                if (StateBox.Text == "Выполнено")
                {
                    MessageBox.Show("Дело уже выполнено");
                }
                if (StateBox.Text == "Пропущено")
                {
                    MessageBox.Show("Дедлайн уже прошел. Вы провалили дело");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
