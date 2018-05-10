using System;

namespace BTBaseServices.Models
{
    public partial class SecurityKeychain
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Algorithm { get; set; }
        public DateTime CreateDate { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
        public string Note { get; set; }
    }

    public partial class SecurityKeychain
    {
        public static void OnDbContextModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SecurityKeychain>(ac =>
            {
                ac.HasKey(e => e.ID);
                ac.Property(e => e.Name).HasMaxLength(128);
                ac.Property(e => e.Algorithm).HasMaxLength(64).IsRequired();
                ac.Property(e => e.CreateDate);
                ac.Property(e => e.PublicKey);
                ac.Property(e => e.PrivateKey);
                ac.HasIndex(e => e.Note);
            });
        }
    }
}