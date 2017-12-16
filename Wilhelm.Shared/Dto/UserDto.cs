using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Shared.Dto
{
    public class UserDto: ModelDto
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
