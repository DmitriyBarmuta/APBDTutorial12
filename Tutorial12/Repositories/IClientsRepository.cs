using Tutorial12.DTOs;
using Tutorial12.Models;

namespace Tutorial12.Repositories;

public interface IClientsRepository
{
    Task<Client?> GetByIdAsync(int idClient, CancellationToken cancellationToken);
    Task<Client?> GetByPeselAsync(string assignDtoPesel, CancellationToken cancellationToken);
    Task<int> DeleteByIdAsync(Client client, CancellationToken cancellationToken);
    Task<int> AddNewAsync(AssignClientToTripDTO assignDto, CancellationToken cancellationToken);
}