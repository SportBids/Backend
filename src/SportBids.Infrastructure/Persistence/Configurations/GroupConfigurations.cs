using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportBids.Domain.Entities;

namespace SportBids.Infrastructure.Persistence.Configurations;

public class GroupConfigurations : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder
            .Property(group => group.Name)
            .HasMaxLength(1);
    }
}
