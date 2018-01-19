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
    public class ActiveActivitiesController : ApiController
    {
        private readonly IActivityService _activityService;
        public ActiveActivitiesController()
        {
            _activityService = new ServiceFactory().CreateActivityService();
        }

        public List<ActivityDto> GetTodaysActivities(int userId)
        {
            return _activityService.GetTodaysActivities(userId);
        }
        public void PostActivities([FromBody]IEnumerable<ActivityDto> activities)
        {
            _activityService.SaveActivities(activities);
        }


    }
}
