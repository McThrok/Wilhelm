using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wilhelm.Backend.Services;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.DataAccess;

namespace Wilhelm.Frontend.ViewModels.Signing
{
    public class SignViewModel : INotifyPropertyChanged
    {
        public readonly IConversionService _conversionService;
        public readonly IAccountsService _accountsService;
        public ICommand SwitchSignCmd { get; private set; }
        private object selectedViewModel;

        public SignViewModel()
        {
            _conversionService = new ConversionService();
            _accountsService = new AccountsService(new WContextFactory(), _conversionService);
            ShowSighIn(null);
        }


        private void ShowSighIn(object obj)
        {
            SelectedViewModel = new SignInViewModel(_accountsService,ShowSighUp);

        }
        private void ShowSighUp(object obj)
        {
            SelectedViewModel = new SignUpViewModel(_accountsService, ShowSighIn);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }

        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set
            {
                selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
    }
}
