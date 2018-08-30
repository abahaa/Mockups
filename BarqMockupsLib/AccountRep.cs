using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BarqMockupsLib
{
    public class AccountRep
    {
        private BarqBECoreMockContext Context;

        public AccountRep(BarqBECoreMockContext Context)
        {
            this.Context = Context;
        }

        public Account GetByMSDIN(string MSDIN)
        {
            var UserAccount = Context.Account.Where(Acc => Acc.Msisdn == MSDIN).FirstOrDefault();
            if (UserAccount != null)
            {
                return UserAccount;
            }
            return null;
        }

        public Account ValidateCorrectCredentials(string Msisdn, string Password)
        {
            var UserAccount = Context.Account.Where(Acc => Acc.Msisdn == Msisdn && Acc.Password == Password).FirstOrDefault();
            if(UserAccount != null)
            {
                OTPRep oTPRep = new OTPRep(Context);
                int GeneratedOTPID =  oTPRep.GenerateOTP();
                UpdateLastOTp(UserAccount, GeneratedOTPID);
                return UserAccount;
            }
            return null;
        }

        public void Update(Account account)
        {
            Context.Account.Update(account);
            Context.SaveChanges();
        }

        private void UpdateLastOTp(Account account,int OtpID)
        {
            account.LastOtpid = OtpID;
            Context.Account.Update(account);
            Context.SaveChanges();
        }

    }
}
