using Tutorial12.DTOs;

namespace Tutorial12.Services;

public interface ITripsService
{
    Task<TripsInfoDTO> GetAllTripsWithParamsAsync(int pageNum, int pageSize, CancellationToken cancellationToken);
}