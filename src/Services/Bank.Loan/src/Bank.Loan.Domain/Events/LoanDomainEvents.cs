using System;
using System.Collections.Generic;
using System.Text;

namespace Bank.Loan.Domain.Events;

/// <summary>Interfaz base para domain events.</summary>
public interface IDomainEvent
{
    DateTime OccurredAt { get; }
}

public record LoanCreatedDomainEvent(
    Guid LoanId,
    string CustomerId,
    decimal Amount,
    DateTime OccurredAt) : IDomainEvent;

public record LoanApprovedDomainEvent(
    Guid LoanId,
    decimal ApprovedAmount,
    decimal InterestRate,
    DateTime OccurredAt) : IDomainEvent;

public record LoanRejectedDomainEvent(
    Guid LoanId,
    string Reason,
    DateTime OccurredAt) : IDomainEvent;

public record LoanDisbursedDomainEvent(
    Guid LoanId,
    decimal Amount,
    DateTime OccurredAt) : IDomainEvent;

public record LoanCancelledDomainEvent(
    Guid LoanId,
    string Reason,
    DateTime OccurredAt) : IDomainEvent;