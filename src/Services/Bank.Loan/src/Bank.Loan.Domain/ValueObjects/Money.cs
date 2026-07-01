using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Loan.Domain.ValueObjects;

/// <summary>
/// Value Object que representa un monto monetario.
/// Garantiza que el monto siempre sea positivo.
/// </summary>
public record Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "PEN")
    {
        if (amount <= 0)
            throw new ArgumentException("El monto debe ser mayor a cero.", nameof(amount));
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("La moneda es requerida.", nameof(currency));

        Amount = Math.Round(amount, 2);
        Currency = currency.ToUpper();
    }

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("No se pueden sumar montos en distintas monedas.");
        return new Money(Amount + other.Amount, Currency);
    }

    public override string ToString() => $"{Currency} {Amount:N2}";
}
