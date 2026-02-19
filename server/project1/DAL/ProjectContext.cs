using Microsoft.EntityFrameworkCore;
using project1.Models;
using System.Collections.Generic;

namespace project1.DAL
{
    public class ProjectContext : DbContext
    {
        public DbSet<Gift> Gifts { get; set; }
        public DbSet<Donor> Donors { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Category> Categories { get; set; }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =====================
            // ======= GIFT ========
            // =====================
            modelBuilder.Entity<Gift>(entity =>
            {
                entity.ToTable("Gifts");

                entity.HasKey(g => g.Id);

                entity.Property(g => g.Name).IsRequired().HasMaxLength(50);
                entity.HasIndex(g => g.Name).IsUnique();

                entity.Property(g => g.Description).IsRequired();

                entity.Property(g => g.Picture).IsRequired();

                entity.Property(g => g.Price).IsRequired();

                // ==== Relations ====
                entity.HasOne(g => g.Donor)
                      .WithMany(d => d.Gifts)
                      .HasForeignKey(g => g.DonorId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(g => g.Category)
                      .WithMany()
                      .HasForeignKey(g => g.CategoryId)
                      .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(g => g.Winner)
                      .WithMany()
                      .HasForeignKey(g => g.WinnerId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // =====================
            // ======= DONOR =======
            // =====================
            modelBuilder.Entity<Donor>(entity =>
            {
                entity.ToTable("Donors");

                entity.HasKey(d => d.Id);

                entity.Property(d => d.IdentityNumber).IsRequired().HasMaxLength(9);

                entity.HasIndex(d => d.IdentityNumber).IsUnique();

                entity.Property(d => d.Name).IsRequired().HasMaxLength(50);

                entity.Property(d => d.Phone).IsRequired();

                entity.Property(d => d.Email).IsRequired();

                // One donor → many gifts
                entity.HasMany(d => d.Gifts).WithOne(g => g.Donor).HasForeignKey(g => g.DonorId).OnDelete(DeleteBehavior.Restrict);
            });

            // =====================
            // ======= USER ========
            // =====================
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");

                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name).IsRequired().HasMaxLength(50);
                entity.HasIndex(u => u.Name).IsUnique();

                entity.Property(u => u.Phone).IsRequired();

                entity.Property(u => u.Email).IsRequired();
                entity.HasIndex(u => u.Email).IsUnique();

                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(100);

                entity.Property(u => u.Role).HasDefaultValue("user");
            });

            // =====================
            // ======= CART ========
            // =====================
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.ToTable("Carts");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Quantity).IsRequired().HasDefaultValue(1);

                entity.Property(c => c.IsPurchased).HasDefaultValue(false);

                entity.HasOne(c => c.User).WithMany(u => u.Carts).HasForeignKey(c => c.UserID).OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Gift).WithMany(g => g.Carts).HasForeignKey(c => c.GiftID).OnDelete(DeleteBehavior.Cascade); // Restrict ???
            });

            // =====================
            // ===== CATEGORY ======
            // =====================
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Categories");

                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name).IsRequired().HasMaxLength(50);
                entity.HasIndex(c => c.Name).IsUnique();
            });
        }
    }
}