using Mapster;
using SportBids.Application.Authentication.Commands.SignUp;
using SportBids.Application.Authentication.Common;
using SportBids.Contracts.Account.SignUp;
using SportBids.Contracts.Authentication.SignIn;
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
