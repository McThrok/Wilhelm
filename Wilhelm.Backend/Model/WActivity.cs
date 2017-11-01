using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Backend.Model
{
    public class WActivity : WModel
    {
        public DateTime Date { get; set; }
        public WTask Task { get; set; }
        public bool IsDone { get; set; }

    }
}
