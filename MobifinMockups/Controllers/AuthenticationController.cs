using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using MobifinMockups.Requests;
using MobifinMockups.Responses;
using Newtonsoft.Json;
using BarqMockupsLib;
using Microsoft.Extensions.Configuration;

namespace MobifinMockups.Controllers
{
    [Produces("application/json")]
    [Route("api/Auth")]
    public class AuthenticationController : Controller
    {
        private BarqBECoreMockContext Context;
        private static Account CurrentAccount;

        public AuthenticationController(IConfiguration configuration, BarqBECoreMockContext Context)
        {
            this.Context = Context;
        }


        [HttpPost("ValidateCredentials")]
        public IActionResult ValidateCredentials([FromBody]LoginRequest request)
        {
            LoginResponse response = new LoginResponse();
            AccountRep accountRep = new AccountRep(Context);
            CurrentAccount = accountRep.ValidateCorrectCredentials(request.BasicInfo.MobileNumberInfo.Number, request.Password);
            if (CurrentAccount != null)
            {
                response.LoginStatus = 1;
                response.AdditionalInfo = "Login Success with password " + request.Password + "\n"
                    + "Basic Info: " + request.BasicInfo.ToString();
            }

            else
            {
                List<CodeLabException> allErrors = new List<CodeLabException>();
                try
                {
                    FireError();
                }
                catch (CodeLabException codelabExp)
                {
                    allErrors.Add(codelabExp);
                    
                    ObjectResult res = new ObjectResult(allErrors);
                    res.ContentTypes.Add("application/json");

                    var formatterSettings = JsonSerializerSettingsProvider.CreateSerializerSettings();
                    formatterSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    JsonOutputFormatter formatter = new JsonOutputFormatter(formatterSettings, ArrayPool<Char>.Create() );

                    res.Formatters.Add(formatter);
                    res.StatusCode = 490;
                    return res;
                }
            }
            return Ok(response);

        }

        private static void FireError()
        {
            CodeLabException codelabExp = new CodeLabException
            {
                ErrorCode = 1,
                SubErrorCode = 2,
                ErrorReferenceNumber = "UU-266169856"
            };
            codelabExp.Data.Add("LoggedInId", 88);
            codelabExp.Data.Add("NoOfTrials", 3);
            codelabExp.ExtraInfoMessage = "call failed because : مزاجي كده";
            throw  codelabExp;
        }

        [HttpPost("ValidateOTPLogin")]
        public IActionResult ValidateOTPLogin([FromBody]OTPValidatorRequest request)
        {
            OTPValidatorResponse response = new OTPValidatorResponse();
            if(CurrentAccount == null)
            {
                throw new NotImplementedException();
            }
            OTPRep oTPRep = new OTPRep(Context);
            bool access = oTPRep.ValidateOTP(request.Otp, CurrentAccount.LastOtpid);
            if (access == true)
            {
                response.Status = 1;
                response.CurrentBalance = 200.23;
                response.AdditionalInfo = "Login successfully with otp " + request.Otp + "\n"
                    + "Basic Info: " + request.BasicInfo.ToString();
            }
            else
            {
                //Not Emplemented Error Code Yet
                response.Status = 2;
                response.CurrentBalance = 200.23;
                response.AdditionalInfo = "Login Failed with otp " + request.Otp + "\n"
                    + "Basic Info: " + request.BasicInfo.ToString();
            }
            return Ok(response);
        }


    }

    [Serializable]
    public class CodeLabException : Exception
    {
        int _ErrorCode = -1;
        string _ErrorReferenceNumber = "";
        int _SubErrorCode = -1;
        string _ExtraInfoMessage = string.Empty;

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("errorCode", ErrorCode);
            info.AddValue("errorRefNO", ErrorReferenceNumber);
            info.AddValue("subErrorCode", SubErrorCode);
            info.AddValue("extraInfoMessage", ExtraInfoMessage);
            base.GetObjectData(info, context);
        }
        //Dictionary<string , object> _AssiciatedParameters = new Dictionary<string, object>();

        /// <summary>
        /// error code for the problem.
        /// Example: InvalidEntityDefinition
        /// </summary>
        /// 
        [JsonProperty("errorCode")]
        public int ErrorCode
        {
            get => _ErrorCode;
            set => _ErrorCode = value;
        }

        [JsonProperty("errorRefNO")]
        public string ErrorReferenceNumber
        {
            get => _ErrorReferenceNumber;
            set => _ErrorReferenceNumber = value;
        }


        /// <summary>
        /// a more specific code to the above one. This is optional
        /// Example: InvalidUserNameLength
        /// </summary>
        /// 
        [JsonProperty("subErrorCode")]
        public int SubErrorCode
        {
            get => _SubErrorCode;
            set => _SubErrorCode = value;
        }

        [JsonProperty("extraInfoMessage")]
        public string ExtraInfoMessage { get => _ExtraInfoMessage; set => _ExtraInfoMessage = value; }


        public CodeLabException() : base()
        {

        }

        public CodeLabException(string message) : base(message)
        { }

        public CodeLabException(string message, Exception innerException) : base(message, innerException)
        { }

        protected CodeLabException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }

}