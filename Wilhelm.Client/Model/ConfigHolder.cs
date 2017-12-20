using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Client.Model
{
    public class ConfigHolder
    {
        public List<GroupHolder> Groups { get; set; }
        public List<TaskHolder> Tasks { get; set; }
    }
}
