using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wilhelm.Client.Services.Interfaces;
using Wilhelm.Client.Support;

namespace Wilhelm.Client.ViewModels.Signing
{
    public class SignUpViewModel : BaseSignViewModel
    {
        public string _confirmPassword;

        public SignUpViewModel(IAccountProxyService accountProxyService, Action<int, string> logInAction, Action signIn) :
            base(accountProxyService, logInAction)
        {
            SignInCmd = new DelegateCommand(signIn);
            SignUpCmd = new AwaitableDelegateCommand(SignUp);
        }

        private async Task SignUp()
        {
            var result = await _accountProxyService.GetNewUer(Login, Password, _confirmPassword);
            if (result.ValidationViolations != null && result.ValidationViolations.Count > 0)
                ErrorMessage = string.Join(Environment.NewLine, result.ValidationViolations);
            else if (result.Dto != null)
                _logInAction(result.Dto.Id, Login);
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
