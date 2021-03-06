namespace BTBaseServices.Models
{
    public partial class BTDeviceSession
    {
        public long ID { get; set; }
        public string AccountId { get; set; }
        public string DeviceId { get; set; }
        public double LoginDateTs { get; set; }
        public double ReactiveDateTs { get; set; }
        public string DeviceName { get; set; }
        public string SessionKey { get; set; }
        public bool IsValid { get; set; }
    }

    public partial class BTDeviceSession
    {
        public static void OnDbContextModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BTDeviceSession>(ac =>
            {
                ac.HasKey(e => e.ID);
                ac.Property(e => e.AccountId).HasMaxLength(32).IsRequired();
                ac.Property(e => e.DeviceId).HasMaxLength(512).IsRequired();
                ac.Property(e => e.LoginDateTs);
                ac.Property(e => e.ReactiveDateTs);
                ac.Property(e => e.DeviceName);
                ac.Property(e => e.SessionKey).HasMaxLength(512).IsRequired();
                ac.HasIndex(e => new { e.AccountId, e.DeviceId, e.SessionKey });
            });
        }
    }
}