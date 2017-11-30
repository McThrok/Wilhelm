using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.DataAccess
{
    public interface IWContext : IDisposable
    {
        IDbSet<WTask> WTasks { get; set; }
        IDbSet<WGroup> WGroups { get; set; }
        IDbSet<WActivity> WActivities { get; set; }
        IDbSet<WUser> Users { get; set; }

        int SaveChanges();
    }
}
