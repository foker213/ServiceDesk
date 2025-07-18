using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk.Application.IServices;
using ServiceDesk.Application.Services;
using ServiceDesk.Contracts;
using ServiceDesk.Contracts.Request;
using ServiceDesk.Contracts.User;
using System.Net.Mime;

namespace ServiceDesk.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RequestsController(IRequestService requestService) : ControllerBase
{

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagingModel<RequestResponse>>> GetAll(
        [FromQuery] int? pageIndex,
        [FromQuery] int? pageSize,
        [FromQuery] string? sort,
        [FromQuery] string dictionaryType,
        CancellationToken ct
    )
    {
        return await requestService.GetAll(pageSize, pageIndex, sort, dictionaryType);
    }

    [HttpGet]
    [Route("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RequestResponse>> GetBy(int id, CancellationToken ct)
    {
        OperationResult<RequestResponse> result = await requestService.GetBy(id);

        if (result.IsError)
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
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, RequestCommonRequest request, CancellationToken ct)
    {
        OperationResult<bool> result = await requestService.UpdateAsync(id, request);

        if (result.IsError)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id, CancellationToken ct)
    {
        OperationResult<bool> result = await requestService.DeleteAsync(id);

        if (result.IsError)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPatch]
    [Route("{id}/status")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateStatus(int id, CancellationToken ct)
    {
        OperationResult<bool> result = await requestService.UpdateStatusAsync(id);

        if (result.IsError)
        {
            return NotFound();
        }

        return NoContent();
    }
}
