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
    public class GroupsController : ApiController
    {
        private readonly IConfigurationService _configurationService;
        public GroupsController()
        {
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            _configurationService = new ServiceFactory().CreateConfigurationService();
        }

        [Route("api/groups/names")]
        public List<KeyValuePair<int, string>> GetGroupsNames(int userId)
        {
            return _configurationService.GetGroupsNames(userId);
        }
        [Route("api/groups/details")]
        public GroupDto GetGroupDetails(int groupId)
        {
            return _configurationService.GetGroupDetails(groupId);
        }
        [Route("api/groups/tasks")]
        public List<Tuple<int, string, string>> GetTasks(int userId)
        {
            return _configurationService.GetTasks(userId);
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

        public void DeleteGroup(int groupId)
        {
            _configurationService.DeleteGroup(groupId);
        }
    }
}
