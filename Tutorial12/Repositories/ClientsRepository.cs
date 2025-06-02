using Microsoft.EntityFrameworkCore;
using Tutorial12.Data;
using Tutorial12.Models;

namespace Tutorial12.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly DatabaseContext _context;

    public ClientsRepository(DatabaseContext context)
    {
        _context = context;
    }


    public async Task<Client?> GetById(int idClient, CancellationToken cancellationToken)
    {
        return await _context.Clients
            .Include(client => client.ClientTrips)
            .ThenInclude(clientTrip => clientTrip.IdTripNavigation)
            .FirstOrDefaultAsync(client => client.IdClient == idClient, cancellationToken);
    }

    public async Task DeleteById(Client client, CancellationToken cancellationToken)
    {
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync(cancellationToken);
    }
}