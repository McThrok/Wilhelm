using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.MockBase;
using Wilhelm.Backend.Model;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.Model;

namespace Wilhelm.Frontend.Services
{
    public class HoldersConversionService : IHoldersConversionService
    {
        public void ConvertFromDto(ActivityHolder activity, ActivityDto dto, IEnumerable<TaskHolder> tasks)
        {
            ConvertFromDto(activity, dto);
            var task = tasks.Where(x => x.Id == dto.Id).SingleOrDefault();
            if (task != null)
                activity.Task = task;
        }
        public void ConvertFromDto(ActivityHolder activity, ActivityDto dto)
        {
            ConvertFromModelDto(activity, dto);
            activity.Date = dto.Date;
            activity.Id = dto.Id;
        }
        public void ConvertToDto(ActivityDto dto, ActivityHolder activity, IEnumerable<TaskDto> tasks)
        {
            ConvertToDto(dto, activity);
            var task = tasks.Where(x => x.Id == dto.Id).SingleOrDefault();
            if (task != null)
                dto.Task = task;
        }
        public void ConvertToDto(ActivityDto dto, ActivityHolder activity)
        {
            ConvertToModelDto(dto, activity);
            dto.Date = activity.Date;
            dto.Id = activity.Id;
        }

        public void ConvertFromDto(GroupHolder group, GroupDto dto, IEnumerable<TaskHolder> groups, bool updateGroups)
        {
            ConvertFromDto(group, dto);
            if (dto.Tasks == null)
                return;

            foreach (var task in dto.Tasks)
            {
                var taskHolder = groups.Where(x => x.Id == task.Id).SingleOrDefault();
                if (taskHolder == null)
                    continue;
                if (!group.Tasks.Contains(taskHolder))
                    group.Tasks.Add(taskHolder);

                if (updateGroups)
                {
                    if (taskHolder.Groups == null)
                        taskHolder.Groups = new ObservableCollection<GroupHolder>();

                    if (!taskHolder.Groups.Contains(group))
                        taskHolder.Groups.Add(group);
                }
            }
        }
        public void ConvertFromDto(GroupHolder group, GroupDto dto)
        {
            ConvertFromNamedModelDto(group, dto);
        }
        public void ConvertFromDto(GroupDto dto, GroupHolder group, IEnumerable<TaskDto> groups, bool updateGroups)
        {
            ConvertFromDto(group, dto);
            if (dto.Tasks == null)
                return;

            foreach (var task in group.Tasks)
            {
                var taskDto = groups.Where(x => x.Id == task.Id).SingleOrDefault();
                if (taskDto == null)
                    continue;
                if (!dto.Tasks.Contains(taskDto))
                    dto.Tasks.Add(taskDto);

                if (updateGroups)
                {
                    if (taskDto.Groups == null)
                        taskDto.Groups = new List<GroupDto>();

                    if (!taskDto.Groups.Contains(dto))
                        taskDto.Groups.Add(dto);
                }
            }
        }
        public void ConvertToDto(GroupDto dto, GroupHolder group)
        {
            ConvertToNamedModelDto(dto, group);
        }

        public void ConvertFromDto(TaskHolder task, TaskDto dto, IEnumerable<GroupHolder> groups, bool updateGroups)
        {
            ConvertFromDto(task, dto);
            if (dto.Groups == null)
                return;

            foreach (var group in dto.Groups)
            {
                var groupHolder = groups.Where(x => x.Id == group.Id).SingleOrDefault();
                if (groupHolder == null)
                    continue;
                if (!task.Groups.Contains(groupHolder))
                    task.Groups.Add(groupHolder);

                if (updateGroups)
                {
                    if (groupHolder.Tasks == null)
                        groupHolder.Tasks = new ObservableCollection<TaskHolder>();

                    if (!groupHolder.Tasks.Contains(task))
                        groupHolder.Tasks.Add(task);
                }
            }
        }
        public void ConvertFromDto(TaskHolder task, TaskDto dto)
        {
            ConvertFromNamedModelDto(task, dto);
            task.Frequency = dto.Frequency;
            task.StartDate = dto.StartDate;
        }
        public void ConvertToDto(TaskDto dto, TaskHolder task, IEnumerable<GroupDto> groups, bool updateGroups)
        {
            ConvertToDto(dto, task);
            if (task.Groups == null)
                return;

            foreach (var group in task.Groups)
            {
                var groupDto = groups.Where(x => x.Id == group.Id).SingleOrDefault();
                if (groupDto == null)
                    continue;
                if (!dto.Groups.Contains(groupDto))
                    dto.Groups.Add(groupDto);

                if (updateGroups)
                {
                    if (groupDto.Tasks == null)
                        groupDto.Tasks = new List<TaskDto>();

                    if (!groupDto.Tasks.Contains(dto))
                        groupDto.Tasks.Add(dto);
                }
            }
        }
        public void ConvertToDto(TaskDto dto, TaskHolder task)
        {
            ConvertToNamedModelDto(dto, task);
            dto.Frequency = task.Frequency;
            dto.StartDate = task.StartDate;
        }

        public void ConvertFromNamedModelDto(NamedHolder namedHolder, NamedModelDto dto)
        {
            ConvertFromModelDto(namedHolder, dto);
            namedHolder.Archivized = dto.Archivized;
            namedHolder.Name = dto.Name;
            namedHolder.Description = dto.Description;
        }
        public void ConvertFromModelDto(Holder holder, ModelDto dto)
        {
            holder.Id = dto.Id;
        }
        public void ConvertToNamedModelDto(NamedModelDto dto, NamedHolder namedHolder)
        {
            ConvertToModelDto(dto, namedHolder);
            dto.Archivized = namedHolder.Archivized;
            dto.Name = namedHolder.Name;
            dto.Description = namedHolder.Description;
        }
        public void ConvertToModelDto(ModelDto dto, Holder holder)
        {
            dto.Id = holder.Id;
        }
    }
}
