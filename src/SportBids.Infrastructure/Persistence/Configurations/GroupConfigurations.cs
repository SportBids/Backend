using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportBids.Domain;

namespace SportBids.Infrastructure;

public class GroupConfigurations : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        builder
            .Property(group => group.Name)
            .HasMaxLength(1);
    }
}
