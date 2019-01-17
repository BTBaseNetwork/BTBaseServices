using System;

namespace BTBaseServices.Models
{
    public partial class BTUserAsset
    {
        public long Id { get; set; }
        public string AccountId { get; set; }
        public string AppBundleId { get; set; }
        public string Category { get; set; } /// Define by app
        public string AssetsId { get; set; } /// Define by app
        public string Assets { get; set; } /// Define by app
        public int Amount { get; set; } /// Define by app
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }

    public partial class BTUserAsset
    {
        public static void OnDbContextModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BTUserAsset>(ac =>
            {
                ac.HasKey(e => e.Id);
                ac.Property(e => e.AccountId).HasMaxLength(32).IsRequired();
                ac.Property(e => e.AppBundleId).IsRequired();
                ac.Property(e => e.AssetsId).HasMaxLength(128);
                ac.Property(e => e.Category).HasMaxLength(64);
                ac.Property(e => e.Assets);
                ac.Property(e => e.Amount);
                ac.Property(e => e.CreateDate);
                ac.Property(e => e.ModifiedDate);
                ac.HasIndex(e => e.AccountId);
                ac.HasIndex(e => e.AppBundleId);
                ac.HasIndex(e => e.AssetsId);
            });
        }
    }
}