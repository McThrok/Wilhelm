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
        private IWContextFactory _wContextFactory = new WContextFactory();
        private EntitiesService _entitiesService = new EntitiesService();

        public ConfigDto GetConfig()
        {
            ConfigDto dto = null;
            using (var db = _wContextFactory.Create())
            {
                _entitiesService.UpdateConfig(dto,db.WTasks, db.WGroups);
            }
            return dto;
        }
        public void SaveConfig(ConfigDto config)
        {
            using (var db = _wContextFactory.Create())
            {
                var wGroups = db.WGroups;
                var wTasks = db.WTasks;
                _entitiesService.UpdateEntities(wTasks, wGroups, config);
                db.SaveChanges();
            }
        }

       
    }

}
