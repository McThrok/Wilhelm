using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services.Interfaces
{
    public interface IActivityGenerationService
    {
        List<WActivity> GenerateActivities(IEnumerable<WActivity> activities, IEnumerable<WTask> tasks, DateTime date);
       
    }
}
