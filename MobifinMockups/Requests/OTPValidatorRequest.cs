using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobifinMockups.Requests
{
    [Serializable]
    public class OTPValidatorRequest : BaseRequest
    {

        [JsonProperty("Otp")]
        public string Otp { get; set; }
    }

}
