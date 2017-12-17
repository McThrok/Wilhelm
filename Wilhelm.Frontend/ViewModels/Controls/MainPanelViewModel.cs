using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wilhelm.Backend.Services;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Pages;
using Wilhelm.Frontend.Services;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.Support;

namespace Wilhelm.Frontend.ViewModels.Controls
{
    public class MainPanelViewModel : INotifyPropertyChanged
    {
        private readonly MenuPagesCollection _pages;
        private readonly IServiceFactory _serviceFactory;
        private readonly IHoldersConversionService _holdersConversionService;
        private readonly IHoldersService _holdersService;
        private object _page;
        private int _userId;
        public string UserName { get; private set; }

        public ICommand HomeCmd { get; protected set; }
        public ICommand TasksCmd { get; protected set; }
        public ICommand GroupsCmd { get; protected set; }
        public ICommand ArchivesCmd { get; protected set; }
        public ICommand ReportsCmd { get; protected set; }

        public MainPanelViewModel(int userId, string login)
        {
            _userId = userId;
            _serviceFactory = new ServiceFactory();
            _holdersConversionService = new HoldersConversionService();
            _holdersService = new HoldersService(_holdersConversionService);
            _pages = new MenuPagesCollection(_serviceFactory, _holdersConversionService, _holdersService);
            UserName = login;

            HomeCmd = new DelegateCommand(Home);
            TasksCmd = new DelegateCommand(Tasks);
            GroupsCmd = new DelegateCommand(Groups);
            ArchivesCmd = new DelegateCommand(Archives);
            ReportsCmd = new DelegateCommand(Reports);

            ClickMenu(_pages.HomePage);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        private void Home(object obj)
        {
            ClickMenu(_pages.HomePage);
        }
        private void Tasks(object obj)
        {
            ClickMenu(_pages.TasksPage);
        }
        private void Groups(object obj)
        {
            ClickMenu(_pages.GroupsPage);
        }
        private void Archives(object obj)
        {
            ClickMenu(_pages.ArchivePage);
        }
        private void Reports(object obj)
        {
            ClickMenu(_pages.ReportPage);
        }
        private void ClickMenu(UserControl page)
        {
            var currentPage = MainPanelContent;
            if (currentPage == page)
                return;

            if (currentPage != null && currentPage is IMenuPage currentMenuPage)
                currentMenuPage.Save();

            if (page != null && page is IMenuPage newMenuPage)
                newMenuPage.Activate(_userId);

            MainPanelContent = page;
        }
        private void ClickMenu(object page)
        {
            var currentPage = MainPanelContent;
            if (currentPage == page)
                return;

            if (currentPage != null && currentPage is IMenuPage currentMenuPage)
                currentMenuPage.Save();

            if (page != null && page is IMenuPage newMenuPage)
                newMenuPage.Activate(_userId);

            MainPanelContent = page;
        }
        public void ProperClose()
        {
            var currentPage = _page;
            if (currentPage != null && currentPage is IMenuPage currentMenuPage)
                currentMenuPage.Save();
        }

        public object MainPanelContent
        {
            get
            {
                return _page;
            }
            set
            {
                _page = value;
                OnPropertyChanged(nameof(MainPanelContent));
            }
        }
    }
}
