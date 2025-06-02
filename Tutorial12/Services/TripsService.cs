using System.Net.Sockets;
using Tutorial12.DTOs;
using Tutorial12.Exceptions;
using Tutorial12.Repositories;

namespace Tutorial12.Services;

public class TripsService : ITripsService
{
    private readonly ITripsRepository _tripsRepository;
    private readonly IClientsRepository _clientsRepository;

    public TripsService(ITripsRepository tripsRepository, IClientsRepository clientsRepository)
    {
        _tripsRepository = tripsRepository;
        _clientsRepository = clientsRepository;
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

    public async Task<int> AssignToTripAsync(AssignClientToTripDTO assignDto, int idTrip, CancellationToken cancellationToken)
    {
        var client = await _clientsRepository.GetByPeselAsync(assignDto.Pesel, cancellationToken);
        if (client != null)
            throw new ClientAlreadyExistsException($"Client with PESEL {assignDto.Pesel} already exists.");

        var idClient = await _clientsRepository.AddNewAsync(assignDto, cancellationToken);

        var trip = await _tripsRepository.GetByIdAsync(idTrip, cancellationToken);
        if (trip == null) 
            throw new NoSuchTripException("No trip with provided id exists.");
        
        if (trip.DateFrom < DateTime.Now)
            throw new TripAlreadyHappenedException("You can't assign user to already started trip.");

        await _tripsRepository.AssignClientToTripAsync(idClient, idTrip, assignDto.PaymentDate, cancellationToken);

        return idClient;
    }
}