using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Model;
using Wilhelm.Frontend.Pages;
using Wilhelm.Frontend.Services;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Frontend.ViewModels.Pages
{
    public class HomePageViewModel : IMenuPage
    {
        private readonly IHoldersService _holdersService;
        private readonly IActivityService _activityService;
        private ObservableCollection<ActivityHolder> _currentList;
        private int _userId;

        public HomePageViewModel(IHoldersService holdersService, IActivityService activityService)
        {
            _holdersService = holdersService;
            _activityService = activityService;
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

        public async Task Activate(int userId)
        {
            _userId = userId;
            var todayTasksList = new List<ActivityHolder>();
            ProxyService p = new ProxyService();
            //_holdersService.UpdateArchiveHolders(todayTasksList, _activityService.GetTodaysActivities(_userId));
            var a = p.GetTodaysTasks(_userId);
             _holdersService.UpdateArchiveHolders(todayTasksList,await a);//GetAwaiter().GetResult() vs getResut();
             //CurrentList = new ObservableCollection<ActivityHolder>(todayTasksList.Where(x => !x.Task.Archivized));
             CurrentList = new ObservableCollection<ActivityHolder>();
        }

        public void Save()
        {
            var activities = new List<ActivityDto>();
            _holdersService.UpdateActivityDtos(activities, _currentList);
            _activityService.SaveActivities(activities);
        }

        public ObservableCollection<ActivityHolder> CurrentList
        {
            get
            {
                return _currentList;
            }
            private set
            {
                _currentList = value;
            }
        }
    }
}
