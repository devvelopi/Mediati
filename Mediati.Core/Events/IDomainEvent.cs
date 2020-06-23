namespace Mediati.Core.Events
{
    /// <summary>
    /// Tag that designates that the class is a domain event
    /// </summary>
    public interface IDomainEvent
    {
        /// <summary>
        /// Unique name for the event
        /// </summary>
        string EventName { get; }
    }
}