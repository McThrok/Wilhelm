using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Frontend.Support;

namespace Wilhelm.Frontend.ViewModels.Signing
{
    public class SignUpViewModel : INotifyPropertyChanged
    {
        public string _login;
        public string _password;
        public string _confirmPassword;

        public readonly IAccountsService _accountsService;
        public ICommand SignUpCmd { get; private set; }
        public ICommand SignInCmd { get; private set; }

        public SignUpViewModel(IAccountsService accountsService, Action<object> signIn)
        {
            SignInCmd = new DelegateCommand(signIn);
            SignUpCmd = new DelegateCommand(SignUp);
            _accountsService = accountsService;
        }

        private void SignUp(object obj)
        {
            var result = _accountsService.CreateUser(_login, _password, _confirmPassword);
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
        public string ConfirmPassword
        {
            get
            {
                return _confirmPassword;
            }
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }
    }
}
