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

namespace Wilhelm.Client.Services.Interfaces
{
    public interface IAccountProxyService
    {
        Task<ValidatedDto<UserDto>> GetNewUer(string login, string password, string confirmpassword);
        Task<ValidatedDto<UserDto>> GetVerifiedUer(string login, string password);
    }
}