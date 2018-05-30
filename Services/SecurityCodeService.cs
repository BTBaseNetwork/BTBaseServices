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
                                 r.ExpiredAt > now
                            select r;

            if (existsRes.Count() > 0)
            {
                return existsRes.First();
            }
            var expiredAt = now.Add(Expiry);
            var code = new BTSecurityCode
            {
                AccountId = accountId,
                RequestDate = now,
                RequestFor = requestFor,
                Receiver = receiver,
                ReceiverType = receiverType,
                Status = BTSecurityCode.STATUS_VALID,
                ExpiredAt = expiredAt,
                Code = NewCode(codeLength, onlyNumber).ToLower()
            };
            dbContext.BTSecurityCode.Add(code);
            dbContext.SaveChanges();
            return code;
        }

        public bool VerifyCode(BTBaseDbContext dbContext, string accountId, int requestFor, string code)
        {
            var now = DateTime.Now;
            var lst = from c in dbContext.BTSecurityCode where c.AccountId == accountId && c.RequestFor == requestFor && c.Code == code && c.ExpiredAt > now && c.Status == BTSecurityCode.STATUS_VALID select c;
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


        public async Task<bool> SendCodeAsync(BTSecurityCode code, object info)
        {
            switch (code.ReceiverType)
            {
                case BTSecurityCode.REC_TYPE_EMAIL: return await SendCodeByAliEmailAsync(code, info);
                case BTSecurityCode.REC_TYPE_MOBILE: return await SendCodeByMobileAsync(code, info);
                default: return false;
            }
        }

        private async Task<bool> SendCodeByMobileAsync(BTSecurityCode code, object info)
        {
            return await Task.Run(() =>
            {
                //TODO: to finish this;
                return false;
            });
        }

        private async Task<bool> SendCodeByAliEmailAsync(BTSecurityCode code, object info)
        {
            string verifyCodeType = null;
            switch (code.RequestFor)
            {
                case BTSecurityCode.REQ_FOR_RESET_EMAIL: verifyCodeType = "Update Email"; break;
                case BTSecurityCode.REQ_FOR_RESET_PASSWORD: verifyCodeType = "Reset Password"; break;
                default: return false;
            }
            var mail = new AliApilUtils.AliMail
            {
                Action = "SingleSendMail",
                AccountName = "verification-code@btbase.mobi",
                ReplyToAddress = false,
                AddressType = AliApilUtils.AliMail.ADDR_TYPE_ACCOUNT,
                ToAddress = code.Receiver,
                FromAlias = "BTBaseNetwork",
                Subject = "Security Code For " + verifyCodeType,
                HtmlBody = string.Format("<p>Your Security Code:</p><br/><h1>{0}</h1>", code.Code),
                ClickTrace = AliApilUtils.AliMail.CLICK_TRACE_OFF
            };
            var aliReqfields = (AliApilUtils.CommonReqFields)info;
            return await AliApilUtils.SendMailAsync(aliReqfields, mail);
        }
    }
}