using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services
{
    public class ActivityService : IActivityService
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

        public List<ActivityDto> GetArchive(int userId)
        {
            List<ActivityDto> dto = new List<ActivityDto>();
            using (var db = _wContextFactory.Create())
            {
                var archive = db.WActivities.Where(x => x.WTask.Owner.Id == userId).Include(x => x.WTask).Include(x => x.WTask.Owner);
                _entitiesService.UpdateDto(dto, archive);
            }
            return dto;
        }
        public List<ActivityDto> GetTodaysActivities(int userId)
        {
            List<ActivityDto> dto = new List<ActivityDto>();
            using (var db = _wContextFactory.Create())
            {
                var generated = _activityGenerationService.GenerateActivities(db.WActivities, db.WTasks.Where(x => x.Owner.Id == userId), DateTime.Today);
                foreach (var activity in generated)
                    db.WActivities.Add(activity);
                db.SaveChanges();

                // cannot take Date.Date in SQL
                var todaysTask = db.WActivities.Where(x => x.WTask.Owner.Id == userId).Where(x => DbFunctions.TruncateTime(x.Date) == DateTime.Today).Include(x => x.WTask).Include(x => x.WTask.Owner).ToList();
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
