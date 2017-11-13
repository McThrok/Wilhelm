using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;

namespace Wilhelm.Frontend.Model
{
    public class ActivityHolder : Holder
    {
        private TaskHolder _task;
        private DateTime _date;
        private bool _isDone;

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
                return _isDone;
            }
            set
            {
                _isDone = value;
                OnChanged(nameof(IsDone));
            }
        }


    }
}
