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
using Wilhelm.Backend.Services.Interfaces;

namespace Wilhelm.Frontend.Services
{
    public class HoldersService : IHoldersService
    {
        private readonly IHoldersConversionService _holdersConversionService;

        public HoldersService(IHoldersConversionService holdersConversionService)
        {
            _holdersConversionService = holdersConversionService;
        }

        public void UpdateArchiveHolders(ICollection<ActivityHolder> activities, IEnumerable<ActivityDto> dtos)
        {
            UpdateHolders(activities, dtos);
        }
        public void UpateTodayActivityHolder(ICollection<ActivityHolder> activities, IEnumerable<ActivityDto> dtos)
        {
            UpdateHolders(activities, dtos);
        }
        private void UpdateHolders(ICollection<ActivityHolder> activities, IEnumerable<ActivityDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var activityHolderToUpdate = activities.Where(x => x.Id == dto.Id).SingleOrDefault();
                if (activityHolderToUpdate == null)
                {
                    activityHolderToUpdate = new ActivityHolder();
                    activities.Add(activityHolderToUpdate);
                }
                if (activityHolderToUpdate.Task == null)
                    activityHolderToUpdate.Task = new TaskHolder();

                if (dto.Task != null)
                    _holdersConversionService.ConvertFromDto(activityHolderToUpdate.Task, dto.Task);
                _holdersConversionService.ConvertFromDto(activityHolderToUpdate, dto);
            }
        }
        public void UpdateActivityDtos(ICollection<ActivityDto> dtos, IEnumerable<ActivityHolder> activities)
        {
            foreach (var holder in activities)
            {
                var activityDtoToUpdate = dtos.Where(x => x.Id == holder.Id).SingleOrDefault();
                if (activityDtoToUpdate == null)
                {
                    activityDtoToUpdate = new ActivityDto();
                    dtos.Add(activityDtoToUpdate);
                }
                if (activityDtoToUpdate.Task == null)
                    activityDtoToUpdate.Task = new TaskDto();

                if (holder.Task != null)
                    _holdersConversionService.ConvertToDto(activityDtoToUpdate.Task, holder.Task);
                _holdersConversionService.ConvertToDto(activityDtoToUpdate, holder);
            }
        }

        public void UpdateConfigHolders(ICollection<GroupHolder> groups, ICollection<TaskHolder> tasks, ConfigDto config)
        {
            foreach (var group in config.Groups)
            {
                var groupHolderToUpdate = groups.Where(x => x.Id == group.Id).SingleOrDefault();
                if (groupHolderToUpdate == null)
                {
                    groupHolderToUpdate = new GroupHolder();
                    groups.Add(groupHolderToUpdate);
                }
                _holdersConversionService.ConvertFromDto(groupHolderToUpdate, group);
            }

            foreach (var task in config.Tasks)
            {
                var taskHolderToUpdate = tasks.Where(x => x.Id == task.Id).SingleOrDefault();
                if (taskHolderToUpdate == null)
                {
                    taskHolderToUpdate = new TaskHolder();
                    tasks.Add(taskHolderToUpdate);
                }
                _holdersConversionService.ConvertFromDto(taskHolderToUpdate, task, groups, true);
            }
        }
        public void UpdateConfigDto(ConfigDto config, ICollection<GroupHolder> groups, ICollection<TaskHolder> tasks)
        {
            if (config.Groups == null) config.Groups = new List<GroupDto>();
            if (config.Tasks == null) config.Tasks = new List<TaskDto>();

            foreach (var group in groups)
            {
                var groupDtoToUpdate = config.Groups.Where(x => x.Id == group.Id).SingleOrDefault();
                if (groupDtoToUpdate == null)
                {
                    groupDtoToUpdate = new GroupDto();
                    config.Groups.Add(groupDtoToUpdate);
                }
                _holdersConversionService.ConvertToDto(groupDtoToUpdate, group);
            }

            foreach (var task in tasks)
            {
                var taskDtoToUpdate = config.Tasks.Where(x => x.Id == task.Id).SingleOrDefault();
                if (taskDtoToUpdate == null)
                {
                    taskDtoToUpdate = new TaskDto();
                    config.Tasks.Add(taskDtoToUpdate);
                }
                _holdersConversionService.ConvertToDto(taskDtoToUpdate, task, config.Groups, true);
            }
        }

        public int GenerateTemporaryId(IEnumerable<Holder> holders)
        {
            int minId = Math.Min(holders.Min(x => x.Id), 0);
            return minId - 1;
        }
    }
}
