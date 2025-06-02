using Microsoft.AspNetCore.Mvc;
using Tutorial12.DTOs;
using Tutorial12.Exceptions;
using Tutorial12.Services;

namespace Tutorial12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripsService _tripsService;

    public TripsController(ITripsService tripsService)
    {
        _tripsService = tripsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllTripsWithParams(int pageNum = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var dto = await _tripsService.GetAllTripsWithParamsAsync(pageNum, pageSize, cancellationToken);

            return Ok(dto);
        }
        catch (PageNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error.", error = ex.Message });
        }
    }
}