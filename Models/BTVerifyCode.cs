using System;

namespace BTBaseServices.Models
{
    public partial class BTVerifyCode
    {
        public long ID { get; set; }
        public string AccountId { get; set; }
        public int Status { get; set; }
        public int RequestFor { get; set; }
        public int UpdateId { get; set; }
        public string Code { get; set; }
        public DateTime RequestDate { get; set; }
    }

    public partial class BTVerifyCode
    {
        public static void OnDbContextModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BTVerifyCode>(ac =>
            {
                ac.HasKey(e => e.ID);
                ac.Property(e => e.AccountId).HasMaxLength(32).IsRequired();
                ac.Property(e => e.Status);
                ac.Property(e => e.RequestFor);
                ac.Property(e => e.UpdateId);
                ac.Property(e => e.Code);
                ac.Property(e => e.RequestDate);
            });
        }
    }
}