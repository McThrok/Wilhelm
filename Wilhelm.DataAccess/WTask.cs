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
    public class WTask
    {
        public WTask()
        {
            WGroups = new List<WGroup>();
        }

        [Key]
        public int TaskId { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int Frequency { get; set; }

        public List<WGroup> WGroups { get; set; }
    }
}
