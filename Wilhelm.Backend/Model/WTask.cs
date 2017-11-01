using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Backend.Model
{
    public class WTask : WNamedModel
    {
        public DateTime StartDate { get; set; }
        public int Frequency { get; set; }
        public List<WGroup> Groups { get; set; }
    }
}
