using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.DataAccess;

namespace Wilhelm.Frontend.ViewModels.Signing
{
    public class SignViewModel : INotifyPropertyChanged
    {
        public ICommand SwitchSignCmd { get; private set; }
        private readonly IAccountsService _accountsService;
        private object selectedViewModel;
        private Action<int> _logIn;

        public SignViewModel(Action<int> LogIn)
        {
            _accountsService = new AccountsService(new WContextFactory(), new ConversionService(), new HashService());
            _logIn = LogIn;
            ShowSighIn(null);
        }

        private void ShowSighIn(object obj)
        {
            SelectedViewModel = new SignInViewModel(_accountsService, ShowSighUp, SetUser);

        }
        private void ShowSighUp(object obj)
        {
            SelectedViewModel = new SignUpViewModel(_accountsService, ShowSighIn, SetUser);
        }
        private void SetUser(int userId)
        {
            _logIn(userId);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public object SelectedViewModel
        {
            get
            {
                return selectedViewModel;
            }
            set
            {
                selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
    }
}
