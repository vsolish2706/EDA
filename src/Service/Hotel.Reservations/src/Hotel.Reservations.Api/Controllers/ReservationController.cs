using Hotel.Reservations.Application.Commands.CreateReservation;
using Hotel.Reservations.Application.Commands.DeleteReservation;
using Hotel.Reservations.Application.Commands.UpdateReservation;
using Hotel.Reservations.Application.DTOs;
using Hotel.Reservations.Application.Queries.GetAllReservations;
using Hotel.Reservations.Application.Queries.GetReservationById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Reservations.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ReservationController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ReservationDto>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAllReservationsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ReservationDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetReservationByIdQuery(id), cancellationToken);
        if (result is null)
            return NotFound();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDto>> Create([FromBody] CreateReservationRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateReservationCommand(request), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<ReservationDto>> Update(Guid id, [FromBody] UpdateReservationRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new UpdateReservationCommand(id, request), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteReservationCommand(id), cancellationToken);
        return NoContent();
    }

}
