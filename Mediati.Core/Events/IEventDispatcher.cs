using System.Threading.Tasks;

namespace Mediati.Core.Events
{
    /// <summary>
    /// Dispatches events
    /// </summary>
    public interface IEventDispatcher
    {
        /// <summary>
        /// Dispatches an event to a event handler
        /// </summary>
        /// <param name="domainEvent">The event to dispatch</param>
        /// <returns>Awaitable task</returns>
        Task DispatchAsync(IDomainEvent domainEvent);
    }
}