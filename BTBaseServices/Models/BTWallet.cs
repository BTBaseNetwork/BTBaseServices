namespace BTBaseServices.Models
{
    public partial class BTWallet
    {
        public long Id { get; set; }
        public string AccountId { get; set; }
        public long TokenMoney { get; set; }
    }

    public partial class BTWallet
    {
        public static void OnDbContextModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BTWallet>(ac =>
            {
                ac.HasKey(e => e.Id);
                ac.Property(e => e.AccountId).HasMaxLength(32);
                ac.Property(e => e.TokenMoney);
                ac.HasIndex(e => e.AccountId);
            });
        }
    }
}