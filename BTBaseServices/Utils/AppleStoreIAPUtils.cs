using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class AppleStoreIAPUtils
{
    public class VerifyReceiptResult
    {
        public int ResponseCode { get; set; }
        public string MatchedProductId { get; set; }
        public string ReceiptData { get; set; }
        public string TransactionId { get; set; }
        public bool IsSandBox { get; set; }
    }

    public static async Task<VerifyReceiptResult> VerifyReceiptAppStoreAsync(string productId, string receiptData)
    {
        var res = await SendReceiptAsync(productId, receiptData, false);
        if (res.ResponseCode != 200)
        {
            if (res.ResponseCode == 21007)
            {
                return await SendReceiptAsync(productId, receiptData, true);
            }
            else if (res.ResponseCode == 21008)
            {
                return await SendReceiptAsync(productId, receiptData, false);
            }
        }
        return res;
    }

    private static async Task<VerifyReceiptResult> SendReceiptAsync(string productId, string receiptData, bool sandbox)
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

                return new VerifyReceiptResult
                {
                    ResponseCode = 200,
                    MatchedProductId = receiptProductId,
                    TransactionId = transactionId,
                    ReceiptData = receiptData,
                    IsSandBox = sandbox
                };
            }
            else
            {
                return new VerifyReceiptResult
                {
                    ResponseCode = statusCode,
                    MatchedProductId = null,
                    TransactionId = null,
                    ReceiptData = receiptData,
                    IsSandBox = sandbox
                };
            }

        }
    }
}