using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services
{
    internal class ConfigurationService : IConfigurationService
    {
        public WConfig GetConfig()
        {
            var confing = new WConfig()
            {
                WTasks = MockBase.MockBase.GetTasks(),
                WGroups = MockBase.MockBase.GetGroups(),
            };
            return confing;
        }

        public void SaveConfig(WConfig config)
        {
            //throw new NotImplementedException();
        }
    }
    
}
