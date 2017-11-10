﻿using System;
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
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Model;

namespace Wilhelm.Frontend.Pages
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private ObservableCollection<ActivityHolder> _currentList;
        private readonly IActivityService _activityService;

        public HomePage(IActivityService activityService)
        {
            _activityService = activityService;
            InitializeComponent();
            DataContext = this;
            //_currentList = new ObservableCollection<ActivityHolder>(MockBase.MockBase.GetTodaysActivities());
            TaskListView.ItemsSource = _currentList;

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