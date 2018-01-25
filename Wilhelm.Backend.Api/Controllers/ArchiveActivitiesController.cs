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
    public class ArchiveActivitiesController : ApiController
    {
        private readonly IActivityService _activityService;
        public ArchiveActivitiesController()
        {
            _activityService = new ServiceFactory().CreateActivityService();
        }

        public List<ActivityDto> GetArchive(int userId)
        {
            return _activityService.GetArchive(userId);
        }
        public List<ActivityDto> GetActivities(int userId, int offset, int amount)
        {
            return _activityService.GetArchiveActivities(userId, offset, amount);
        }

        public void PostActivities(int userId, [FromBody]IEnumerable<ActivityDto> activities)
        {
            _activityService.SaveActivities(activities);
        }

        //http://localhost:8080/api/archiveactivities?activityId=12&value=true
        public void PutActivity(int activityId, bool value)
        {
            _activityService.UpdateActivity(activityId, value);
        }

        //[{"Key":12,"Value":true},{"Key":13,"Value":true}]
        public void PutActivities([FromBody]List<KeyValuePair<int, bool>> activities)
        {
            _activityService.UpdateActivities(activities);
        }


    }
}
