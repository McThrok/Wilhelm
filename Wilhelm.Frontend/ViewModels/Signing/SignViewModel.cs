using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Wilhelm.Frontend.ViewModels.Signing
{
    public class SignViewModel : INotifyPropertyChanged
    {
        public ICommand SwitchSignCmd { get; private set; }
        private object selectedViewModel;
        public object SelectedViewModel
        {
            get { return selectedViewModel; }
            set { selectedViewModel = value; OnPropertyChanged(nameof(SelectedViewModel)); }
        }

        public SignViewModel()
        {
            ShowSighIn(null);
        }


        private void ShowSighIn(object obj)
        {
            SelectedViewModel = new SignInViewModel(ShowSighUp);

        }
        private void ShowSighUp(object obj)
        {
            SelectedViewModel = new SignUpViewModel(ShowSighIn);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
