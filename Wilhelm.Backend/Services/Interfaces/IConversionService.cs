using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;
using Wilhelm.DataAccess;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Backend.Services.Interfaces
{
    public interface IConversionService
    {
        void ConvertToDto(ActivityDto dto, WActivity wActivity, IEnumerable<TaskDto> tasks);
        void ConvertToDto(ActivityDto dto, WActivity wActivity);
        void ConvertFromDto(WActivity wActivity, ActivityDto dto, IEnumerable<WTask> tasks);
        void ConvertFromDto(WActivity wActivity, ActivityDto dto);

        void ConvertToDto(GroupDto dto, WGroup wGroup, IEnumerable<TaskDto> tasks, bool updateTasks);
        void ConvertToDto(GroupDto dto, WGroup group, bool copyTasks);
        void ConvertFromDto(WGroup wGroup, GroupDto dto, IEnumerable<WTask> tasks, bool updateTasks);
        void ConvertFromDto(WGroup wGroup, GroupDto dto);

        void ConvertToDto(TaskDto dto, WTask wtask, IEnumerable<GroupDto> groups, bool updateGroups);
        void ConvertToDto(TaskDto dto, WTask task, bool copyGroups);
        void ConvertFromDto(WTask wTask, TaskDto dto, IEnumerable<WGroup> groups, bool updateGroups);
        void ConvertFromDto(WTask wTask, TaskDto dto);

        void ConvertToDto(UserDto dto, WUser wUser);
        void ConvertFromDto(WUser wUser, UserDto dto);

        void ConvertToNamedModelDto(NamedModelDto dto, WNamedModel namedModel);
        void ConvertToModelDto(ModelDto dto, WModel model);
        void ConvertFromNamedModelDto(WNamedModel namedModel, NamedModelDto dto);
        void ConvertFromModelDto(WModel model, ModelDto dto);
    }
}
