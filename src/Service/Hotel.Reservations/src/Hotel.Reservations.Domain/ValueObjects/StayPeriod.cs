using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Domain.ValueObjects;

/// <summary>
/// Value Object que representa el rango de fechas de una estadía.
/// Garantiza su propia invariante (CheckOut posterior a CheckIn) en el
/// constructor, de modo que un StayPeriod inválido nunca puede existir.
/// </summary>
public sealed class StayPeriod
{
    public DateTime CheckIn { get; }
    public DateTime CheckOut { get; }

    /// <summary>Número de noches de la estadía (mínimo 1).</summary>
    public int Nights => Math.Max(1, (CheckOut.Date - CheckIn.Date).Days);

    public StayPeriod(DateTime checkIn, DateTime checkOut)
    {
        if (checkOut <= checkIn)
        {
            throw new ArgumentException(
                "La fecha de salida (CheckOut) debe ser posterior a la fecha de ingreso (CheckIn).");
        }

        CheckIn = checkIn;
        CheckOut = checkOut;
    }

    public override bool Equals(object? obj) =>
        obj is StayPeriod other && CheckIn == other.CheckIn && CheckOut == other.CheckOut;

    public override int GetHashCode() => HashCode.Combine(CheckIn, CheckOut);

    public override string ToString() => $"{CheckIn:yyyy-MM-dd} → {CheckOut:yyyy-MM-dd} ({Nights} noche(s))";
}
