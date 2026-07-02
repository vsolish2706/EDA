using Hotel.Reservations.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.Commands.CreateReservation;

public record CreateReservationCommand(CreateReservationRequest Request) : IRequest<ReservationDto>;
