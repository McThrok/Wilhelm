using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Wilhelm.Frontend.Pages;
using Wilhelm.Frontend.Services;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.Support;

namespace Wilhelm.Frontend.ViewModels.Controls
{
    public class MainPanelViewModel : INotifyPropertyChanged
    {
        private readonly MenuPagesCollection _pages;
        private readonly IProxyService _proxyService;
        private readonly IHoldersConversionService _holdersConversionService;
        private readonly IHoldersService _holdersService;
        private object _page;
        private int _userId;
        private Action _changeMainWIndowContent;
        public string UserName { get; private set; }

        public ICommand HomeCmd { get; protected set; }
        public ICommand TasksCmd { get; protected set; }
        public ICommand GroupsCmd { get; protected set; }
        public ICommand ArchivesCmd { get; protected set; }
        public ICommand ReportsCmd { get; protected set; }
        public ICommand LogOutCmd { get; protected set; }

        public MainPanelViewModel(int userId, string login, Action changeMainWindowContent)
        {
            _userId = userId;
            _proxyService = new ProxyService();
            _holdersConversionService = new HoldersConversionService();
            _holdersService = new HoldersService(_holdersConversionService);
            _pages = new MenuPagesCollection(_proxyService, _holdersConversionService, _holdersService);
            _changeMainWIndowContent = changeMainWindowContent;
            UserName = login;

            HomeCmd = new DelegateCommand(Home);
            TasksCmd = new DelegateCommand(Tasks);
            GroupsCmd = new DelegateCommand(Groups);
            ArchivesCmd = new DelegateCommand(Archives);
            ReportsCmd = new DelegateCommand(Reports);
            LogOutCmd = new DelegateCommand(LogOut);

            ClickMenu(_pages.HomePage);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        private void Home()
        {
            ClickMenu(_pages.HomePage);
        }
        private void Tasks()
        {
            ClickMenu(_pages.TasksPage);
        }
        private void Groups()
        {
            ClickMenu(_pages.GroupsPage);
        }
        private void Archives()
        {
            ClickMenu(_pages.ArchivePage);
        }
        private void Reports()
        {
            ClickMenu(_pages.ReportPage);
        }
        private void LogOut()
        {
            var currentPage = MainPanelContent;
            if (currentPage != null && currentPage is IMenuPage currentMenuPage)
                currentMenuPage.Save();
            _changeMainWIndowContent();
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
