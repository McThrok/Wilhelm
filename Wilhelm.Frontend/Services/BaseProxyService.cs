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

namespace Wilhelm.Frontend.Services
{
    public class BaseProxyService
    {
        protected UriBuilder GetBaseUriBuilder(string controller)
        {
            UriBuilder builder = new UriBuilder("http://localhost:8080/api");
            builder.Path += "/"+ controller;
            return builder;
        }
        protected HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            return client;
        }

    }
}