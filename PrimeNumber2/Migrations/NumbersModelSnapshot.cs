﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PrimeNumber2;

namespace PrimeNumber2.Migrations
{
    [DbContext(typeof(NumbersContext))]
    partial class NumbersModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("PrimeNumber2.Models.PrimeNumber", b =>
                {
                    b.Property<long>("IDN")
                        .HasColumnType("bigint")
                        .HasColumnName("IDN");

                    b.HasKey("IDN");

                    b.ToTable("PrimeNumbers");
                });
#pragma warning restore 612, 618
        }
    }
}