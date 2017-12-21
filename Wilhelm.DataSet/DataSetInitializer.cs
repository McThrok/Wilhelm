using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Wilhelm.DataAccess;

namespace Wilhelm.MainDataSet
{
    public class DataSetInitializer
    {
        static void Main(string[] args)
        {
            var builder = GetBaseUriBuilder("MainDataSet");
            GetClient().GetAsync(builder.ToString());

        }
        protected static UriBuilder GetBaseUriBuilder(string controller)
        {
            UriBuilder builder = new UriBuilder("http://localhost:8080/api");
            builder.Path += "/" + controller;
            return builder;
        }
        protected static HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            return client;
        }

    }
}
