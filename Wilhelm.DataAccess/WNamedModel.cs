using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.DataAccess
{
    public abstract class WNamedModel:WModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public WUser Owner { get; set; }
        public string Description { get; set; }
        public bool Archivized { get; set; }
    }
}
