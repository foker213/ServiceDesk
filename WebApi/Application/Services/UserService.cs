using Mapster;
using MapsterMapper;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts;
using ServiceDesk.Contracts.User;
using ServiceDesk.Domain.Database.Models;

namespace ServiceDesk.Application.Services;

public class UserService(
    IUserRepository repository,
    IMapper mapper,
    TimeProvider tp
) : Service<UserCommonRequest, UserResponse, IUserRepository, User>(repository, mapper, tp),
    IUserService
{
    public async override Task<OperationResult<UserResponse>> CreateAsync(UserCommonRequest request, CancellationToken ct)
    {
        User? existUser = await repository.GetByLoginOrEmail(
            login: request.Login!,
            email: request.Email
        );

        if (existUser?.UserName == request.Login)
            return new()
            {
                IsError = true,
                ErrorMessage = "Сотрудник с данным логином уже существует"
            };

        else if (existUser?.Email == request.Email)
            return new()
            {
                IsError = true,
                ErrorMessage = "Сотрудник с данным Email уже существует"
            };

        var user = request.Adapt<User>();
        try
        {
            await repository.Create(user, request.Password!);
        }
        catch (Exception ex)
        {
            return new()
            {
                IsError = true,
                ErrorMessage = ex.Message
            };
        }

        User? createdUser = await repository.GetBy(user.Id)!;

        return new()
        {
            Value = createdUser.Adapt<UserResponse>()
        };
    }

    public async Task<User?> GetByLogin(string userName, CancellationToken ct) =>
        await repository.GetByLogin(userName);
}
