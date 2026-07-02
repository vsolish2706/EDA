using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.Commands.CreateReservation;

public class CreateReservationValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationValidator()
    {
        RuleFor(x => x.Request.CustomerId)
            .NotEmpty().WithMessage("CustomerId is required")
            .MaximumLength(128).WithMessage("CustomerId cannot exceed 128 characters");

        RuleFor(x => x.Request.HotelId)
            .NotEmpty().WithMessage("HotelId is required")
            .MaximumLength(128).WithMessage("HotelId cannot exceed 128 characters");

        RuleFor(x => x.Request.RoomType)
            .NotEmpty().WithMessage("RoomType is required")
            .MaximumLength(64).WithMessage("RoomType cannot exceed 64 characters");

        RuleFor(x => x.Request.CheckIn)
            .NotEmpty().WithMessage("CheckIn is required");

        RuleFor(x => x.Request.CheckOut)
            .NotEmpty().WithMessage("CheckOut is required")
            .GreaterThan(x => x.Request.CheckIn).WithMessage("CheckOut must be later than CheckIn");

        RuleFor(x => x.Request.Guests)
            .GreaterThan(0).WithMessage("Guests must be at least 1")
            .LessThanOrEqualTo(10).WithMessage("Guests cannot exceed 10");
    }
}