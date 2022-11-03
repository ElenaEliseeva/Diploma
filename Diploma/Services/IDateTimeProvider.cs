namespace Diploma.Services;

public interface IDateTimeProvider
{
    public DateTime DateTimeNow { get; }
}