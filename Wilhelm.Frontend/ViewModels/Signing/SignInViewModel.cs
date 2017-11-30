using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wilhelm.Frontend.Support;

namespace Wilhelm.Frontend.ViewModels.Signing
{
    public class SignInViewModel : INotifyPropertyChanged
    {
        public string _login;
        public string _password;

        public ICommand SignInCmd { get; private set; }
        public ICommand SignUpCmd { get; private set; }

        public SignInViewModel(Action<object> signUp)
        {
            SignUpCmd = new DelegateCommand(signUp);
            SignInCmd = new DelegateCommand(SignIn);
        }
        private void SignIn(object obj)
        {

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
                _password = value;
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
