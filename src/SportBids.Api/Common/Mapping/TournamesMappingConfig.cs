using Mapster;
using SportBids.Api.Contracts.Tournament.GetTournament;
using SportBids.Domain;

namespace SportBids.Api.Common.Mapping;

public class TournamesMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Team, TeamDto>().Map(dst => dst, src => src);
        config.NewConfig<Match, MatchDto>().Map(dst => dst, src => src);
        config.NewConfig<Score, ScoreDto>().Map(dst => dst, src => src);
    }
}
