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
    public abstract class BaseSignViewModel : INotifyPropertyChanged
    {
        private string _login;
        private string _password;
        private string _errorMessage;
        protected Action<int, string> _logInAction;

        protected readonly IAccountsService _accountsService;
        public ICommand SignInCmd { get; protected set; }
        public ICommand SignUpCmd { get; protected set; }


        public BaseSignViewModel(IAccountsService accountsService, Action<int,string> logInAction)
        {
            _logInAction = logInAction;
            _accountsService = accountsService;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
        protected void ClearErrorMessage()
        {
            ErrorMessage = null;
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
                ClearErrorMessage();
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
                ClearErrorMessage();
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }
    }
}
