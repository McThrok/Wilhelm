using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Shared.Dto
{
    public class ActivityDto: ModelDto
    {
        public DateTime Date { get; set; }
        public TaskDto Task { get; set; }
        public bool IsDone { get; set; }
    }
}
