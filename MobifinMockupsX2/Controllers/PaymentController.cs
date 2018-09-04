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
using System.Text;

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
            if (!request.ValidateObject())
            {
                try
                {
                    ExceptionHandeling.FireError((int)ErrorCode.General_Error, (int)GeneralError.Nullable_Request, Constants.Constants.GeneralErrorDic[GeneralError.Nullable_Request]);
                }
                catch (CodeLabException codelabExp)
                {
                    return ExceptionHandeling.GenerateErrorResponse(codelabExp);
                }
            }
            MerchantPaymentResponse response = new MerchantPaymentResponse();
            response.Amount = new Amount();
            response.Amount.ServiceFees = Constants.Constants.TransactionFees;
            response.Amount.Commission = (long)request.Amount - 1;
            response.Amount.Total = (long)request.Amount + Constants.Constants.TransactionFees;
            response.TransactionId = request.TransactionId + "--" + "\n" + "Basic Info:" + request.BasicInfo.ToString();
            return Ok(response);
        }

        [HttpPost("PerformTransaction")]
        public IActionResult PerformTransaction([FromBody]ConfirmPaymentRequest request)
        {
            if (!request.ValidateObject())
            {
                try
                {
                    ExceptionHandeling.FireError((int)ErrorCode.General_Error, (int)GeneralError.Nullable_Request, Constants.Constants.GeneralErrorDic[GeneralError.Nullable_Request]);
                }
                catch (CodeLabException codelabExp)
                {
                    return ExceptionHandeling.GenerateErrorResponse(codelabExp);
                }
            }
            ConfirmPaymentResponse response = new ConfirmPaymentResponse();
            AccountRep accountRep = new AccountRep(Context);
            Account fromWallet = accountRep.GetByMSDIN(request.BasicInfo.MobileNumberInfo.Number);
            Account toWallet = accountRep.GetByMSDIN(request.ToWalletNumber);
            decimal totalAmount = (decimal)request.TotalAmount;
            string DecodedMPIN = "";
            try
            {
                byte[] data = Convert.FromBase64String(request.MPin);
                DecodedMPIN = Encoding.UTF8.GetString(data);
            }
            catch(Exception ex)
            {
                DecodedMPIN = "";
            }
            try
            {
                if (fromWallet != null && toWallet != null)
                {
                    if (fromWallet == toWallet)
                    {
                        ExceptionHandeling.FireError((int)ErrorCode.Payment_Error, (int)PaymentError.Same_From_To_Wallet, Constants.Constants.PaymentErrorDic[PaymentError.Same_From_To_Wallet]);
                    }
                    if (fromWallet.Mpin == DecodedMPIN)
                    {         
                        if (toWallet.Balance >= totalAmount)
                        {
                            if (request.TotalAmount <= Constants.Constants.MaxLimit)
                            {
                                if (request.TotalAmount >= Constants.Constants.MinLimit)
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
                                   
                                }
                                else
                                    ExceptionHandeling.FireError((int)ErrorCode.Payment_Error, (int)PaymentError.Less_Than_Min_Amount, Constants.Constants.PaymentErrorDic[PaymentError.Less_Than_Min_Amount]);

                            }
                            else
                                ExceptionHandeling.FireError((int)ErrorCode.Payment_Error, (int)PaymentError.Greater_Than_Max_Amount, Constants.Constants.PaymentErrorDic[PaymentError.Greater_Than_Max_Amount]);

                        }
                        else
                            ExceptionHandeling.FireError((int)ErrorCode.Payment_Error, (int)PaymentError.Insufficient_Balance, Constants.Constants.PaymentErrorDic[PaymentError.Insufficient_Balance]);
                    }
                    else
                        ExceptionHandeling.FireError((int)ErrorCode.Wrong_Input_Error, (int)WrongInputError.Wrong_MPIN, Constants.Constants.WrongInputDic[WrongInputError.Wrong_MPIN]);

                }
                else
                    ExceptionHandeling.FireError((int)ErrorCode.Wrong_Input_Error, (int)WrongInputError.Wrong_Wallet_Number, Constants.Constants.WrongInputDic[WrongInputError.Wrong_Wallet_Number]);
            }
            catch (CodeLabException codelabExp)
            {
                return ExceptionHandeling.GenerateErrorResponse(codelabExp);
            }
            return Ok(response);

        }

        [HttpPost("CheckTransactionStatus")]
        public IActionResult CheckTransactionStatus([FromBody]CheckStatusRequest request)
        {
            if (!request.ValidateObject())
            {
                try
                {
                    ExceptionHandeling.FireError((int)ErrorCode.General_Error, (int)GeneralError.Nullable_Request, Constants.Constants.GeneralErrorDic[GeneralError.Nullable_Request]);
                }
                catch (CodeLabException codelabExp)
                {
                    return ExceptionHandeling.GenerateErrorResponse(codelabExp);
                }
            }
            ConfirmPaymentResponse response = new ConfirmPaymentResponse();
            TransactionRep transactionRep = new TransactionRep(Context);
            AccountRep accountRep = new AccountRep(Context);
            Account account = accountRep.GetByMSDIN(request.BasicInfo.MobileNumberInfo.Number);
            Transaction transaction = transactionRep.GetByTransactionID(request.TransactionId);
            //Question
            try
            {
                if (account != null)
                {
                    if (transaction != null)
                    {
                        response.TransactionStatus = 1;
                        response.AdditionalInfo = "string information" + "\n" + "Basic Info:" + request.BasicInfo.ToString();
                        response.TransactionId = transaction.TransactionId;
                        response.CurrentBalance = (double)account.Balance;
                        response.CompletionDateTime = transaction.IssueTime.ToString(Constants.Constants.DateTimeFormat);
                        response.TotalAmount = (double)transaction.Amount;
                        response.CurrencyCode = transaction.CurrencyCode;
                    }
                    else
                        ExceptionHandeling.FireError((int)ErrorCode.Payment_Error, (int)PaymentError.Transaction_Id_Not_Exist, Constants.Constants.PaymentErrorDic[PaymentError.Transaction_Id_Not_Exist]);

                }
                else
                    ExceptionHandeling.FireError((int)ErrorCode.Wrong_Input_Error, (int)WrongInputError.Wrong_Wallet_Number, Constants.Constants.WrongInputDic[WrongInputError.Wrong_Wallet_Number]);
            }
            catch (CodeLabException codelabExp)
            {
                return ExceptionHandeling.GenerateErrorResponse(codelabExp);
            }
            return Ok(response);

        }


    }
}