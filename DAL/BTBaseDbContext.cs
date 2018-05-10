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

        public virtual DbSet<BTWebServerAuthKey> BTWebServerAuthKey { get; set; }

        public BTBaseDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            BTBaseServices.Models.BTAccount.OnDbContextModelCreating(modelBuilder);
            BTBaseServices.Models.BTDeviceSession.OnDbContextModelCreating(modelBuilder);
            BTBaseServices.Models.BTMember.OnDbContextModelCreating(modelBuilder);
            BTBaseServices.Models.BTMemberOrder.OnDbContextModelCreating(modelBuilder);
            BTBaseServices.Models.BTWebServerAuthKey.OnDbContextModelCreating(modelBuilder);
        }
    }
}