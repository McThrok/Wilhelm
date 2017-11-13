using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Model;
using Wilhelm.Backend.Model.Dto;

namespace Wilhelm.Frontend.Pages
{
    /// <summary>
    /// Interaction logic for ActionTypesPage.xaml
    /// </summary>
    public partial class ArchivePage : Page, IMenuPage
    {
        private readonly IHoldersService _holdersService;
        private readonly IActivityService _activityService;
        private ObservableCollection<ActivityHolder> _currentList;

        public ArchivePage(IHoldersService holdersService, IActivityService activityService)
        {
            _holdersService = holdersService;
            _activityService = activityService;
            InitializeComponent();
            DataContext = this;
        }

        private void ListViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item.Content is ActivityHolder activity)
                activity.IsDone = !activity.IsDone;
        }

        public void Activate()
        {
            _currentList = new ObservableCollection<ActivityHolder>();
            _holdersService.UpdateArchiveHolders(_currentList, _activityService.GetArchive());
            TaskListView.ItemsSource = _currentList;
        }

        public void Save()
        {
            var activities = new List<ActivityDto>();
            _holdersService.UpdateActivityDtos(activities, _currentList);
            _activityService.SaveActivities(activities);
        }
    }
}
