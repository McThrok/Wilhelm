using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model.Dto;

namespace Wilhelm.Backend.Services.Interfaces
{
    public interface IActivityService
    {
        List<ActivityDto> GetArchive(int userId);
        List<ActivityDto> GetTodaysActivities(int userId);
        void SaveActivities(IEnumerable<ActivityDto> activities);
    }
}
