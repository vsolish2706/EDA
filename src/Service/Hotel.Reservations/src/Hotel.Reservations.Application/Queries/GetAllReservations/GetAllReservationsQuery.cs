using Hotel.Reservations.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.Queries.GetAllReservations;

public record GetAllReservationsQuery : IRequest<IReadOnlyList<ReservationDto>>;