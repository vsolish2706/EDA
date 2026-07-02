using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.Commands.UpdateReservation;

public class UpdateReservationValidator : AbstractValidator<UpdateReservationCommand>
{
    public UpdateReservationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Reservation ID is required");

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
