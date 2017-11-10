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
    public partial class HomePage : Page
    {
        private ObservableCollection<ActivityHolder> _currentList;
        private readonly IActivityService _activityService;
        private readonly IHoldersConversionService _holdersConversionService;

        public HomePage(IActivityService activityService, IHoldersConversionService holdersConversionService)
        {
            _activityService = activityService;
            _holdersConversionService = holdersConversionService;
            InitializeComponent();
            DataContext = this;
            _currentList = GetTodaysTasks();
            TaskListView.ItemsSource = _currentList;
        }

        private ObservableCollection<ActivityHolder> GetTodaysTasks()
        {
            var activities = _activityService.GetTodaysTasks();
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
            _activityService.SaveTodaysTasks(dtos);
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

    }
}