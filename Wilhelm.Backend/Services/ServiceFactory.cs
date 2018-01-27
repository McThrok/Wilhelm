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
        private readonly IActivityGenerationService _activityGenerationService;

        public ServiceFactory()
        {
            _wContextFactory = new WContextFactory();
            _conversionService = new ConversionService();
            _entitiesService = new EntitiesService(_conversionService);
            _activityGenerationService = new ActivityGenerationService();
        }

        public IConfigurationService CreateConfigurationService()
        {
            return new ConfigurationService(_wContextFactory, _entitiesService, _conversionService);
        }
        public IActivityService CreateActivityService()
        {
            return new ActivityService(_wContextFactory, _entitiesService,_activityGenerationService);
        }
        public IReportService CreateReportService()
        {
            return new ReportService(_wContextFactory);
        }

    }
}
