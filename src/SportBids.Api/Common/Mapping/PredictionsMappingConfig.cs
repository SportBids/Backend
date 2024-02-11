using Mapster;
using SportBids.Domain.Entities;

namespace SportBids.Api.Common.Mapping;

public class PredictionsMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
            .NewConfig<IEnumerable<Prediction>, IEnumerable<Contracts.MatchPrediction.PredictionDto>>()
            .Map(dst => dst, src => src);
    }
}
