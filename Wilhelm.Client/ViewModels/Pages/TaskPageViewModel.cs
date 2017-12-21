using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wilhelm.Client.Model;
using Wilhelm.Client.Pages;
using Wilhelm.Client.Services.Interfaces;
using Wilhelm.Client.Support;
using Wilhelm.Client.ViewModels.Controls;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Client.ViewModels.Pages
{
    public class TaskPageViewModel : INotifyPropertyChanged, IMenuPage
    {
        private readonly ObservableCollection<TaskHolder> _tasks = new ObservableCollection<TaskHolder>();
        private readonly List<GroupHolder> _groups = new List<GroupHolder>();
        private TaskHolder _activeTask;
        private readonly IHoldersService _holdersService;
        private readonly IProxyService _proxyService;
        private TaskDetailsViewModel _taskDetailsControl;
        private Visibility _dataVisibility;
        public ICommand AddNewTaskCmd { get; protected set; }
        public ICommand ApplyCmd { get; protected set; }
        public ICommand ResetCmd { get; protected set; }
        public ICommand DeleteCmd { get; protected set; }
        public ICommand TaskCmd { get; protected set; }
        private int _userId;

        public TaskPageViewModel(IHoldersService holdersService, IProxyService proxyService)
        {
            _holdersService = holdersService;
            _proxyService = proxyService;

            _taskDetailsControl = new TaskDetailsViewModel(_holdersService);
            TaskDetailsControl = _taskDetailsControl;
            AddNewTaskCmd = new DelegateCommand(AddNewTask);
            ApplyCmd = new AwaitableDelegateCommand<object>(Apply);
            ResetCmd = new DelegateCommand(Reset);
            DeleteCmd = new AwaitableDelegateCommand(Delete);
            TaskCmd = new DelegateCommand<object>(Task);
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

        private void Task(object task)
        {
            ActiveTask = task as TaskHolder;
            ShowCurrentTask();
        }
        private void AddNewTask()
        {
            ActiveTask = _holdersService.CreateNewTask(_tasks, _userId);
            ShowCurrentTask();
        }
        private async Task Apply(object obj)
        {
            _holdersService.ApplyChanges(_tasks, _groups, _taskDetailsControl.ShownTask);
            await SaveChanges();
            await Activate(_userId);
        }
        private void Reset()
        {
            ShowCurrentTask();
        }
        private async Task Delete()
        {
            var result = MessageBox.Show("Do you really want to delete " + ActiveTask.Name + "?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ActiveTask.Archivized = true;
                ActiveTask = null;
                ShowCurrentTask();
            }
            await SaveChanges();
        }

        public async Task Activate(int userId)
        {
            _userId = userId;
            ActiveTask = null;
            _groups.Clear();
            _tasks.Clear();
            _holdersService.UpdateConfigHolders(_groups, _tasks, await _proxyService.GetConfig(_userId));
            ShowCurrentTask();
        }
        private async Task SaveChanges()
        {
            var config = new ConfigDto();
            _holdersService.UpdateConfigDto(config, _groups, _tasks);
            //await _proxyService.SaveConfig(_userId, config);
        }
        public async Task Save()
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
