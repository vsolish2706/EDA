using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.DTOs;

public class UpdateReservationRequest
{
    public string RoomType { get; set; } = default!;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Guests { get; set; }
}