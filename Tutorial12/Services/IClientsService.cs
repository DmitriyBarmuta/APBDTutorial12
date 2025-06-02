namespace Tutorial12.Services;

public interface IClientsService
{
    Task DeleteClient(int idClient, CancellationToken cancellationToken);
}