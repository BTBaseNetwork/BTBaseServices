using System;
using System.Linq;
using BahamutCommon.Utils;
using BTBaseServices.DAL;
using BTBaseServices.Models;

public class AppleStoreIAPManager
{
    public static bool IsAppleStoreOrderExists(BTBaseDbContext dbContext, AppleStoreIAPOrder order)
    {
        return dbContext.AppleStoreIAPOrder.Count(x => x.OrderKey == order.OrderKey) > 0;
    }

    public static AppleStoreIAPOrder AddAppleStoreOrder(BTBaseDbContext dbContext, AppleStoreIAPOrder order)
    {
        return dbContext.AppleStoreIAPOrder.Add(order).Entity;
    }
}