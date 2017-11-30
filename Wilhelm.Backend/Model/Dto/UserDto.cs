using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Backend.Model.Dto
{
    public class UserDto: ModelDto
    {
        public string Name { get; set; }
        public string Password { get; set; }
    }
}
