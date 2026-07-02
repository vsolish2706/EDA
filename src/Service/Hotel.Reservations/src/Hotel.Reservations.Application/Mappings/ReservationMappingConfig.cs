using Hotel.Reservations.Application.DTOs;
using Hotel.Reservations.Domain.Entities;
using Mapster;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Application.Mappings;

/// <summary>
/// Configura explícitamente el mapeo Reservation -> ReservationDto.
/// Es necesario porque StayPeriod es un Value Object anidado (CheckIn,
/// CheckOut, Nights) y Mapster no lo "aplana" automáticamente a
/// propiedades de primer nivel en el DTO sin esta configuración.
/// </summary>
public class ReservationMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Reservation, ReservationDto>()
            .Map(dest => dest.CheckIn, src => src.StayPeriod.CheckIn)
            .Map(dest => dest.CheckOut, src => src.StayPeriod.CheckOut)
            .Map(dest => dest.Nights, src => src.StayPeriod.Nights)
            .Map(dest => dest.Status, src => src.Status.ToString());
    }
}