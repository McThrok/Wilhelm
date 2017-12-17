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
            // return _activityService.GetTodaysActivities(userId);
            UserDto User1 = new UserDto() { Login = "user1", Password = "Ἱꏁ\u2438ꥅ䫥쪋邳躮Ᏺ껫ꪉ⏺꼿ᆴ넿BD106B80630350E9B080DFB569CD0C337814169FA9350774ECB50AEB0164BD38" };
            TaskDto t1 = new TaskDto() { Name = "Nakarmić kota", Owner = User1, Description = "Tom jest wybredny i je tylko Royal Canin", Frequency = 1, StartDate = DateTime.Today };
            ActivityDto act = new ActivityDto() { Id = 1, Task = t1, IsDone = true, Date = DateTime.Now };
            var result = new List<ActivityDto>();
            result.Add(act);

            return result;
            // return new int[] { 1, 2, 3, 4, 5, 6 }.ToList();
        }
        public void SaveActivities(IEnumerable<ActivityDto> activities)
        {
            _activityService.SaveActivities(activities);
        }

    }
}
