using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.MockBase;
using Wilhelm.Backend.Model;
using Wilhelm.Frontend.Services.Interfaces;
using Wilhelm.Frontend.Model;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Shared.Dto;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Specialized;
using System.Threading;
using System.Net.Http.Formatting;

namespace Wilhelm.Frontend.Services
{
    public class ProxyService : IProxyService
    {
        public async Task<IEnumerable<ActivityDto>> GetTodaysTasks(int userId)
        {
            var activities = new ActivityDto[0];
            var query = HttpUtility.ParseQueryString("");
            query["userId"] = userId.ToString();
            var builder = GetBaseUriBuilder("ActiveActivities");
            builder.Query = query.ToString();
            HttpResponseMessage response = await GetClient().GetAsync(builder.ToString());

            if (response.IsSuccessStatusCode)
            {
                activities = await response.Content.ReadAsAsync<ActivityDto[]>();
            }
            return activities;
        }
        public async Task SaveTodaysTasks(int userId, IEnumerable<ActivityDto> activities)
        {
            var query = HttpUtility.ParseQueryString("");
            query["userId"] = userId.ToString();
            var builder = GetBaseUriBuilder("ActiveActivities");
            builder.Query = query.ToString();

            HttpResponseMessage postResponse = await GetClient().PostAsJsonAsync(builder.ToString(), JsonConvert.SerializeObject(activities));
            postResponse.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<ActivityDto>> GetArchive(int userId)
        {
            var activities = new ActivityDto[0];
            var query = HttpUtility.ParseQueryString("");
            query["userId"] = userId.ToString();
            var builder = GetBaseUriBuilder("ArchiveActivities");
            builder.Query = query.ToString();
            HttpResponseMessage response = await GetClient().GetAsync(builder.ToString());

            if (response.IsSuccessStatusCode)
            {
                activities = await response.Content.ReadAsAsync<ActivityDto[]>();
            }
            return activities;
        }
        public async Task SaveArchive(int userId, IEnumerable<ActivityDto> archive)
        {
            var query = HttpUtility.ParseQueryString("");
            query["userId"] = userId.ToString();
            var builder = GetBaseUriBuilder("ArchiveActivities");
            builder.Query = query.ToString();

            HttpResponseMessage postResponse = await GetClient().PostAsJsonAsync(builder.ToString(), JsonConvert.SerializeObject(archive));
            postResponse.EnsureSuccessStatusCode();
        }

        public async Task<ConfigDto> GetConfig(int userId)
        {
            ConfigDto config = null;
            var query = HttpUtility.ParseQueryString("");
            query["userId"] = userId.ToString();
            var builder = GetBaseUriBuilder("Configuration");
            builder.Query = query.ToString();
            HttpResponseMessage response = await GetClient().GetAsync(builder.ToString());

            if (response.IsSuccessStatusCode)
            {
                config = await response.Content.ReadAsAsync<ConfigDto>();
            }
            return config;
        }
        public async Task SaveConfig(int userId, ConfigDto config)
        {
            var query = HttpUtility.ParseQueryString("");
            query["userId"] = userId.ToString();
            var builder = GetBaseUriBuilder("Configuration");
            builder.Query = query.ToString();

            HttpResponseMessage postResponse = await GetClient().PostAsJsonAsync(builder.ToString(), JsonConvert.SerializeObject(config));
            postResponse.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<ReportDto>> GetReports(int userId)
        {
            ReportDto[] config = null;
            var query = HttpUtility.ParseQueryString("");
            query["userId"] = userId.ToString();
            var builder = GetBaseUriBuilder("Report");
            builder.Query = query.ToString();
            HttpResponseMessage response = await GetClient().GetAsync(builder.ToString());

            if (response.IsSuccessStatusCode)
            {
                config = await response.Content.ReadAsAsync<ReportDto[]>();
            }
            return config;
        }

        private UriBuilder GetBaseUriBuilder(string controller)
        {
            UriBuilder builder = new UriBuilder("http://localhost:8080/api");
            builder.Path += "/"+ controller;
            return builder;
        }
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            return client;
        }

    }
}