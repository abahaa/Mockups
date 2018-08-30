using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobifinMockups.Requests
{
    public class MerchantPaymentRequest : BaseRequest
    {
        [JsonProperty("toWalletNO")]
        public string ToWalletNumber { get; set; }

        [JsonProperty("amount")]
        public long Amount { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("transactionRef")]
        public string TransactionRef { get; set; }

    }


}
