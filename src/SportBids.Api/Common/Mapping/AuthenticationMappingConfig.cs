using Mapster;
using SportBids.Api.Contracts.Account.CreateAccount;
using SportBids.Api.Contracts.Authentication.SignIn;
using SportBids.Application.Authentication.Commands.SignUp;
using SportBids.Application.Authentication.Common;
using SportBids.Domain.Entities;

namespace SportBids.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthResult, SignInResponse>()
            .Map(dst => dst, src => src);
        config.NewConfig<AuthResult, SignUpResponse>()
            .Map(dst => dst, src => src);

        config.NewConfig<SignUpCommand, AppUser>()
            .Map(dst => dst, src => src);
    }
}
