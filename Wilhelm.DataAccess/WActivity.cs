using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.DataAccess
{
    [Table("Activities")]
    public class WActivity
    {
        [Key]
        public int ActivityId { get; set; }
        [Required]
        public WTask WTask { get; set; }
        [Required]
        public DateTime Date { get; set; }
        public bool Status { get; set; }
    }
}
