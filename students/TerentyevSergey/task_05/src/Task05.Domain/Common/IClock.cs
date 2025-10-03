namespace Task05.Domain.Common;

public interface IClock
{
    DateTime UtcNow { get; }
}
