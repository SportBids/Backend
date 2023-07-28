using SportBids.Application.Authentication.Commands.SignIn;

namespace SportBids.Application.UnitTests.Authentication.Commands.TestUtils;

public class CreateSignInCommandUtil
{
    public static SignInCommand CreateCommand()
    {
        return new SignInCommand("username", "password");
    }
}
