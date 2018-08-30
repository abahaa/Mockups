using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobifinMockups.Requests;
using MobifinMockups.Responses;
using MobifinMockups.Constants;

namespace MobifinMockups.Controllers
{
    [Produces("application/json")]
    [Route("api/Payment/MerchantToSR")]
    public class PaymentController : Controller
    {
        [HttpPost("EstimateTransactionDetails")]
        public IActionResult EstimateTransactionDetails([FromBody]MerchantPaymentRequest request)
        {
            MerchantPaymentResponse response = new MerchantPaymentResponse();
            response.Amount = new Amount();
            response.Amount.ServiceFees = Constants.Constants.TransactionFees;
            response.Amount.Commission = request.Amount - 1;
            response.Amount.Total = request.Amount + Constants.Constants.TransactionFees;
            response.TransactionId = request.TransactionId + "--" + "\n" + "Basic Info:" + request.BasicInfo.ToString();
            return Ok(response);
        }

        [HttpPost("PerformTransaction")]
        public IActionResult PerformTransaction([FromBody]ConfirmPaymentRequest request)
        {
            ConfirmPaymentResponse response = new ConfirmPaymentResponse();
            response.TransactionStatus = 1;
            response.AdditionalInfo = "string information" + "\n" + "Basic Info:" + request.BasicInfo.ToString(); ;
            response.TransactionId = request.TransactionId;
            response.CurrentBalance = 200.36;
            response.CompletionDateTime = DateTime.Now.ToString(Constants.Constants.DateTimeFormat);
            response.TotalAmount = request.TotalAmount;
            response.CurrencyCode = request.CurrencyCode;
            return Ok(response);

        }
        [HttpPost("CheckTransactionStatus")]
        public IActionResult CheckTransactionStatus([FromBody]CheckStatusRequest request)
        {
            ConfirmPaymentResponse response = new ConfirmPaymentResponse();
            response.TransactionStatus = 1;
            response.AdditionalInfo = "string information" + "\n" + "Basic Info:" + request.BasicInfo.ToString();
            response.TransactionId = request.TransactionId;
            response.CurrentBalance = 200.36;
            response.CompletionDateTime = DateTime.Now.ToString(Constants.Constants.DateTimeFormat);
            response.TotalAmount = 50.6;
            response.CurrencyCode = "SAR";
            return Ok(response);
        }


    }
}