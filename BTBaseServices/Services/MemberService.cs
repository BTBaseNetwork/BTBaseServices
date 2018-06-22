using System.Linq;
using System.Data;
using System;
using BahamutCommon.Utils;
using BTBaseServices.DAL;
using BTBaseServices.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BTBaseServices.Services
{
    public partial class MemberService
    {
        public BTMemberProfile GetProfile(BTBaseDbContext dbContext, string accountId)
        {
            var list = from u in dbContext.BTMember where u.AccountId == accountId select u;
            var profile = new BTMemberProfile
            {
                AccountId = accountId,
                Members = list.ToArray()
            };
            return profile;
        }

        public bool RechargeMember(BTBaseDbContext dbContext, BTMemberOrder order)
        {
            var member = dbContext.BTMember.FirstOrDefault(u => u.AccountId == order.AccountId && u.MemberType == order.MemberType);
            ;
            var now = DateTimeUtil.UnixTimeSpanSec;
            if (member == null)
            {
                member = new BTMember
                {
                    AccountId = order.AccountId,
                    FirstChargeDateTs = now,
                    MemberType = order.MemberType,
                    ExpiredDateTs = now + order.ChargeTimes
                };
                dbContext.BTMember.Add(member);
            }
            else
            {
                member.ExpiredDateTs = Math.Max(member.ExpiredDateTs, now) + order.ChargeTimes;
                member.MemberType = order.MemberType;
                dbContext.BTMember.Update(member);
            }
            order.OrderDateTs = now;
            order.ChargedExpiredDateTime = DateTimeUtil.UnixTimeSpanZeroDate().Add(TimeSpan.FromSeconds(member.ExpiredDateTs));
            dbContext.BTMemberOrder.Add(order);
            return true;
        }
    }

    #region iTunes App Store
    /*
   public partial class MemberService
   {

       public async Task<ApiResult> VerifyReceiptAppStoreAsync(BTBaseDbContext dbContext, string accountId, string productId, string receiptData, bool sandbox)
       {
           var res = await SendReceiptAppStoreAsync(dbContext, accountId, productId, receiptData, sandbox);
           if (res.code != 200)
           {
               if (res.error.code == 21007)
               {
                   return await SendReceiptAppStoreAsync(dbContext, accountId, productId, receiptData, true);
               }
               else if (res.error.code == 21008)
               {
                   return await SendReceiptAppStoreAsync(dbContext, accountId, productId, receiptData, false);
               }
           }
           return res;
       }

       private async Task<ApiResult> SendReceiptAppStoreAsync(BTBaseDbContext dbContext, string accountId, string productId, string receiptData, bool sandbox)
       {
           //购买凭证验证地址  
           const string certificateUrl = "https://buy.itunes.apple.com/verifyReceipt";
           //测试的购买凭证验证地址   
           const string certificateUrlTest = "https://sandbox.itunes.apple.com/verifyReceipt";

           var url = sandbox ? certificateUrlTest : certificateUrl;
           var postBody = new Dictionary<string, string>() { { "receipt-data", receiptData } };

           using (var client = new HttpClient())
           {
               var content = new StringContent(JsonConvert.SerializeObject(postBody, Formatting.None), System.Text.Encoding.UTF8, "application/json");
               var msg = await client.PostAsync(url, content);
               var result = await msg.Content.ReadAsStringAsync();
               var jsonResult = JsonConvert.DeserializeObject<JObject>(result);
               var statusCode = jsonResult.GetValue("status").Value<int>();
               if (statusCode == 0)
               {
                   string transactionId = null;
                   string receiptProductId = null;
                   var receipts = jsonResult["receipt"]["in_app"].HasValues ? jsonResult["receipt"]["in_app"] : jsonResult["receipt"];

                   foreach (var item in receipts.ToArray())
                   {
                       var product_id = item["product_id"].Value<string>();
                       if (product_id == productId)
                       {
                           transactionId = item["transaction_id"].Value<string>().ToString();
                           receiptProductId = product_id;
                       }
                   }


               }
               else
               {
                   return new ApiResult { code = 400, error = new ErrorResult { code = statusCode, msg = "App Store Error" } };
               }

           }
       }
   }
    */
    #endregion
}