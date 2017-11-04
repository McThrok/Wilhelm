using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model.Dto;

namespace Wilhelm.Backend.Model
{
    public class ConfigDto
    {
        public List<TaskDto> Tasks { get; set; }
        public List<GroupDto> Groups { get; set; }
    }
}
