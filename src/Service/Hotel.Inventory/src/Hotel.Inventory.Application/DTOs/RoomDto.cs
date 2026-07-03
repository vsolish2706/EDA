using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Inventory.Application.DTOs;

public class RoomDto
{
    public Guid Id { get; set; }
    public string HotelId { get; set; } = default!;
    public string RoomType { get; set; } = default!;
    public string RoomNumber { get; set; } = default!;
    public decimal PricePerNight { get; set; }
    public bool IsReserved { get; set; }
    public Guid? ReservationId { get; set; }
    public string Status { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}