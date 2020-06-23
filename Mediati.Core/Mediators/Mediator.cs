using System;
using System.Linq;
using System.Threading.Tasks;
using Mediati.Core.Commands;
using Mediati.Core.Queries;

namespace Mediati.Core.Mediators
{
    /// <summary>
    /// Default implementation of the mediator using a service provider
    /// </summary>
    /// <inheritdoc />
    public class Mediator : IMediator
    {
        private readonly IServiceProvider _provider;

        /// <summary>
        /// Default constructor initialising dependencies 
        /// </summary>
        /// <param name="serviceProvider">Service provider containing handler registrations</param>
        public Mediator(IServiceProvider serviceProvider)
        {
            _provider = serviceProvider;
        }
        
        /// <inheritdoc />
        public async Task<TReturn> Dispatch<TReturn>(ICommand<TReturn> command)
        {
            // Generate types for service registration lookup
            var commandType = new[] {command.GetType(), typeof(TReturn)};
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(commandType);
            
            dynamic handler = _provider.GetService(handlerType);
            if (handler == null)
                throw new NotSupportedException(
                    $"Command handler for command {commandType.First()} is not supported or is not injected");

            // Dispatch the command
            return await handler.Handle((dynamic) command);
        }

        /// <inheritdoc />
        public async Task<TReturn> Dispatch<TReturn>(IQuery<TReturn> query)
        {
            // Generate types for service registration lookup
            var queryType = new[] {query.GetType(), typeof(TReturn)};
            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(queryType);

            dynamic handler = _provider.GetService(handlerType);
            if (handler == null)
                throw new NotSupportedException(
                    $"Query handler for query {queryType.First()} is not supported or is not injected");

            // Dispatch the query
            return await handler.Handle((dynamic) query);
        }
    }
}