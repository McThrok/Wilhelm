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
                if (!task.Archivized)
                {
                    WActivity latestActivity = null;
                    if (activities != null && activities.FirstOrDefault() !=null)
                    {
                        var latestActivityDate = activities.Max(x => x.Date);
                        latestActivity = activities.First(x => x.Date == latestActivityDate);
                    }
                    generatedActivities.AddRange(GenerateActivitiesForTask(latestActivity, task, date));
                }

            return generatedActivities;
        }
        public List<WActivity> GenerateActivitiesForTask(WActivity lastActivity, WTask task, DateTime date)
        {
            var result = new List<WActivity>();

            DateTime nextActivityDate = task.StartDate.Date;

            if (lastActivity != null)
            {
                var nextFromActivity = lastActivity.Date.Date.AddDays(task.Frequency);
                if (DateTime.Compare(nextFromActivity, nextActivityDate) > 0)
                    nextActivityDate = nextFromActivity;
            }

            while (DateTime.Compare(nextActivityDate, date) <= 0)
            {
                result.Add(new WActivity() { Date = nextActivityDate, WTask = task });
                nextActivityDate = nextActivityDate.AddDays(task.Frequency);
            }

            return result;
        }
    }
}
