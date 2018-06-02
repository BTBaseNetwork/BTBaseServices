using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BahamutCommon.Utils;
using BTBaseServices.DAL;
using BTBaseServices.Models;

namespace BTBaseServices.Services
{
    public class SecurityCodeService
    {
        public BTSecurityCode RequestNewCode(BTBaseDbContext dbContext, string accountId, int requestFor, int receiverType, string receiver, TimeSpan Expiry, int codeLength = 6, bool onlyNumber = true)
        {
            var now = DateTime.Now;
            var existsRes = from r in dbContext.BTSecurityCode
                            where r.AccountId == accountId &&
                                 r.RequestFor == requestFor &&
                                 r.Receiver == receiver &&
                                 r.ReceiverType == r.ReceiverType &&
                                 r.Status == BTSecurityCode.STATUS_VALID &&
                                 r.ExpiredOn > now
                            select r;

            if (existsRes.Count() > 0)
            {
                return existsRes.First();
            }
            var expiredOn = now.Add(Expiry);
            var code = new BTSecurityCode
            {
                AccountId = accountId,
                RequestDate = now,
                RequestFor = requestFor,
                Receiver = receiver,
                ReceiverType = receiverType,
                Status = BTSecurityCode.STATUS_VALID,
                ExpiredOn = expiredOn,
                Code = NewCode(codeLength, onlyNumber).ToLower()
            };
            dbContext.BTSecurityCode.Add(code);
            dbContext.SaveChanges();
            return code;
        }

        public bool VerifyCode(BTBaseDbContext dbContext, string accountId, int requestFor, string code)
        {
            var now = DateTime.Now;
            var lst = from c in dbContext.BTSecurityCode where c.AccountId == accountId && c.RequestFor == requestFor && c.Code == code && c.ExpiredOn > now && c.Status == BTSecurityCode.STATUS_VALID select c;
            if (lst.Count() > 0)
            {
                var result = lst.First();
                result.Status = BTSecurityCode.STATUS_INVALID;
                dbContext.BTSecurityCode.Update(result);
                dbContext.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        private readonly char[] NUM_CODE_LETTERS = "012345678998765432100123456789987654321001234567899876543210".ToArray();
        private readonly char[] NUM_LATIN_CODE_LETTERS = "0123456789abcdefghijklmnopqrstuvwxyzzyxwvutsrqponmlkjihgfedcba9876543210".ToArray();

        private string NewCode(int codeLength, bool onlyNumber)
        {
            var arr = onlyNumber ? NUM_CODE_LETTERS : NUM_LATIN_CODE_LETTERS;
            var sb = new System.Text.StringBuilder();
            var random = new Random();
            for (int i = 0; i < codeLength; i++)
            {
                sb.Append(arr[random.Next(0, arr.Length)]);
            }
            return sb.ToString();
        }
    }
}