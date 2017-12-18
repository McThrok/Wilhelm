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

namespace Wilhelm.Frontend.Services
{
    public class ProxyService
    {
        public async Task<IEnumerable<ActivityDto>> GetTodaysTasks(int userId)
        {

            var activities = new ActivityDto[0];
            var query = HttpUtility.ParseQueryString("");
            query["userId"] = userId.ToString();
            var builder = GetBaseUri();
            builder.Path += "/ActiveActivities";
            builder.Query = query.ToString();
            var a = builder.ToString();
          //  HttpResponseMessage response = await GetClient().GetAsync(builder.ToString());

            //var client = new HttpClient();
            //var a = "http://localhost:55378/api/ActiveActivities?userId=1";
            //HttpResponseMessage response = await GetClient().GetAsync(a);

            //if (response.IsSuccessStatusCode)
            //{
            //    activities = await response.Content.ReadAsAsync<ActivityDto[]>();
            //}
            return activities;
        }

        public async Task SaveTodaysTasks(int userId, IEnumerable<ActivityDto> archive)
        {
           // HttpResponseMessage response = await GetClient().PostAsJsonAsync($"api/ActiveActivities/{userId}", JsonConvert.SerializeObject(archive));
        }

        public async Task<IEnumerable<ActivityDto>> GetArchive(int userId)
        {
            ActivityDto[] products = null;
            HttpResponseMessage response = await GetClient().GetAsync("api/ArchiveActivities/");
            if (response.IsSuccessStatusCode)
            {
                products = await response.Content.ReadAsAsync<ActivityDto[]>();
            }
            return products;
        }
        public async Task SaveArchive(int userId, IEnumerable<ActivityDto> archive)
        {

        }

        public async Task<ConfigDto> GetConfig(int userId)
        {
            return null;
        }
        public async Task SaveConfig(int userId, ConfigDto config)
        {

        }

        public async Task<ConfigDto> GetReports(int userId)
        {
            return null;
        }

        private UriBuilder GetBaseUri()
        {
            UriBuilder builder = new UriBuilder("http://localhost:55378/api");
            return builder;
        }
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

    }
}