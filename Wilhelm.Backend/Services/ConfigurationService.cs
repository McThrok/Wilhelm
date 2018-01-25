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

        public ConfigurationService(IWContextFactory wContextFactory, IEntitiesService entitiesService)
        {
            _wContextFactory = wContextFactory;
            _entitiesService = entitiesService;
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
        public void SaveConfig(ConfigDto config)
        {
            using (var db = _wContextFactory.Create())
            {
                _entitiesService.UpdateEntities(db.WTasks, db.WGroups, config);
                _entitiesService.PrepareConfigToSave(db.WTasks, db.WGroups);
                db.SaveChanges();
            }
        }

        public void UpdateTask(TaskDto task)
        {
            var config = GetConfig(task.OwnerId);
            var taskToUpdate = config.Tasks.SingleOrDefault(x => x.Id == task.Id);
            taskToUpdate.Frequency = task.Frequency;
            taskToUpdate.StartDate = task.StartDate;
            taskToUpdate.Description = task.Description;
            taskToUpdate.Name = task.Name;

            var includedGroups = task.Groups.Select(x => x.Id).ToList();

            taskToUpdate.Groups = new List<GroupDto>();
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

        public void AddTask(TaskDto task)
        {
            var config = GetConfig(task.OwnerId);
            config.Tasks.Add(task);
            foreach (var group in task.Groups)
                config.Groups.SingleOrDefault(x => x.Id == group.Id)?.Tasks.Add(task);
            SaveConfig(config);
        }
    }
}
