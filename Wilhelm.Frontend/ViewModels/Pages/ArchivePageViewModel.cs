using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Model;
using Wilhelm.Frontend.Pages;
using Wilhelm.Frontend.Services.Interfaces;

namespace Wilhelm.Frontend.ViewModels.Pages
{
    class ArchivePageViewModel: IMenuPage
    {
        private readonly IHoldersService _holdersService;
        private readonly IActivityService _activityService;
        private ObservableCollection<ActivityHolder> _currentList;

        public ArchivePageViewModel(IHoldersService holdersService, IActivityService activityService)
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

        public void Activate()
        {
            var archiveList = new List<ActivityHolder>();
            _holdersService.UpdateArchiveHolders(archiveList, _activityService.GetArchive());
            archiveList.Sort((a, b) => DateTime.Compare(a.Date, b.Date));
            _currentList = new ObservableCollection<ActivityHolder>(archiveList.Where(x => !x.Task.Archivized));
            //TaskListView.ItemsSource = _currentList;
        }

        public void Save()
        {
            var activities = new List<ActivityDto>();
            _holdersService.UpdateActivityDtos(activities, _currentList);
            _activityService.SaveActivities(activities);
        }
    }
}
