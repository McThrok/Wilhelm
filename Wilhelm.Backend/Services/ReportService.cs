﻿using System;
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
        public List<ReportDto> GetReports()
        {
            var reports = new List<ReportDto>();
          //  var data = GetDataToAnalyze();

            using (var db = _wContextFactory.Create())
            {
                var data = db.WActivities.Include(a => a.WTask);
                reports.Add(GetTotalNumberOfAcitivitiesReport(data));
                reports.Add(GetCountOfDoneActivities(data));
                reports.Add(GetPercentOfDoneActivities(data));
                reports.Add(GetCountOfSkipActivities(data));
                reports.Add(GetPercentOfSkipActivities(data));

                reports.Add(GetCountOfMostDoneActivities(data));
                reports.Add(GetPercentOfMostDoneActivities(data));
                reports.Add(GetCountOfMostSkippedActivities(data));
                reports.Add(GetPercentOfMostSkippedActivities(data));
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
        public ReportDto GetCountOfMostDoneActivities(IEnumerable<WActivity> data)
        {
            var aa = data.GroupBy(a => a.WTask.Id).ToList();
            var report = new ReportDto
            {
                Category = "Count of most done activity",
                Value = "a"
            };
            return report;
        }
        public ReportDto GetPercentOfMostDoneActivities(IEnumerable<WActivity> data)
        {
            var report = new ReportDto
            {
                Category = "Percent of most done activity (to all done activities)",
                Value = "a"
            };
            return report;
        }
        public ReportDto GetCountOfMostSkippedActivities(IEnumerable<WActivity> data)
        {
            var report = new ReportDto
            {
                Category = "Count of most skipped activities",
                Value = "a"
            };
            return report;
        }
        public ReportDto GetPercentOfMostSkippedActivities(IEnumerable<WActivity> data)
        {
            var report = new ReportDto
            {
                Category = "Percent of most skipped activities",
                Value = "a"
            };
            return report;
        }
    }
}
