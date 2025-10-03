using Task05.Application.Common.Interfaces;

namespace Task05.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
