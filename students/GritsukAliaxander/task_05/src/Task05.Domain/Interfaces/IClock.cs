namespace Task05.Domain.Interfaces;

public interface IClock
{
    DateTime UtcNow { get; }
}