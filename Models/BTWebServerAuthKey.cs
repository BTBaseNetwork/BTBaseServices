namespace BTBaseServices.Models
{
    public partial class BTWebServerAuthKey
    {
        public long ID { get; set; }
        public string ServerName { get; set; }
        public string Algorithm { get; set; }
        public double CreateDateTs { get; set; }
        public string Key { get; set; }
    }

    public partial class BTWebServerAuthKey
    {
        public static void OnDbContextModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BTWebServerAuthKey>(ac =>
            {
                ac.HasKey(e => e.ID);
                ac.Property(e => e.ServerName).HasMaxLength(128);
                ac.Property(e => e.Algorithm).HasMaxLength(64).IsRequired();
                ac.Property(e => e.CreateDateTs);
                ac.Property(e => e.Key);
                ac.HasIndex(e => e.ServerName);
            });
        }
    }
}