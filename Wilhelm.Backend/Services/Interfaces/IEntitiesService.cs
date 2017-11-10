using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services.Interfaces
{
    public interface IEntitiesService
    {
        void UpdateConfig(ConfigDto dto, IDbSet<WTask> wTasks, IDbSet<WGroup> wGroups);
        void UpdateEntities(IDbSet<WTask> wTasks, IDbSet<WGroup> wGroups, ConfigDto config);
    }
}
