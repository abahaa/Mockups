using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobifinMockupsX2.Requests
{
    public class LoginRequest : BaseRequest
    {
        [JsonProperty("password")]
        public string Password { get; set; }

        public override bool ValidateObject()
        {
            bool ret = false;
            if (Password != null)
            {
                ret = true;
            }
            return ret & base.ValidateObject();

        }
    }

}
