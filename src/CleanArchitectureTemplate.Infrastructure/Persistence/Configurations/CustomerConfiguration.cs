using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CleanArchitectureTemplate.Domain.Customers;

namespace CleanArchitectureTemplate.Infrastructure.Persistence.Configurations
{
	public class CustomerEntityConfiguration : IEntityTypeConfiguration<Customer>
	{
		public void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.ToTable("Customers");
			builder.Property(e => e.Id).ValueGeneratedOnAdd();
			builder.Property(e => e.Email).IsRequired().HasMaxLength(100);
			builder.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
			builder.Property(e => e.LastName).IsRequired().HasMaxLength(50);
			builder.HasIndex(e => e.Email).IsUnique();
			builder.HasIndex(e => e.UserName).IsUnique().HasFilter($"{nameof(Customer.UserName)} IS NOT NULL");
			builder.HasIndex(e => e.ReferralCode).IsUnique().HasFilter($"{nameof(Customer.ReferralCode)} IS NOT NULL");
			builder.OwnsOne(e => e.EmailChangeRequest, x =>
			{
				x.Property(z => z.Email).HasColumnName("NewEmail");
				x.Property(z => z.VerificationCode).HasColumnName("NewEmailVerificationCode");
			});
			
			builder.Property(e => e.BirthDate).HasConversion(new ValueConverter<DateOnly, DateTime>(
				d => new DateTime(d.Year, d.Month, d.Day),
				dt => new DateOnly(dt.Year, dt.Month, dt.Day)
			));
		}
	}
}