﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Shared.Dto;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Web;
using System.Collections.Specialized;
using System.Threading;
using System.Net.Http.Formatting;

namespace Wilhelm.Client.Services
{
    public class ProxyService
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

        public async Task<ValidatedDto<UserDto>> GetNewUer(string login, string password, string confirmpassword)
        {
            ValidatedDto<UserDto> user = null;
            var query = HttpUtility.ParseQueryString("");
            query["login"] = login;
            query["password"] = password;
            query["confirmpassword"] = confirmpassword;
            var builder = GetBaseUriBuilder("Account");
            builder.Query = query.ToString();
            HttpResponseMessage response = await GetClient().GetAsync(builder.ToString());

            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<ValidatedDto<UserDto>>();
            }
            return user;
        }
        public async Task<ValidatedDto<UserDto>> GetVerifiedUer(string login, string password)
        {
            ValidatedDto<UserDto> user = null;
            var query = HttpUtility.ParseQueryString("");
            query["login"] = login;
            query["password"] = password;
            var builder = GetBaseUriBuilder("Account");
            builder.Query = query.ToString();
            HttpResponseMessage response = await GetClient().GetAsync(builder.ToString());

            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<ValidatedDto<UserDto>>();
            }
            return user;
        }
        public async Task<UserDto> GetUser(int userId)
        {
            UserDto user = null;
            var query = HttpUtility.ParseQueryString("");
            query["userId"] = userId.ToString();
            var builder = GetBaseUriBuilder("Account");
            builder.Query = query.ToString();
            HttpResponseMessage response = await GetClient().GetAsync(builder.ToString());

            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadAsAsync<UserDto>();
            }
            return user;
        }

        protected UriBuilder GetBaseUriBuilder(string controller)
        {
            UriBuilder builder = new UriBuilder("http://localhost:8080/api");
            builder.Path += "/" + controller;
            return builder;
        }
        protected HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            return client;
        }
    }
}