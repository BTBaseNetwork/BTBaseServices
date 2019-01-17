using System;
using System.Collections.Generic;
using System.Linq;
using BahamutCommon.Utils;
using BTBaseServices.DAL;
using BTBaseServices.Models;
using Microsoft.EntityFrameworkCore;

namespace BTBaseServices.Services
{
    public class UserAssetService
    {
        public BTUserAsset AddAssets(BTBaseDbContext dbContext, BTUserAsset newAssets)
        {
            var res = dbContext.BTUserAsset.Add(newAssets);
            dbContext.SaveChanges();
            return res.Entity;
        }

        public BTUserAsset UpdateAssets(BTBaseDbContext dbContext, BTUserAsset modifiedAssets)
        {
            try
            {
                var model = dbContext.BTUserAsset.Single(x => x.Id == modifiedAssets.Id && x.AssetsId == modifiedAssets.AssetsId && x.AccountId == modifiedAssets.AccountId && x.AppBundleId == modifiedAssets.AppBundleId);
                model.Amount = modifiedAssets.Amount;
                model.Assets = modifiedAssets.Assets;
                model.ModifiedDate = DateTime.UtcNow;
                var res = dbContext.BTUserAsset.Update(model);
                dbContext.SaveChanges();
                return res.Entity;
            }
            catch (System.Exception ex)
            {
#if DEBUG
                Console.WriteLine(ex.Message);
#endif
                return null;
            }
        }

        public IEnumerable<BTUserAsset> GetAssets(BTBaseDbContext dbContext, string accountId, string appBundleId)
        {
            var res = from a in dbContext.BTUserAsset where a.AccountId == accountId && a.AppBundleId == appBundleId select a;
            return res;
        }

        public IEnumerable<BTUserAsset> GetAssets(BTBaseDbContext dbContext, string accountId, string appBundleId, string assetsId)
        {
            var res = from x in dbContext.BTUserAsset where x.AssetsId == assetsId && x.AccountId == accountId && x.AppBundleId == appBundleId select x;
            return res;
        }

        public IEnumerable<BTUserAsset> GetAssetsByCategory(BTBaseDbContext dbContext, string accountId, string appBundleId, string category)
        {
            var res = from a in dbContext.BTUserAsset where a.AccountId == accountId && a.AppBundleId == appBundleId && a.Category == category select a;
            return res;
        }
    }
}