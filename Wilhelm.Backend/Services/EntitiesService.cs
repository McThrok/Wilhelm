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
                _conversionService.ConvertToDto(taskToUpdate, wTask);
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
                _conversionService.ConvertFromDto(wTaskToUpdate, task);
            }
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
                    var tasDto = new TaskDto();
                    _conversionService.ConvertToDto(tasDto, activity.WTask);
                    dtos.Add(activityDto);
                }
        }
        public void UpdateEntities(IDbSet<WActivity> activities, IEnumerable<ActivityDto> dtos)
        {
            if (activities == null || dtos == null)
                return;

            foreach (var activity in dtos)
                if (!activities.Any(x => x.Id == activity.Id))
                {
                    var wActivity = new WActivity();
                    _conversionService.ConvertFromDto(wActivity, activity);
                    var wTask = new WTask();
                    _conversionService.ConvertFromDto(wTask, activity.Task);
                    activities.Add(wActivity);
                }
        }
    }
}
