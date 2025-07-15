using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts;
using ServiceDesk.Contracts.User;
using System.Net.Mime;

namespace ServiceDesk.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagingModel<UserResponse>>> GetAll(
        [FromQuery] int? pageIndex,
        [FromQuery] int? pageSize,
        [FromQuery] string? sort
    )
    {
        return await userService.GetAll(pageSize, pageIndex, sort);
    }

    [HttpGet]
    [Route("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserResponse>> GetBy(int id)
    {
        OperationResult<UserResponse> result = await userService.GetBy(id);

        if (result.IsError)
        {
            return BadRequest(new { details = result.ErrorMessage });
        }

        return CreatedAtAction(
            nameof(GetBy),
            new { id = result.Value!.Id },
            result.Value);
    }

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UserResponse>> Create(UserCommonRequest newUser)
    {
        OperationResult<UserResponse> result = await userService.CreateAsync(newUser);

        if(result.IsError)
        {
            return BadRequest(new { details = result.ErrorMessage });
        }

        return CreatedAtAction(
            nameof(GetBy),
            new { id = result.Value!.Id },
            result.Value);
    }

    [HttpPut]
    [Route("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Update(int id, UserCommonRequest updatedUser)
    {
        OperationResult<bool> result = await userService.UpdateAsync(id, updatedUser);

        if (result.IsError)
        {
            return BadRequest(new { details = result.ErrorMessage });
        }

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        OperationResult<bool> result = await userService.DeleteAsync(id);

        if (result.IsError)
        {
            return NotFound();
        }

        return NoContent();
    }
}
