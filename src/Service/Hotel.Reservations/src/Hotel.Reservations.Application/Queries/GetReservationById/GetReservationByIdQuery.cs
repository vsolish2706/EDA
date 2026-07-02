using Hotel.Reservations.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.Queries.GetReservationById;


public record GetReservationByIdQuery(Guid Id) : IRequest<ReservationDto?>;
