using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportBids.Domain;

namespace SportBids.Infrastructure;

public class MatchConfigurations : IEntityTypeConfiguration<Match>
{
    public void Configure(EntityTypeBuilder<Match> builder)
    {
        // builder
        //     .OwnsOne(match => match.Score);
    }
}

// public class GroupMatchConfigurations : IEntityTypeConfiguration<GroupMatch>
// {
//     public void Configure(EntityTypeBuilder<GroupMatch> builder)
//     {
//         builder
//             .OwnsOne(match => match.Score);
//     }
// }
// public class KnockOutMatchConfigurations : IEntityTypeConfiguration<KnockOutMatch>
// {
//     public void Configure(EntityTypeBuilder<KnockOutMatch> builder)
//     {
//         builder
//             .OwnsOne(match => match.Score);
//     }
// }
