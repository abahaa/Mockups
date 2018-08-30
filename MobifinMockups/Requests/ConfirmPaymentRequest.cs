using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobifinMockups.Requests
{
    [JsonObject("data")]
    public class ConfirmPaymentRequest : BaseRequest
    {
        [JsonProperty("toWalletNO")]
        public string ToWalletNumber { get; set; }

        [JsonProperty("totalAmount")]
        public double TotalAmount { get; set; }

        [JsonProperty("currencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("transactionId")]
        public string TransactionId { get; set; }

        [JsonProperty("transactionRef")]
        public string TransactionRef { get; set; }

        [JsonProperty("MPin")]
        public string MPin { get; set; }
    }
}
