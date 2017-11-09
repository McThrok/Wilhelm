using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services.Interfaces;

namespace Wilhelm.Backend.Services
{
   internal class ReportService : IReportService
    {
        public List<ReportDto> GetReports()
        {
            return MockBase.MockBase.GetReports();
        }
    }
}
