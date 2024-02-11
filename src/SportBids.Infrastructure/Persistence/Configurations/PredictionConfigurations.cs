using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportBids.Domain.Entities;

namespace SportBids.Infrastructure.Persistence.Configurations;

public class PredictionConfigurations : IEntityTypeConfiguration<Prediction>
{
    public void Configure(EntityTypeBuilder<Prediction> builder)
    {
        // builder
        //     .OwnsOne(prediction => prediction.Score);

        builder
            .HasKey(prediction => new { prediction.MatchId, prediction.OwnerId });
    }
}
