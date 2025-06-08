using Microsoft.AspNetCore.Mvc;
using Zad12_s30766.DTOs;
using Zad12_s30766.Services;

namespace Zad12_2.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripsService;

        public TripsController(ITripsService tripsService)
        {
            _tripsService = tripsService;
        }
        [HttpGet]
        public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _tripsService.GetTrips(page, pageSize);
            return Ok(result);
        }

        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AddClients(int idTrip, [FromBody] AddClientDto addClientDto)
        {
            try
            {
                await _tripsService.AddClient(idTrip, addClientDto);
                return Ok(new { message = "klient został przypisany do wycieczki" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}