﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Backend.Services;
using Wilhelm.Backend.Model;
using Wilhelm.Shared.Dto;
using Newtonsoft.Json;

namespace Wilhelm.Backend.Api.Controllers
{
    public class ConfigurationController : ApiController
    {
        private readonly IConfigurationService _configurationService;
        public ConfigurationController()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            _configurationService = new ServiceFactory().CreateConfigurationService();
        }

        public ConfigDto GetConfig(int userId)
        {
            return _configurationService.GetConfig(userId);
        }

        public void PostConfig(int userId, [FromBody]ConfigDto config)
        {
            _configurationService.SaveConfig(config);
        }

        [Route("task")]
        public void PostTask([FromBody]TaskDto task)
        {
            _configurationService.AddTask(task);
        }
        [Route("task")]
        public void PutTask([FromBody]TaskDto task)
        {
            _configurationService.UpdateTask( task);
        }
    }
}
 