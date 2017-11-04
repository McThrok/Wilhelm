using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model.Dto;

namespace Wilhelm.Frontend.Model
{
    public abstract class Holder : INotifyPropertyChanged
    {
        public ModelDto Model { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int Id
        {
            get
            {
                return Model.Id;
            }
            set
            {
                Model.Id = value;
            }
        }
    }
}
