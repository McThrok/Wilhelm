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
        private readonly IHoldersService _holdersService;

        public ArchivePage( IHoldersService holdersService)
        {
            _holdersService = holdersService;
            InitializeComponent();
            DataContext = this;
            _currentList = new ObservableCollection<ActivityHolder>(_holdersService.GetArchiveHolders());
            TaskListView.ItemsSource = _currentList;
        }

        //TODO: use it on close/changepage/...
        private void ONCLOSE()
        {
            _holdersService.SaveActivities(_currentList);
        }

        private void ListViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item.Content is ActivityHolder activity)
                activity.IsDone = !activity.IsDone;
        }
    }
}
