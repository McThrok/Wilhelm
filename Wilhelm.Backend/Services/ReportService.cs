using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services
{
    internal class ReportService : IReportService
    {
        private readonly IWContextFactory _wContextFactory;

        public ReportService(IWContextFactory wContextFactory)
        {
            _wContextFactory = wContextFactory;
        }
        public List<ReportDto> GetReports(int userId)
        {
            var reports = new List<ReportDto>();
            using (var db = _wContextFactory.Create())
            {
                var data = db.WActivities.Where(x=>x.WTask.OwnerId == userId).Include(a => a.WTask);
                var tasks = db.WTasks;
                reports.Add(GetTotalNumberOfAcitivitiesReport(data));
                reports.Add(GetCountOfDoneActivities(data));
                reports.Add(GetPercentOfDoneActivities(data));
                reports.Add(GetCountOfSkipActivities(data));
                reports.Add(GetPercentOfSkipActivities(data));

                reports.AddRange(GetPercentActivities(data, tasks));
                reports.AddRange(GetCountOfActivities(data, tasks));
            }
            return reports;
        }

        public IEnumerable<WActivity> GetDataToAnalyze()
        {
            IEnumerable<WActivity> data = null;

            return data;
        }
        public ReportDto GetTotalNumberOfAcitivitiesReport(IEnumerable<WActivity> data)
        {
            var report = new ReportDto
            {
                Category = "Total number of activities",
                Value = data.Count().ToString()
            };
            return report;
        }
        public ReportDto GetCountOfDoneActivities(IEnumerable<WActivity> data)
        {
            var report = new ReportDto
            {
                Category = "Count of done activities",
                Value = data.Where(a => a.IsDone == true).Count().ToString()
            };
            return report;
        }
        public ReportDto GetPercentOfDoneActivities(IEnumerable<WActivity> data)
        {
            double percent = data.Where(a => a.IsDone == true).Count() / (float)data.Count() * 100;
            percent = Math.Round(percent, 2);
            var report = new ReportDto
            {
                Category = "Percent of done activities",
                Value = percent.ToString() + "%"
            };
            return report;
        }
        public ReportDto GetCountOfSkipActivities(IEnumerable<WActivity> data)
        {
            var report = new ReportDto
            {
                Category = "Count of skip activities",
                Value = data.Where(a => a.IsDone == false).Count().ToString()
            };
            return report;
        }
        public ReportDto GetPercentOfSkipActivities(IEnumerable<WActivity> data)
        {
            double percent = data.Where(a => a.IsDone == false).Count() / (float)data.Count() * 100;
            percent = Math.Round(percent, 2);
            var report = new ReportDto
            {
                Category = "Percent of skip activities",
                Value = percent.ToString() + "%"
            };
            return report;
        }
        public List<ReportDto> GetCountOfActivities(IEnumerable<WActivity> data, IEnumerable<WTask> tasks)
        {
            var doneActivity = data.Where(a => a.IsDone == true).GroupBy(a => a.WTask.Id).Select(r => new { Id = r.Key, count = r.Count() }).ToList();
            doneActivity = doneActivity.OrderByDescending(a => a.count).ToList();
            List<ReportDto> reports = new List<ReportDto>();
            for (int i = 0; i < doneActivity.Count; i++)
            {
                var task = tasks.Where(t => t.Id == doneActivity[i].Id).Single();
                var taskCount = data.Where(a => a.WTask.Id == task.Id).Count();
                var report = new ReportDto
                {
                    Category = "Activity: " + task.Name.ToUpper(),
                    Value = "Done " + doneActivity[i].count.ToString() + " / " + taskCount.ToString() + " times"
                };
                reports.Add(report);
            }
            return reports;
        }
        public List<ReportDto> GetPercentActivities(IEnumerable<WActivity> data, IEnumerable<WTask> tasks)
        {
            var doneActivity = data.Where(a => a.IsDone == true).GroupBy(a => a.WTask.Id).Select(r => new { Id = r.Key, count = r.Count() }).ToList();
            doneActivity = doneActivity.OrderByDescending(a => a.count).ToList();
            double maxPercent = 0;
            int maxPercentIndex = -1;
            string maxPersentTaskName="";
            double minPercent = 100;
            int minPercentIndex = -1;
            string minPersentTaskName="";
            for (int i = 0; i < doneActivity.Count; i++)
            {
                var task = tasks.Where(t => t.Id == doneActivity[i].Id).Single();
                var taskCount = data.Where(a => a.WTask.Id == task.Id).Count();
                double percent = doneActivity[i].count / (double)taskCount;
                if (percent < minPercent)
                {
                    minPercent = percent;
                    minPercentIndex = i;
                    minPersentTaskName = task.Name;
                }
                if (percent > maxPercent)
                {
                    maxPercent = percent;
                    maxPercentIndex = i;
                    maxPersentTaskName = task.Name;
                }
            }
            List<ReportDto> reports = new List<ReportDto>();
            if (minPercentIndex != -1)
                reports.Add(new ReportDto
                {
                    Category = "Activity with lowest percentage: " + minPersentTaskName.ToUpper(),
                    Value = "Done " + Math.Round(minPercent*100, 2) + " % "
                });
            if (maxPercentIndex != -1)
                reports.Add(new ReportDto
                {
                    Category = "Activity with highest percentage: " + maxPersentTaskName.ToUpper(),
                    Value = "Done " + Math.Round(maxPercent * 100, 2) + " % "
                });
            return reports;
        }
    }
}
