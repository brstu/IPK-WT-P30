using Task05.Domain.Common;

namespace Task05.Application.Common
{
    public class DateTimeService : IClock
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}