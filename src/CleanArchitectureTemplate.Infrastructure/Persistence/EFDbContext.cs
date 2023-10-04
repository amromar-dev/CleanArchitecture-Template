using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CleanArchitectureTemplate.Infrastructure.Persistence
{
	public class EFDbContext : DbContext
    {

		public EFDbContext(DbContextOptions<EFDbContext> options) : base(options)
		{

		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(modelBuilder);
		}
	}
}
