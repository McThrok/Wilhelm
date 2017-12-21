using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wilhelm.Client.Pages;
using Wilhelm.Client.Services.Interfaces;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Client.ViewModels.Pages
{
    public class ReportPageViewModel : IMenuPage, INotifyPropertyChanged
    {
        private readonly IProxyService _proxyService;
        private List<ReportDto> _reportList;

        public ReportPageViewModel(IProxyService proxyService)
        {
            _proxyService = proxyService;
        }

        public async Task Activate(int userId)
        {
            _reportList = (await _proxyService.GetReports(userId)).ToList();
        }

        public async Task Save()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }


        public List<ReportDto> ReportList
        {
            get
            {
                return _reportList;
            }
            private set
            {
                _reportList = value;

            }
        }
    }
}
