using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.DataAccess
{
    public class WContextFactory: IWContextFactory
    {
        public IWContext Create()
        {
          return new WContext();
        }
    }
}
