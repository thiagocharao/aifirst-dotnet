namespace AIFirst.Core;

/// <summary>
/// Captures a trace event for observability and replay.
/// </summary>
public sealed record TraceEvent(
    string Kind,
    DateTimeOffset Timestamp,
    string PayloadJson);

/// <summary>
/// Receives trace events.
/// </summary>
public interface ITraceSink
{
    ValueTask WriteAsync(TraceEvent traceEvent, CancellationToken cancellationToken = default);
}
