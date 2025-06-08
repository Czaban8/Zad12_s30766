using Microsoft.EntityFrameworkCore;

namespace Zad12_s30766.Services;

public class ClientService : IClientService
{
    private readonly Zad122Context _context;
 
    public ClientService(Zad122Context context)
    {
        _context = context;
    }
    public async Task DeleteClient(int idClient)
    {
        var client = await _context.Clients
            .Include(c => c.ClientTrips)
            .FirstOrDefaultAsync(c => c.IdClient == idClient);
 
        if (client == null)
        {
            throw new InvalidOperationException("klient nie został znaleziony");
        }
 
        if (client.ClientTrips.Any())
        {
            throw new InvalidOperationException("kient ma przypisane wycieczki i nie może zostać usunięty");
        }
 
        _context.Clients.Remove(client);
        await _context.SaveChangesAsync();
    }
}