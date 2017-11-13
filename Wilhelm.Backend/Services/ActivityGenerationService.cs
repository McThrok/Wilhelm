using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.DataAccess;
using Wilhelm.Backend.Services.Interfaces;

namespace Wilhelm.Backend.Services
{
    public class ActivityGenerationService : IActivityGenerationService
    {
        public List<WActivity> GenerateActivities(IEnumerable<WActivity> activities, IEnumerable<WTask> tasks, DateTime date)
        {
            List<WActivity> generatedActivities = new List<WActivity>();
            foreach (var task in tasks)
            {
                var lastActivity = activities.LastOrDefault(x => x.WTask.Id == task.Id);
                generatedActivities.AddRange(GenerateActivitiesForTask(lastActivity, task, date));
            }

            return generatedActivities;
        }
        public List<WActivity> GenerateActivitiesForTask(WActivity lastActivity, WTask task, DateTime date)
        {
            var result = new List<WActivity>();
            if (lastActivity != null && lastActivity.Date.Date == date.Date)
                return result;

            if (DateTime.Compare(task.StartDate.Date, date.Date) > 0)
                return result;

            if (lastActivity != null && DateTime.Compare(lastActivity.Date.AddDays(task.Frequency), date.Date) > 0)
                return result;

            DateTime lastdate;
            if (DateTime.Compare(lastActivity.Date.Date, task.StartDate.Date) > 0)
                lastdate = lastActivity.Date.Date;
            else
                lastdate = task.StartDate.Date;

            while (DateTime.Compare(lastdate, date) <= 0)
            {
                lastdate.AddDays(task.Frequency);
                result.Add(new WActivity() { Date = lastdate, WTask = task });
            }

            return result;
        }
    }
}
