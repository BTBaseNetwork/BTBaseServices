namespace BTBaseServices.Models
{
    public partial class BTWalletRecord
    {
        public const int TYPE_RECHARGE = 1;
        public const int TYPE_PAYOUT = 2;
        public const int TYPE_INCOME = 3;

        public long Id { get; set; }
        public long WalletId { get; set; }
        public string AccountId { get; set; }
        public long TokenMoney { get; set; }
        public int Type { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Note { get; set; }
    }

    public partial class BTWalletRecord
    {
        public static void OnDbContextModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BTWalletRecord>(ac =>
            {
                ac.HasKey(e => e.Id);
                ac.Property(e => e.WalletId);
                ac.Property(e => e.AccountId).HasMaxLength(32);
                ac.Property(e => e.TokenMoney);
                ac.Property(e => e.Type);
                ac.Property(e => e.Source);
                ac.Property(e => e.Destination);
                ac.Property(e => e.Note);
                ac.HasIndex(e => e.WalletId);
                ac.HasIndex(e => e.AccountId);
            });
        }
    }
}