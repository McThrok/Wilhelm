﻿using System;
using System.Collections.Generic;
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
using Wilhelm.Backend.Services;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Pages;
using Wilhelm.Frontend.Services;
using Wilhelm.Frontend.Services.Interfaces;

namespace Wilhelm.Frontend.Controls
{
    /// <summary>
    /// Interaction logic for MainPanel.xaml
    /// </summary>
    public partial class MainPanel : UserControl
    {
        private readonly MenuPagesCollection _pages;
        private readonly IServiceFactory _serviceFactory;
        private readonly IHoldersConversionService _holdersConversionService;
        private readonly IHoldersService _holdersService;

        public MainPanel(int userId)
        {
            _serviceFactory = new ServiceFactory();
            _holdersConversionService = new HoldersConversionService();
            _holdersService = new HoldersService(_holdersConversionService);
            _pages = new MenuPagesCollection(_serviceFactory, _holdersConversionService, _holdersService);

            InitializeComponent();
            ClickMenu(_pages.HomePage);
        }

        private void HomeButto_Click(object sender, RoutedEventArgs e)
        {
            ClickMenu(_pages.HomePage);
        }

        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            ClickMenu(_pages.TasksPage);
        }

        private void GroupsButton_Click(object sender, RoutedEventArgs e)
        {
            ClickMenu(_pages.GroupsPage);
        }

        private void ArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            ClickMenu(_pages.ArchivePage);
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            ClickMenu(_pages.ReportPage);
        }
        private void ClickMenu(UserControl page)
        {
            var currentPage = MainFrame.Content;
            if (currentPage == page)
                return;

            if (currentPage != null && currentPage is IMenuPage currentMenuPage)
                currentMenuPage.Save();

            if (page != null && page is IMenuPage newMenuPage)
                newMenuPage.Activate();

            MainFrame.Content = page;
        }
        private void ClickMenu(Object page)
        {
            var currentPage = MainFrame.Content;
            if (currentPage == page)
                return;

            if (currentPage != null && currentPage is IMenuPage currentMenuPage)
                currentMenuPage.Save();

            if (page != null && page is IMenuPage newMenuPage)
                newMenuPage.Activate();

            MainFrame.Content = page;
        }
        public void properClose()
        {
            var currentPage = MainFrame.Content;
            if (currentPage != null && currentPage is IMenuPage currentMenuPage)
                currentMenuPage.Save();
        }
    }
}
