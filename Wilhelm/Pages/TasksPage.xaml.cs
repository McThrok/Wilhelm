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
using Wilhelm.Frontend.Model;
using Wilhelm.Frontend.Services.Interfaces;

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
        private readonly IConfigurationService _configurationService;
        private readonly IHoldersConversionService _holderConversionService;

        public event PropertyChangedEventHandler PropertyChanged;

        public TasksPage(IConfigurationService configurationService, IHoldersConversionService holderConversionService)
        {
            _configurationService = configurationService;
            _holderConversionService = holderConversionService;
            InitializeComponent();
            DataContext = this;
            //_tasks = new ObservableCollection<TaskHolder>(MockBase.MockBase.GetTasks());
            //_groups = new List<GroupHolder>(MockBase.MockBase.GetGroups());
            TasksListView.ItemsSource = _tasks;
            ShowCurrentTask();
        }

        private void TaskButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ActiveTask = button.Tag as TaskHolder;
            ShowCurrentTask();
        }
        private void AddNewTask_Click(object sender, RoutedEventArgs e)
        {
            var addedTask = new TaskHolder()
            {
                Id = 1,
                Name = "New Task",
                Groups = new ObservableCollection<GroupHolder>(),
                StartDate = DateTime.Now,
                Frequency = 1,
            };
            _tasks.Insert(0, addedTask);
            ActiveTask = addedTask;
            ShowCurrentTask();
        }

        public void ShowCurrentTask()
        {
            if (ActiveTask == null)
                TaskButtonsPanel.Visibility = Visibility.Hidden;
            else
                TaskButtonsPanel.Visibility = Visibility.Visible;

            TaskDetails.Initialize(ActiveTask, _groups);
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

            SaveConfig();
        }

        private void RestetChanges_Click(object sender, RoutedEventArgs e)
        {
            ShowCurrentTask();
        }
        private void Delete_Task(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Do you really want to delete " + ActiveTask.Name, "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ActiveTask.Archivized = true;
                ActiveTask = null;
                ShowCurrentTask();
            }
        }
        private void SetConfiguration()
        {
            var config = _configurationService.GetConfig();
            _tasks = new ObservableCollection<TaskHolder>();
            _groups = new List<GroupHolder>();

            foreach (var group in config.Groups)
            {
                var groupHolder = new GroupHolder();
                _holderConversionService.ConvertFromDto(groupHolder, group);
                _groups.Add(groupHolder);
            }

            foreach (var task in config.Tasks)
            {
                var taskHolder = new TaskHolder();
                _holderConversionService.ConvertFromDto(taskHolder, task, _groups, true);
                _tasks.Add(taskHolder);
            }
        }
        private void SaveConfig()
        {
            var config = new ConfigDto();

            foreach (var group in _groups)
            {
                var groupDto = new GroupDto();
                _holderConversionService.ConvertToDto(groupDto, group);
                config.Groups.Add(groupDto);
            }

            foreach (var task in _tasks)
            {
                var taskDto = new TaskDto();
                _holderConversionService.ConvertToDto(taskDto, task);
                config.Tasks.Add(taskDto);
            }

            _configurationService.SaveConfig(config);
        }
    }
}
