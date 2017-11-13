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
                var generatedActivity = GenerateActivity(lastActivity, task, date);
                if (generatedActivity != null)
                    generatedActivities.Add(generatedActivity);
            }

            return generatedActivities;
        }
        public WActivity GenerateActivity(WActivity lastActivity, WTask task, DateTime date)
        {
            if (lastActivity != null && lastActivity.Date.Date == date.Date)
                return null;

            if (DateTime.Compare(task.StartDate.Date, date.Date) > 0)
                return null;

            if (lastActivity != null && DateTime.Compare(lastActivity.Date.AddDays(task.Frequency), date.Date) > 0)
                return null;

            var newActivity = new WActivity()
            {
                WTask = task,
                Date = date,
            };

            if (lastActivity == null && task.StartDate.Date == date.Date)
                return newActivity;

            if (lastActivity != null && DateTime.Compare(lastActivity.Date.AddDays(task.Frequency), date.Date) == 0)
                return newActivity;

            throw new Exception("activity generation exception");
        }
    }
}
