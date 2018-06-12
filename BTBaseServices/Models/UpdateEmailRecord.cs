using System;

namespace BTBaseServices.Models
{
    public partial class UpdateEmailRecord
    {
        public long ID { get; set; }
        public string AccountId { get; set; }
        public string ReplacedEmail { get; set; }
        public DateTime RequestDate { get; set; }
    }

    public partial class UpdateEmailRecord
    {
        public static void OnDbContextModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UpdateEmailRecord>(ac =>
            {
                ac.HasKey(e => e.ID);
                ac.Property(e => e.AccountId).HasMaxLength(32).IsRequired();
                ac.Property(e => e.ReplacedEmail).IsRequired();
                ac.Property(e => e.RequestDate);
            });
        }
    }
}