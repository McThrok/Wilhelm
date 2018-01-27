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
    public class TasksController : ApiController
    {
        private readonly IConfigurationService _configurationService;
        public TasksController()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            _configurationService = new ServiceFactory().CreateConfigurationService();
        }

        [Route("api/tasks/names")]
        public List<KeyValuePair<int, string>> GetTaskNames(int userId)
        {
            return _configurationService.GetTaskNames(userId);
        }
        [Route("api/tasks/details")]
        public TaskDto GetTaskDetails(int taskId)
        {
            return _configurationService.GetTaskDetails(taskId);
        }
        [Route("api/tasks/groups")]
        public List<Tuple<int, string, string>> GetGroups(int userId)
        {
            return _configurationService.GetGroups(userId);
        }

        public void PostTask([FromBody] KeyValuePair<TaskDto, List<int>> task)
        {
            _configurationService.AddTask(task);
        }

        public void PutTask([FromBody] KeyValuePair<TaskDto, List<int>> task)
        {
            _configurationService.UpdateTask(task);
        }

        public void DeleteTask(int taskId)
        {
            _configurationService.DeleteTask(taskId);
        }
    }
}
