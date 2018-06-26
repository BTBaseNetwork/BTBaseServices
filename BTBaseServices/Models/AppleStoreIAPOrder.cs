using System;
using BahamutCommon.Utils;
namespace BTBaseServices.Models
{
    public partial class AppleStoreIAPOrder
    {
        public long ID { get; set; }
        public string OrderKey { get; set; }
        public string AccountId { get; set; }
        public string ProductId { get; set; }
        public string ReceiptData { get; set; }
        public DateTime Date { get; set; }
        public bool SandBox { get; set; }
    }

    public partial class AppleStoreIAPOrder
    {
        public static void OnDbContextModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppleStoreIAPOrder>(ac =>
            {
                ac.HasKey(e => e.ID);
                ac.Property(e => e.OrderKey).HasMaxLength(512);
                ac.Property(e => e.AccountId).HasMaxLength(32).IsRequired();
                ac.Property(e => e.ProductId);
                ac.Property(e => e.ReceiptData);
                ac.Property(e => e.Date);
                ac.Property(e => e.SandBox);
            });
        }
    }
}