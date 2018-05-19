using System;

namespace BTBaseServices.Models
{
    public partial class BTVerifyCode
    {
        public long ID { get; set; }
        public string AccountId { get; set; }
        public int Status { get; set; }
        public int RequestFor { get; set; }
        public int ReceiverType { get; set; }
        public string Receiver { get; set; }
        public string Code { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime ExpiredAt { get; set; }
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
                ac.Property(e => e.ReceiverType);
                ac.Property(e => e.Receiver).HasMaxLength(128);
                ac.Property(e => e.Code).HasMaxLength(32);
                ac.Property(e => e.RequestDate);
                ac.Property(e => e.ExpiredAt);
            });
        }
    }

    public partial class BTVerifyCode
    {
        public const int REC_TYPE_UNKNOW = 0;
        public const int REC_TYPE_MOBILE = 1;
        public const int REC_TYPE_EMAIL = 2;
        public const int TYPE_EMAIL = 2;
        public const int STATUS_INVALID = 0;
        public const int STATUS_VALID = 1;
        public const int REQ_FOR_NOTHING = 0;
        public const int REQ_FOR_RESET_PASSWORD = 1;
        public const int REQ_FOR_RESET_EMAIL = 1;
    }
}