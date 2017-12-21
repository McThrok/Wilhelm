using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Wilhelm.Client.Model;
using Wilhelm.Client.Support;

namespace Wilhelm.Client.ViewModels.Windows
{
    class ChooseItemWindowViewModel
    {
        public NamedHolder SelectedHolder { get; private set; }
        private List<NamedHolder> _holders;
        public ICommand OkCmd { get; protected set; }
        public ICommand CancelCmd { get; protected set; }
        public Action CloseAction { get; set; }
        public ChooseItemWindowViewModel()
        {
            OkCmd = new DelegateCommand<object>(Ok);
            CancelCmd = new DelegateCommand(Cancel);
        }
        private void Ok(object obj)
        {
            SelectedHolder = Holders[(int)obj];
            CloseAction();
        }
        private void Cancel()
        {
            SelectedHolder = null;
            CloseAction();
        }
        public List<NamedHolder> Holders
        {
            get { return _holders; }
            set { _holders = value; }
        }
    }
}
