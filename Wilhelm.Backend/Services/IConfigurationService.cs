using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;

namespace Wilhelm.Backend.Services
{
    public interface IConfigurationService
    {
        void SaveTasks(List<WTask> tasks);
        List<WTask> GetTasks();
        void SaveGroups(List<WGroup> groups);
        List<WGroup> GetGroups();
    }
}
