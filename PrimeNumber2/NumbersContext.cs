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
        public virtual DbSet<Number> Numbers { get; set; }

        public NumbersContext() { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(@"Data Source=ABUWIN-MK-IV2;Initial Catalog=abuNumbers;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Number>(entity =>
            {
                entity.HasKey(e => e.Position);

                entity.Property(e => e.IDN)
                .ValueGeneratedNever()
                .HasColumnName("IDN")
                .HasColumnType("bigint");

                entity.Property(e => e.Position)
                .ValueGeneratedOnAdd()
                .HasColumnName("Position")
                .HasColumnType("Guid");

                entity.Property(e => e.NTH)
                .ValueGeneratedOnAdd()
                .HasColumnName("NTH")
                .HasColumnType("bigint");
            });
        }



    }

    
}
