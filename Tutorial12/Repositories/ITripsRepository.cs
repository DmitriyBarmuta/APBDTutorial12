using Tutorial12.Models;

namespace Tutorial12.Repositories;

public interface ITripsRepository
{
    Task<List<Trip>> GetPagedTripsAsync(int pageNum, int pageSize, CancellationToken cancellationToken);
    Task<int> GetAllPagesAsync(int pageSize, CancellationToken cancellationToken);
}