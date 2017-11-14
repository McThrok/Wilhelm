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
    public partial class TasksPage : Page, INotifyPropertyChanged, IMenuPage
    {
        private ObservableCollection<TaskHolder> _tasks = new ObservableCollection<TaskHolder>();
        private List<GroupHolder> _groups = new List<GroupHolder>();
        private TaskHolder _activeTask;
        private readonly IHoldersService _holdersService;
        private readonly IConfigurationService _configurationService;

        public event PropertyChangedEventHandler PropertyChanged;

        public TasksPage(IHoldersService holdersService, IConfigurationService configurationService)
        {
            _holdersService = holdersService;
            _configurationService = configurationService;
            InitializeComponent();

            DataContext = this;
        }
        public void ShowCurrentTask()
        {
            if (ActiveTask == null)
                TaskButtonsPanel.Visibility = Visibility.Hidden;
            else
                TaskButtonsPanel.Visibility = Visibility.Visible;

            TaskDetails.Initialize(ActiveTask, _groups);
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
                Id = _holdersService.GenerateTemporaryId(_tasks),
                Name = "New Task",
                Groups = new ObservableCollection<GroupHolder>(),
                StartDate = DateTime.Now,
                Frequency = 1,
            };
            ActiveTask = addedTask;
            ShowCurrentTask();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var changedTask = TaskDetails.ShownTask;
            if(!_tasks.Contains(ActiveTask))
                _tasks.Insert(0, ActiveTask);
            ActiveTask.Name = changedTask.Name;
            ActiveTask.Description = changedTask.Description;
            ActiveTask.StartDate = changedTask.StartDate;
            ActiveTask.Frequency = changedTask.Frequency;

            foreach (var group in _groups)
            {
                var taskInDetails = changedTask.Groups.Where(x => x.Id == group.Id).SingleOrDefault();

                if (!ActiveTask.Groups.Contains(group) && taskInDetails != null)
                {
                    group.Tasks.Add(ActiveTask);
                    ActiveTask.Groups.Add(group);
                }

                if (ActiveTask.Groups.Contains(group) && taskInDetails == null)
                {
                    group.Tasks.Remove(ActiveTask);
                    ActiveTask.Groups.Remove(group);
                }
            }
            SaveChanges();
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
            SaveChanges();
        }

        public void Activate()
        {
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
