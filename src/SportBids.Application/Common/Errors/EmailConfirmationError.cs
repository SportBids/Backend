using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportBids.Application.Common.Errors;

public class EmailConfirmationError : BadRequestError
{
    public EmailConfirmationError() : base("Failed to verify email confirmation token")
    {
    }
}
