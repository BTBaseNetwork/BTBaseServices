using System;
using System.Linq;
using BahamutCommon.Utils;
using BTBaseServices.DAL;
using BTBaseServices.Models;

namespace BTBaseServices.Services
{
    public class WalletService
    {

        public BTWallet GetWallet(BTBaseDbContext dbContext, string accountId)
        {
            var wallet = dbContext.BTWallet.FirstOrDefault(x => x.AccountId == accountId);
            if (wallet == null)
            {
                return new BTWallet
                {
                    AccountId = accountId,
                    TokenMoney = 0
                };
            }
            else
            {
                return wallet;
            }
        }

        public bool Payout(BTBaseDbContext dbContext, BTWalletRecord record)
        {
            return false;
        }

        public bool RechargeTokenMoneyByAppleIAP(BTBaseDbContext dbContext, AppleStoreIAPOrder order)
        {
            var tokenMoney = ParseIAPProductIdToTokenMoney(order.ProductId);
            if (tokenMoney > 0)
            {
                var wallet = dbContext.BTWallet.FirstOrDefault(x => x.AccountId == order.AccountId);
                if (wallet == null)
                {
                    wallet = dbContext.BTWallet.Add(new BTWallet
                    {
                        AccountId = order.AccountId,
                        TokenMoney = tokenMoney,
                    }).Entity;
                }
                else
                {
                    wallet.TokenMoney += tokenMoney;
                    dbContext.BTWallet.Update(wallet);
                }
                dbContext.BTWalletRecord.Add(new BTWalletRecord
                {
                    WalletId = wallet.Id,
                    AccountId = wallet.AccountId,
                    TokenMoney = tokenMoney,
                    Source = "AppleStoreIAPOrder",
                    Destination = "BTWallet",
                    Note = order.OrderKey
                });
                return true;
            }
            return false;
        }

        public static int ParseIAPProductIdToTokenMoney(string iapProductId)
        {
            if (CommonRegexTestUtil.TestPattern(iapProductId, @"^[0-9a-zA-Z-_.]+\.iap.btoken\.[0-9]+$"))
            {
                var moneyStr = System.Text.RegularExpressions.Regex.Replace(iapProductId, @"[0-9a-zA-Z-_.]+\.iap.btoken\.", "");
                int res = 0;
                if (int.TryParse(moneyStr, out res))
                {
                    return res;
                }
            }
            return -1;
        }
    }
}