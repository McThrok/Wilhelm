using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Frontend.Model;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Frontend.Services.Interfaces
{
    public interface IHoldersConversionService
    {
        void ConvertFromDto(ActivityHolder activity, ActivityDto dto, IEnumerable<TaskHolder> tasks);
        void ConvertFromDto(ActivityHolder activity, ActivityDto dto);
        void ConvertToDto(ActivityDto dto, ActivityHolder activity, IEnumerable<TaskDto> tasks);
        void ConvertToDto(ActivityDto dto, ActivityHolder activity);
        void ConvertFromDto(GroupHolder group, GroupDto dto, IEnumerable<TaskHolder> groups, bool updateGroups);
        void ConvertFromDto(GroupHolder group, GroupDto dto);
        void ConvertFromDto(GroupDto dto, GroupHolder group, IEnumerable<TaskDto> groups, bool updateGroups);
        void ConvertToDto(GroupDto dto, GroupHolder group);
        void ConvertFromDto(TaskHolder task, TaskDto dto, IEnumerable<GroupHolder> groups, bool updateGroups);
        void ConvertFromDto(TaskHolder task, TaskDto dto);
        void ConvertToDto(TaskDto dto, TaskHolder task, IEnumerable<GroupDto> groups, bool updateGroups);
        void ConvertToDto(TaskDto dto, TaskHolder task);
        void ConvertFromNamedModelDto(NamedHolder namedHolder, NamedModelDto dto);
        void ConvertFromModelDto(Holder holder, ModelDto dto);
        void ConvertToNamedModelDto(NamedModelDto dto, NamedHolder namedHolder);
        void ConvertToModelDto(ModelDto dto, Holder holder);
    }
}
