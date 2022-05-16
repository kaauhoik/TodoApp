using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using ToDoApp.Models;
using ToDoApp.Repo;

namespace ToDoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        //Commandit
        public static RoutedUICommand AddCommand = new RoutedUICommand("add", "addCommand", typeof(Window), new InputGestureCollection { new KeyGesture(Key.Enter) });
        public static RoutedUICommand DeleteCommand = new RoutedUICommand("delete","deleteCommand", typeof(Window), new InputGestureCollection { new KeyGesture(Key.Delete)});
        public static RoutedUICommand CheckedCommand = new RoutedUICommand();

        //Propertyt
        private ObservableCollection<TodoItem> _itemsList = new ObservableCollection<TodoItem>();
        private readonly DataAccess db = new DataAccess();
        public ObservableCollection<TodoItem> ItemsList
        {
            get { return _itemsList; }
            set { 
                _itemsList = value;
                NotifyPropertyChanged(nameof(ItemsList));
            }

        }
        //Main
        public MainWindow()
        {
            this.DataContext = this;
            InitializeComponent();
            InitCommands();
            InitData();
            lbLista.ItemsSource = ItemsList;
        }
        //Haetaan kannasta lista todoitemeistä
        private void InitData()
        {
            List<TodoItem> todoitems = db.GetItems();
            ItemsList = new ObservableCollection<TodoItem>(todoitems);   
       
        }
        
        //alustetaan commandit
        private void InitCommands()
        {
            this.CommandBindings.Add(new CommandBinding(AddCommand, AddCommandExecuted, AddCommandCanExecute));
            this.CommandBindings.Add(new CommandBinding(DeleteCommand, DeleteCommandExecuted, DeleteCommandCanExecute));
            this.CommandBindings.Add(new CommandBinding(CheckedCommand, CheckedCommandExecuted));
        }


        #region Executed and canExecute
        private void CheckedCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            db.SaveChanges(ItemsList.ToList());
        }

        private void DeleteCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (lbLista.SelectedItem != null) e.CanExecute = true;
        }

        private void DeleteCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {

            db.DeleteItem((TodoItem)lbLista.SelectedItem);
            ItemsList.Remove((TodoItem)lbLista.SelectedItem);
        }

        private void AddCommandCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (tbDescription.Text.Length > 0) e.CanExecute = true;
        }

        private void AddCommandExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            TodoItem item = new TodoItem();
            int maxId = 0;

            if (ItemsList.Count > 0) maxId = ItemsList.MaxBy(x => x.Id).Id;

            item.Id = ++maxId;
            item.Description = tbDescription.Text;
            item.IsDone = false;
            ItemsList.Add(item);
            db.AddItem(item);
        }

        #endregion

        //Notifypropertchanged interfacen toteutus
        public event PropertyChangedEventHandler? PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
