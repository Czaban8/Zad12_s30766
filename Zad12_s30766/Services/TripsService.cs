using Zad12_s30766.DTOs;
using Microsoft.EntityFrameworkCore;
namespace Zad12_s30766.Services;

public class TripsService : ITripsService
{
    private readonly Zad122Context _context;

    public TripsService(Zad122Context context)
    {
        _context = context;
    }

    public async Task<TripsPageDto> GetTrips(int page, int pageSize)
        {
            if (page <= 0)
                page = 1;
            if (pageSize <= 0)
                pageSize = 10;

            int totalTrips = await _context.Trips.CountAsync();
            int allPages = (int)Math.Ceiling(totalTrips / (double)pageSize);

            var trips = await _context.Trips
                .OrderByDescending(t => t.DateFrom)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(t => t.IdCountries)
                .Include(t => t.ClientTrips)
                .ThenInclude(ct => ct.IdClientNavigation)
                .ToListAsync();

            var tripDtos = trips.Select(t => new TripDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom,
                DateTo = t.DateTo,
                MaxPeople = t.MaxPeople,
                Countries = t.IdCountries
                    .Select(c => new CountryDto { Name = c.Name })
                    .ToList(),
                Clients = t.ClientTrips
                    .Select(ct => new ClientDto 
                    { 
                        FirstName = ct.IdClientNavigation.FirstName,
                        LastName = ct.IdClientNavigation.LastName
                    })
                    .ToList()
            }).ToList();

            return new TripsPageDto
            {
                PageNum = page,
                PageSize = pageSize,
                AllPages = allPages,
                Trips = tripDtos
            };
        }

        public async Task AddClient(int idTrip, AddClientDto request)
        {
            var trip = await _context.Trips.FindAsync(idTrip);
            if (trip == null)
            {
                throw new InvalidOperationException("wycieczka nie istnieje");
            }

            if (trip.DateFrom <= DateTime.Now)
            {
                throw new InvalidOperationException("nie można zapisać się na wycieczkę, która już się odbyła");
            }

            var existingClient = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == request.Pesel);
            if (existingClient != null)
            {
                bool alreadyAssigned = await _context.ClientTrips.AnyAsync(ct =>
                    ct.IdClient == existingClient.IdClient && ct.IdTrip == idTrip);
                if (alreadyAssigned)
                {
                    throw new InvalidOperationException("klient o tym peselu jest już zapisany na wycieczkę");
                }

                throw new InvalidOperationException("lient o takim peselu już istnieje");
            }

            var newClient = new Client
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Telephone = request.Telephone,
                Pesel = request.Pesel
            };
            _context.Clients.Add(newClient);
            await _context.SaveChangesAsync();

            var clientTrip = new ClientTrip
            {
                IdClient = newClient.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = request.PaymentDate
            };
            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();
        }
}