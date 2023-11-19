using Catalog.Database.Entities;
using Catalog.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Database
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

        public DbSet<Item> Items { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ReservedItem> ReservedItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Brand>()
                .HasMany(brand => brand.Items)
                .WithOne(item => item.Brand)
                .HasForeignKey(item => item.BrandId);

            modelBuilder.Entity<Category>()
                .HasMany(category => category.Items)
                .WithOne(item => item.Category)
                .HasForeignKey(item => item.CategoryId);

            modelBuilder.Entity<Category>()
                .HasMany(category => category.Brands)
                .WithOne(brand => brand.Category)
                .HasForeignKey(brand => brand.CategoryId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
