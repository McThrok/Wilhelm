using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Backend.Model.Dto
{
    public class ActivityDto: ModelDto
    {
        public DateTime Date { get; set; }
        public TaskDto Task { get; set; }
        public bool IsDone { get; set; }
    }
}
