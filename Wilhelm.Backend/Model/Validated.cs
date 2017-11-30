using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Backend.Model
{
    public class Validated<T>
    {
        public T Object { get; set; }
        public List<string> ValidationViolations { get; set; }
    }
}
