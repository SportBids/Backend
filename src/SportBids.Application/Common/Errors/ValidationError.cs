namespace SportBids.Application.Common.Errors;

public class ValidationError : BadRequestError
{
    public ValidationError() : base("One or more validation errors occurred.")
    {
    }
}
