using Zad12_s30766.DTOs;

namespace Zad12_s30766.Services;

public interface ITripsService
{
    Task<TripsPageDto> GetTrips(int page, int pageSize);
    Task AddClient(int idTrip, AddClientDto request);
}