using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BahamutCommon.Utils;
using BTBaseServices.DAL;
using BTBaseServices.Models;

namespace BTBaseServices.Services
{
    public class VerifyCodeService
    {
        public BTVerifyCode RequestNewVerifyCode(BTBaseDbContext dbContext, string accountId, int requestFor, int receiverType, string receiver, TimeSpan Expiry, int codeLength = 6, bool onlyNumber = true)
        {
            var now = DateTime.Now;
            var code = new BTVerifyCode
            {
                AccountId = accountId,
                RequestDate = now,
                RequestFor = requestFor,
                Receiver = receiver,
                ReceiverType = receiverType,
                Status = BTVerifyCode.STATUS_VALID,
                ExpiredAt = now.Add(Expiry),
                Code = NewCode(codeLength, onlyNumber).ToLower()
            };
            dbContext.BTVerifyCode.Add(code);
            dbContext.SaveChanges();
            return code;
        }

        public bool VerifyCode(BTBaseDbContext dbContext, string accountId, int requestFor, string code)
        {
            var now = DateTime.Now;
            var lst = from c in dbContext.BTVerifyCode where c.AccountId == accountId && c.RequestFor == requestFor && c.Code == code && c.ExpiredAt > now && c.Status == BTVerifyCode.STATUS_VALID select c;
            if (lst.Count() > 0)
            {
                var result = lst.First();
                result.Status = BTVerifyCode.STATUS_INVALID;
                dbContext.BTVerifyCode.Update(result);
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


        public async Task<bool> SendVerifyCodeAsync(BTVerifyCode code, object info)
        {
            switch (code.ReceiverType)
            {
                case BTVerifyCode.REC_TYPE_EMAIL: return await SendVerifyCodeByAliEmailAsync(code, info);
                case BTVerifyCode.REC_TYPE_MOBILE: return await SendVerifyCodeByMobileAsync(code, info);
                default: return false;
            }
        }

        private async Task<bool> SendVerifyCodeByMobileAsync(BTVerifyCode code, object info)
        {
            throw new NotImplementedException();
        }

        private async Task<bool> SendVerifyCodeByAliEmailAsync(BTVerifyCode code, object info)
        {
            var mail = new AliApilUtils.AliMail
            {
                Action = "SingleSendMail",
                AccountName = "verification-code@btbase.mobi",
                ReplyToAddress = false,
                AddressType = AliApilUtils.AliMail.ADDR_TYPE_ACCOUNT,
                ToAddress = code.Receiver,
                FromAlias = "no-replay",
                Subject = "Verify Code",
                HtmlBody = "<p></p>",
                ClickTrace = AliApilUtils.AliMail.CLICK_TRACE_OFF
            };
            var aliReqfields = (AliApilUtils.CommonReqFields)info;
            return await AliApilUtils.SendMailAsync(aliReqfields, mail);
        }
    }
}