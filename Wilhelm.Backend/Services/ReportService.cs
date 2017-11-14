using System;
using System.Collections.Generic;
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
            var data = GetDataToAnalyze();

            reports.Add(GetTotalNumberOfAcitivitiesReport(data));

            return reports;
        }
        
        public IEnumerable<WActivity> GetDataToAnalyze()
        {
            IEnumerable<WActivity> data = null;
            using (var db = _wContextFactory.Create())
            {
                data = db.WActivities.ToList();
            }
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

    }
}
