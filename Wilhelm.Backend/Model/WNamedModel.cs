using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Backend.Model
{
    public abstract class WNamedModel :WModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Archivized { get; set; }
    }
}
