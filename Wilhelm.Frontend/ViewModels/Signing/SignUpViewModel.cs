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
    public class SignUpViewModel : BaseSignViewModel
    {
        public string _confirmPassword;

        public SignUpViewModel(IAccountsService accountsService, Action<int> logInAction, Action<object> signIn):
            base(accountsService,logInAction)
        {
            SignInCmd = new DelegateCommand(signIn);
            SignUpCmd = new DelegateCommand(SignUp);
        }

        private void SignUp(object obj)
        {
            var result = _accountsService.CreateUser(_login, _password, _confirmPassword);
            if (result.Object != null)
                _logInAction(result.Object.Id);
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
