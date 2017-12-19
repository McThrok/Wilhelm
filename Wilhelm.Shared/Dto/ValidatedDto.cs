using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Shared.Dto
{
    public class ValidatedDto<T>
    {
        public T Dto { get; set; }
        public List<string> ValidationViolations { get; set; }
    }
}
