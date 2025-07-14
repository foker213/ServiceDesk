using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceDesk.Application.IRepository;
using ServiceDesk.Application.IServices;
using ServiceDesk.Contracts;
using ServiceDesk.Contracts.Request;
using System.Net.Mime;

namespace ServiceDesk.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RequestController : ControllerBase
{
    private readonly IRequestService _requestService;

    public RequestController(IRequestService requestService)
    {
        _requestService = requestService;
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<PagingModel<RequestReadModel>>> GetAll(
        [FromQuery] int? pageIndex,
        [FromQuery] int? pageSize,
        [FromQuery] string? sort,
        [FromQuery] string dictionaryType
    )
    {
        int limit = pageSize ?? 10;
        int offset = ((pageIndex ?? 1) - 1) * limit;

        return await _requestService.GetAll(limit, offset, sort, dictionaryType);
    }

    [HttpGet]
    [Route("{id}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<RequestReadModel>> GetBy(int id)
    {
        return await _requestService.GetBy(id);
    }

    [HttpPut]
    [Route("{id}")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Update(int id, RequestChangeModel request)
    {
        bool wasUpdated = await _requestService.UpdateAsync(id, request);

        return wasUpdated ? NoContent() : NotFound();
    }

    [HttpDelete]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        bool wasDeleted = await _requestService.DeleteAsync(id);

        return wasDeleted ? NoContent() : NotFound();
    }

    [HttpPatch]
    [Route("{id}/status")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateStatus(int id)
    {
        bool wasUpdated = await _requestService.UpdateStatusAsync(id);

        return wasUpdated ? NoContent() : NotFound();
    }
}
