using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Frontend.Services;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.ViewModels.Pages;

namespace Wilhelm.Frontend.Pages
{
    public class MenuPagesCollection
    {
        private readonly IHoldersConversionService _holdersConversionService;
        private readonly IHoldersService _holdersService;
        private readonly IProxyService _proxyService;
        private HomePageViewModel _homePage;
        private TaskPageViewModel _tasksPage;
        private ReportPageViewModel _reportPage;
        private GroupPageViewModel _groupsPage;
        private ArchivePageViewModel _archivePage;

        public MenuPagesCollection(IProxyService proxyService, IHoldersConversionService holdersConversionService, IHoldersService holdersService)
        {
            _holdersConversionService = holdersConversionService;
            _holdersService = holdersService;
            _proxyService = proxyService;
        }

        public HomePageViewModel HomePage
        {
            get
            {
                if (_homePage == null)
                    _homePage = new HomePageViewModel(_holdersService, _proxyService);
                return _homePage;
            }
        }
        public TaskPageViewModel TasksPage
        {
            get
            {
                if (_tasksPage == null)
                    _tasksPage = new TaskPageViewModel(_holdersService, _proxyService);
                return _tasksPage;
            }
        }
        public ReportPageViewModel ReportPage
        {
            get
            {
                if (_reportPage == null)
                    _reportPage = new ReportPageViewModel(_proxyService);
                return _reportPage;
            }
        }
        public GroupPageViewModel GroupsPage
        {
            get
            {
                if (_groupsPage == null)
                    _groupsPage = new GroupPageViewModel(_holdersService, _proxyService);
                return _groupsPage;
            }
        }
        public ArchivePageViewModel ArchivePage
        {
            get
            {
                if (_archivePage == null)
                    _archivePage = new ArchivePageViewModel(_holdersService, _proxyService);
                return _archivePage;
            }
        }
    }
}
