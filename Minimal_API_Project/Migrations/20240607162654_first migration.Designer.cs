﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Minimal_API_Project.Data;

#nullable disable

namespace Minimal_API_Project.Migrations
{
    [DbContext(typeof(MinimalApiContext))]
    [Migration("20240607162654_first migration")]
    partial class firstmigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Minimal_API_Project.Models.Errand", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DifficultyLevel")
                        .HasPrecision(1)
                        .HasColumnType("int");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Errands");
                });

            modelBuilder.Entity("Minimal_API_Project.Models.ErrandWorker", b =>
                {
                    b.Property<int>("WorkerId")
                        .HasColumnType("int");

                    b.Property<int>("ErrandId")
                        .HasColumnType("int");

                    b.HasKey("WorkerId", "ErrandId");

                    b.HasIndex("ErrandId");

                    b.ToTable("ErrandWorkers");
                });

            modelBuilder.Entity("Minimal_API_Project.Models.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("Age")
                        .HasPrecision(3)
                        .HasColumnType("int");

                    b.Property<DateOnly?>("HireDate")
                        .HasColumnType("date");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Workers");
                });

            modelBuilder.Entity("Minimal_API_Project.Models.ErrandWorker", b =>
                {
                    b.HasOne("Minimal_API_Project.Models.Errand", "Errand")
                        .WithMany("ErrandWorkers")
                        .HasForeignKey("ErrandId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Minimal_API_Project.Models.Worker", "Worker")
                        .WithMany("ErrandWorkers")
                        .HasForeignKey("WorkerId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Errand");

                    b.Navigation("Worker");
                });

            modelBuilder.Entity("Minimal_API_Project.Models.Errand", b =>
                {
                    b.Navigation("ErrandWorkers");
                });

            modelBuilder.Entity("Minimal_API_Project.Models.Worker", b =>
                {
                    b.Navigation("ErrandWorkers");
                });
#pragma warning restore 612, 618
        }
    }
}