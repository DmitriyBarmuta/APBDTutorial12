using Microsoft.AspNetCore.Mvc;
using Tutorial12.Exceptions;
using Tutorial12.Services;

namespace Tutorial12.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientsService _clientsService;

    public ClientsController(IClientsService clientsService)
    {
        _clientsService = clientsService;
    }
    
    [HttpDelete("{idClient:int}")]
    public async Task<IActionResult> DeleteClient(int idClient, CancellationToken cancellationToken = default)
    {
        try
        {
            await _clientsService.DeleteClientAsync(idClient, cancellationToken);
            return NoContent();
        }
        catch (NoSuchClientException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ClientAssignedToTripsException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal Server Error.", error = ex.Message });
        }
    }
}