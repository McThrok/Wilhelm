using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services
{
    public class ConversionService
    {
        public ConfigDto ConvertToDto(WConfig config)
        {
            var dto = new ConfigDto()
            {
                Groups = new List<GroupDto>(),
                Tasks = new List<TaskDto>(),
            };

            foreach (var wgroup in config.WGroups)
                dto.Groups.Add(ConvertToDto(wgroup));

            foreach (var wtasks in config.WTasks)
                dto.Tasks.Add(ConvertToDto(wtasks,dto.Groups,true));

            return dto;
        }
        public WConfig ConvertFromDto(ConfigDto dto)
        {
            var config = new WConfig()
            {
                WGroups = new List<WGroup>(),
                WTasks = new List<WTask>(),
            };

            foreach (var group in dto.Groups)
                config.WGroups.Add(ConvertFromDto(group));

            foreach (var task in dto.Tasks)
                config.WTasks.Add(ConvertFromDto(task, config.WGroups, true));

            return config;
        }

        public ActivityDto ConvertToDto(WActivity activity, IEnumerable<TaskDto> tasks)
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
        public WActivity ConvertFromDto(ActivityDto activity, IEnumerable<WTask> tasks)
        {
            var model = ConvertFromDto(activity);
            var wtask = tasks.Where(x => x.Id == activity.Id).SingleOrDefault();
            if (wtask != null)
                model.WTask = wtask;
            return model;
        }
        public WActivity ConvertFromDto(ActivityDto activity)
        {
            var model = new WActivity();
            ConvertToDto(model, activity);
            model.Date = activity.Date;
            model.IsDone = activity.IsDone;
            return model;
        }

        public GroupDto ConvertToDto(WGroup group, IEnumerable<TaskDto> tasks, bool updateTasks)
        {
            var dto = ConvertToDto(group);
            dto.Tasks = new List<TaskDto>();
            foreach (var wtask in group.WTasks)
            {
                var task = tasks.Where(x => x.Id == wtask.Id).SingleOrDefault();
                if (task == null)
                    continue;

                dto.Tasks.Add(task);
                if (updateTasks)
                {
                    if (task.Groups == null)
                        task.Groups = new List<GroupDto>();
                    task.Groups.Add(dto);
                }
            }
            return dto;
        }
        public GroupDto ConvertToDto(WGroup group)
        {
            var dto = new GroupDto();
            ConvertToDto(group, dto);
            return dto;
        }
        public WGroup ConvertFromDto(GroupDto group, IEnumerable<WTask> tasks, bool updateTasks)
        {
            var model = ConvertFromDto(group);
            foreach (var task in group.Tasks)
            {

                var wtask = tasks.Where(x => x.Id == task.Id).SingleOrDefault();
                if (wtask == null)
                    continue;

                model.WTasks.Add(wtask);
                if (updateTasks)
                {
                    if (wtask.WGroups == null)
                        wtask.WGroups = new List<WGroup>();
                    wtask.WGroups.Add(model);
                }
            }
            return model;
        }
        public WGroup ConvertFromDto(GroupDto group)
        {
            var model = new WGroup();
            ConvertFromDto(group, model);
            return model;
        }

        public TaskDto ConvertToDto(WTask wtask, IEnumerable<GroupDto> groups, bool updateGroups)
        {
            var dto = ConvertToDto(wtask);
            dto.Groups = new List<GroupDto>();
            foreach (var wgroup in wtask.WGroups)
            {

                var group = groups.Where(x => x.Id == wgroup.Id).SingleOrDefault();
                if (group == null)
                    continue;

                dto.Groups.Add(group);
                if (updateGroups)
                {
                    if (group.Tasks == null)
                        group.Tasks = new List<TaskDto>();
                    group.Tasks.Add(dto);
                }
            }
            return dto;
        }
        public TaskDto ConvertToDto(WTask task)
        {
            var dto = new TaskDto();
            ConvertToDto(task, dto);
            return dto;
        }
        public WTask ConvertFromDto(TaskDto wtask, IEnumerable<WGroup> groups, bool updateGroups)
        {
            var model = ConvertFromDto(wtask);
            foreach (var group in wtask.Groups)
            {

                var wgroup = groups.Where(x => x.Id == group.Id).SingleOrDefault();
                if (wgroup == null)
                    continue;

                model.WGroups.Add(wgroup);
                if (updateGroups)
                {
                    if (wgroup.WTasks == null)
                        wgroup.WTasks = new List<WTask>();
                    wgroup.WTasks.Add(model);
                }
            }
            return model;
        }
        public WTask ConvertFromDto(TaskDto task)
        {
            var model = new WTask();
            ConvertFromDto(task, model);
            return model;
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
        public void ConvertFromDto(NamedModelDto dto, WNamedModel namedModel)
        {
            namedModel.Name = dto.Name;
            namedModel.Description = dto.Description;
            namedModel.Archivized = dto.Archivized;
        }
        public void ConvertFromDto(ModelDto dto, WModel model)
        {
            model.Id = dto.Id;
        }

    }
}
