using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Shared.Dto
{
    public class TaskDto: NamedModelDto
    {
        public DateTime StartDate { get; set; }
        public int Frequency { get; set; }
        public List<GroupDto> Groups { get; set; }
    }
}
