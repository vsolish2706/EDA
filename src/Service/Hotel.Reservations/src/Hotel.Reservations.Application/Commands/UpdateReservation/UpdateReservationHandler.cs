using Hotel.Reservations.Application.DTOs;
using Hotel.Reservations.Application.Exceptions;
using Hotel.Reservations.Domain.Interfaces;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.Commands.UpdateReservation;

public class UpdateReservationHandler : IRequestHandler<UpdateReservationCommand, ReservationDto>
{
    private readonly IReservationRepository _repository;

    public UpdateReservationHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReservationDto> Handle(UpdateReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Reservation with ID {request.Id} not found");

        // El propio agregado valida que la reserva siga en Pending; si ya
        // fue confirmada/rechazada/cancelada, UpdateInfo lanza
        // InvalidOperationException.
        reservation.UpdateInfo(
            request.Request.RoomType,
            request.Request.CheckIn,
            request.Request.CheckOut,
            request.Request.Guests);

        await _repository.UpdateAsync(reservation, cancellationToken);

        return reservation.Adapt<ReservationDto>();
    }
}