using System;

namespace BTBaseServices.Models
{
    public partial class UpdatePasswordRecord
    {
        public long ID { get; set; }
        public string AccountId { get; set; }
        public string Password { get; set; }
        public string PswHash { get; set; }
        public DateTime ExpiredAt { get; set; }
    }

    public partial class UpdatePasswordRecord
    {
        public static void OnDbContextModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UpdatePasswordRecord>(ac =>
            {
                ac.HasKey(e => e.ID);
                ac.Property(e => e.AccountId).HasMaxLength(32).IsRequired();
                ac.Property(e => e.Password).HasMaxLength(512).IsRequired();
                ac.Property(e => e.PswHash);
                ac.Property(e => e.ExpiredAt);
            });
        }
    }
}