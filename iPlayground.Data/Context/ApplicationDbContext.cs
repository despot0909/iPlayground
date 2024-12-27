// ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using iPlayground.Core.Models;
using iPlayground.Core.Models.Base;
using System;

namespace iPlayground.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Child> Children { get; set; }
        public DbSet<Parent> Parents { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<MonthlyPass> MonthlyPasses { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<SyncLog> SyncLogs { get; set; }
        public DbSet<SessionVoucher> SessionVouchers { get; set; }
        public DbSet<VoucherValidation> VoucherValidations { get; set; }


        // Dodajemo konstruktor bez parametara za design-time
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Dodajemo konfiguraciju za design-time
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=iPlayground.db");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.HasIndex(e => e.FiscalNumber).IsUnique();
                entity.HasIndex(e => e.FiscalQRCode).IsUnique();

                entity.Property(e => e.OriginalAmount).HasPrecision(18, 2);

                entity.HasOne(v => v.Session)
                    .WithMany()
                    .HasForeignKey(v => v.SessionId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Configure Child entity
            modelBuilder.Entity<Child>()
                .HasOne(c => c.Parent)
                .WithMany(p => p.Children)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Session entity
            modelBuilder.Entity<Session>()
                .HasOne(s => s.Child)
                .WithMany()
                .HasForeignKey(s => s.ChildId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure decimal properties
            modelBuilder.Entity<Session>()
                .Property(s => s.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MonthlyPass>()
                .Property(m => m.AmountPaid)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Voucher>()
                .Property(v => v.OriginalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SessionVoucher>(entity =>
            {
                entity.HasIndex(e => e.FiscalNumber).IsUnique();
                entity.HasIndex(e => e.QRCode).IsUnique();

                entity.Property(e => e.OriginalAmount).HasPrecision(18, 2);
                entity.Property(e => e.DiscountAmount).HasPrecision(18, 2);
            });

            modelBuilder.Entity<VoucherValidation>(entity =>
            {
                entity.HasIndex(e => e.FiscalNumber);
                entity.HasIndex(e => e.QRCode);

                entity.Property(e => e.Amount).HasPrecision(18, 2);
            });


            // Add indexes
            modelBuilder.Entity<Parent>()
                .HasIndex(p => p.Phone);

            modelBuilder.Entity<Session>()
                .HasIndex(s => s.StartTime);

            modelBuilder.Entity<Session>()
       .HasMany(s => s.Vouchers)
       .WithOne(v => v.Session)
       .HasForeignKey(v => v.SessionId)
       .OnDelete(DeleteBehavior.Cascade);
        }

        public override int SaveChanges()
        {
            UpdateTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added ||
                    e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (BaseEntity)entry.Entity;

                if (entry.State == EntityState.Added)
                {
                    entity.CreatedAt = DateTime.UtcNow;
                }

                entity.UpdatedAt = DateTime.UtcNow;
            }
        }
    }
}