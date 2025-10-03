using Task05.Application.Common.Interfaces;

namespace Task05.Infrastructure.Common;

public class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}