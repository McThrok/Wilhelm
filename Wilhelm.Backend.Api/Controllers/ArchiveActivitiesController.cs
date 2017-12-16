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

        public void SaveActivities(int userId,IEnumerable<ActivityDto> activities)
        {
            _activityService.SaveActivities(activities);
        }

    }
}
