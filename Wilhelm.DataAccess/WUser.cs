using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.DataAccess
{
    [Table("Users")]
    public class WUser : WModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
