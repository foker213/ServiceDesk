using Api.Models;
using Api.Models.User;
using Application.Repository;
using Domain.DataBase.Models;
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
        [FromQuery] int? pageIndex,
        [FromQuery] int? pageSize,
        [FromQuery] string? sort
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

    [HttpGet]
    [Route("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserReadModel>> GetBy(int id)
    {
        var user = await usersRepository.GetBy(id);
        return user is null ? NotFound() : user.Adapt<UserReadModel>();
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserReadModel>> Create(UserCreateModel newUser)
    {
        var existUser = await usersRepository.GetByLoginOrEmail(
            login: newUser.Login,
            email: newUser.Email
        );

        if (existUser?.UserName! == newUser.Login)
        {
            return BadRequest(new { detail = "Сотрудник с таким логином уже существует" });
        }
        else if (existUser?.Email == newUser.Email)
        {
            return BadRequest(new { detail = "Сотрудник с таким Email уже существует" });
        }

        var user = newUser.Adapt<User>();
        try
        {
            await usersRepository.Create(user, newUser.Password);
        }
        catch (Exception ex)
        {
            return BadRequest(new { detail = ex.Message });
        }

        var createdUser = await usersRepository.GetBy(user.Id)!;
        return CreatedAtAction(
            nameof(GetBy),
            new { id = user.Id },
            createdUser.Adapt<UserReadModel>());
    }

    [HttpPut]
    [Route("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, UserChangeModel user)
    {
        var existUser = await usersRepository.GetBy(id);
        if (existUser == null)
            return NotFound();

        if (existUser.Name != user.Name)
            existUser.Name = user.Name;

        if (existUser.Email != user.Email)
        {
            var anotherUserWithSameEmail = await usersRepository.GetByEmail(user.Email);
            if (anotherUserWithSameEmail != null)
                return BadRequest(new { detail = "Другой сотрудник с таким Email уже существует" });

            existUser.Email = user.Email;
        }

        if (existUser.PhoneNumber != user.PhoneNumber)
            existUser.PhoneNumber = user.PhoneNumber;

        if (existUser.Blocked != user.Blocked)
            existUser.Blocked = user.Blocked;

        if (existUser.BlockedReason != user.BlockedReason)
            existUser.BlockedReason = user.BlockedReason;

        if (existUser.Blocked && existUser.BlockedAt == null)
        {
            existUser.BlockedAt = DateTime.UtcNow;
        }
        else if (!existUser.Blocked && existUser.BlockedAt != null)
        {
            existUser.BlockedAt = null;
        }

        await usersRepository.Update(existUser);
        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var existUser = await usersRepository.GetBy(id);
        if (existUser == null)
            return NotFound();

        await usersRepository.Delete(id);
        return NoContent();
    }
}
