using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MobifinMockupsX2.Requests;
using MobifinMockupsX2.Responses;
using MobifinMockupsX2.Constants;
using BarqMockupsLib;
using Microsoft.Extensions.Configuration;

namespace MobifinMockupsX2.Controllers
{
    [Produces("application/json")]
    [Route("api/Payment/MerchantToSR")]
    public class PaymentController : Controller
    {
        private BarqBECoreMockContext Context;

        public PaymentController(IConfiguration configuration, BarqBECoreMockContext Context)
        {
            this.Context = Context;
        }

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
            AccountRep accountRep = new AccountRep(Context);
            Account fromWallet = accountRep.GetByMSDIN(request.BasicInfo.MobileNumberInfo.Number);
            Account toWallet = accountRep.GetByMSDIN(request.ToWalletNumber);
            decimal totalAmount = (decimal)request.TotalAmount;
            if (fromWallet != null && toWallet != null)
            {
                if (fromWallet.Mpin == request.MPin)
                {
                    //Question
                    //What if total amount is 0
                    if (toWallet.Balance >= totalAmount)
                    {
                        fromWallet.Balance -= totalAmount;
                        toWallet.Balance += totalAmount;
                        accountRep.Update(fromWallet);
                        accountRep.Update(toWallet);
                        //insert Transaction
                        Transaction transaction = new Transaction()
                        {
                            TransactionId = request.TransactionId,
                            FromAccount = fromWallet.Id,
                            ToAccount = toWallet.Id,
                            Status = 2,
                            IssueTime = DateTime.Now,
                            LastUpdateTime = DateTime.Now,
                            Amount = totalAmount,
                            CurrencyCode = request.CurrencyCode
                            
                        };
                        TransactionRep transactionRep = new TransactionRep(Context);
                        transactionRep.InsertTransaction(transaction);
                        response.TransactionStatus = 1;
                        response.AdditionalInfo = "string information" + "\n" + "Basic Info:" + request.BasicInfo.ToString(); ;
                        response.TransactionId = request.TransactionId;
                        response.CurrentBalance = (double)fromWallet.Balance;
                        response.CompletionDateTime = transaction.IssueTime.ToString(Constants.Constants.DateTimeFormat);
                        response.TotalAmount = (double)transaction.Amount;
                        response.CurrencyCode = transaction.CurrencyCode;
                        return Ok(response);
                    }
                    throw new NotImplementedException();
                }
                throw new NotImplementedException();

            }
            throw new NotImplementedException();
        }
        [HttpPost("CheckTransactionStatus")]
        public IActionResult CheckTransactionStatus([FromBody]CheckStatusRequest request)
        {
            ConfirmPaymentResponse response = new ConfirmPaymentResponse();
            TransactionRep transactionRep = new TransactionRep(Context);
            AccountRep accountRep = new AccountRep(Context);
            Account account = accountRep.GetByMSDIN(request.BasicInfo.MobileNumberInfo.Number);
            Transaction transaction = transactionRep.GetByTransactionID(request.TransactionId);
            //Question
            if (account != null && transaction != null)
            {
                response.TransactionStatus = 1;
                response.AdditionalInfo = "string information" + "\n" + "Basic Info:" + request.BasicInfo.ToString();
                response.TransactionId = transaction.TransactionId;
                response.CurrentBalance = (double)account.Balance;
                response.CompletionDateTime = transaction.IssueTime.ToString(Constants.Constants.DateTimeFormat);
                response.TotalAmount = (double)transaction.Amount;
                response.CurrencyCode = transaction.CurrencyCode;
                return Ok(response);
            }
            throw new NotImplementedException();
        }


    }
}