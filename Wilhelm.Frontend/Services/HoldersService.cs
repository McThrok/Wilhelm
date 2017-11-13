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

        public List<ActivityHolder> GetArchiveHolders()
        {
            return GetActivityHolders(_activityService.GetArchive());
        }
        public List<ActivityHolder> GetTodaysActivitiesHolders()
        {
            return GetActivityHolders(_activityService.GetTodaysActivities());
        }
        private List<ActivityHolder> GetActivityHolders(List<ActivityDto> activities)
        {
            var holders = new List<ActivityHolder>();
            foreach (var activity in activities)
            {
                var holder = new ActivityHolder();
                _holdersConversionService.ConvertFromDto(holder, activity);

                holder.Task = new TaskHolder();
                _holdersConversionService.ConvertFromDto(holder.Task, activity.Task);

                holders.Add(holder);
            }
            return holders;
        }
        public void SaveActivities(IEnumerable<ActivityHolder> activities)
        {
            var dtos = new List<ActivityDto>();
            foreach (var holder in activities)
            {
                var dto = new ActivityDto();
                _holdersConversionService.ConvertToDto(dto, holder);
                dtos.Add(dto);
            }
            _activityService.SaveActivities(dtos);
        }

        public void SetConfiguration(ICollection<GroupHolder> groups, ICollection<TaskHolder> tasks)
        {
            var config = _configurationService.GetConfig();

            foreach (var group in config.Groups)
            {
                var groupHolder = new GroupHolder();
                _holdersConversionService.ConvertFromDto(groupHolder, group);
                groups.Add(groupHolder);
            }

            foreach (var task in config.Tasks)
            {
                var taskHolder = new TaskHolder();
                _holdersConversionService.ConvertFromDto(taskHolder, task, groups, true);
                tasks.Add(taskHolder);
            }
        }
        public void SaveConfig(ICollection<GroupHolder> groups, ICollection<TaskHolder> tasks)
        {
            var config = new ConfigDto();

            foreach (var group in groups)
            {
                var groupDto = new GroupDto();
                _holdersConversionService.ConvertToDto(groupDto, group);
                config.Groups.Add(groupDto);
            }

            foreach (var task in tasks)
            {
                var taskDto = new TaskDto();
                _holdersConversionService.ConvertToDto(taskDto, task);
                config.Tasks.Add(taskDto);
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
