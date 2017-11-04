using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services
{
    internal class ConversionService
    {
        public ActivityDto ConvertTo(WActivity activity, IEnumerable<TaskDto> tasks)
        {
            var dto = ConvertToDto(activity);
            var task = tasks.Where(x => x.Id == activity.Id).SingleOrDefault();
            if (task != null)
                dto.Task = task;
            return dto;
        }
        public ActivityDto ConvertToDto(WActivity activity)
        {
            var dto = new ActivityDto();
            ConvertToDto(activity, dto);
            dto.Date = activity.Date;
            dto.IsDone = activity.IsDone;
            return dto;
        }

        public GroupDto ConvertTo(WGroup group, IEnumerable<TaskDto> tasks)
        {
            var dto = ConvertToDto(group);
            dto.Tasks = new List<TaskDto>();
            foreach (var wtask in group.WTasks)
            {

                var task = tasks.Where(x => x.Id == wtask.Id).SingleOrDefault();
                if (task != null)
                    dto.Tasks.Add(task);
            }
            return dto;
        }
        public GroupDto ConvertToDto(WGroup group)
        {
            var dto = new GroupDto();
            ConvertToDto(group, dto);
            return dto;
        }

        public TaskDto ConvertTo(WTask wtask, IEnumerable<GroupDto> groups)
        {
            var dto = ConvertToDto(wtask);
            dto.Groups = new List<GroupDto>();
            foreach (var wgroup in wtask.WGroups)
            {

                var group = groups.Where(x => x.Id == wgroup.Id).SingleOrDefault();
                if (group != null)
                    dto.Groups.Add(group);
            }
            return dto;
        }
        public TaskDto ConvertToDto(WTask group)
        {
            var dto = new TaskDto();
            ConvertToDto(group, dto);
            return dto;
        }

        public void ConvertToDto(WNamedModel namedModel, NamedModelDto dto)
        {
            dto.Name = namedModel.Name;
            dto.Description = namedModel.Description;
            dto.Archivized = namedModel.Archivized;
        }
        public void ConvertToDto(WModel model, ModelDto dto)
        {
            dto.Id = model.Id;
        }
    }
}
