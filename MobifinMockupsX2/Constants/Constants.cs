﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MobifinMockupsX2.Constants
{
    public enum ErrorCode
    {
        Wrong_Input_Error = 1,
        Payment_Error = 2,
        General_Error = 3,
    }
    public enum WrongInputError
    {
        Wrong_Wallet_Number_OR_Password = 1,
        Wrong_Wallet_Number = 2,
        Wrong_Otp = 3,
        Wrong_MPIN = 4
    }
    public enum PaymentError
    {
        Insufficient_Balance = 1,
        Less_Than_Min_Amount = 2,
        Greater_Than_Max_Amount = 3,
        Transaction_Id_Not_Exist = 4,
        Same_From_To_Wallet = 5
    }
    public enum GeneralError
    {
        Nullable_Request = 1,
    }

    public class Constants
    {
        public static string DateTimeFormat = "yyyy/MM/dd HH:mm:ss";
        public static double TransactionFees = 1.5;
        public static double TransactionCommission = 5.00;
        public static double MinLimit = 20;
        public static double MaxLimit = 1000;

        private static Dictionary<WrongInputError, string> _WrongInputDic;
        public static Dictionary<WrongInputError, string> WrongInputDic
        {
            get
            {
                if(_WrongInputDic == null)
                {
                    _WrongInputDic = new Dictionary<WrongInputError, string>();
                    //_WrongInputDic.Add(4, "Connection issue Before transaction");
                    //_WrongInputDic.Add(5, "Connection issue during the time of the transaction");
                    //_WrongInputDic.Add(6, "Selected contact number is bigger than wallet number field length");
                    _WrongInputDic.Add(WrongInputError.Wrong_MPIN, "Wrong MPIN");
                    _WrongInputDic.Add(WrongInputError.Wrong_Wallet_Number_OR_Password, "Wrong Wallet Number Or Password");
                    _WrongInputDic.Add(WrongInputError.Wrong_Wallet_Number, "Wrong Wallet Number");
                    _WrongInputDic.Add(WrongInputError.Wrong_Otp, "Wrong OTP");
                }
                return _WrongInputDic;
            }
            set
            {
                _WrongInputDic = value;
            }
        }

        private static Dictionary<PaymentError, string> _PaymentErrorDic;
        public static Dictionary<PaymentError, string> PaymentErrorDic
        {
            get
            {
                if (_PaymentErrorDic == null)
                {
                    _PaymentErrorDic = new Dictionary<PaymentError, string>();
                    _PaymentErrorDic.Add(PaymentError.Greater_Than_Max_Amount, "Payment Exceed Maximun Amount");
                    _PaymentErrorDic.Add(PaymentError.Less_Than_Min_Amount, "Amount is Less than Minimum Amount");
                    _PaymentErrorDic.Add(PaymentError.Insufficient_Balance, "Insufficient Balance in the wallet");
                    _PaymentErrorDic.Add(PaymentError.Transaction_Id_Not_Exist, "Transaction Id not exist");
                    _PaymentErrorDic.Add(PaymentError.Same_From_To_Wallet, "Can not send the amount to the same wallet");

                }
                return _PaymentErrorDic;
            }
            set
            {
                _PaymentErrorDic = value;
            }
        }

        private static Dictionary<GeneralError, string> _GeneralErrorDic;
        public static Dictionary<GeneralError, string> GeneralErrorDic
        {
            get
            {
                if (_GeneralErrorDic == null)
                {
                    _GeneralErrorDic = new Dictionary<GeneralError, string>();
                    _GeneralErrorDic.Add(GeneralError.Nullable_Request, "Check your request is sent as expected");
                }
                return _GeneralErrorDic;
            }
            set
            {
                _GeneralErrorDic = value;
            }
        }


    }
}
