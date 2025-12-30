// Polyfill for IAsyncDisposable in netstandard2.0
#if NETSTANDARD2_0 && !NET
namespace System;

/// <summary>
/// Provides a mechanism for releasing unmanaged resources asynchronously.
/// </summary>
internal interface IAsyncDisposable
{
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources asynchronously.
    /// </summary>
    System.Threading.Tasks.ValueTask DisposeAsync();
}
#endif
