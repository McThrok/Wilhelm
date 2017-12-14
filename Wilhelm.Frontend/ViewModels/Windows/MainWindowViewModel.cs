using System.ComponentModel;
using Wilhelm.Frontend.ViewModels.Signing;
using Wilhelm.Frontend.ViewModels.Controls;

namespace Wilhelm.Frontend.ViewModels.Windows
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly SigningPanelViewModel _signViewModel;
        private MainPanelViewModel _mainPanel;
        private object _mainContent;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowViewModel()
        {
            _signViewModel = new SigningPanelViewModel(SetContentForLoggedUser);
            MainContent = _signViewModel;
            //MainContent = new MainPanelViewModel(1); //DEBUG
        }

        public void SetContentForLoggedUser(int userId)
        {
            _mainPanel = new MainPanelViewModel(userId);
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
