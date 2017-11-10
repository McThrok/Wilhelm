using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services.Interfaces;

namespace Wilhelm.Backend.Services
{
    internal class ActivityService : IActivityService
    {
        public List<ActivityDto> GetArchive()
        {
            throw new NotImplementedException();
        }

        public List<ActivityDto> GetTodaysTasks()
        {
            throw new NotImplementedException();
        }

        public void SaveArchive(List<ActivityDto> activities)
        {
            throw new NotImplementedException();
        }

        public void SaveTodaysTasks(List<ActivityDto> activities)
        {
            throw new NotImplementedException();
        }
    }
}
