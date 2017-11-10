﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.DataAccess
{
    [Table("Activities")]
    public class WActivity:WModel
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public WTask WTask { get; set; }
        public bool IsDone { get; set; }

        public static bool operator == (WActivity a1, WActivity a2)
        {
            return a1.Id == a2.Id;
        }
        public static bool operator !=(WActivity a1, WActivity a2)
        {
            return !(a1==a2);
        }
    }
}
