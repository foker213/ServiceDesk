using Api.Models.Request;
using Api.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Api.Controllers;

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

    }

    [HttpGet]
    [Route("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UserReadModel>> GetBy(int id)
    {

    }

    [HttpPut]
    [Route("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, RequestUpdateModel request)
    {
        
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {

    }
}
