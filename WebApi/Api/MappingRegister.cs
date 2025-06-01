using Api.Models.User;
using Domain.DataBase.Models;
using Mapster;

namespace Api;

public class MappingRegister : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, UserReadModel>();
    }
}
