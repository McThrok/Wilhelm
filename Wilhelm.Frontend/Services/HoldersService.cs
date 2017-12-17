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

        public void UpdateArchiveHolders(ICollection<ActivityHolder> archives, IEnumerable<ActivityDto> dtos)
        {
            UpdateHolders(archives, dtos);
        }
        public void UpateTodayActivityHolder(ICollection<ActivityHolder> todayTasks, IEnumerable<ActivityDto> dtos)
        {
            UpdateHolders(todayTasks, dtos);
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
                groupHolderToUpdate.Tasks = new ObservableCollection<TaskHolder>();
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
                taskHolderToUpdate.Groups = new ObservableCollection<GroupHolder>();
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
                groupDtoToUpdate.Tasks = new List<TaskDto>();
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
                taskDtoToUpdate.Groups = new List<GroupDto>();
                _holdersConversionService.ConvertToDto(taskDtoToUpdate, task, config.Groups, true);
            }
        }

        public int GenerateTemporaryId(IEnumerable<Holder> holders)
        {
            int minId = 0;
            if (holders.Count() > 0)
                 minId = Math.Min(holders.Min(x => x.Id), 0);
            return minId - 1;
        }
        public string GetNameWithIndexIfNeeded(string startName, IEnumerable<NamedHolder> holders)
        {
            if (holders.Any(x => x.Name == startName))
            {
                int index = 1;
                while (holders.Any(x => x.Name == startName + index.ToString()))
                    index++;
                startName += index.ToString();
            }
            return startName;
        }

        public void ApplyChanges(ICollection<TaskHolder> currnetTasks, IEnumerable<GroupHolder> currentGroups, TaskHolder updatedTask)
        {
            var taskToUpdate = currnetTasks.SingleOrDefault(x => x.Id == updatedTask.Id);
            if (taskToUpdate == null)
            {
                taskToUpdate = new TaskHolder();
                currnetTasks.Add(taskToUpdate);
            }
            taskToUpdate.Name = updatedTask.Name;
            taskToUpdate.Description = updatedTask.Description;
            taskToUpdate.StartDate = updatedTask.StartDate;
            taskToUpdate.Frequency = updatedTask.Frequency;
            if (taskToUpdate.Groups == null)
                taskToUpdate.Groups = new ObservableCollection<GroupHolder>();

            foreach (var group in currentGroups)
            {
                var taskInDetails = updatedTask.Groups.Where(x => x.Id == group.Id).SingleOrDefault();

                if (!taskToUpdate.Groups.Contains(group) && taskInDetails != null)
                {
                    group.Tasks.Add(taskToUpdate);
                    taskToUpdate.Groups.Add(group);
                }

                if (taskToUpdate.Groups.Contains(group) && taskInDetails == null)
                {
                    group.Tasks.Remove(taskToUpdate);
                    taskToUpdate.Groups.Remove(group);
                }
            }
        }
        public void ApplyChanges(ICollection<GroupHolder> currentGroups, IEnumerable<TaskHolder> currnetTasks, GroupHolder updatedGroup)
        {
            var groupToUpdate = currentGroups.SingleOrDefault(x => x.Id == updatedGroup.Id);
            if (groupToUpdate == null)
            {
                groupToUpdate = new GroupHolder();
                currentGroups.Add(groupToUpdate);
            }
            groupToUpdate.Name = updatedGroup.Name;
            groupToUpdate.Description = updatedGroup.Description;
            if (groupToUpdate.Tasks == null)
                groupToUpdate.Tasks = new ObservableCollection<TaskHolder>();


            foreach (var task in currnetTasks)
            {
                var groupInDetails = updatedGroup.Tasks.Where(x => x.Id == task.Id).SingleOrDefault();

                if (!groupToUpdate.Tasks.Contains(task) && groupInDetails != null)
                {
                    task.Groups.Add(groupToUpdate);
                    groupToUpdate.Tasks.Add(task);
                }

                if (groupToUpdate.Tasks.Contains(task) && groupInDetails == null)
                {
                    task.Groups.Remove(groupToUpdate);
                    groupToUpdate.Tasks.Remove(task);
                }
            }
        }

        public GroupHolder CreateNewGroup(IEnumerable<GroupHolder> groups)
        {
            var newGroup = new GroupHolder()
            {
                Id = GenerateTemporaryId(groups),
                Name = GetNameWithIndexIfNeeded("New group", groups),
                Tasks = new ObservableCollection<TaskHolder>(),
            };
            return newGroup;
        }
        public TaskHolder CreateNewTask(IEnumerable<TaskHolder> tasks)
        {
            var newTask = new TaskHolder()
            {
                Id = GenerateTemporaryId(tasks),
                Name = GetNameWithIndexIfNeeded("New task", tasks),
                Groups = new ObservableCollection<GroupHolder>(),
                StartDate = DateTime.Now,
                Frequency = 1,
            };
            return newTask;
        }
        public TaskHolder InitializeTaskDetails(List<GroupHolder> availableGroupsToAdd, TaskHolder choosenTask, List<GroupHolder> groups)
        {
            TaskHolder shownTask = new TaskHolder()
            {
                Id = choosenTask.Id,
                Name = choosenTask.Name,
                Description = choosenTask.Description,
                StartDate = choosenTask.StartDate,
                Frequency = choosenTask.Frequency,
                Archivized = choosenTask.Archivized,
                Groups = new ObservableCollection<GroupHolder>(),
            };

            foreach (var group in groups)
            {
                var newGroup = new GroupHolder
                {
                    Id = group.Id,
                    Name = group.Name,
                    Description = group.Description,
                    Archivized = group.Archivized,
                    Tasks = new ObservableCollection<TaskHolder>(),
                };

                if (group.Tasks.Contains(choosenTask))
                {
                    newGroup.Tasks.Add(shownTask);
                    shownTask.Groups.Add(newGroup);
                }
                else
                {
                    availableGroupsToAdd.Add(newGroup);
                }
            }
            return shownTask;
        }
        public GroupHolder InitializeGroupDetails(List<TaskHolder> availableTasksToAdd, GroupHolder chooosenGroup, List<TaskHolder> tasks)
        {
            GroupHolder shownGroup = new GroupHolder()
            {
                Id = chooosenGroup.Id,
                Name = chooosenGroup.Name,
                Description = chooosenGroup.Description,
                Archivized = chooosenGroup.Archivized,
                Tasks = new ObservableCollection<TaskHolder>(),
            };

            foreach (var task in tasks)
            {
                var newTask = new TaskHolder
                {
                    Id = task.Id,
                    Name = task.Name,
                    Description = task.Description,
                    Archivized = task.Archivized,
                    Groups = new ObservableCollection<GroupHolder>(),
                };

                if (task.Groups.Contains(chooosenGroup))
                {
                    newTask.Groups.Add(shownGroup);
                    shownGroup.Tasks.Add(newTask);
                }
                else
                {
                    availableTasksToAdd.Add(newTask);
                }
            }
            return shownGroup;
        }
    }
}