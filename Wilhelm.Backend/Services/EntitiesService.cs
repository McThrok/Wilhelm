using System;
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
    public class EntitiesService : IEntitiesService
    {
        private IConversionService _conversionService;
        public EntitiesService(IConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        public void UpdateDto(ConfigDto dto, IEnumerable<WTask> wTasks, IEnumerable<WGroup> wGroups)
        {
            if (dto == null)
                return;

            if (dto.Groups == null)
                dto.Groups = new List<GroupDto>();
            if (dto.Tasks == null)
                dto.Tasks = new List<TaskDto>();

            if (wTasks == null || wGroups == null)
                return;

            foreach (var wGroup in wGroups)
            {
                var groupToUpdate = dto.Groups.Where(x => x.Id == wGroup.Id).SingleOrDefault();
                if (groupToUpdate == null)
                {
                    groupToUpdate = new GroupDto();
                    dto.Groups.Add(groupToUpdate);
                }
                groupToUpdate.Tasks = new List<TaskDto>();
                _conversionService.ConvertToDto(groupToUpdate, wGroup);
            }

            foreach (var wTask in wTasks)
            {
                var taskToUpdate = dto.Tasks.Where(x => x.Id == wTask.Id).SingleOrDefault();
                if (taskToUpdate == null)
                {
                    taskToUpdate = new TaskDto();
                    dto.Tasks.Add(taskToUpdate);
                }
                taskToUpdate.Groups = new List<GroupDto>();
                _conversionService.ConvertToDto(taskToUpdate, wTask, dto.Groups, true);
            }
        }
        public void UpdateEntities(IDbSet<WTask> wTasks, IDbSet<WGroup> wGroups, ConfigDto config)
        {
            if (config == null || config.Groups == null || config.Tasks == null || wTasks == null || wGroups == null)
                return;

            foreach (var group in config.Groups)
            {
                var wGroupToUpdate = wGroups.Where(x => x.Id == group.Id).SingleOrDefault();
                if (wGroupToUpdate == null)
                {
                    wGroupToUpdate = new WGroup();
                    wGroups.Add(wGroupToUpdate);
                }
                wGroupToUpdate.WTasks = new List<WTask>();
                _conversionService.ConvertFromDto(wGroupToUpdate, group);
            }

            foreach (var task in config.Tasks)
            {
                var wTaskToUpdate = wTasks.Where(x => x.Id == task.Id).SingleOrDefault();
                if (wTaskToUpdate == null)
                {
                    wTaskToUpdate = new WTask();
                    wTasks.Add(wTaskToUpdate);
                }
                wTaskToUpdate.WGroups = new List<WGroup>();
                _conversionService.ConvertFromDto(wTaskToUpdate, task, wGroups, true);
            }
        }
        public void PrepareConfigToSave(IEnumerable<WTask> wTasks, IEnumerable<WGroup> wGroups)
        {
            foreach (var item in wTasks)
                if (item.Id < 0)
                    item.Id = 0;

            foreach (var item in wGroups)
                if (item.Id < 0)
                    item.Id = 0;
        }

        public void UpdateDto(ICollection<ActivityDto> dtos, IEnumerable<WActivity> activities)
        {
            if (dtos == null || activities == null)
                return;
            foreach (var activity in activities)
                if (!dtos.Any(x => x.Id == activity.Id))
                {
                    var activityDto = new ActivityDto();
                    _conversionService.ConvertToDto(activityDto, activity);
                    var taskDto = new TaskDto();
                    _conversionService.ConvertToDto(taskDto, activity.WTask);
                    activityDto.Task = taskDto;
                    dtos.Add(activityDto);
                }
        }
        public void UpdateEntities(IDbSet<WActivity> activities, IEnumerable<ActivityDto> dtos)
        {
            if (activities == null || dtos == null)
                return;

            foreach (var dto in dtos)
            {
                var activityModelToUpdate = activities.Where(x => x.Id == dto.Id).SingleOrDefault();
                if (activityModelToUpdate == null)
                {
                    activityModelToUpdate = new WActivity();
                    activities.Add(activityModelToUpdate);
                }
                if (activityModelToUpdate.WTask == null)
                    activityModelToUpdate.WTask = new WTask();

                if (dto.Task != null)
                    _conversionService.ConvertFromDto(activityModelToUpdate.WTask, dto.Task);
                _conversionService.ConvertFromDto(activityModelToUpdate, dto);
            }
        }

    }
}
