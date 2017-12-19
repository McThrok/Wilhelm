using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.Model;
using Wilhelm.Shared.Dto;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Specialized;
using System.Threading;
using System.Net.Http.Formatting;

namespace Wilhelm.Frontend.Services.Interfaces
{
    public interface IProxyService
    {
        Task<IEnumerable<ActivityDto>> GetTodaysTasks(int userId);
        Task SaveTodaysTasks(int userId, IEnumerable<ActivityDto> activities);

        Task<IEnumerable<ActivityDto>> GetArchive(int userId);
        Task SaveArchive(int userId, IEnumerable<ActivityDto> archive);

        Task<ConfigDto> GetConfig(int userId);
        Task SaveConfig(int userId, ConfigDto config);

        Task<IEnumerable<ReportDto>> GetReports(int userId);
    }
}