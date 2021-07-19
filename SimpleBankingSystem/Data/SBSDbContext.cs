using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleBankingSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleBankingSystem.Data
{
    public class SBSDbContext : IdentityDbContext<ApplicationUser>
    {
        public SBSDbContext(DbContextOptions<SBSDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<Address> Addresses { get; set; }

        public DbSet<BankAccount> BankAccounts { get; set; }

        public DbSet<Card> Cards { get; set; }

        public DbSet<News> News { get; set; }

        public DbSet<Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
            .HasOne(a => a.Address)
            .WithOne(a => a.User)
            .HasForeignKey<Address>(c => c.UserId);

            modelBuilder.Entity<Address>()
            .HasOne(x => x.User)
            .WithOne(x => x.Address)
            .HasForeignKey<ApplicationUser>(x => x.AddressId);

            modelBuilder.Entity<ApplicationUser>()
            .HasOne(a => a.BankAccount)
            .WithOne(a => a.User)
            .HasForeignKey<BankAccount>(c => c.UserId);

            modelBuilder.Entity<BankAccount>()
            .HasOne(a => a.User)
            .WithOne(a => a.BankAccount)
            .HasForeignKey<ApplicationUser>(c => c.BankAccountId);
        }

    }
}
