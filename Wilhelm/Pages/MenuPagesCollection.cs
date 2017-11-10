using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Backend.Services;
using Wilhelm.Frontend.Services;
using Wilhelm.Frontend.Services.Interfaces;

namespace Wilhelm.Frontend.Pages
{
    public class MenuPagesCollection
    {
        private readonly IServiceFactory _serviceFactory;
        private readonly IHoldersConversionService _holdersConversionService;
        private HomePage _homePage;
        private TasksPage _tasksPage;
        private ReportPage _reportPage;
        private GroupsPage _groupsPage;
        private ArchivePage _archivePage;

        public MenuPagesCollection(IServiceFactory serviceFactory, IHoldersConversionService holdersConversionService)
        {
            _serviceFactory = serviceFactory;
            _holdersConversionService = holdersConversionService;
        }

        public HomePage HomePage
        {
            get
            {
                if (_homePage == null)
                    _homePage = new HomePage(_serviceFactory.CreateActivityService());
                return _homePage;
            }
        }
        public TasksPage TasksPage
        {
            get
            {
                if (_tasksPage == null)
                    _tasksPage = new TasksPage(_serviceFactory.CreateConfigurationService());
                return _tasksPage;
            }
        }
        public ReportPage ReportPage
        {
            get
            {
                if (_reportPage == null)
                    _reportPage = new ReportPage(_serviceFactory.CreateReportService());
                return _reportPage;
            }
        }
        public GroupsPage GroupsPage
        {
            get
            {
                if (_groupsPage == null)
                    _groupsPage = new GroupsPage(_serviceFactory.CreateConfigurationService());
                return _groupsPage;
            }
        }
        public ArchivePage ArchivePage
        {
            get
            {
                if (_archivePage == null)
                    _archivePage = new ArchivePage(_serviceFactory.CreateActivityService(), holdersConversionService);
                return _archivePage;
            }
        }
    }
}
