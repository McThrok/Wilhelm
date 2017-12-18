using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Model;
using Wilhelm.Frontend.Pages;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Frontend.ViewModels.Pages
{
    public class ArchivePageViewModel : IMenuPage
    {
        private readonly IHoldersService _holdersService;
        private readonly IProxyService _proxyService;
        private ObservableCollection<ActivityHolder> _currentList;
        private int _userId;

        public ArchivePageViewModel(IHoldersService holdersService, IProxyService proxyService)
        {
            _holdersService = holdersService;
            _proxyService = proxyService;
            _currentList = new ObservableCollection<ActivityHolder>();
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

        public async void Activate(int userId)
        {
            _userId = userId; CurrentList.Clear();
            var archiveList = new List<ActivityHolder>();
            _holdersService.UpdateArchiveHolders(archiveList, await _proxyService.GetArchive(_userId));
            archiveList.Sort((a, b) => DateTime.Compare(a.Date, b.Date));
            foreach (var activity in archiveList.Where(x => !x.Task.Archivized))
                CurrentList.Add(activity);
        }   

        public async void Save()
        {
            var activities = new List<ActivityDto>();
            _holdersService.UpdateActivityDtos(activities, _currentList);
            await _proxyService.SaveArchive(_userId,activities);
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
