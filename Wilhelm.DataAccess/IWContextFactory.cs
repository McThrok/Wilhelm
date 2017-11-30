using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.DataAccess
{
    public interface IWContextFactory
    {
        IWContext Create();
    }
}
