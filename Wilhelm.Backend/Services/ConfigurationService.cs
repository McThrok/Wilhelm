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
                _entitiesService.UpdateDto(dto, tasks, groups);
            }
            return dto;
        }
        public void SaveConfig( ConfigDto config)
        {
            using (var db = _wContextFactory.Create())
            {
                _entitiesService.UpdateEntities(db.WTasks, db.WGroups, config);
                _entitiesService.PrepareConfigToSave(db.WTasks, db.WGroups);
                db.SaveChanges();
            }
        }
    }
}
