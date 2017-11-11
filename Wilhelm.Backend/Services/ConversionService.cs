﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;
using Wilhelm.Backend.Model.Dto;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services
{
    public class ConversionService : IConversionService
    {
        public void ConvertToDto(ActivityDto dto, WActivity wActivity, IEnumerable<TaskDto> tasks)
        {
            ConvertToDto(dto, wActivity);
            var task = tasks.Where(x => x.Id == wActivity.Id).SingleOrDefault();
            if (task != null)
                dto.Task = task;
        }
        public void ConvertToDto(ActivityDto dto, WActivity wActivity)
        {
            ConvertToModelDto(dto, wActivity);
            dto.Date = wActivity.Date;
            dto.IsDone = wActivity.IsDone;
        }
        public void ConvertFromDto(WActivity wActivity, ActivityDto dto, IEnumerable<WTask> tasks)
        {
            ConvertFromDto(wActivity, dto);
            var wtask = tasks.Where(x => x.Id == dto.Id).SingleOrDefault();
            if (wtask != null)
                wActivity.WTask = wtask;
        }
        public void ConvertFromDto(WActivity wActivity, ActivityDto dto)
        {
            ConvertFromModelDto(wActivity, dto);
            wActivity.Date = dto.Date;
            wActivity.IsDone = dto.IsDone;
        }

        public void ConvertToDto(GroupDto dto, WGroup wGroup, IEnumerable<TaskDto> tasks, bool updateTasks)
        {
            ConvertToDto(dto, wGroup);
            dto.Tasks = new List<TaskDto>();
            foreach (var wtask in wGroup.WTasks)
            {
                var task = tasks.Where(x => x.Id == wtask.Id).SingleOrDefault();
                if (task == null)
                    continue;

                if (!dto.Tasks.Contains(task))
                    dto.Tasks.Add(task);

                if (updateTasks)
                {
                    if (task.Groups == null)
                        task.Groups = new List<GroupDto>();

                    if (!task.Groups.Contains(dto))
                        task.Groups.Add(dto);
                }
            }
        }
        public void ConvertToDto(GroupDto dto, WGroup group)
        {
            ConvertToNamedModelDto(dto, group);
        }
        public void ConvertFromDto(WGroup wGroup, GroupDto dto, IEnumerable<WTask> tasks, bool updateTasks)
        {
            ConvertFromDto(wGroup, dto);
            foreach (var task in dto.Tasks)
            {
                var wtask = tasks.Where(x => x.Id == task.Id).SingleOrDefault();
                if (wtask == null)
                    continue;

                if (!wGroup.WTasks.Contains(wtask))
                    wGroup.WTasks.Add(wtask);

                if (updateTasks)
                {
                    if (wtask.WGroups == null)
                        wtask.WGroups = new List<WGroup>();

                    if (!wtask.WGroups.Contains(wGroup))
                        wtask.WGroups.Add(wGroup);
                }
            }
        }
        public void ConvertFromDto(WGroup wGroup, GroupDto dto)
        {
            ConvertFromNamedModelDto(wGroup, dto);
        }

        public void ConvertToDto(TaskDto dto, WTask wtask, IEnumerable<GroupDto> groups, bool updateGroups)
        {
            ConvertToDto(dto, wtask);
            dto.Groups = new List<GroupDto>();
            foreach (var wgroup in wtask.WGroups)
            {
                var group = groups.Where(x => x.Id == wgroup.Id).SingleOrDefault();
                if (group == null)
                    continue;

                if (!dto.Groups.Contains(group))
                    dto.Groups.Add(group);

                if (updateGroups)
                {
                    if (group.Tasks == null)
                        group.Tasks = new List<TaskDto>();

                    if (!group.Tasks.Contains(dto))
                        group.Tasks.Add(dto);
                }
            }
        }
        public void ConvertToDto(TaskDto dto, WTask task)
        {
            ConvertToNamedModelDto(dto, task);
            dto.Frequency = task.Frequency;
            dto.StartDate = task.StartDate;
        }
        public void ConvertFromDto(WTask wTask, TaskDto dto, IEnumerable<WGroup> groups, bool updateGroups)
        {
            ConvertFromDto(wTask, dto);
            foreach (var group in dto.Groups)
            {
                var wgroup = groups.Where(x => x.Id == group.Id).SingleOrDefault();
                if (wgroup == null)
                    continue;

                if (!wTask.WGroups.Contains(wgroup))
                    wTask.WGroups.Add(wgroup);

                if (updateGroups)
                {
                    if (wgroup.WTasks == null)
                        wgroup.WTasks = new List<WTask>();

                    if (!wgroup.WTasks.Contains(wTask))
                        wgroup.WTasks.Add(wTask);
                }
            }
        }
        public void ConvertFromDto(WTask wTask, TaskDto dto)
        {
            var model = new WTask();
            ConvertFromNamedModelDto(model, dto);
            model.StartDate = dto.StartDate;
            model.Frequency = dto.Frequency;
        }

        public void ConvertToNamedModelDto(NamedModelDto dto, WNamedModel namedModel)
        {
            dto.Name = namedModel.Name;
            dto.Description = namedModel.Description;
            dto.Archivized = namedModel.Archivized;
            ConvertToModelDto(dto, namedModel);

        }
        public void ConvertToModelDto(ModelDto dto, WModel model)
        {
            dto.Id = model.Id;
        }
        public void ConvertFromNamedModelDto(WNamedModel namedModel, NamedModelDto dto)
        {
            namedModel.Name = dto.Name;
            namedModel.Description = dto.Description;
            namedModel.Archivized = dto.Archivized;
            ConvertFromModelDto(namedModel, dto);
        }
        public void ConvertFromModelDto(WModel model, ModelDto dto)
        {
            //model.Id = dto.Id;

            //if(dto.Id>0)
            //model.Id = dto.Id;

        }
    }
}
