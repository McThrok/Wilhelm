using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Frontend.Model
{
    public class UserHolder : Holder
    {
        private string _login;

        public string Login
        {
            get
            {
                return _login;
            }
            set
            {
                _login = value;
                OnChanged(nameof(Login));
            }
        }
    }
}
