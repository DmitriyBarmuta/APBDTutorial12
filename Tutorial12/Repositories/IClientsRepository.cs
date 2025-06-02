using Tutorial12.Models;

namespace Tutorial12.Repositories;

public interface IClientsRepository
{
    Task<Client?> GetById(int idClient, CancellationToken cancellationToken);
    Task DeleteById(Client client, CancellationToken cancellationToken);
}