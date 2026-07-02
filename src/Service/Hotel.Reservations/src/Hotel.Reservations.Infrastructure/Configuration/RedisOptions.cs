using System;
using System.Collections.Generic;
using System.Text;

namespace Hotel.Reservations.Infrastructure.Configuration;

public class RedisOptions
{
    public string ConnectionString { get; set; } = "localhost:6379";
}
