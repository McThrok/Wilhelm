using System;
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
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.Services;
using Wilhelm.Frontend.Pages;

namespace Wilhelm.Frontend.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MenuPagesCollection _pages;
        private IServiceFactory _serviceFactory = new ServiceFactory();
        private IHoldersConversionService _holdersConversionService = new HoldersConversionService();
        private IHoldersService _holdersService;

        public MainWindow()
        {
            _holdersService = new HoldersService(_serviceFactory.CreateConfigurationService(), _serviceFactory.CreateActivityService(), _holdersConversionService);
            InitializeComponent();
            _serviceFactory = new ServiceFactory();
            _pages = new MenuPagesCollection(_serviceFactory, _holdersConversionService, _holdersService);
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
        private void ClickMenu(Page page)
        {
            var currentPage = MainFrame.Content;
            if (currentPage != null && currentPage is IMenuPage currentMenuPage)
                currentMenuPage.Save();

            if (currentPage != null && page is IMenuPage newMenuPage)
                newMenuPage.Activate();

            MainFrame.Content = page;
        }
    }
}
