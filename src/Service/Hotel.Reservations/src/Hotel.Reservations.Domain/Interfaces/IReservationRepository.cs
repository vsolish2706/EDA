using Hotel.Reservations.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Domain.Interfaces;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Reservation>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken cancellationToken = default);

    Task AddAsync(Reservation reservation, CancellationToken cancellationToken = default);

    Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    Task<int> GetNightsAsync(Guid reservationId, CancellationToken cancellationToken = default);
}