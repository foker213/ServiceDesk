using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk.Contracts.Request;
using System.Net.Mime;

namespace ServiceDesk.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RequestController : ControllerBase
{
    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<RequestReadModel>> GetAll(
        [FromQuery] int? pageIndex,
        [FromQuery] int? pageSize,
        [FromQuery] string? sort,
        [FromQuery] string dictionaryType
    )
    {
        return Ok(new());
    }

    [HttpGet]
    [Route("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RequestReadModel>> GetBy(int id)
    {
        return Ok(new());
    }

    [HttpPut]
    [Route("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(RequestUpdateModel request)
    {
        return Ok(new());
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        return Ok(new());
    }
}
