using Hotel.Reservations.Application.Exceptions;
using Hotel.Reservations.Domain.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.Commands.DeleteReservation;

public class DeleteReservationHandler : IRequestHandler<DeleteReservationCommand, bool>
{
    private readonly IReservationRepository _repository;

    public DeleteReservationHandler(IReservationRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteReservationCommand request, CancellationToken cancellationToken)
    {
        var reservation = await _repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new NotFoundException($"Reservation with ID {request.Id} not found");

        await _repository.DeleteAsync(request.Id, cancellationToken);
        return true;
    }
}