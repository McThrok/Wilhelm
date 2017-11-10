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

        public MainWindow()
        {
            InitializeComponent();
            _serviceFactory = new ServiceFactory();
            _pages = new MenuPagesCollection(_serviceFactory, _holdersConversionService);
            MainFrame.Content = _pages.HomePage;
        }

        private void HomeButto_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = _pages.HomePage;
        }

        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = _pages.TasksPage;
        }

        private void GroupsButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = _pages.GroupsPage;
        }

        private void ArchiveButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = _pages.ArchivePage;
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Content = _pages.ReportPage;
        }
    }
}
