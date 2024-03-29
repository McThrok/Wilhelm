﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Client.Services.Interfaces;
using Wilhelm.Client.Model;
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
    public class AccountProxyService : BaseProxyService, IAccountProxyService
    {
        public async Task<ValidatedDto<UserDto>> GetNewUer(string login,string password, string confirmpassword)
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
    }
}