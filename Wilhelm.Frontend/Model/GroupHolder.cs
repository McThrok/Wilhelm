using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Frontend.Model
{
    public class GroupHolder : NamedHolder
    {
        public ObservableCollection<TaskHolder> Tasks { get;  set; }
    }
}
