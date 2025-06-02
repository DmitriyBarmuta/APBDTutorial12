using Tutorial12.DTOs;
using Tutorial12.Exceptions;
using Tutorial12.Models;
using Tutorial12.Repositories;

namespace Tutorial12.Services;

public class TripsService : ITripsService
{
    private readonly ITripsRepository _tripsRepository;

    public TripsService(ITripsRepository tripsRepository)
    {
        _tripsRepository = tripsRepository;
    }


    public async Task<TripsInfoDTO> GetAllTripsWithParamsAsync(int pageNum, int pageSize, CancellationToken cancellationToken)
    {
        var allPages = await _tripsRepository.GetAllPagesAsync(pageSize, cancellationToken);

        if (pageNum > allPages)
            throw new PageNotFoundException("Requested page wasn't found.");
        
        var tripsPage = await _tripsRepository.GetPagedTripsAsync(pageNum, pageSize, cancellationToken);

        var tripsInfoDto = new TripsInfoDTO
        {
            PageNum = pageNum,
            PageSize = pageSize,
            AllPages = allPages,
            trips = tripsPage.Select(trip => new TripDTO
            {
                Name = trip.Name,
                Description = trip.Description,
                DateFrom = trip.DateFrom,
                DateTo = trip.DateTo,
                MaxPeople = trip.MaxPeople,
                Countries = trip.IdCountries.Select(country => new CountryDTO
                {
                    Name = country.Name
                }).ToList(),
                Clients = trip.ClientTrips.Select(clientTrip => new ClientDTO
                {
                    FirstName = clientTrip.IdClientNavigation.FirstName,
                    LastName = clientTrip.IdClientNavigation.LastName
                }).ToList()
            }).ToList()
        };

        return tripsInfoDto;
    }
}