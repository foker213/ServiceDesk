using Mapster;
using ServiceDesk.Contracts.ExternalUser;
using ServiceDesk.Contracts.User;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Api;

public class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<UserCreateModel, User>()
                .Map(dest => dest.UserName, src => src.Login)
                .Map(dest => dest.Name, src => $"{src.LastName.Trim()} {src.FirstName.Trim()} {src.MiddleName.Trim()}");

        config.NewConfig<UserChangeModel, User>()
            .Map(dest => dest.Name, src => $"{src.LastName.Trim()} {src.FirstName.Trim()} {src.MiddleName.Trim()}");

        config.NewConfig<User, UserReadModel>()
            .Map(dest => dest.LastName, src => src.Name!.Split()[0])
            .Map(dest => dest.FirstName, src => src.Name!.Split()[1])
            .Map(dest => dest.MiddleName, src => src.Name!.Split()[2]);

        config.NewConfig<ExternalUserCommonRequested, ExternalUser>()
            .Map(dest => dest.Name, src => src.FullName);

        config.NewConfig<ExternalUser, ExternalUserCommonRequested>()
            .Map(dest => dest.FullName, src => src.Name);
    }
}
