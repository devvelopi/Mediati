using System.Threading.Tasks;
using Mediati.Core.Commands;
using Mediati.Core.Queries;

namespace Mediati.Core.Mediators
{
    /// <summary>
    /// Global command, query and event dispatcher
    /// </summary>
    public interface IMediator
    {
        /// <summary>
        /// Dispatch a command to be handled
        /// </summary>
        /// <param name="command">The command to handle</param>
        /// <typeparam name="TReturn">The expected return from handling the command</typeparam>
        /// <returns>Outcome of the command</returns>
        Task<TReturn> Dispatch<TReturn>(ICommand<TReturn> command);
        /// <summary>
        /// Dispatch a query to be handled
        /// </summary>
        /// <param name="query">The query to handle</param>
        /// <typeparam name="TReturn">The expected return from handling the query</typeparam>
        /// <returns>Outcome of the query</returns>
        Task<TReturn> Dispatch<TReturn>(IQuery<TReturn> query);
    }
}