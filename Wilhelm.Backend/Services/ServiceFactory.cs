using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Services.Interfaces;

namespace Wilhelm.Backend.Services

{
    public class ServiceFactory : IServiceFactory
    {
        public IConfigurationService CreateConfigurationService()
        {
            return new ConfigurationService();
        }
        public IActivityService CreateActivityService()
        {
            return new ActivityService();
        }
        public IReportService CreateReportService()
        {
            return new ReportService();
        }
    }
}
