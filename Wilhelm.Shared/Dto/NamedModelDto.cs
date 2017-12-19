using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Shared.Dto
{
    public abstract class NamedModelDto : ModelDto
    {
        public string Name { get; set; }
        public int OwnerId {get;set;}
        public string Description { get; set; }
        public bool Archivized { get; set; }
    }
}
