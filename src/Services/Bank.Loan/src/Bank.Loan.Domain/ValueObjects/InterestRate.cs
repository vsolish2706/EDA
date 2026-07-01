using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Loan.Domain.ValueObjects;

/// <summary>
/// Value Object que representa una tasa de interés anual (TEA).
/// </summary>
public record InterestRate
{
    public decimal Value { get; }

    public InterestRate(decimal value)
    {
        if (value < 0 || value > 1)
            throw new ArgumentException("La tasa debe estar entre 0 y 1 (ej: 0.12 = 12%).", nameof(value));
        Value = value;
    }

    public decimal AsPercentage() => Value * 100;
    public override string ToString() => $"{AsPercentage():N2}%";
}
