using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Shared.Dto
{
    public class ConfigDto
    {
        public List<TaskDto> Tasks { get; set; }
        public List<GroupDto> Groups { get; set; }
    }
}
