using SportBids.Application.Common.Errors;

namespace SportBids.Application;

public class PredictionOnStartedMatchError : BadRequestError
{
    public PredictionOnStartedMatchError() : base("Set prediction on started matches not allowed!")
    {
    }
}
