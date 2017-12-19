using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Shared.Dto
{
    public class GroupDto : NamedModelDto
    {
        public List<TaskDto> Tasks { get; set; }
    }
}
