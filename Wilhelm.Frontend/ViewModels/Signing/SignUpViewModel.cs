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
    public class SignUpViewModel : BaseSignViewModel
    {
        public string _confirmPassword;

        public SignUpViewModel(IAccountsService accountsService, Action<int> logInAction, Action<object> signIn) :
            base(accountsService, logInAction)
        {
            SignInCmd = new DelegateCommand(signIn);
            SignUpCmd = new DelegateCommand(SignUp);
        }

        private void SignUp(object obj)
        {
            var result = _accountsService.CreateUserDto(Login, Password, _confirmPassword);
            if (result.ValidationViolations != null && result.ValidationViolations.Count > 0)
                ErrorMessage = string.Join(Environment.NewLine, result.ValidationViolations);
            else if (result.Dto != null)
                _logInAction(result.Dto.Id);
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
                ClearErrorMessage();
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }
    }
}
