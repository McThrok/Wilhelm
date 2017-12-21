using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Client.Model
{
    public class TaskHolder : NamedHolder
    {
        public ObservableCollection<GroupHolder> Groups { get; set; }
        private DateTime _startDate;
        private int _frequency;
        
        public DateTime StartDate
        {
            get
            {
                return _startDate;
            }
            set
            {
                _startDate = value;
                OnChanged(nameof(StartDate));
            }
        }
        public int Frequency
        {
            get
            {
                return _frequency;
            }
            set
            {
                _frequency = value;
                OnChanged(nameof(Frequency));
            }
        }
    }
}
