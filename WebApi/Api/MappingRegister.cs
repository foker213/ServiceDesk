using Mapster;
using ServiceDesk.Contracts;
using ServiceDesk.Contracts.Chat;
using ServiceDesk.Contracts.ExternalUser;
using ServiceDesk.Contracts.Request;
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
            .Map(dest => dest.Name, src => src.FullName)
            .Map(dest => dest.Id, src => src.UserId);

        config.NewConfig<ExternalUser, ExternalUserCommonRequested>()
            .Map(dest => dest.FullName, src => src.Name)
            .Map(dest => dest.UserId, src => src.Id);

        config.NewConfig<ChatInitiated, Chat>();

        config.NewConfig<RequestCreateModel, Request>();

        config.NewConfig<Request, RequestReadModel>();

        config.NewConfig<PagingModel<Request>, PagingModel<RequestReadModel>>();
    }
}
