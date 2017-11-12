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
        private readonly IHoldersService _holdersService;

        public HomePage(IHoldersService holdersService)
        {
            _holdersService = holdersService;
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
            _currentList = new ObservableCollection<ActivityHolder>(_holdersService.GetTodaysActivitiesHolders());
            TaskListView.ItemsSource = _currentList;
        }

        public void Save()
        {
            _holdersService.SaveActivities(_currentList);
        }

    }
}