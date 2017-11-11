using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services

{
    public class ServiceFactory : IServiceFactory
    {
        private readonly IWContextFactory _wContextFactory;
        private readonly IEntitiesService _entitiesService;
        private readonly IConversionService _conversionService;

        public ServiceFactory()
        {
            _wContextFactory = new WContextFactory();
            _conversionService = new ConversionService();
            _entitiesService = new EntitiesService(_conversionService);
        }

        public IConfigurationService CreateConfigurationService()
        {
            return new ConfigurationService(_wContextFactory, _entitiesService);
        }
        public IActivityService CreateActivityService()
        {
            return new ActivityService(_wContextFactory, _entitiesService);
        }
        public IReportService CreateReportService()
        {
            return new ReportService(_wContextFactory);
        }
    }
}
