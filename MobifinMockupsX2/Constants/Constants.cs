using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobifinMockupsX2.Constants
{
    public enum ErrorCodes
    {
        Insufficient_Balance = 1,
        SR_Not_Found = 2,
        Exceed_Threshold_Limit = 3,
        Wrong_MPIN = 4,
        Less_Than_Min_Amount = 5,
        Greater_Than_Max_Amount = 6,
        Wrong_Wallet_Number_OR_Password = 7,
        Wrong_Wallet_Number = 8,
        Wrong_Otp = 9,
        Transaction_Id_Not_Exist = 10
    }
    public class Constants
    {
        public static string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        public static double TransactionFees = 1.5;
        public static double TransactionCommission = 5.00;
        public static double MinLimit = 20;
        public static double MaxLimit = 1000;
        private static Dictionary<ErrorCodes, string> _ExceptionDic;
        public static Dictionary<ErrorCodes, string> ExceptionDic
        {
            get
            {
                if(_ExceptionDic == null)
                {
                    _ExceptionDic = new Dictionary<ErrorCodes, string>();
                    _ExceptionDic.Add(ErrorCodes.Insufficient_Balance, "Merchant has insufficient balance");
                    _ExceptionDic.Add(ErrorCodes.SR_Not_Found, "SR account not found (i.e. wrong) or inactive ");
                    _ExceptionDic.Add(ErrorCodes.Exceed_Threshold_Limit, "Merchant is exceeding threshold limits");
                    //_ExceptionDic.Add(4, "Connection issue Before transaction");
                    //_ExceptionDic.Add(5, "Connection issue during the time of the transaction");
                    //_ExceptionDic.Add(6, "Selected contact number is bigger than wallet number field length");
                    _ExceptionDic.Add(ErrorCodes.Wrong_MPIN, "Wrong MPIN");
                    _ExceptionDic.Add(ErrorCodes.Less_Than_Min_Amount, "Entered amount less than minimum amount");
                    _ExceptionDic.Add(ErrorCodes.Greater_Than_Max_Amount, "Entered amount greater than Maximum amount");
                    _ExceptionDic.Add(ErrorCodes.Wrong_Wallet_Number_OR_Password, "Wrong Wallet Number Or Password");
                    _ExceptionDic.Add(ErrorCodes.Wrong_Wallet_Number, "Wrong Wallet Number");
                    _ExceptionDic.Add(ErrorCodes.Wrong_Otp, "Wrong OTP");
                    _ExceptionDic.Add(ErrorCodes.Transaction_Id_Not_Exist, "Transaction ID is Not Exist");
                }
                return _ExceptionDic;
            }
            set
            {
                _ExceptionDic = value;
            }
        }

        //public static void IntializeErrorDic()
        //{
           

        //}
        
        

    }
}
