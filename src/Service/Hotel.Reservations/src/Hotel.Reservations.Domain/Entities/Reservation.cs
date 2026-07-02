using Hotel.Reservations.Domain.Enums;
using Hotel.Reservations.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Domain.Entities;

public class Reservation
{
    public Guid Id { get; private set; }
    public string CustomerId { get; private set; } = string.Empty;
    public string HotelId { get; private set; } = string.Empty;
    public string RoomType { get; private set; } = string.Empty;
    public StayPeriod StayPeriod { get; private set; } = default!;
    public int Guests { get; private set; }
    public ReservationStatus Status { get; private set; }
    public string? StatusReason { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    private Reservation() { }

    public Reservation(string customerId, string hotelId, string roomType, DateTime checkIn, DateTime checkOut, int guests)
    {
        if (string.IsNullOrWhiteSpace(customerId))
            throw new ArgumentException("El cliente (CustomerId) es obligatorio.", nameof(customerId));

        if (string.IsNullOrWhiteSpace(hotelId))
            throw new ArgumentException("El hotel (HotelId) es obligatorio.", nameof(hotelId));

        if (string.IsNullOrWhiteSpace(roomType))
            throw new ArgumentException("El tipo de habitación (RoomType) es obligatorio.", nameof(roomType));

        if (guests <= 0)
            throw new ArgumentOutOfRangeException(nameof(guests), "Debe haber al menos un huésped.");

        Id = Guid.NewGuid();
        CustomerId = customerId;
        HotelId = hotelId;
        RoomType = roomType;
        StayPeriod = new StayPeriod(checkIn, checkOut); // valida CheckOut > CheckIn
        Guests = guests;
        Status = ReservationStatus.Pending;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }


    /// <summary>
    /// Actualiza los detalles editables de la reserva (tipo de habitación,
    /// fechas, huéspedes). Sólo se permite mientras la reserva sigue en
    /// Pending: una vez que el SAGA la confirma, rechaza o cancela, los
    /// detalles quedan congelados y deben gestionarse mediante Cancel().
    /// </summary>
    public void UpdateInfo(string roomType, DateTime checkIn, DateTime checkOut, int guests)
    {
        EnsureIsPending(nameof(UpdateInfo));

        if (string.IsNullOrWhiteSpace(roomType))
            throw new ArgumentException("El tipo de habitación (RoomType) es obligatorio.", nameof(roomType));

        if (guests <= 0)
            throw new ArgumentOutOfRangeException(nameof(guests), "Debe haber al menos un huésped.");

        RoomType = roomType;
        StayPeriod = new StayPeriod(checkIn, checkOut); // valida CheckOut > CheckIn
        Guests = guests;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Confirma la reserva. Se invoca cuando el SAGA completó exitosamente
    /// el bloqueo de habitación y el cobro (evento ReservationConfirmed).
    /// </summary>
    public void Confirm()
    {
        EnsureIsPending(nameof(Confirm));

        Status = ReservationStatus.Confirmed;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Rechaza la reserva porque no había disponibilidad de habitación
    /// (evento ReservationRejected). No requiere compensar ningún pago,
    /// ya que el cobro nunca llegó a procesarse.
    /// </summary>
    public void Reject(string reason)
    {
        EnsureIsPending(nameof(Reject));

        Status = ReservationStatus.Rejected;
        StatusReason = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancela la reserva, ya sea por fallo de pago (compensación
    /// automática del SAGA) o por solicitud explícita del cliente.
    /// </summary>
    public void Cancel(string reason)
    {
        if (Status is ReservationStatus.Cancelled or ReservationStatus.Rejected)
        {
            throw new InvalidOperationException(
                $"La reserva {Id} ya está finalizada en estado '{Status}' y no puede cancelarse.");
        }

        Status = ReservationStatus.Cancelled;
        StatusReason = reason;
        UpdatedAt = DateTime.UtcNow;
    }

    private void EnsureIsPending(string operation)
    {
        if (Status != ReservationStatus.Pending)
        {
            throw new InvalidOperationException(
                $"No se puede ejecutar '{operation}' sobre la reserva {Id}: su estado actual es '{Status}', se esperaba '{ReservationStatus.Pending}'.");
        }
    }
}
