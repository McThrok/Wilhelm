using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.DataAccess
{
    [Table("Groups")]
    public class WGroup : WNamedModel
    {
        public WGroup()
        {
            WTasks = new List<WTask>();
        }

        public virtual List<WTask> WTasks { get; set; }
    }
}
