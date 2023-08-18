using Mapster;
using SportBids.Application.Authentication.Commands.SignUp;
using SportBids.Application.Authentication.Common;
using SportBids.Contracts.Account.SignUp;
using SportBids.Contracts.Authentication.SignIn;
using SportBids.Domain.Models;
using SportBids.Infrastructure.Persistence.Entities;

namespace SportBids.Api.Common.Mapping;

public class AuthenticationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<AuthResult, SignInResponse>()
            .Map(dst => dst, src => src);
        config.NewConfig<AuthResult, SignUpResponse>()
            .Map(dst => dst, src => src);

        config.NewConfig<SignUpCommand, User>()
            .Map(dst => dst, src => src);

        config.NewConfig<AppUser, User>()
            .Map(dst => dst, src => src)
            .TwoWays();
    }
}
