using System;
using Microsoft.EntityFrameworkCore;
using BTBaseServices.Models;

namespace BTBaseServices.DAL
{
    public class BTBaseDbContext : DbContext
    {
        public virtual DbSet<BTAccount> BTAccount { get; set; }
        public virtual DbSet<BTDeviceSession> BTDeviceSession { get; set; }
        public virtual DbSet<BTMember> BTMember { get; set; }
        public virtual DbSet<BTMemberOrder> BTMemberOrder { get; set; }

        public virtual DbSet<SecurityKeychain> SecurityKeychain { get; set; }
        public virtual DbSet<UpdatePasswordRecord> UpdatePasswordRecord { get; set; }
        public virtual DbSet<UpdateEmailRecord> UpdateEmailRecord { get; set; }
        public virtual DbSet<BTSecurityCode> BTSecurityCode { get; set; }

        public BTBaseDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            BTBaseServices.Models.BTAccount.OnDbContextModelCreating(modelBuilder);
            BTBaseServices.Models.BTDeviceSession.OnDbContextModelCreating(modelBuilder);
            BTBaseServices.Models.BTMember.OnDbContextModelCreating(modelBuilder);
            BTBaseServices.Models.BTMemberOrder.OnDbContextModelCreating(modelBuilder);
            BTBaseServices.Models.SecurityKeychain.OnDbContextModelCreating(modelBuilder);
            BTBaseServices.Models.UpdatePasswordRecord.OnDbContextModelCreating(modelBuilder);
            BTBaseServices.Models.UpdateEmailRecord.OnDbContextModelCreating(modelBuilder);
            BTBaseServices.Models.BTSecurityCode.OnDbContextModelCreating(modelBuilder);
        }
    }
}