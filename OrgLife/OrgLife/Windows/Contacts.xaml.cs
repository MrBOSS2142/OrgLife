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

namespace OrgLife.Windows
{
    /// <summary>
    /// Логика взаимодействия для Contacts.xaml
    /// </summary>
    public partial class Contacts : UserControl
    {
        public Contacts()
        {
            InitializeComponent();
            FillTable();
        }

        public void FillTable()
        {
            using (Models.OrganizerDB context = new Models.OrganizerDB())
            {
                context.Person.Load();
                personDataGrid.ItemsSource = context.Person.Local.ToBindingList().Where(p => p.User == Classes.SelectUser.SelectUserID);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            // Не загружайте свои данные во время разработки.
            // if (!System.ComponentModel.DesignerProperties.GetIsInDesignMode(this))
            // {
            // 	//Загрузите свои данные здесь и присвойте результат CollectionViewSource.
            // 	System.Windows.Data.CollectionViewSource myCollectionViewSource = (System.Windows.Data.CollectionViewSource)this.Resources["Resource Key for CollectionViewSource"];
            // 	myCollectionViewSource.Source = your data
            // }
        }

        private void ButtonOpenNewContact_Click(object sender, RoutedEventArgs e)
        {
            Windows.NewContact newc = new NewContact();
            newc.Show();
        }

        int index = -1;
        private void PersonDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGrid dg = sender as DataGrid;
                Models.Person row = (Models.Person)dg.SelectedItems[0];
                index = Convert.ToInt32(row.ID_Person.ToString());
                Classes.SelectContact.SelectContactID = index;
                ShowContact show = new ShowContact();
                show.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearchPerson_Click(object sender, RoutedEventArgs e)
        {
            using (Models.OrganizerDB context = new Models.OrganizerDB())
            {
                context.Person.Load();
                personDataGrid.ItemsSource = context.Person.Local.ToBindingList()
                .Where(p => p.User == Classes.SelectUser.SelectUserID
                && p.Lastname.Contains(PersonSearch.Text) || p.Name.Contains(PersonSearch.Text));
            }
        }

        private void RefreshTable_Click(object sender, RoutedEventArgs e)
        {
            FillTable();
        }
    }
}
