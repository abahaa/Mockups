using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobifinMockups.Requests
{
    public class LoginRequest : BaseRequest
    {
        [JsonProperty("password")]
        public string Password { get; set; }
    }

}
