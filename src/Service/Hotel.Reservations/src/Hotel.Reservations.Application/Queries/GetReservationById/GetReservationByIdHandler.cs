using Hotel.Reservations.Application.DTOs;
using Hotel.Reservations.Application.Exceptions;
using Hotel.Reservations.Domain.Interfaces;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.Queries.GetReservationById;

public class GetReservationByIdHandler : IRequestHandler<GetReservationByIdQuery, ReservationDto?>
{
    private readonly IReservationRepository _repository;

    public GetReservationByIdHandler(IReservationRepository repository)
    {
        _repository = repository;
    }
    public async Task<ReservationDto?> Handle(GetReservationByIdQuery request, CancellationToken cancellationToken)
    {
        var reservation = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (reservation is null)
            throw new NotFoundException($"Reservation with ID {request.Id} not found");

        return reservation.Adapt<ReservationDto>();
    }
}
