﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using iPlayground.Data.Context;

#nullable disable

namespace iPlayground.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241227195447_AddStornoFields")]
    partial class AddStornoFields
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.20");

            modelBuilder.Entity("iPlayground.Core.Models.Child", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ParentId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ParentId");

                    b.ToTable("Children");
                });

            modelBuilder.Entity("iPlayground.Core.Models.MonthlyPass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("AmountPaid")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<int>("ChildId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("QrCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChildId");

                    b.ToTable("MonthlyPasses");
                });

            modelBuilder.Entity("iPlayground.Core.Models.Parent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ContactInfo")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Phone");

                    b.ToTable("Parents");
                });

            modelBuilder.Entity("iPlayground.Core.Models.Session", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CanPause")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ChildId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<bool>("HasLoss")
                        .HasColumnType("INTEGER");

                    b.Property<string>("InvoiceNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPauseOverdue")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsPaused")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsStorno")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSynced")
                        .HasColumnType("INTEGER");

                    b.Property<decimal?>("LossAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("PauseButtonText")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("PauseStartTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("SessionStatus")
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<bool>("ShowEndButton")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("StornoReason")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("StornoTime")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("TotalVaucer")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("ChildId");

                    b.HasIndex("StartTime");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("iPlayground.Core.Models.SessionVoucher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("DiscountAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<string>("FiscalNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsValid")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("OriginalAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<string>("QRCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ScanTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("SessionId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FiscalNumber")
                        .IsUnique();

                    b.HasIndex("QRCode")
                        .IsUnique();

                    b.HasIndex("SessionId");

                    b.ToTable("SessionVouchers");
                });

            modelBuilder.Entity("iPlayground.Core.Models.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("iPlayground.Core.Models.SyncLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("ErrorMessage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("SyncTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("SyncType")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("SyncLogs");
                });

            modelBuilder.Entity("iPlayground.Core.Models.Voucher", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("FiscalNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FiscalQRCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsUsed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsValid")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("IssueDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("JIB")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<decimal>("OriginalAmount")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<int?>("SessionId")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UsedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ValidationDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("ValidationMessage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FiscalNumber")
                        .IsUnique();

                    b.HasIndex("FiscalQRCode")
                        .IsUnique();

                    b.HasIndex("SessionId");

                    b.ToTable("Vouchers");
                });

            modelBuilder.Entity("iPlayground.Core.Models.VoucherValidation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("ErrorMessage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FiscalNumber")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsValid")
                        .HasColumnType("INTEGER");

                    b.Property<string>("JIB")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("QRCode")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ValidationTime")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FiscalNumber");

                    b.HasIndex("QRCode");

                    b.ToTable("VoucherValidations");
                });

            modelBuilder.Entity("iPlayground.Core.Models.Child", b =>
                {
                    b.HasOne("iPlayground.Core.Models.Parent", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("iPlayground.Core.Models.MonthlyPass", b =>
                {
                    b.HasOne("iPlayground.Core.Models.Child", "Child")
                        .WithMany()
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Child");
                });

            modelBuilder.Entity("iPlayground.Core.Models.Session", b =>
                {
                    b.HasOne("iPlayground.Core.Models.Child", "Child")
                        .WithMany()
                        .HasForeignKey("ChildId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Child");
                });

            modelBuilder.Entity("iPlayground.Core.Models.SessionVoucher", b =>
                {
                    b.HasOne("iPlayground.Core.Models.Session", "Session")
                        .WithMany("Vouchers")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Session");
                });

            modelBuilder.Entity("iPlayground.Core.Models.Voucher", b =>
                {
                    b.HasOne("iPlayground.Core.Models.Session", "Session")
                        .WithMany()
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Session");
                });

            modelBuilder.Entity("iPlayground.Core.Models.Parent", b =>
                {
                    b.Navigation("Children");
                });

            modelBuilder.Entity("iPlayground.Core.Models.Session", b =>
                {
                    b.Navigation("Vouchers");
                });
#pragma warning restore 612, 618
        }
    }
}
