namespace Marketplace.Common.Outbox.Exceptions;

/// <summary>
/// Outbox пуст
/// </summary>
public class OutboxEmptyException()
    : Exception("Outbox пуст")
{ }
