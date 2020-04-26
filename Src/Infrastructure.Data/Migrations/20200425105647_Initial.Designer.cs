﻿// <auto-generated />
using System;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(PolDbContext))]
    [Migration("20200425105647_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Entities.CustomerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("CustomerEntities");
                });

            modelBuilder.Entity("Domain.Entities.FlowStepEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CustomerEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("From")
                        .HasColumnType("int");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerEntityId");

                    b.ToTable("FlowStepEntity");
                });

            modelBuilder.Entity("Domain.Entities.FlowStepUserPermissionEntity", b =>
                {
                    b.Property<Guid>("FlowStepId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("FlowStepId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("FlowStepUserPermissionEntity");
                });

            modelBuilder.Entity("Domain.Entities.TravelExpenseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CustomerEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAssignedPayment")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCertified")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReportedDone")
                        .HasColumnType("bit");

                    b.Property<Guid?>("OwnedByUserEntityId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Stage")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerEntityId");

                    b.HasIndex("OwnedByUserEntityId");

                    b.ToTable("TravelExpenses");
                });

            modelBuilder.Entity("Domain.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.ToTable("UserEntity");
                });

            modelBuilder.Entity("Domain.Entities.FlowStepEntity", b =>
                {
                    b.HasOne("Domain.Entities.CustomerEntity", null)
                        .WithMany("FlowSteps")
                        .HasForeignKey("CustomerEntityId");
                });

            modelBuilder.Entity("Domain.Entities.FlowStepUserPermissionEntity", b =>
                {
                    b.HasOne("Domain.Entities.FlowStepEntity", "FlowStep")
                        .WithMany("FlowStepUserPermissions")
                        .HasForeignKey("FlowStepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.UserEntity", "User")
                        .WithMany("FlowStepUserPermissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.TravelExpenseEntity", b =>
                {
                    b.HasOne("Domain.Entities.CustomerEntity", null)
                        .WithMany("TravelExpenses")
                        .HasForeignKey("CustomerEntityId");

                    b.HasOne("Domain.Entities.UserEntity", "OwnedByUserEntity")
                        .WithMany()
                        .HasForeignKey("OwnedByUserEntityId");
                });

            modelBuilder.Entity("Domain.Entities.UserEntity", b =>
                {
                    b.HasOne("Domain.Entities.CustomerEntity", "Customer")
                        .WithMany("Users")
                        .HasForeignKey("CustomerId");
                });
#pragma warning restore 612, 618
        }
    }
}