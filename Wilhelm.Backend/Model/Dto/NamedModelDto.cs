using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Backend.Model.Dto
{
    public abstract class NamedModelDto : ModelDto
    {
        public string Name { get; set; }
        public UserDto Owner {get;set;}
        public string Description { get; set; }
        public bool Archivized { get; set; }
    }
}
