using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Backend.Services;
using Wilhelm.Frontend.Services;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.ViewModels.Pages;

namespace Wilhelm.Frontend.Pages
{
    public class MenuPagesCollection
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IHoldersConversionService _holdersConversionService;//not used
        private readonly IHoldersService _holdersService;
        private readonly IConfigurationService _configurationService;
        private readonly IActivityService _activityService;
        private readonly IReportService _reportService;
        private HomePageViewModel _homePage;
        private TasksPage _tasksPage;
        private ReportPageViewModel _reportPage;
        private GroupPageViewModel _groupsPage;
        private ArchivePageViewModel _archivePage;

        public MenuPagesCollection(IServiceFactory serviceFactory, IHoldersConversionService holdersConversionService, IHoldersService holdersService)
        {
            _serviceFactory = serviceFactory;
            _holdersConversionService = holdersConversionService;
            _holdersService = holdersService;
            _configurationService = serviceFactory.CreateConfigurationService();
            _activityService = _serviceFactory.CreateActivityService();
            _reportService = serviceFactory.CreateReportService();
        }

        public HomePageViewModel HomePage
        {
            get
            {
                if (_homePage == null)
                    _homePage = new HomePageViewModel(_holdersService, _activityService);
                return _homePage;
            }
        }
        public TasksPage TasksPage
        {
            get
            {
                if (_tasksPage == null)
                    _tasksPage = new TasksPage(_holdersService, _configurationService);
                return _tasksPage;
            }
        }
        public ReportPageViewModel ReportPage
        {
            get
            {
                if (_reportPage == null)
                    _reportPage = new ReportPageViewModel(_reportService);
                return _reportPage;
            }
        }
        public GroupPageViewModel GroupsPage
        {
            get
            {
                if (_groupsPage == null)
                    _groupsPage = new GroupPageViewModel(_holdersService,_configurationService);
                return _groupsPage;
            }
        }
        public ArchivePageViewModel ArchivePage
        {
            get
            {
                if (_archivePage == null)
                    _archivePage = new ArchivePageViewModel(_holdersService, _activityService);
                return _archivePage;
            }
        }
    }
}
