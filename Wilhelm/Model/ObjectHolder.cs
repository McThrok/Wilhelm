using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Frontend.Model
{

    public class ObjectHolder : NamedHolder
    {
        public ObservableCollection<TaskHolder> Tasks { get; private set; }
        public ObjectHolder(long id, string name) :
            base(id, name)
        {
            Tasks = new ObservableCollection<TaskHolder>();
        }
    }
}