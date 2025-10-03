namespace Task05.Application.Common.Interfaces;

public interface IClock
{
    DateTime UtcNow { get; }
}