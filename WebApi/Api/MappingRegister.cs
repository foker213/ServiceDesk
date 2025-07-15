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
        config.NewConfig<UserCommonRequest, User>()
            .Map(dest => dest.UserName, src => src.Login)
            .Map(dest => dest.Name, src => $"{src.LastName.Trim()} {src.FirstName.Trim()} {src.MiddleName.Trim()}");

        config.NewConfig<User, UserResponse>()
            .Map(dest => dest.LastName, src => src.Name!.Split()[0])
            .Map(dest => dest.FirstName, src => src.Name!.Split()[1])
            .Map(dest => dest.MiddleName, src => src.Name!.Split()[2]);

        config.NewConfig<List<User>, List<UserResponse>>();

        config.NewConfig<ExternalUserCommonRequest, ExternalUser>()
            .Map(dest => dest.Name, src => src.FullName)
            .Map(dest => dest.Id, src => src.UserId)
            .Map(dest => dest.NumberPhone, src => src.Phone);

        config.NewConfig<ExternalUser, ExternalUserResponse>()
            .Map(dest => dest.FullName, src => src.Name)
            .Map(dest => dest.UserId, src => src.Id)
            .Map(dest => dest.Phone, src => src.NumberPhone);

        config.NewConfig<ChatCommonRequest, Chat>();

        config.NewConfig<RequestCommonRequest, Request>();

        config.NewConfig<Request, RequestResponse>();

        config.NewConfig<PagingModel<Request>, PagingModel<RequestResponse>>();
    }
}
