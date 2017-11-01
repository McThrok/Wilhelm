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
    public class WGroup
    {
        public WGroup()
        {
            WTasks = new List<WTask>();
        }

        [Key]
        public int GroupId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public List<WTask> WTasks { get; set; }
    }
}
