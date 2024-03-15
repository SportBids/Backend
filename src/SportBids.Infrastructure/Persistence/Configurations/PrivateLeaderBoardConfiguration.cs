using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportBids.Domain.Entities;

namespace SportBids.Infrastructure.Persistence.Configurations;

public class PrivateLeaderBoardConfiguration : IEntityTypeConfiguration<PrivateLeaderBoard>
{
    public void Configure(EntityTypeBuilder<PrivateLeaderBoard> builder)
    {
        builder
            .HasKey(x => x.Id);
        
        builder
            .HasIndex(x => x.JoinCode)
            .IsUnique();
    }
}
