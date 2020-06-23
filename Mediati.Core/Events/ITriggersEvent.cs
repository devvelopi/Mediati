namespace Mediati.Core.Events
{
    /// <summary>
    /// Tag that designates that a command or query triggers an event
    /// </summary>
    /// <typeparam name="TEvent">The event type to be created from the triggering message</typeparam>
    /// <typeparam name="TMessageReturn">The return type of the message that triggers the event</typeparam>
    public interface ITriggersEvent<TEvent, TMessageReturn> where TEvent : IDomainEvent
    {
        /// <summary>
        /// Converts the result of a message into an event object
        /// </summary>
        /// <param name="messageReturn">The return of the message that triggers the event</param>
        /// <returns>A domain event object</returns>
        TEvent ToEvent(TMessageReturn messageReturn);
    }
}