using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Backend.Model.Dto
{
    public class GroupDto : NamedModelDto
    {
        public List<TaskDto> Tasks { get; set; }
    }
}
