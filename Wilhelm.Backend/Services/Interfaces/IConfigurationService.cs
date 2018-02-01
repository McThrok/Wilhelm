using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;
using Wilhelm.DataAccess;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Backend.Services.Interfaces
{
    public interface IConfigurationService
    {
        ConfigDto GetConfig(int userId);
        void SaveConfig(ConfigDto config);

        Tuple<bool, List<KeyValuePair<int, string>>> GetTaskNames(int userId, int offset, int amount);
        TaskDto GetTaskDetails(int taskId);
        List<Tuple<int, string, string>> GetGroups(int userId);
        void DeleteTask(int taskId);

        List<KeyValuePair<int, string>> GetGroupsNames(int userId);
        GroupDto GetGroupDetails(int groupId);
        List<Tuple<int, string, string>> GetTasks(int userId);
        void DeleteGroup(int groupId);

        void AddTask(KeyValuePair<TaskDto, List<int>> task);
        void UpdateTask(KeyValuePair<TaskDto, List<int>> task);
        void AddGroup(KeyValuePair<GroupDto, List<int>> group);
        void UpdateGroup(KeyValuePair<GroupDto, List<int>> group);
    }
}
