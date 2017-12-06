using System.ComponentModel;
using Wilhelm.Frontend.ViewModels.Signing;
using Wilhelm.Frontend.ViewModels.Controls;

namespace Wilhelm.Frontend.ViewModels.Windows
{
    class MainWindowViewModel
    {
        private readonly SigningPanelViewModel _signViewModel;
        private MainPanelViewModel _mainPanel;

        public MainWindowViewModel()
        {
            _signViewModel = new SigningPanelViewModel(SetMainManetASContent);
            //MainContent.Content = _signViewModel;
            //MainContent.Content = new MainPanelViewModel(123321); //DEBUG
            MainContent = new MainPanelViewModel(123321); //DEBUG
        }

        public void SetMainManetASContent(int userId)
        {
            _mainPanel = new MainPanelViewModel(userId);
           // MainContent.Content = _mainPanel;
        }

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    //if (MainContent.Content == _mainPanel)
        //    if (MainContent == _mainPanel)
        //        _mainPanel.ProperClose();
        //    base.OnClosing(e);
        //}
        public MainPanelViewModel MainContent
        {
            get
            {
                return _mainPanel;
            }
            set
            {
                _mainPanel = value;
            }
        }
    }
}
