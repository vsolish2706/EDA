using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Domain.Enums;

public enum ReservationStatus
{
    Pending,
    Confirmed,
    Rejected,
    Cancelled
}