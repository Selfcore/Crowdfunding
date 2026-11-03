using Crowdfunding.Domain.Entities;
using Crowdfunding.Domain.Value_Objects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crowdfunding.Infrastructure.Postgres.Configurations;

public class UsersConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        builder.Property(u => u.Name).HasMaxLength(256).IsRequired();
        builder.Property(u => u.Email)
            .HasConversion(email => email.Address, address => Email.Create(address))
            .IsRequired();

        builder.HasIndex(u => u.Email).IsUnique();
    }
}