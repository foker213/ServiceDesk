using Api.Models;
using Api.Models.User;
using Application.Repository;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UsersController(IUserRepository repository) : ControllerBase
{
    private readonly IUserRepository usersRepository = repository;

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagingModel<UserReadModel>>> GetAll(
        [FromQuery(Name = "pageIndex")] int? pageIndex,
        [FromQuery(Name = "pageSize")] int? pageSize,
        [FromQuery(Name = "sort")] string? sort
    )
    {
        int limit = pageSize ?? 10;
        int offset = ((pageIndex ?? 1) - 1) * limit;
        var users = await usersRepository.GetAll(limit, offset, sort);
        return new PagingModel<UserReadModel>
        (
            Total: await usersRepository.Count(),
            Data: users.Adapt<List<UserReadModel>>()
        );
    }

}
