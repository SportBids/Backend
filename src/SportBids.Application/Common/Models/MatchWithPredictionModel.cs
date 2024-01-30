#nullable disable

using SportBids.Domain;

namespace SportBids.Application.Common.Models;

public class MatchWithPredictionModel
{
    public Tournament Tournament { get; set; }
    public Match Match { get; set; }
    public Prediction Prediction { get; set; }

}
