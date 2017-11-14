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
    public partial class GroupsPage : Page, INotifyPropertyChanged, IMenuPage
    {
        private ObservableCollection<GroupHolder> _groups = new ObservableCollection<GroupHolder>();
        private List<TaskHolder> _tasks = new List<TaskHolder>();
        private GroupHolder _activeGroup;
        private readonly IHoldersService _holdersService;
        private readonly IConfigurationService _configurationService;

        public event PropertyChangedEventHandler PropertyChanged;

        public GroupsPage(IHoldersService holdersService, IConfigurationService configurationService)
        {
            _holdersService = holdersService;
            _configurationService = configurationService;

            InitializeComponent();
            DataContext = this;
        }
        public void ShowCurrentGroup()
        {
            if (ActiveGroup == null)
                GroupButtonsPanel.Visibility = Visibility.Hidden;
            else
                GroupButtonsPanel.Visibility = Visibility.Visible;

            GroupDetails.Initialize(ActiveGroup, _tasks);
        }

        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            ActiveGroup = button.Tag as GroupHolder;
            ShowCurrentGroup();
        }
        private void AddNewGroup_Click(object sender, RoutedEventArgs e)
        {
            var addedGroup = new GroupHolder()
            {
                Id = _holdersService.GenerateTemporaryId(_groups),
                Name = "New group",
                Tasks = new ObservableCollection<TaskHolder>(),
            };
            ActiveGroup = addedGroup;
            ShowCurrentGroup();
        }
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var changedGroup = GroupDetails.ShownGroup;
            if (!_groups.Contains(ActiveGroup))
                _groups.Insert(0, ActiveGroup);
            ActiveGroup.Name = changedGroup.Name;
            ActiveGroup.Description = changedGroup.Description;

            foreach (var task in _tasks)
            {
                var groupInDetails = changedGroup.Tasks.Where(x => x.Id == task.Id).SingleOrDefault();

                if (ActiveGroup.Tasks.Contains(task) && groupInDetails == null)
                {
                    task.Groups.Add(ActiveGroup);
                    ActiveGroup.Tasks.Add(task);
                }

                if (!ActiveGroup.Tasks.Contains(task) && groupInDetails != null)
                {
                    task.Groups.Remove(ActiveGroup);
                    ActiveGroup.Tasks.Remove(task);
                }
            }
            SaveChanges();

        }
        private void RestetChanges_Click(object sender, RoutedEventArgs e)
        {
            ShowCurrentGroup();
        }
        private void Delete_Group(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Do you really want to delete " + ActiveGroup.Name, "", MessageBoxButton.YesNo);
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
