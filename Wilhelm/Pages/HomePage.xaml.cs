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
using Wilhelm.Frontend.Model;

namespace Wilhelm.Frontend.Pages
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private ObservableCollection<ActivityHolder> _currentList;
        public HomePage()
        {
            InitializeComponent();
            DataContext = this;
            _currentList = new ObservableCollection<ActivityHolder>(MockBase.MockBase.GetActivities());
            TaskListView.ItemsSource = _currentList;

        }

        private void ListViewItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            var activity = item.Content as ActivityHolder;
            if (activity != null)
                activity.IsDone = !activity.IsDone;
        }
    }
}