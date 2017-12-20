using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wilhelm.Client.Services.Interfaces;
using Wilhelm.Client.Services;

namespace Wilhelm.Client.ViewModels.Signing
{
    public class SigningPanelViewModel : INotifyPropertyChanged
    {
        public ICommand SwitchSignCmd { get; private set; }
        private readonly IAccountProxyService _accountProxyService;
        private BaseSignViewModel _selectedViewModel;
        private Action<int, string> _logInAction;

        public SigningPanelViewModel(Action<int,string> LogInAction)
        {
            _accountProxyService = new AccountProxyService();
            _logInAction = LogInAction;
            ShowSignIn(null);
        }
        
        private void ShowSignIn(object obj)
        {
            SelectedViewModel = new SignInViewModel(_accountProxyService, SetUser, ShowSighUp);
        }
        private void ShowSighUp(object obj)
        {
            SelectedViewModel = new SignUpViewModel(_accountProxyService, SetUser, ShowSignIn);
        }
        private void SetUser(int userId, string login)
        {
            _logInAction(userId, login);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public BaseSignViewModel SelectedViewModel
        {
            get
            {
                return _selectedViewModel;
            }
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
    }
}
