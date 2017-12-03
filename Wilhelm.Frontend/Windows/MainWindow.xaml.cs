using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wilhelm.Backend.Services;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.Services;
using Wilhelm.Frontend.Pages;
using System.ComponentModel;
using Wilhelm.Frontend.ViewModels.Signing;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Frontend.ViewModels.Controls;

namespace Wilhelm.Frontend.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly SigningPanelViewModel _signViewModel;
        private MainPanelViewModel _maipanel;


        public MainWindow()
        {
            InitializeComponent();
            _signViewModel = new SigningPanelViewModel(SetMainManetASContent);
            //MainContent.Content = _signViewModel;
            MainContent.Content = new MainPanelViewModel(123321); //DEBUG
        }

        public void SetMainManetASContent(int userId)
        {
            _maipanel = new MainPanelViewModel(userId);
            MainContent.Content = _maipanel;

        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (MainContent.Content == _maipanel)
                _maipanel.ProperClose();
            base.OnClosing(e);
        }
    }
}
