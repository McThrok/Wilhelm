using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.DataAccess
{
    [Table("Tasks")]
    public class WTask : WNamedModel
    {
        public WTask()
        {
            WGroups = new List<WGroup>();
        }

        public DateTime StartDate { get; set; }
        [Required]
        public int Frequency { get; set; }
        [Required]
        public WUser Owner { get; set; }

        public virtual List<WGroup> WGroups { get; set; }
    }
}
