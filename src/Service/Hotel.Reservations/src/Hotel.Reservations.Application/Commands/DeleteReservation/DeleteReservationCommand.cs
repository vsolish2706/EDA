using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.Commands.DeleteReservation;

public record DeleteReservationCommand(Guid Id) : IRequest<bool>;
