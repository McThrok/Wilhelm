using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.DataAccess;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Backend.Services
{
    public class ConfigurationService : IConfigurationService
    {
        private IWContextFactory _wContextFactory;
        private IEntitiesService _entitiesService;
        private IConversionService _conversionService;

        public ConfigurationService(IWContextFactory wContextFactory, IEntitiesService entitiesService, IConversionService conversionService)
        {
            _wContextFactory = wContextFactory;
            _entitiesService = entitiesService;
            _conversionService = conversionService;
        }

        public ConfigDto GetConfig(int userId)
        {
            ConfigDto dto = new ConfigDto();
            using (var db = _wContextFactory.Create())
            {
                var tasks = db.WTasks.Where(x => x.OwnerId == userId && !x.Archivized);
                var groups = db.WGroups.Where(x => x.OwnerId == userId && !x.Archivized);
                _entitiesService.UpdateDto(dto, tasks.ToList(), groups.ToList());
            }
            return dto;
        }
        public List<KeyValuePair<int, string>> GetTaskNames(int userId)
        {
            List<KeyValuePair<int, string>> tasks = new List<KeyValuePair<int, string>>();
            using (var db = _wContextFactory.Create())
            {
                tasks = db.WTasks.Where(x => x.OwnerId == userId && !x.Archivized)
                    .Select(o => new { o.Id, o.Name }).AsEnumerable()
                    .Select(o => new KeyValuePair<int, string>(o.Id, o.Name)).ToList();
            }
            return tasks;
        }
        public TaskDto GetTaskDetails(int taskId)
        {
            TaskDto task = new TaskDto();
            using (var db = _wContextFactory.Create())
            {
                WTask wTask = db.WTasks.SingleOrDefault(x => x.Id == taskId);
                _conversionService.ConvertToDto(task, wTask, true);
            }
            return task;
        }
        public List<Tuple<int, string, string>> GetGroups(int userId)
        {
            List<Tuple<int, string, string>> groups = new List<Tuple<int, string, string>>();
            using (var db = _wContextFactory.Create())
            {
                groups = db.WGroups.Where(o => o.OwnerId == userId)
                    .Select(o => new { o.Id, o.Name, o.Description }).AsEnumerable()
                    .Select(o => new Tuple<int, string, string>(o.Id, o.Name, o.Description)).ToList();
            }
            return groups;
        }

        public void SaveConfig(ConfigDto config)
        {
            using (var db = _wContextFactory.Create())
            {
                _entitiesService.UpdateEntities(db.WTasks, db.WGroups, config);
                _entitiesService.PrepareConfigToSave(db.WTasks, db.WGroups);
                db.SaveChanges();
            }
        }

        public void AddTask(KeyValuePair<TaskDto, List<int>> taskPair)
        {
            taskPair.Key.Groups = new List<GroupDto>();
            var config = GetConfig(taskPair.Key.OwnerId);
            config.Tasks.Add(taskPair.Key);
            foreach (var group in config.Groups)
            {
                if (taskPair.Value.Contains(group.Id))
                {
                    taskPair.Key.Groups.Add(group);
                    group.Tasks.Add(taskPair.Key);
                }
            }
            SaveConfig(config);
        }

        public void UpdateTask(KeyValuePair<TaskDto, List<int>> taskPair)
        {
            var task = taskPair.Key;
            var config = GetConfig(task.OwnerId);
            var taskToUpdate = config.Tasks.SingleOrDefault(x => x.Id == task.Id);
            taskToUpdate.Frequency = task.Frequency;
            taskToUpdate.StartDate = task.StartDate;
            taskToUpdate.Description = task.Description;
            taskToUpdate.Archivized = task.Archivized;
            taskToUpdate.Name = task.Name;

            var includedGroups = taskPair.Value;

            foreach (var group in config.Groups)
            {
                if (group.Tasks.Contains(taskToUpdate))
                {
                    if (!includedGroups.Contains(group.Id))
                    {
                        taskToUpdate.Groups.Remove(group);
                        group.Tasks.Remove(taskToUpdate);
                    }
                }
                else
                {
                    if (includedGroups.Contains(group.Id))
                    {
                        taskToUpdate.Groups.Add(group);
                        group.Tasks.Add(taskToUpdate);
                    }
                }
            }

            SaveConfig(config);
        }

        public void AddGroup(KeyValuePair<GroupDto, List<int>> groupPair)
        {
            groupPair.Key.Tasks = new List<TaskDto>();
            var config = GetConfig(groupPair.Key.OwnerId);
            config.Groups.Add(groupPair.Key);
            foreach (var task in config.Tasks)
            {
                if (groupPair.Value.Contains(task.Id))
                {
                    groupPair.Key.Tasks.Add(task);
                    task.Groups.Add(groupPair.Key);
                }
            }
            SaveConfig(config);
        }

        public void UpdateGroup(KeyValuePair<GroupDto, List<int>> groupPair)
        {
            var group = groupPair.Key;
            var config = GetConfig(group.OwnerId);
            var groupToUpdate = config.Groups.SingleOrDefault(x => x.Id == group.Id);
            groupToUpdate.Description = group.Description;
            groupToUpdate.Name = group.Name;
            groupToUpdate.Archivized = group.Archivized;

            var includedGroups = groupPair.Value;

            foreach (var task in config.Tasks)
            {
                if (task.Groups.Contains(groupToUpdate))
                {
                    if (!includedGroups.Contains(task.Id))
                    {
                        groupToUpdate.Tasks.Remove(task);
                        task.Groups.Remove(groupToUpdate);
                    }
                }
                else
                {
                    if (includedGroups.Contains(task.Id))
                    {
                        groupToUpdate.Tasks.Add(task);
                        task.Groups.Add(groupToUpdate);
                    }
                }
            }

            SaveConfig(config);
        }

    }
}
