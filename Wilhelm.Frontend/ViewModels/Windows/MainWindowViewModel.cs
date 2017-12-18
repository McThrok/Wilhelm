using System.ComponentModel;
using Wilhelm.Frontend.ViewModels.Signing;
using Wilhelm.Frontend.ViewModels.Controls;

namespace Wilhelm.Frontend.ViewModels.Windows
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private SigningPanelViewModel _signViewModel;
        private MainPanelViewModel _mainPanel;
        private object _mainContent;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            SetContentForNotLoggedUser();
        }
        public void SetContentForNotLoggedUser()
        {
            _signViewModel = new SigningPanelViewModel(SetContentForLoggedUser);
            MainContent = _signViewModel;
        }

        public void SetContentForLoggedUser(int userId, string login)
        {
            _mainPanel = new MainPanelViewModel(userId, login, SetContentForNotLoggedUser);
            MainContent = _mainPanel;
        }

        public void OnWindowClosing()
        {
            if (MainContent == _mainPanel)
                _mainPanel.ProperClose();
        }

        protected void OnChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public object MainContent
        {
            get
            {
                return _mainContent;
            }
            private set
            {
                _mainContent = value;
                OnChanged(nameof(MainContent));
            }
        }
    }
}
