using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.EntityFrameworkCore;
using PrimeNumber2.Models;

namespace PrimeNumber2
{
    class NumbersContext : DbContext
    {
        public virtual DbSet<PrimeNumber> PrimeNumbers { get; set; }

        public NumbersContext() { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Data Source=ABUWIN-MK-IV2;Initial Catalog=Numbers;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PrimeNumber>(entity =>
            {
                entity.HasKey(k => k.IDN);

                entity.Property(e => e.IDN)
                .ValueGeneratedNever()
                .HasColumnName("IDN")
                .HasColumnType("bigint");
            });
        }



    }

    
}
