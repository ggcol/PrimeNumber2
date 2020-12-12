using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Microsoft.EntityFrameworkCore;
using PrimeNumber2.Models;

namespace PrimeNumber2
{
    class PrimeNumberContext : DbContext
    {
        public virtual DbSet<Models.PrimeNumber> PrimeNumbers { get; set; }

        public PrimeNumberContext() { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Data Source=ABUWIN-MK-IV2;Initial Catalog=abuNumbers;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PrimeNumber>(entity =>
            {
                entity.HasKey(e => e.NTH);

                entity.Property(e => e.IDN)
                .ValueGeneratedNever()
                .HasColumnName("IDN")
                .HasColumnType("bigint");

                entity.Property(e => e.NTH)
                .ValueGeneratedOnAdd()                
                .HasColumnName("NTH")
                .HasColumnType("bigint");
            });
        }



    }

    
}
