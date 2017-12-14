using System.ComponentModel;
using Wilhelm.Frontend.ViewModels.Signing;
using Wilhelm.Frontend.ViewModels.Controls;

namespace Wilhelm.Frontend.ViewModels.Windows
{
    class MainWindowViewModel: INotifyPropertyChanged
    {
        private readonly SigningPanelViewModel _signViewModel;
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
            _mainContent = new MainPanelViewModel(userId);
            MainContent = _mainContent;
        }

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    //if (MainContent.Content == _mainPanel)
        //    if (MainContent == _mainPanel)
        //        _mainPanel.ProperClose();
        //    base.OnClosing(e);
        //}
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        public object MainContent
        {
            get
            {
                return _mainContent;
            }
            set
            {
                _mainContent = value;
                OnPropertyChanged(nameof(MainContent));
            }
        }
    }
}
