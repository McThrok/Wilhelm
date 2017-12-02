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
        protected string _login;
        protected string _password;
        protected Action<int> _logInAction;

        protected readonly IAccountsService _accountsService;
        public ICommand SignInCmd { get; protected set; }
        public ICommand SignUpCmd { get; protected set; }


        public BaseSignViewModel(IAccountsService accountsService, Action<int> logInAction)
        {
            _logInAction = logInAction;
            _accountsService = accountsService;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
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
