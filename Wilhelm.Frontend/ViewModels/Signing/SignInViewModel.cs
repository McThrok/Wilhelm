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
    public class SignInViewModel : BaseSignViewModel
    {
        public SignInViewModel(IAccountsService accountsService, Action<int> logInAction, Action<object> signUp) :
            base(accountsService, logInAction)
        {
            SignUpCmd = new DelegateCommand(signUp);
            SignInCmd = new DelegateCommand(SignIn);
        }
        private void SignIn(object obj)
        {
            var result = _accountsService.VerifyUserDto(_login, _password);
            if (result.Object != null)
                _logInAction(result.Object.Id);
        }
    }
}
