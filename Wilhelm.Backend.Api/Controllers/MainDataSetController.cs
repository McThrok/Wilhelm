using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Wilhelm.Backend.Services.Interfaces;
using Wilhelm.Backend.Services;
using Wilhelm.Shared.Dto;

namespace Wilhelm.Backend.Api.Controllers
{
    public class MainDataSetController : ApiController
    {
        public MainDataSetController()
        {

        }
        public void GetDataSet()
        {
            var initializer = new DataSetInitializationService();
            initializer.Clean();
            initializer.Init();

        }
    }
}
