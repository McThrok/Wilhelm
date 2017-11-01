using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Frontend.Model
{
    public class ActivityHolder : Holder
    {
        private TaskHolder _task;
        private DateTime _date;
        private bool _state;

        public TaskHolder Task
        {
            get
            {
                return _task;
            }
            set
            {
                _task = value;
                OnChanged(nameof(Task));
            }
        }
        public DateTime Date
        {
            get
            {
                return _date;
            }
            set
            {
                _date = value;
                OnChanged(nameof(Date));
                OnChanged(nameof(DisplayDate));
            }
        }
        public string DisplayDate
        {
            get
            {
                return _date.ToLongDateString();
            }

        }
        public bool IsDone
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
                OnChanged(nameof(IsDone));
            }
        }

        public ActivityHolder() : base() { }
        public ActivityHolder(long id) : base(id) { }

    }
}
