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
    public partial class ArchivePage : Page
    {
        private ObservableCollection<ActivityHolder> _currentList;
        private readonly IActivityService _activityService;
        private readonly IHoldersConversionService _holdersConversionService;

        public ArchivePage(IActivityService activityService, IHoldersConversionService holdersConversionService)
        {
            _activityService = activityService;
            _holdersConversionService = holdersConversionService;
            InitializeComponent();
            DataContext = this;
            _currentList = GetArchives();
            TaskListView.ItemsSource = _currentList;
        }

        private ObservableCollection<ActivityHolder> GetArchives()
        {
            var activities = _activityService.GetArchive();
            var holders = new ObservableCollection<ActivityHolder>();
            foreach (var activity in activities)
            {
                var holder = new ActivityHolder();
                _holdersConversionService.ConvertFromDto(holder, activity);
                holder.Task = new TaskHolder();
                _holdersConversionService.ConvertFromDto(holder.Task, activity.Task);
                holders.Add(holder);
            }
            return holders;
        }

        private void SaveArchive()
        {
            var dtos = new List<ActivityDto>();
            foreach (var holder in _currentList)
            {
                var dto = new ActivityDto();
                _holdersConversionService.ConvertToDto(dto, holder);
                dtos.Add(dto);
            }
            _activityService.SaveArchive(dtos);
        }

        private void ListViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item.Content is ActivityHolder activity)
                activity.IsDone = !activity.IsDone;
        }
    }
}
