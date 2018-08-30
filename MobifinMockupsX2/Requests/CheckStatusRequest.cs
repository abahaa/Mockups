using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobifinMockupsX2.Requests
{
    public class CheckStatusRequest : BaseRequest
    {
        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }
    }
}
