using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.DataAccess;

namespace Wilhelm.Backend.Services
{
    public interface IConfigurationService
    {
        WConfig GetConfig();
        void SaveConfig(WConfig config);
    }
}
