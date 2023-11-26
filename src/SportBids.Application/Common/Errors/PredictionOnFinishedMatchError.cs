using SportBids.Application.Common.Errors;

namespace SportBids.Application;

public class PredictionOnFinishedMatchError : BadRequestError
{
    public PredictionOnFinishedMatchError() : base("Set prediction on finished matches not allowed!")
    {
    }
}
