namespace Schedule.Domain.ValueObjects;

public record TimeRange
{
    public DateTimeOffset Start { get; init; }
    public DateTimeOffset End { get; init; }

    public TimeRange(DateTimeOffset start, DateTimeOffset end)
    {
        if (start >= end)
        {
            throw new DomainLevelException("Start time must be before end time.");
        }

        Start = start;
        End = end;
    }
}
