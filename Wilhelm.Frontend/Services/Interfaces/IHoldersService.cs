using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.MockBase;
using Wilhelm.Backend.Model;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Frontend.Model;

namespace Wilhelm.Frontend.Services.Interfaces
{
    public interface IHoldersService
    {
        List<ActivityHolder> GetArchiveHolders();
        List<ActivityHolder> GetTodaysActivitiesHolders();
        void SaveActivities(IEnumerable<ActivityHolder> activities);

        void SetConfiguration(ICollection<GroupHolder> groups, ICollection<TaskHolder> tasks);
        void SaveConfig(ICollection<GroupHolder> groups, ICollection<TaskHolder> tasks);

        int GenerateTemporaryId(IEnumerable<Holder> holders);

    }
}
