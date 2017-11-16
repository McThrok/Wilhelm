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
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Model;
using Wilhelm.Frontend.Services.Interfaces;

namespace Wilhelm.Frontend.Pages
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class HomePage : Page, IMenuPage
    {
        private readonly IHoldersService _holdersService;
        private readonly IActivityService _activityService;
        private ObservableCollection<ActivityHolder> _currentList;

        public HomePage(IHoldersService holdersService, IActivityService activityService)
        {
            _holdersService = holdersService;
            _activityService = activityService;
            InitializeComponent();
            DataContext = this;
        }

        private void ListViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(e.OriginalSource is null))
            {
                if (e.Source is ContentPresenter)
                {
                    var content = VisualTreeHelper.GetChild(e.Source as ContentPresenter, 0);
                    if (content is CheckBox)
                        return;
                }
                var item = sender as ListViewItem;
                if (item.Content is ActivityHolder activity)
                    activity.IsDone = !activity.IsDone;
            }
        }

        public void Activate()
        {
            var todayTasksList = new List<ActivityHolder>();
            _holdersService.UpdateArchiveHolders(todayTasksList, _activityService.GetTodaysActivities());
            _currentList = new ObservableCollection<ActivityHolder>(todayTasksList);
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