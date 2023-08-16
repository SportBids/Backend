using System;
namespace SportBids.Application.Common.Errors
{
    public class UserNotFoundError : BadRequestError
    {
        public UserNotFoundError() : base("Failed to update user info.")
        {
        }
    }
}

