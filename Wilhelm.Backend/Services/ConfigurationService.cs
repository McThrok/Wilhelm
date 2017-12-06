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
    internal class ConfigurationService : IConfigurationService
    {
        private IWContextFactory _wContextFactory;
        private IEntitiesService _entitiesService;

        public ConfigurationService(IWContextFactory wContextFactory, IEntitiesService entitiesService)
        {
            _wContextFactory = wContextFactory;
            _entitiesService = entitiesService;
        }

        public ConfigDto GetConfig()
        {
            ConfigDto dto = new ConfigDto();
            using (var db = _wContextFactory.Create())
            {
                _entitiesService.UpdateDto(dto, db.WTasks.Where(x=>!x.Archivized).Include(x=>x.Owner), db.WGroups.Where(x => !x.Archivized).Include(x => x.Owner));
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
    }
}
