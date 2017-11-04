using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Model
{
    public class WConfig
    {
        public List<WTask> WTasks { get; set; }
        public List<WGroup> WGroups { get; set; }
    }
}
