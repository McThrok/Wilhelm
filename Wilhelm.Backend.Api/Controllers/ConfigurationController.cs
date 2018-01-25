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

        //public ConfigDto GetConfig(int userId)
        //{
        //    return _configurationService.GetConfig(userId);
        //}
        [Route("api/configuration/")]
        public List<KeyValuePair<int, string>> GetTaskNames(int userId)
        {
            return _configurationService.GetTaskNames(userId);
        }
        [Route("api/configuration/taskDetails")]
        public TaskDto GetTaskDetails(int taskId)
        {
            return _configurationService.GetTaskDetails(taskId);
        }

        public void PostConfig(int userId, [FromBody]ConfigDto config)
        {
            _configurationService.SaveConfig(config);
        }

        [Route("api/configuration/task")]
        public void PostTask([FromBody] KeyValuePair<TaskDto, List<int>> task)
        {
            _configurationService.AddTask(task);
        }

        [Route("api/configuration/task")]
        public void PutTask([FromBody] KeyValuePair<TaskDto, List<int>> task)
        {
            _configurationService.UpdateTask(task);
        }

        [Route("api/configuration/group")]
        public void PostGroup([FromBody] KeyValuePair<GroupDto, List<int>> group)
        {
            _configurationService.AddGroup(group);
        }

        [Route("api/configuration/group")]
        public void PutGroup([FromBody] KeyValuePair<GroupDto, List<int>> group)
        {
            _configurationService.UpdateGroup(group);
        }
    }
}
