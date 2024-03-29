﻿using System;
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
    public class SignInViewModel : BaseSignViewModel
    {
        public SignInViewModel(IAccountProxyService accountProxyService, Action<int,string> logInAction, Action signUp) :
            base(accountProxyService, logInAction)
        {
            SignUpCmd = new DelegateCommand(signUp);
            SignInCmd = new AwaitableDelegateCommand(SignIn);
        }
        private async Task SignIn()
        {
            var result = await _accountProxyService.GetVerifiedUer(Login, Password);
            if (result.ValidationViolations != null && result.ValidationViolations.Count > 0)
                ErrorMessage = string.Join(Environment.NewLine, result.ValidationViolations);
            else if (result.Dto != null)
                _logInAction(result.Dto.Id, Login);
        }
    }
}
