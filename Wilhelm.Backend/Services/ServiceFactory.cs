using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wilhelm.Backend.Services

{
    public class ServiceFactory
    {
        public IConfigurationService GetConfigurationService()
        {
            return new ConfigurationService();
        }
        public IActivityService GetActivityService()
        {
            return new ActivityService();
        }
        public IReportService GetReportService()
        {
            return new ReportService();
        }
    }
}
