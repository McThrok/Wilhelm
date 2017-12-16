using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.MockBase;
using Wilhelm.Backend.Model;
using Wilhelm.Frontend.Model;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Frontend.Services.Interfaces
{
    public interface IHoldersService
    {
        void UpdateArchiveHolders(ICollection<ActivityHolder> activities, IEnumerable<ActivityDto> dtos);
        void UpateTodayActivityHolder(ICollection<ActivityHolder> activities, IEnumerable<ActivityDto> dtos);
        void UpdateActivityDtos(ICollection<ActivityDto> dtos, IEnumerable<ActivityHolder> activities);

        void UpdateConfigHolders(ICollection<GroupHolder> groups, ICollection<TaskHolder> tasks, ConfigDto config);
        void UpdateConfigDto(ConfigDto config, ICollection<GroupHolder> groups, ICollection<TaskHolder> tasks);

        int GenerateTemporaryId(IEnumerable<Holder> holders);
        string GetNameWithIndexIfNeeded(string startName, IEnumerable<NamedHolder> holders);
        GroupHolder CreateNewGroup(IEnumerable<GroupHolder> groups);
        TaskHolder CreateNewTask(IEnumerable<TaskHolder> tasks);

        void ApplyChanges(ICollection<TaskHolder> currnetTasks, IEnumerable<GroupHolder> currentGroups, TaskHolder updatedTask);
        void ApplyChanges(ICollection<GroupHolder> currentGroups, IEnumerable<TaskHolder> currnetTasks, GroupHolder updatedGroup);

        TaskHolder InitializeTaskDetails(List<GroupHolder> availableGroupsToAdd, TaskHolder choosenTask, List<GroupHolder> groups);
        GroupHolder InitializeGroupDetails(List<TaskHolder> availableTasksToAdd, GroupHolder chooosenGroup, List<TaskHolder> tasks);

    }
}
