using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wilhelm.Frontend.Model;
using Wilhelm.Frontend.Pages;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.Support;
using Wilhelm.Frontend.ViewModels.Controls;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Frontend.ViewModels.Pages
{
    public class GroupPageViewModel : INotifyPropertyChanged, IMenuPage
    {
        private readonly ObservableCollection<GroupHolder> _groups = new ObservableCollection<GroupHolder>();
        private readonly List<TaskHolder> _tasks = new List<TaskHolder>();
        private GroupHolder _activeGroup;
        private readonly IHoldersService _holdersService;
        private readonly IProxyService _proxyService;
        private GroupDetailsViewModel _groupDetailsControl;
        private Visibility _dataVisibility;
        public ICommand AddNewGroupCmd { get; protected set; }
        public ICommand ApplyCmd { get; protected set; }
        public ICommand ResetCmd { get; protected set; }
        public ICommand DeleteCmd { get; protected set; }
        public ICommand GroupCmd { get; protected set; }
        private int _userId;

        public GroupPageViewModel(IHoldersService holdersService, IProxyService proxyService)
        {
            _holdersService = holdersService;
            _proxyService = proxyService;

            _groupDetailsControl = new GroupDetailsViewModel(holdersService);
            GroupDetailsContent = _groupDetailsControl;

            AddNewGroupCmd = new DelegateCommand(AddNewGroup);
            ApplyCmd = new AwaitableDelegateCommand(Apply);
            ResetCmd = new DelegateCommand(Reset);
            DeleteCmd = new AwaitableDelegateCommand(Delete);
            GroupCmd = new DelegateCommand<object>(Group);
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

        private void Group(object group)
        {
            ActiveGroup = group as GroupHolder;
            ShowCurrentGroup();
        }
        private void AddNewGroup()
        {
            ActiveGroup = _holdersService.CreateNewGroup(Groups, _userId);
            ShowCurrentGroup();
        }
        private async Task Apply()
        {
            _holdersService.ApplyChanges(Groups, _tasks, _groupDetailsControl.ShownGroup);
            await SaveChanges();
            await Activate(_userId);
        }
        private void Reset()
        {
            ShowCurrentGroup();
        }
        private async Task Delete()
        {
            var result = MessageBox.Show("Do you really want to delete " + ActiveGroup.Name + "?", "", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                ActiveGroup.Archivized = true;
                ActiveGroup = null;
                ShowCurrentGroup();
            }
            await SaveChanges();
        }

        public async Task Activate(int userId)
        {
            _userId = userId;
            ActiveGroup = null;
            _groups.Clear();
            _tasks.Clear();
            _holdersService.UpdateConfigHolders(Groups, _tasks, await _proxyService.GetConfig(_userId));
            ShowCurrentGroup();
        }
        public async Task SaveChanges()
        {
            var config = new ConfigDto();
            _holdersService.UpdateConfigDto(config, Groups, _tasks);
            await _proxyService.SaveConfig(_userId, config);
        }
        public async Task Save()
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
        public GroupDetailsViewModel GroupDetailsContent
        {
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
