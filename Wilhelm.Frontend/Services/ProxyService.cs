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

namespace Wilhelm.Frontend.Services
{
    public class ProxyService
    {
        public async Task<IEnumerable<ActivityDto>> GetTodaysTasks(int userId)
        {
            ActivityDto[] products = null;
            HttpResponseMessage response = await GetClient().GetAsync($"api/ActiveActivities/{userId}");
            
            if (response.IsSuccessStatusCode)
            {
                products = await response.Content.ReadAsAsync<ActivityDto[]>();
            }
            return products;
        }
        public async Task SaveTodaysTasks(int userId, IEnumerable<ActivityDto> archive)
        {

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


        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:60869/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

    }
}