using Hotel.Reservations.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.Commands.UpdateReservation;

public record UpdateReservationCommand(Guid Id, UpdateReservationRequest Request) : IRequest<ReservationDto>;