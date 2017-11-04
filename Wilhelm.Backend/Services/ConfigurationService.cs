using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services
{
    internal class ConfigurationService : IConfigurationService
    {
        public WConfig GetConfig()
        {
            var confing = new WConfig()
            {
                Tasks = MockBase.MockBase.GetTasks(),
                Groups = MockBase.MockBase.GetGroups(),
            };
            return confing;
        }

        public void SaveConfig(WConfig config)
        {
            //throw new NotImplementedException();
        }
    }
    
}
