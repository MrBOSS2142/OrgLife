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
    /// Логика взаимодействия для Notebook.xaml
    /// </summary>
    public partial class Notebook : UserControl
    {   
        public Notebook()
        {
            InitializeComponent();
            FillTable();
        }

        public void FillTable()
        {
            using (Models.OrganizerDB context = new Models.OrganizerDB())
            {
                context.Notebook.Load();
                notebookDataGrid.ItemsSource = context.Notebook.Local.ToBindingList().Where(p => p.User == Classes.SelectUser.SelectUserID);
            }
        }

        private void AddRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string emptyRichTxtBox = new TextRange(NoteText.Document.ContentStart, NoteText.Document.ContentEnd).Text;

                if (NoteTitle.Text == "")
                {
                    MessageBox.Show("Заголовок пуст!");
                    return;
                }

                if (emptyRichTxtBox.Length - 2 == 0)
                {
                    MessageBox.Show("Текстовое поле пусто!");
                    return;
                }

                using (Models.OrganizerDB context = new Models.OrganizerDB())
                {

                    Models.Notebook NewNote = new Models.Notebook();
                    NewNote.HeaderNB = NoteTitle.Text;
                    NewNote.TextNotebook = new TextRange(NoteText.Document.ContentStart, NoteText.Document.ContentEnd).Text;
                    NewNote.DateTime = DateTime.Today.ToString().Remove(10, 8);
                    NewNote.User = Classes.SelectUser.SelectUserID;
                    context.Notebook.Add(NewNote);
                    context.SaveChanges();
                }
                FillTable();
                ChangeNote.IsEnabled = false;
                DeleteNote.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        int index = -1;
        private void notebookDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e) // получение индекса выделенной строки и запись инф-и в окно
        {
            try
            {
                DataGrid dg = sender as DataGrid;
                Models.Notebook row = (Models.Notebook)dg.SelectedItems[0];
                index = Convert.ToInt32(row.ID_NB.ToString());
                using (Models.OrganizerDB context = new Models.OrganizerDB())
                {
                    var notebook = context.Notebook.FirstOrDefault(s => s.ID_NB == index && s.User==Classes.SelectUser.SelectUserID);
                    if (notebook != null)
                    {
                        NoteTitle.Text = notebook.HeaderNB;
                        FlowDocument flow = new FlowDocument(new Paragraph(new Run(notebook.TextNotebook)));
                        NoteText.Document = flow;
                        ChangeNote.IsEnabled = true;
                        DeleteNote.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ChangeRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Models.OrganizerDB context = new Models.OrganizerDB())
                {
                    Models.Notebook notebook = context.Notebook.Find(index);

                    notebook.HeaderNB = NoteTitle.Text;
                    notebook.TextNotebook = new TextRange(NoteText.Document.ContentStart, NoteText.Document.ContentEnd).Text;
                    notebook.DateTime = DateTime.Today.ToString().Remove(10, 8);
                    context.SaveChanges();
                }
                FillTable();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (Models.OrganizerDB context = new Models.OrganizerDB())
                {
                    Models.Notebook notebook = context.Notebook.Find(index);
                    context.Notebook.Remove(notebook);
                    context.SaveChanges();
                }
                FillTable();
                ChangeNote.IsEnabled = false;
                DeleteNote.IsEnabled = false;

                NoteTitle.Text = "";
                FlowDocument flow = new FlowDocument(new Paragraph(new Run("")));
                NoteText.Document = flow;
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

        private void SearchNote_Click(object sender, RoutedEventArgs e)
        {
            using (Models.OrganizerDB context = new Models.OrganizerDB())
            {
                context.Notebook.Load();
                notebookDataGrid.ItemsSource = context.Notebook.Local.ToBindingList().Where(p => p.User == Classes.SelectUser.SelectUserID).Where(p => p.HeaderNB.Contains(NoteSearch.Text));
            }
        }
    }
}
