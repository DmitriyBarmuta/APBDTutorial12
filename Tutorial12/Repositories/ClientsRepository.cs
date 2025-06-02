using Microsoft.EntityFrameworkCore;
using Tutorial12.Data;
using Tutorial12.DTOs;
using Tutorial12.Exceptions;
using Tutorial12.Models;

namespace Tutorial12.Repositories;

public class ClientsRepository : IClientsRepository
{
    private readonly DatabaseContext _context;

    public ClientsRepository(DatabaseContext context)
    {
        _context = context;
    }


    public async Task<Client?> GetByIdAsync(int idClient, CancellationToken cancellationToken)
    {
        return await _context.Clients
            .Include(client => client.ClientTrips)
            .ThenInclude(clientTrip => clientTrip.IdTripNavigation)
            .FirstOrDefaultAsync(client => client.IdClient == idClient, cancellationToken);
    }

    public async Task<Client?> GetByPeselAsync(string assignDtoPesel, CancellationToken cancellationToken)
    {
        return await _context.Clients
            .FirstOrDefaultAsync(client => client.Pesel == assignDtoPesel, cancellationToken);
    }

    public async Task<int> DeleteByIdAsync(Client client, CancellationToken cancellationToken)
    {
        _context.Clients.Remove(client);
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<int> AddNewAsync(AssignClientToTripDTO assignDto, CancellationToken cancellationToken)
    {
        try
        {
            var newClient = new Client
            {
                FirstName = assignDto.FirstName,
                LastName = assignDto.LastName,
                Email = assignDto.Email,
                Telephone = assignDto.Telephone,
                Pesel = assignDto.Pesel
            }; 
            _context.Clients.Add(newClient);
        
            await _context.SaveChangesAsync(cancellationToken);

            return newClient.IdClient;
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationException("Failed to add client to the database.", ex);
        }
    }
}