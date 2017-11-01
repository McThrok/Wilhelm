using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Backend.Model
{
    public class WGroup : WNamedModel
    {
        List<WTask> Tasks { get; set; }

    }
}
