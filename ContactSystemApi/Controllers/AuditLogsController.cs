
using ContactsSystemApplication.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ContactSystemApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class AuditLogsController(
    IAuditLogQueryService auditLogQueryService)
    : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPaged(
        [FromQuery] int? userId,
        [FromQuery] string? action,
        [FromQuery] string? entityName,
        [FromQuery] DateTime? fromUtc,
        [FromQuery] DateTime? toUtc,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (fromUtc.HasValue &&
            fromUtc.Value.Kind != DateTimeKind.Utc)
        {
            return BadRequest(new
            {
                Message = "FromUtc must be a UTC date."
            });
        }

        if (toUtc.HasValue &&
            toUtc.Value.Kind != DateTimeKind.Utc)
        {
            return BadRequest(new
            {
                Message = "ToUtc must be a UTC date."
            });
        }

        try
        {
            var result = await auditLogQueryService.GetPagedAsync(
                userId,
                action,
                entityName,
                fromUtc,
                toUtc,
                pageNumber,
                pageSize,
                cancellationToken);

            return Ok(result);
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                Message = exception.Message
            });
        }
    }
}
