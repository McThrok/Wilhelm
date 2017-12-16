using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.Model;
using Wilhelm.DataAccess;
using Wilhelm.Shared.Model;

namespace Wilhelm.Backend.Services.Interfaces
{
    public interface IConfigurationService
    {
        ConfigDto GetConfig(int userId);
        void SaveConfig(ConfigDto config);
    }
}
