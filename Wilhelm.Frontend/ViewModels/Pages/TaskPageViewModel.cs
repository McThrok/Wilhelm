using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wilhelm.Backend.Model;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Model;
using Wilhelm.Frontend.Pages;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.Support;
using Wilhelm.Frontend.ViewModels.Controls;

namespace Wilhelm.Frontend.ViewModels.Pages
{
    public class TaskPageViewModel : INotifyPropertyChanged, IMenuPage
    {
        private readonly ObservableCollection<TaskHolder> _tasks = new ObservableCollection<TaskHolder>();
        private readonly List<GroupHolder> _groups = new List<GroupHolder>();
        private TaskHolder _activeTask;
        private readonly IHoldersService _holdersService;
        private readonly IConfigurationService _configurationService;
        private TaskDetailsViewModel _taskDetailsControl;
        private Visibility _dataVisibility;
        public ICommand AddNewTaskCmd { get; protected set; }
        public ICommand ApplyCmd { get; protected set; }
        public ICommand ResetCmd { get; protected set; }
        public ICommand DeleteCmd { get; protected set; }
        public ICommand TaskCmd { get; protected set; }
        public TaskPageViewModel(IHoldersService holdersService, IConfigurationService configurationService)
        {
            _holdersService = holdersService;
            _configurationService = configurationService;

            _taskDetailsControl = new TaskDetailsViewModel(_holdersService);
            TaskDetailsControl = _taskDetailsControl;
            AddNewTaskCmd = new DelegateCommand(AddNewTask);
            ApplyCmd = new DelegateCommand(Apply);
            ResetCmd = new DelegateCommand(Reset);
            DeleteCmd = new DelegateCommand(Delete);
            TaskCmd = new DelegateCommand(Task);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public void ShowCurrentTask()
        {
            if (ActiveTask == null)
                DataVisibility = Visibility.Hidden;
            else
                DataVisibility = Visibility.Visible;

            _taskDetailsControl.Initialize(ActiveTask, _groups);
        }

        private void Task(object obj)
        {
            ActiveTask = obj as TaskHolder;
            ShowCurrentTask();
        }
        private void AddNewTask(object obj)
        {
            ActiveTask = _holdersService.CreateNewTask(_tasks);
            ShowCurrentTask();
        }
        private void Apply(object obj)
        {
            _holdersService.ApplyChanges(_tasks, _groups, _taskDetailsControl.ShownTask);
            SaveChanges();
            Activate();
        }
        private void Reset(object obj)
        {
            ShowCurrentTask();
        }
        private void Delete(object obj)
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
        public TaskDetailsViewModel TaskDetailsControl
        {
            get
            {
                return _taskDetailsControl;
            }
            set
            {
                _taskDetailsControl = value;
            }
        }
        public Visibility DataVisibility
        {
            get
            {
                return _dataVisibility;
            }
            set
            {
                _dataVisibility = value;
                OnPropertyChanged(nameof(DataVisibility));
            }
        }
        public ObservableCollection<TaskHolder> Tasks
        {
            get
            {
                return _tasks;
            }
        }
    }
}
