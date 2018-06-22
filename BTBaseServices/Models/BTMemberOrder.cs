using System;
using BahamutCommon.Utils;
namespace BTBaseServices.Models
{
    public partial class BTMemberOrder
    {
        public const string PAYMENT_TYPE_APPLE_IAP = "AppleStoreIAP";
        public const string PAYMENT_TYPE_BTOKEN_MONTY = "BTokenMoney";

        public long ID { get; set; }
        public string AccountId { get; set; }
        public string PaymentType { get; set; }
        public int MemberType { get; set; }
        public double ChargeTimes { get; set; }
        public double OrderDateTs { get; set; }
        public DateTime ChargedExpiredDateTime { get; set; }
    }

    public partial class BTMemberOrder
    {
        public static void OnDbContextModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BTMemberOrder>(ac =>
            {
                ac.HasKey(e => e.ID);
                ac.Property(e => e.AccountId).HasMaxLength(32).IsRequired();
                ac.Property(e => e.OrderDateTs);
                ac.Property(e => e.PaymentType).HasMaxLength(45);
                ac.Property(e => e.MemberType);
                ac.Property(e => e.ChargeTimes);
                ac.Property(e => e.ChargedExpiredDateTime);
                ac.HasIndex(e => e.AccountId);
            });
        }
    }
}