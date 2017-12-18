using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Backend.Services;
using Wilhelm.Backend.Model;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Backend.Api.Controllers
{
    public class ConfigurationController : ApiController
    {
        private readonly IConfigurationService _configurationService;
        public ConfigurationController()
        {
            _configurationService = new ServiceFactory().CreateConfigurationService();
        }

        //public ConfigDto GetConfig(int userId)
        //{
        //    return _configurationService.GetConfig(userId);
        //}
        //public void SaveConfig(ConfigDto config)
        //{
        //    _configurationService.SaveConfig(config);
        //}

    }
}
