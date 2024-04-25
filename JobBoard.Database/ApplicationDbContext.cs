﻿using JobBoard.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Database
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<CompanyAccount> CompanyAccounts { get; set; }

        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CompanyAccount>()
                .Property(e => e.Name).HasMaxLength(200);
        }
    }
}
