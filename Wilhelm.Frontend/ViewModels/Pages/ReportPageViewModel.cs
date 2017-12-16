using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Pages;

namespace Wilhelm.Frontend.ViewModels.Pages
{
    public class ReportPageViewModel : IMenuPage
    {
        private readonly IReportService _reportService;
        private int _userId;

        public List<ReportDto> ReportList
        {
            get
            {
                return _reportService.GetReports(_userId);
            }
        }
        public ReportPageViewModel(IReportService reportService)
        {
            _reportService = reportService;
        }

        public void Activate(int userId)
        {
            _userId = userId;
        }

        public void Save()
        {
        }
    }
}
