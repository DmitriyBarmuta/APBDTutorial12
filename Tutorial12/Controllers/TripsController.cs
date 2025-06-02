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

    [HttpPost("{idTrip:int}/clients")]
    public async Task<IActionResult> AssignClientToTrip([FromBody] AssignClientToTripDTO assignDto,
        int idTrip,
        CancellationToken cancellationToken = default)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        try
        {
            var idClient = await _tripsService.AssignToTripAsync(assignDto, idTrip, cancellationToken);
            return CreatedAtAction(nameof(GetAllTripsWithParams), new { idClient }, new { idClient });
        }
        catch (Exception ex) when (ex is ClientAlreadyExistsException or TripAlreadyHappenedException)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (NoSuchTripException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error.", error = ex.Message });
        }
    }
}