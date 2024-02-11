namespace SportBids.Application.Common.Errors;

public class PredictionOnStartedMatchError : BadRequestError
{
    public PredictionOnStartedMatchError() : base("Set prediction on started matches not allowed!")
    {
    }
}
