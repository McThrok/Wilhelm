using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Support;

namespace Wilhelm.Frontend.ViewModels.Signing
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        public string _login;
        public string _password;

        public readonly IAccountsService _accountsService;
        public ICommand SignInCmd { get; private set; }
        public ICommand SignUpCmd { get; private set; }

        private Action<int> _setUser;

        public SignInViewModel(IAccountsService accountsService, Action<object> signUp, Action<int> setUser)
        {
            SignUpCmd = new DelegateCommand(signUp);
            SignInCmd = new DelegateCommand(SignIn);
            _setUser = setUser;
            _accountsService = accountsService;
        }
        private void SignIn(object obj)
        {
            var result = _accountsService.VerifyUser(_login, _password);
            if (result.Object != null)
                _setUser(result.Object.Id);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
                OnPropertyChanged(nameof(Login));
            }
        }
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
    }
}
