﻿// <auto-generated />
using ConsoleDevice.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ConsoleDevice.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231014225448_Testar")]
    partial class Testar
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("ConsoleDevice.Contexts.ConfigurationEntity", b =>
                {
                    b.Property<string>("DeviceId")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConnectionString")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceType")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .HasColumnType("TEXT");

                    b.HasKey("DeviceId");

                    b.ToTable("Configuration");
                });
#pragma warning restore 612, 618
        }
    }
}