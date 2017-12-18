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
    public class GroupPageViewModel : INotifyPropertyChanged, IMenuPage
    {
        private readonly ObservableCollection<GroupHolder> _groups = new ObservableCollection<GroupHolder>();
        private readonly List<TaskHolder> _tasks = new List<TaskHolder>();
        private GroupHolder _activeGroup;
        private readonly IHoldersService _holdersService;
        private readonly IConfigurationService _configurationService;
        private GroupDetailsViewModel _groupDetailsControl;
        private Visibility _dataVisibility;
        public ICommand AddNewGroupCmd { get; protected set; }
        public ICommand ApplyCmd { get; protected set; }
        public ICommand ResetCmd { get; protected set; }
        public ICommand DeleteCmd { get; protected set; }
        public ICommand GroupCmd { get; protected set; }
        private int _userId;

        public GroupPageViewModel(IHoldersService holdersService, IConfigurationService configurationService)
        {
            _holdersService = holdersService;
            _configurationService = configurationService;

            _groupDetailsControl = new GroupDetailsViewModel(holdersService);
            GroupDetailsContent = _groupDetailsControl;

            AddNewGroupCmd = new DelegateCommand(AddNewGroup);
            ApplyCmd = new DelegateCommand(Apply);
            ResetCmd = new DelegateCommand(Reset);
            DeleteCmd = new DelegateCommand(Delete);
            GroupCmd = new DelegateCommand(Group);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public void ShowCurrentGroup()
        {
            if (ActiveGroup == null)
               DataVisibility = Visibility.Hidden;
            else
               DataVisibility = Visibility.Visible;

            _groupDetailsControl.Initialize(ActiveGroup, _tasks);
        }

        private void Group(object obj)
        {
            ActiveGroup = obj as GroupHolder;
            ShowCurrentGroup();
        }
        private void AddNewGroup(object obj)
        {
            ActiveGroup = _holdersService.CreateNewGroup(_groups, _userId);
            ShowCurrentGroup();
        }
        private void Apply(object obj)
        {
            _holdersService.ApplyChanges(_groups, _tasks, _groupDetailsControl.ShownGroup);
            SaveChanges();
            Activate(_userId);
        }
        private void Reset(object obj)
        {
            ShowCurrentGroup();
        }
        private void Delete(object obj)
        {
            var result = MessageBox.Show("Do you really want to delete " + ActiveGroup.Name + "?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ActiveGroup.Archivized = true;
                ActiveGroup = null;
                ShowCurrentGroup();
            }
            SaveChanges();
        }

        public void Activate(int userId)
        {
            _userId = userId;
            ActiveGroup = null;
            _groups.Clear();
            _tasks.Clear();
            _holdersService.UpdateConfigHolders(_groups, _tasks, _configurationService.GetConfig(_userId));
            //GroupsListView.ItemsSource = _groups;
            ShowCurrentGroup();
        }
        public void SaveChanges()
        {
            var config = new ConfigDto();
            _holdersService.UpdateConfigDto(config, _groups, _tasks);
            _configurationService.SaveConfig(config);
        }
        public void Save()
        {
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
                OnPropertyChanged(nameof(ActiveGroup));
            }
        }
        public GroupDetailsViewModel GroupDetailsContent {
            get
            {
                return _groupDetailsControl;
            }
            set
            {
                _groupDetailsControl = value;
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
        public ObservableCollection<GroupHolder> Groups
        {
            get
            {
                return _groups;
            }
        }
    }
}
