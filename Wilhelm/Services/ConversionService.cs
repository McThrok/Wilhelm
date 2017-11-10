using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wilhelm.Backend.MockBase;
using Wilhelm.Backend.Model;
using Wilhelm.Backend.Model.Dto;

namespace Wilhelm.Services
{
    public class ConversionService
    {
        public ConfigHolder ConvertFromDto(ConfigDto config)
        {
            var holder = new ConfigHolder();

            return holder;
        }

        public Holder ConvertFromDto(ConfigDto)
    }
}
