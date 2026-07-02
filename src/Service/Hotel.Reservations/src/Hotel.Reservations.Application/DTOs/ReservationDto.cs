using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.DTOs;

public class ReservationDto
{
    public Guid Id { get; set; }
    public string CustomerId { get; set; } = default!;
    public string HotelId { get; set; } = default!;
    public string RoomType { get; set; } = default!;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int Nights { get; set; }
    public int Guests { get; set; }
    public string Status { get; set; } = default!;
    public string? StatusReason { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}