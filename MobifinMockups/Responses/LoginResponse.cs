﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobifinMockups.Responses
{
    public class LoginResponse
    {

        [JsonProperty("loginStatus")]
        public int LoginStatus { get; set; }

        [JsonProperty("additionalInfo")]
        public string AdditionalInfo { get; set; }
    }
}
