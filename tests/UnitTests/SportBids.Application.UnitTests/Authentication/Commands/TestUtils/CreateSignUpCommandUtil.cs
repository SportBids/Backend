using SportBids.Application.Authentication.Commands.SignUp;

namespace SportBids.Application.UnitTests.Authentication.Commands.TestUtils;

public class CreateSignUpCommandUtil
{
    public static SignUpCommand CreateCommand()
    {
        return new SignUpCommand("username", "password", "email", "firstname", "lastname");
    }
}
