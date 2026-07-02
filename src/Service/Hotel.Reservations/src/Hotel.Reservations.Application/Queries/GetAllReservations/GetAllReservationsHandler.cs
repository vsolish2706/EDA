using Hotel.Reservations.Application.DTOs;
using Hotel.Reservations.Domain.Interfaces;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.Queries.GetAllReservations;

public class GetAllReservationsHandler : IRequestHandler<GetAllReservationsQuery, IReadOnlyList<ReservationDto>>
{
    private readonly IReservationRepository _repository;

    public GetAllReservationsHandler(IReservationRepository repository)
    {
        _repository = repository;
    }
    public async Task<IReadOnlyList<ReservationDto>> Handle(GetAllReservationsQuery request, CancellationToken cancellationToken)
    {
        var reservations = await _repository.GetAllAsync(cancellationToken);
        return reservations.Adapt<IReadOnlyList<ReservationDto>>();
    }
}
