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
    public partial class GroupsPage : UserControl, INotifyPropertyChanged, IMenuPage
    {
        private readonly ObservableCollection<GroupHolder> _groups = new ObservableCollection<GroupHolder>();
        private readonly List<TaskHolder> _tasks = new List<TaskHolder>();
        private GroupHolder _activeGroup;
        private readonly IHoldersService _holdersService;
        private readonly IConfigurationService _configurationService;
        private GroupDetailsControl _groupDetailsControl;

        public event PropertyChangedEventHandler PropertyChanged;

        public GroupsPage(IHoldersService holdersService, IConfigurationService configurationService)
        {
            _holdersService = holdersService;
            _configurationService = configurationService;

            InitializeComponent();
            _groupDetailsControl = new GroupDetailsControl(holdersService);
            GroupDetailsContentControl.Content = _groupDetailsControl;
            DataContext = this;
        }
        public void ShowCurrentGroup()
        {
            if (ActiveGroup == null)
                GroupButtonsPanel.Visibility = Visibility.Hidden;
            else
                GroupButtonsPanel.Visibility = Visibility.Visible;

            _groupDetailsControl.Initialize(ActiveGroup, _tasks);
        }

        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ActiveGroup = button.Tag as GroupHolder;
            ShowCurrentGroup();
        }
        private void AddNewGroup_Click(object sender, RoutedEventArgs e)
        {
            ActiveGroup = _holdersService.CreateNewGroup(_groups);
            ShowCurrentGroup();
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            _holdersService.ApplyChanges(_groups, _tasks, _groupDetailsControl.ShownGroup);
            SaveChanges();
            Activate();
        }
        private void RestetChanges_Click(object sender, RoutedEventArgs e)
        {
            ShowCurrentGroup();
        }
        private void Delete_Group(object sender, RoutedEventArgs e)
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

        public void Activate()
        {
            ActiveGroup = null;
            _groups.Clear();
            _tasks.Clear();
            _holdersService.UpdateConfigHolders(_groups, _tasks, _configurationService.GetConfig());
            GroupsListView.ItemsSource = _groups;
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
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ActiveGroup)));
            }
        }
    }
}
