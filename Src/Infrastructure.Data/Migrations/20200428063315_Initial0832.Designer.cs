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
    [Migration("20200428063315_Initial0832")]
    partial class Initial0832
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

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("Domain.Entities.CustomerUserPermissionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("UserStatus")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("UserId");

                    b.ToTable("CustomerUserPermissions");
                });

            modelBuilder.Entity("Domain.Entities.FlowStepEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("FromId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Key")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("FromId");

                    b.ToTable("FlowSteps");
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

                    b.ToTable("FlowStepUserPermissions");
                });

            modelBuilder.Entity("Domain.Entities.StageEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Value")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Stages");
                });

            modelBuilder.Entity("Domain.Entities.TravelExpenseEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAssignedPayment")
                        .HasColumnType("bit");

                    b.Property<bool>("IsCertified")
                        .HasColumnType("bit");

                    b.Property<bool>("IsReportedDone")
                        .HasColumnType("bit");

                    b.Property<Guid?>("OwnedByUserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Stage")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("OwnedByUserId");

                    b.ToTable("TravelExpenses");
                });

            modelBuilder.Entity("Domain.Entities.UserEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Entities.CustomerUserPermissionEntity", b =>
                {
                    b.HasOne("Domain.Entities.CustomerEntity", "Customer")
                        .WithMany("CustomerUserPermissions")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Entities.FlowStepEntity", b =>
                {
                    b.HasOne("Domain.Entities.CustomerEntity", "Customer")
                        .WithMany("FlowSteps")
                        .HasForeignKey("CustomerId");

                    b.HasOne("Domain.Entities.StageEntity", "From")
                        .WithMany()
                        .HasForeignKey("FromId");
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
                    b.HasOne("Domain.Entities.CustomerEntity", "Customer")
                        .WithMany("TravelExpenses")
                        .HasForeignKey("CustomerId");

                    b.HasOne("Domain.Entities.UserEntity", "OwnedByUser")
                        .WithMany()
                        .HasForeignKey("OwnedByUserId");
                });
#pragma warning restore 612, 618
        }
    }
}
