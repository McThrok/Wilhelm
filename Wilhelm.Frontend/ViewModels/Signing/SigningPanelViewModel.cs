using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.DataAccess;

namespace Wilhelm.Frontend.ViewModels.Signing
{
    public class SigningPanelViewModel : INotifyPropertyChanged
    {
        public ICommand SwitchSignCmd { get; private set; }
        private readonly IAccountsService _accountsService;
        private BaseSignViewModel _selectedViewModel;
        private Action<int, string> _logInAction;

        public SigningPanelViewModel(Action<int,string> LogInAction)
        {
            _accountsService = new AccountsService(new WContextFactory(), new ConversionService(), new HashService());
            _logInAction = LogInAction;
            ShowSignIn(null);
        }
        
        private void ShowSignIn(object obj)
        {
            SelectedViewModel = new SignInViewModel(_accountsService, SetUser, ShowSighUp);
        }
        private void ShowSighUp(object obj)
        {
            SelectedViewModel = new SignUpViewModel(_accountsService, SetUser, ShowSignIn);
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
