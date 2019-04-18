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
using System.Data.Entity;
using System.Data;

namespace OrgLife.Windows
{
    /// <summary>
    /// Логика взаимодействия для Event.xaml
    /// </summary>
    public partial class Event : UserControl
    {        
        public Event()
        {
            InitializeComponent();
            DateEvent.SelectedDate = DateTime.Today;
            FillTable();
        }

        public void FillTable()
        {
            using (Models.OrganizerDB dc = new Models.OrganizerDB())
            {
                dc.Event.Load();
                eventDataGrid.ItemsSource = dc.Event.Local.ToBindingList().Where(p => p.User == Classes.SelectUser.SelectUserID);
            }
        }

        int index = -1;
        private void EventDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGrid dg = sender as DataGrid;
                Models.Event row = (Models.Event)dg.SelectedItems[0];
                index = Convert.ToInt32(row.ID_Event.ToString());
                DeleteEvent.IsEnabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private async void AddEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string emptyRichTxtBox = new TextRange(EventText.Document.ContentStart, EventText.Document.ContentEnd).Text;

                if (emptyRichTxtBox.Length - 2 == 0)
                {
                    MessageBox.Show("Текстовое поле пусто!");
                    return;
                }
                using (Models.OrganizerDB dc = new Models.OrganizerDB())
                {
                    Models.Event Work = new Models.Event();
                    Work.DateEvent = DateEvent.SelectedDate.ToString().Remove(10, 8);
                    Work.Text_Event = new TextRange(EventText.Document.ContentStart, EventText.Document.ContentEnd).Text;
                    Work.User = Classes.SelectUser.SelectUserID;
                    dc.Event.Add(Work);
                    await dc.SaveChangesAsync();
                    FillTable();
                    DeleteEvent.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteEvent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Models.OrganizerDB dc = new Models.OrganizerDB())
                {
                    Models.Event work = dc.Event.Find(index);
                    dc.Event.Remove(work);
                    dc.SaveChanges();
                    FillTable();
                    DeleteEvent.IsEnabled = false;
                    FlowDocument flow = new FlowDocument(new Paragraph(new Run("")));
                    EventText.Document = flow;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
    }
}
