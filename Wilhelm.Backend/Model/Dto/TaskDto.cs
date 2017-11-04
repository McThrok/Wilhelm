using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Backend.Model.Dto
{
    public class TaskDto: NamedModelDto
    {
        public DateTime StartDate { get; set; }
        public int Frequency { get; set; }
        public List<GroupDto> Groups { get; set; }
    }
}
