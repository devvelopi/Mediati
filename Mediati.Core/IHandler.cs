using System.Threading.Tasks;

namespace Mediati.Core
{
    /// <summary>
    /// Base level abstraction handler designation shared by commands, queries and events
    /// </summary>
    /// <typeparam name="TMessage">The command, event or query that is processed by the handler</typeparam>
    /// <typeparam name="TReturn">The returning object of the command, event or query</typeparam>
    public interface IHandler<in TMessage, TReturn>
    {
        /// <summary>
        /// Processes the provided command, event or query
        /// </summary>
        /// <param name="message">A command, event or query to process</param>
        /// <returns>The outcome of a command, event or query</returns>
        Task<TReturn> Handle(TMessage message);
    }
}