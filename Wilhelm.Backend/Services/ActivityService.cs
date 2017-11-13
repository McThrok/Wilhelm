using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services
{
    internal class ActivityService : IActivityService
    {
        private IWContextFactory _wContextFactory;
        private IEntitiesService _entitiesService;
        private IActivityGenerationService _activityGenerationService;

        public ActivityService(IWContextFactory wContextFactory, IEntitiesService entitiesService, IActivityGenerationService activityGenerationService)
        {
            _wContextFactory = wContextFactory;
            _entitiesService = entitiesService;
            _activityGenerationService = activityGenerationService;
        }


        public List<ActivityDto> GetArchive()
        {
            List<ActivityDto> dto = new List<ActivityDto>();
            using (var db = _wContextFactory.Create())
            {
                _entitiesService.UpdateDto(dto, db.WActivities);
            }
            return dto;
        }
        public List<ActivityDto> GetTodaysActivities()
        {
            List<ActivityDto> dto = new List<ActivityDto>();
            using (var db = _wContextFactory.Create())
            {
                //var generated = _activityGenerationService.GenerateActivities(db.WActivities, db.WTasks.Where(x => !x.Archivized), DateTime.Today);
                //foreach (var activity in generated)
                //    db.WActivities.Add(activity);
                //db.SaveChanges();

                var todaysTask = db.WActivities.ToList().Where(x => x.Date.Date == DateTime.Now.Date).ToList();
                _entitiesService.UpdateDto(dto, todaysTask);
            }
            return dto;
        }
        public void SaveActivities(IEnumerable<ActivityDto> activities)
        {
            using (var db = _wContextFactory.Create())
            {
                _entitiesService.UpdateEntities(db.WActivities, activities);
                db.SaveChanges();
            }
        }
    }
}
