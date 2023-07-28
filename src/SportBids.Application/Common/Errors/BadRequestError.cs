using FluentResults;

namespace SportBids.Application.Common.Errors;

public abstract class BadRequestError : Error
{
    // protected BadRequestError()
    // {
    // }

    protected BadRequestError(string message) : base(message)
    {
    }

    protected BadRequestError(string message, IError causedBy) : base(message, causedBy)
    {
    }
}
