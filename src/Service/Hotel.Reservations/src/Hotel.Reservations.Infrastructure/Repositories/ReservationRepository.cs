using Hotel.Reservations.Application.Exceptions;
using Hotel.Reservations.Domain.Entities;
using Hotel.Reservations.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Infrastructure.Repositories;

public class ReservationRepository(AppDbContext context) : IReservationRepository
{
    private readonly AppDbContext context = context;

    public async Task<Reservation?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Reservations.FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Reservation>> GetByCustomerIdAsync(string customerId, CancellationToken cancellationToken = default)
    {
        return await context.Reservations
            .Where(r => r.CustomerId == customerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await context.Reservations.ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        await context.Reservations.AddAsync(reservation, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Reservation reservation, CancellationToken cancellationToken = default)
    {
        context.Reservations.Update(reservation);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var reservation = await GetByIdAsync(id, cancellationToken);
        if (reservation is not null)
        {
            context.Reservations.Remove(reservation);
            await context.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<int> GetNightsAsync(Guid reservationId, CancellationToken cancellationToken = default)
    {
        var reservation = await GetByIdAsync(reservationId, cancellationToken);
        if (reservation is null)
            throw new NotFoundException($"Reservation with ID {reservationId} not found");

        return reservation.StayPeriod.Nights;
    }
}
