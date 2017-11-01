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
    public partial class TasksPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<TaskHolder> _tasks;
        private List<GroupHolder> _groups;
        private TaskHolder _activeTask;

        public event PropertyChangedEventHandler PropertyChanged;

        public TasksPage()
        {
            InitializeComponent();
            DataContext = this;
            _tasks = new ObservableCollection<TaskHolder>(MockBase.MockBase.GetTasks());
            _groups = new List<GroupHolder>(MockBase.MockBase.GetGroups());
            TasksListView.ItemsSource = _tasks;
            TaskDetails.Initialize(_activeTask, _groups);
        }

        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ActiveTask = button.Tag as TaskHolder;
            TaskDetails.Initialize(_activeTask, _groups);
        }
        private void AddNewTask_Click(object sender, RoutedEventArgs e)
        {
            var addedTask = new TaskHolder(1, "New Task");
            addedTask.StartDate = DateTime.Now;
            addedTask.Frequency = 1;
            _tasks.Insert(0, addedTask);
            ActiveTask = addedTask;
            TaskDetails.Initialize(addedTask, _groups);
        }

        public TaskHolder ActiveTask
        {
            get
            {
                return _activeTask;
            }
            set
            {
                _activeTask = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActiveTask)));
            }
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var changedTask = TaskDetails.ShownTask;
            ActiveTask.Name = changedTask.Name;
            ActiveTask.Description = changedTask.Description;
            ActiveTask.StartDate = changedTask.StartDate;
            ActiveTask.Frequency = changedTask.Frequency;

            foreach (var group in _groups)
            {
                if (changedTask.Groups.Contains(group) && !ActiveTask.Groups.Contains(group))
                {
                    group.Tasks.Add(ActiveTask);
                    ActiveTask.Groups.Add(group);
                }

                if (!changedTask.Groups.Contains(group) && ActiveTask.Groups.Contains(group))
                {
                    group.Tasks.Remove(ActiveTask);
                    ActiveTask.Groups.Remove(group);
                }
            }
        }

        private void RestetChanges_Click(object sender, RoutedEventArgs e)
        {
            TaskDetails.Initialize(ActiveTask, _groups);
        }
        private void Delete_Task(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Do you really want to delete " + ActiveTask.Name, "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ActiveTask.Archivized = true;
                ActiveTask = null;
                TaskDetails.Initialize(ActiveTask, _groups);
            }
        }
    }
}
