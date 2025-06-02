using Tutorial12.Exceptions;
using Tutorial12.Repositories;

namespace Tutorial12.Services;

public class ClientsService : IClientsService
{
    private readonly IClientsRepository _clientsRepository;

    public ClientsService(IClientsRepository clientsRepository)
    {
        _clientsRepository = clientsRepository;
    }
    
    public async Task DeleteClientAsync(int idClient, CancellationToken cancellationToken)
    {
        var client = await _clientsRepository.GetByIdAsync(idClient, cancellationToken);
        if (client == null)
            throw new NoSuchClientException("Client with given id was not found.");

        var tripsAssigned = client.ClientTrips.Count(clientTrip => clientTrip.IdTripNavigation.DateFrom > DateTime.Now);
        if (tripsAssigned > 0)
            throw new ClientAssignedToTripsException("Cannot delete client: he is currently assigned to at least one trip.");

        await _clientsRepository.DeleteByIdAsync(client, cancellationToken);
    }
}