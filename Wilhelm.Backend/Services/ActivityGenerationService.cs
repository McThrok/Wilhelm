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
            List<WActivity> generatedTodayTasks = new List<WActivity>();
            foreach (var task in tasks)
                if (!task.Archivized)
                {
                    WActivity latestActivity = null;
                    var activitiesForTask = activities.Where(x => x.WTask.Id == task.Id);
                    if (activitiesForTask != null && activitiesForTask.FirstOrDefault() !=null)
                    {
                        var latestActivityDate = activitiesForTask.Max(x => x.Date);
                        latestActivity = activitiesForTask.First(x => x.Date == latestActivityDate);
                    }
                    generatedTodayTasks.AddRange(GenerateActivitiesForTask(latestActivity, task, date.Date));
                }

            return generatedTodayTasks;
        }
        private List<WActivity> GenerateActivitiesForTask(WActivity latestActivity, WTask task, DateTime date)
        {
            var result = new List<WActivity>();

            DateTime nextActivityDate = task.StartDate.Date;

            if (latestActivity != null)
            {
                var nextFromActivity = latestActivity.Date.Date.AddDays(task.Frequency);
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
