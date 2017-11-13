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
        private readonly IConfigurationService _configurationService;
        private readonly IActivityService _activityService;
        private readonly IHoldersConversionService _holdersConversionService;

        public HoldersService(IConfigurationService configurationService, IActivityService activityService, IHoldersConversionService holdersConversionService)
        {
            _configurationService = configurationService;
            _activityService = activityService;
            _holdersConversionService = holdersConversionService;
        }

        public void UpdateArchiveHolders(ICollection<ActivityHolder> activities, IEnumerable<ActivityDto> dtos)
        {
             UpdateHolders(activities,dtos);
        }
        public void UpateTodayActivityHolder(ICollection<ActivityHolder> activities, IEnumerable<ActivityDto> dtos)
        {
             UpdateHolders(activities, dtos);
        }
        private void UpdateHolders(ICollection<ActivityHolder> activities, IEnumerable<ActivityDto> dtos)
        {
            foreach (var dto in dtos)
            {
                var holder = new ActivityHolder();
                _holdersConversionService.ConvertFromDto(holder, dto);

                holder.Task = new TaskHolder();
                _holdersConversionService.ConvertFromDto(holder.Task, dto.Task);

                activities.Add(holder);
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
                _holdersConversionService.ConvertToDto(activityDtoToUpdate, holder);
                dtos.Add(activityDtoToUpdate);
            }
        }

        public void UpdateConfigDto(ICollection<GroupHolder> groups, ICollection<TaskHolder> tasks)
        {
            var config = _configurationService.GetConfig();

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
                _holdersConversionService.ConvertFromDto(taskHolderToUpdate, task);
            }
        }
        public void UpdateConfigHolders(ICollection<GroupHolder> groups, ICollection<TaskHolder> tasks)
        {
            var config = new ConfigDto();

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
                _holdersConversionService.ConvertToDto(taskDtoToUpdate, task);

            }

            _configurationService.SaveConfig(config);
        }

        public int GenerateTemporaryId(IEnumerable<Holder> holders)
        {
            int minId = Math.Min(holders.Min(x => x.Id), 0);
            return minId - 1;
        }
    }
}
