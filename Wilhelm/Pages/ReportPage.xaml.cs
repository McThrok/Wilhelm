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

namespace Wilhelm.Frontend.Pages
{
    /// <summary>
    /// Interaction logic for ActionTypesPage.xaml
    /// </summary>
    public partial class ReportPage : Page
    {
        private readonly IReportService _reportService;

        public ReportPage(IReportService reportService)
        {
            _reportService = reportService;
            InitializeComponent();
            DataContext = this;
            ReportListView.ItemsSource = reportService.GetReports();
        }
    }
}
