namespace Mediati.Core.Events
{
    /// <summary>
    /// Event handler tag used to designate the inheriting class is a event handler
    /// </summary>
    /// <typeparam name="TEvent">The event type that the handler processes</typeparam>
    /// <typeparam name="TReturn">The expected response from handling the event</typeparam>
    public interface IDomainEventHandler<in TEvent, TReturn> : IHandler<TEvent, TReturn> where TEvent : IDomainEvent
    {
    }
}