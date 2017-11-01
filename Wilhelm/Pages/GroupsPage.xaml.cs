using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Wilhelm.Frontend.Model;

namespace Wilhelm.Frontend.Pages
{
    /// <summary>
    /// Interaction logic for ActionTypesPage.xaml
    /// </summary>
    public partial class GroupsPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<GroupHolder> _groups;
        private List<TaskHolder> _tasks;
        private GroupHolder _activeGroup;

        public event PropertyChangedEventHandler PropertyChanged;

        public GroupsPage()
        {
            InitializeComponent();
            DataContext = this;
            _groups = new ObservableCollection<GroupHolder>(MockBase.MockBase.GetGroups());
            _tasks = new List<TaskHolder>(MockBase.MockBase.GetTasks());
            GroupsListView.ItemsSource = _groups;
            GroupDetails.Initialize(_activeGroup, _tasks);
        }

        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ActiveGroup = button.Tag as GroupHolder;
            GroupDetails.Initialize(_activeGroup, _tasks);
        }
        private void AddNewGroup_Click(object sender, RoutedEventArgs e)
        {
            var addedGroup = new GroupHolder(1, "New Group");
            _groups.Insert(0, addedGroup);
            ActiveGroup = addedGroup;
            GroupDetails.Initialize(addedGroup, _tasks);
        }

        public GroupHolder ActiveGroup
        {
            get
            {
                return _activeGroup;
            }
            set
            {
                _activeGroup = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActiveGroup)));
            }
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var changedGroup = GroupDetails.ShownGroup;
            ActiveGroup.Name = changedGroup.Name;
            ActiveGroup.Description = changedGroup.Description;

            foreach (var task in _tasks)
            {
                if (changedGroup.Tasks.Contains(task) && !ActiveGroup.Tasks.Contains(task))
                {
                    task.Groups.Add(ActiveGroup);
                    ActiveGroup.Tasks.Add(task);
                }

                if (!changedGroup.Tasks.Contains(task) && ActiveGroup.Tasks.Contains(task))
                {
                    task.Groups.Remove(ActiveGroup);
                    ActiveGroup.Tasks.Remove(task);
                }
            }
        }

        private void RestetChanges_Click(object sender, RoutedEventArgs e)
        {
            GroupDetails.Initialize(ActiveGroup, _tasks);
        }
        private void Delete_Group(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Do you really want to delete " + ActiveGroup.Name, "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ActiveGroup.Archivized = true;
                ActiveGroup = null;
                GroupDetails.Initialize(ActiveGroup, _tasks);
            }
        }
    }
}
