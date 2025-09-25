using Task05.Domain.Interfaces;

namespace Task05.Infrastructure.Services;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}