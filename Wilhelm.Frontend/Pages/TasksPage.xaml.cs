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
using Wilhelm.Backend.Model;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Controls;
using Wilhelm.Frontend.Model;
using Wilhelm.Frontend.Services.Interfaces;

namespace Wilhelm.Frontend.Pages
{
    public partial class TasksPage : UserControl, INotifyPropertyChanged, IMenuPage
    {
        private readonly ObservableCollection<TaskHolder> _tasks = new ObservableCollection<TaskHolder>();
        private readonly List<GroupHolder> _groups = new List<GroupHolder>();
        private TaskHolder _activeTask;
        private readonly IHoldersService _holdersService;
        private readonly IConfigurationService _configurationService;
        private TaskDetailsControl _taskDetailsControl;

        public event PropertyChangedEventHandler PropertyChanged;

        public TasksPage(IHoldersService holdersService, IConfigurationService configurationService)
        {
            _holdersService = holdersService;
            _configurationService = configurationService;

            InitializeComponent();
            _taskDetailsControl = new TaskDetailsControl(_holdersService);
            TaskDetailsContentControl.Content = _taskDetailsControl;
            DataContext = this;
        }
        public void ShowCurrentTask()
        {
            if (ActiveTask == null)
                TaskButtonsPanel.Visibility = Visibility.Hidden;
            else
                TaskButtonsPanel.Visibility = Visibility.Visible;

            _taskDetailsControl.Initialize(ActiveTask, _groups);
        }

        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ActiveTask = button.Tag as TaskHolder;
            ShowCurrentTask();
        }
        private void AddNewTask_Click(object sender, RoutedEventArgs e)
        {
            ActiveTask = _holdersService.CreateNewTask(_tasks);
            ShowCurrentTask();
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            _holdersService.ApplyChanges(_tasks, _groups, _taskDetailsControl.ShownTask);
            SaveChanges();
            Activate();
        }
        private void RestetChanges_Click(object sender, RoutedEventArgs e)
        {
            ShowCurrentTask();
        }
        private void Delete_Task(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Do you really want to delete " + ActiveTask.Name + "?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ActiveTask.Archivized = true;
                ActiveTask = null;
                ShowCurrentTask();
            }
            SaveChanges();
        }

        public void Activate()
        {
            ActiveTask = null;
            _groups.Clear();
            _tasks.Clear();
            _holdersService.UpdateConfigHolders(_groups, _tasks, _configurationService.GetConfig());
            TasksListView.ItemsSource = _tasks;
            ShowCurrentTask();
        }
        private void SaveChanges()
        {
            var config = new ConfigDto();
            _holdersService.UpdateConfigDto(config, _groups, _tasks);
            _configurationService.SaveConfig(config);

        }
        public void Save()
        {
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
    }
}
