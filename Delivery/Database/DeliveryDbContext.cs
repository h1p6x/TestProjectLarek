using Delivery.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Delivery.Database
{
	public class DeliveryDbContext : DbContext
	{
		public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options) : base(options) { }
		public DbSet<DeliveryItems> DeliveryItems { get; set; }
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}
