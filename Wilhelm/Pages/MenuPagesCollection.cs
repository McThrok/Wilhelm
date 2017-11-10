using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Services.Interfaces;

namespace Wilhelm.Frontend.Pages
{
    public class MenuPagesCollection
    {

        private readonly IServiceFactory _serviceFactory;
        HomePage _homePage;
        TasksPage _tasksPage;
        ReportPage _reportPage;
        GroupsPage _groupsPage;
        ArchivePage _archivePage;

        public MenuPagesCollection(IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public HomePage HomePage
        {
            get
            {
                if (_homePage == null)
                    _homePage = new HomePage();
                return _homePage;
            }
        }
        public TasksPage TasksPage
        {
            get
            {
                if (_tasksPage == null)
                    _tasksPage = new TasksPage();
                return _tasksPage;
            }
        }
        public ReportPage ReportPage
        {
            get
            {
                if (_reportPage == null)
                    _reportPage = new ReportPage();
                return _reportPage;
            }
        }
        public GroupsPage GroupsPage
        {
            get
            {
                if (_groupsPage == null)
                    _groupsPage = new GroupsPage();
                return _groupsPage;
            }
        }
        public ArchivePage ArchivePage
        {
            get
            {
                if (_archivePage == null)
                    _archivePage = new ArchivePage();
                return _archivePage;
            }
        }
    }
}
