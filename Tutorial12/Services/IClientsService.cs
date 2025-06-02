namespace Tutorial12.Services;

public interface IClientsService
{
    Task DeleteClientAsync(int idClient, CancellationToken cancellationToken);
}