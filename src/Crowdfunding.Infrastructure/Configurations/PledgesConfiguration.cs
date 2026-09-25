using Crowdfunding.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Crowdfunding.Infrastructure.Configurations;

public class PledgesConfiguration : IEntityTypeConfiguration<Pledge>
{
    public void Configure(EntityTypeBuilder<Pledge> builder)
    {
        builder.ToTable("Pledges");

        builder.Property(p => p.Amount).IsRequired();
        builder.Property(p => p.CreatedAt).IsRequired();

        builder.Property(p => p.Status)
            .HasConversion<string>()
            .IsRequired();
        
        builder.HasOne<Project>()
            .WithMany()
            .HasForeignKey(p => p.ProjectId);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(p => p.UserId);

        builder.HasOne<Reward>()
            .WithMany()
            .HasForeignKey(p => p.RewardId);
    }
}