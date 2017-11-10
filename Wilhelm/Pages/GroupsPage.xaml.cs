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
    public partial class GroupsPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<GroupHolder> _groups;
        private List<TaskHolder> _tasks;
        private GroupHolder _activeGroup;
        private readonly IConfigurationService _configurationService;
        private readonly IHoldersConversionService _holderConversionService;

        public event PropertyChangedEventHandler PropertyChanged;

        public GroupsPage(IConfigurationService configurationService, IHoldersConversionService holderConversionService)
        {
            _configurationService = configurationService;
            _holderConversionService = holderConversionService;
            InitializeComponent();
            DataContext = this;
            SetConfiguration();
            GroupsListView.ItemsSource = _groups;
            ShowCurrentGroup();
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
                Name = "New group",
                Tasks = new ObservableCollection<TaskHolder>(),
            };
            _groups.Add(addedGroup);
            ActiveGroup = addedGroup;
            ShowCurrentGroup();
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
        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            var changedGroup = GroupDetails.ShownGroup;
            ActiveGroup.Name = changedGroup.Name;
            ActiveGroup.Description = changedGroup.Description;

            foreach (var task in _tasks)
            {
                if (changedGroup.Tasks.Contains(task) && !ActiveGroup.Tasks.Contains(task))
                {
                    task.Groups.Add(ActiveGroup);
                    ActiveGroup.Tasks.Add(task);
                }

                if (!changedGroup.Tasks.Contains(task) && ActiveGroup.Tasks.Contains(task))
                {
                    task.Groups.Remove(ActiveGroup);
                    ActiveGroup.Tasks.Remove(task);
                }
            }

            SaveConfig();
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
        }
        private void SetConfiguration()
        {
            var config = _configurationService.GetConfig();
            _groups = new ObservableCollection<GroupHolder>();
            _tasks = new List<TaskHolder>();

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
