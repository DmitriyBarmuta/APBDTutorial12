using Microsoft.EntityFrameworkCore;
using Tutorial12.Data;
using Tutorial12.Models;

namespace Tutorial12.Repositories;

public class TripsRepository : ITripsRepository
{
    private readonly DatabaseContext _context;

    public TripsRepository(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<List<Trip>> GetPagedTripsAsync(int pageNum, int pageSize, CancellationToken cancellationToken)
    {
        return await _context.Trips
            .OrderByDescending(trip => trip.DateFrom)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .Include(trip => trip.ClientTrips)
            .ThenInclude(clientTrip => clientTrip.IdClientNavigation)
            .Include(trip => trip.IdCountries)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> GetAllPagesAsync(int pageSize, CancellationToken cancellationToken)
    {
        var totalCount = await _context.Trips.CountAsync(cancellationToken);
        Console.WriteLine($"Total trips count: {totalCount}");
        return (int)Math.Ceiling(totalCount / (double)pageSize);
    }
}