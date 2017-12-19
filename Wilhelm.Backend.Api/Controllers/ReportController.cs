using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Backend.Services;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Backend.Api.Controllers
{
    public class ReportController : ApiController
    {
        private readonly IReportService _reportService;
        public ReportController()
        {
            _reportService = new ServiceFactory().CreateReportService();
        }

        public List<ReportDto> GetReports(int userId)
        {
            return _reportService.GetReports(userId);
        }
    }
}
