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
        List<KeyValuePair<int, string>> GetTaskNames(int userId);
        TaskDto GetTaskDetails(int taskId);
        void SaveConfig(ConfigDto config);
        void AddTask(KeyValuePair<TaskDto, List<int>> task);
        void UpdateTask(KeyValuePair<TaskDto, List<int>> task);
        void AddGroup(KeyValuePair<GroupDto, List<int>> group);
        void UpdateGroup(KeyValuePair<GroupDto, List<int>> group);
    }
}
