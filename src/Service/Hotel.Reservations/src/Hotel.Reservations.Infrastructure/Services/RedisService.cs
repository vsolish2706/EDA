using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Infrastructure.Services;

public interface IRedisService
{
    Task<string?> GetReservationStatusAsync(Guid reservationId);
    Task SetReservationStatusAsync(Guid reservationId, string status, TimeSpan? expiry = null);
    Task InvalidateReservationStatusAsync(Guid reservationId);
}

/// <summary>
/// Cache-aside sobre Redis para el estado de una reserva. No reemplaza
/// al read model completo que construye Projections.Worker (eso vive en
/// claves "reservation:{id}" con el JSON completo); este servicio es un
/// caché ligero y de corta duración dentro del propio Reservations.Api,
/// pensado para evitar ir a SQL Server en lecturas frecuentes del estado
/// de escritura (por ejemplo, polling desde el cliente tras crear la
/// reserva).
/// </summary>
public class RedisService : IRedisService
{
    private readonly StackExchange.Redis.IConnectionMultiplexer _redis;
    private readonly StackExchange.Redis.IDatabase _db;

    public RedisService(StackExchange.Redis.IConnectionMultiplexer redis)
    {
        _redis = redis;
        _db = redis.GetDatabase();
    }

    private static string GetKey(Guid reservationId) => $"reservation:{reservationId}:status";

    public async Task<string?> GetReservationStatusAsync(Guid reservationId)
    {
        var value = await _db.StringGetAsync(GetKey(reservationId));
        return value.HasValue ? value.ToString() : null;
    }

    public async Task SetReservationStatusAsync(Guid reservationId, string status, TimeSpan? expiry = null)
    {
        await _db.StringSetAsync(GetKey(reservationId), status, expiry ?? TimeSpan.FromMinutes(30));
    }

    public async Task InvalidateReservationStatusAsync(Guid reservationId)
    {
        await _db.KeyDeleteAsync(GetKey(reservationId));
    }
}
