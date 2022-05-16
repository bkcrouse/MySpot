using Microsoft.AspNetCore.Mvc;
using MySpot.Api.Models;

namespace MySpot.Api.Controllers;

[Route("reservations")]
public class ReservationsController : ControllerBase
{
    private static string[] _parkingSpotNames = { "P1", "P2", "P3", "P4", "P5" };
    private static readonly List<Reservation> _reservations = new();
    private static int Id = 1;

    [HttpGet("{id:int}")]
    public ActionResult<Reservation> Get([FromRoute] int id)
    {
        var reservation = _reservations.SingleOrDefault(x => x.Id == id);
        if (reservation is null) return NotFound();
        return Ok();
    }

    [HttpPost]
    public ActionResult Post([FromBody] Reservation reservation)
    {
        reservation.Id = Id;
        reservation.Date = DateTime.Now.AddDays(1).Date;

        // Business Rules
        //
        // 1. Check existence of parking spot name
        var invalidParkingSpotName = _parkingSpotNames.All(x => x != reservation.ParkingSpotName);
        if (invalidParkingSpotName) return BadRequest();

        // 2. Check if reservation exists.
        var reservationAlreadyExists = _reservations.Any(x =>
            x.Date.Date == reservation.Date.Date && x.ParkingSpotName == reservation.ParkingSpotName);
        if (reservationAlreadyExists) return BadRequest();
        Id++;
        _reservations.Add(reservation);
        return CreatedAtAction(nameof(Get), new {Id = reservation.Id}, default);
    }

    [HttpPut]
    public void Put()
    {
    }

    [HttpDelete]
    public void Delete()
    {
    }
}