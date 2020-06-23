using System.Threading.Tasks;

namespace Mediati.Core.Pipelines
{
    /// <summary>
    /// Base pipeline handler definition
    /// </summary>
    /// <typeparam name="TMessage">The message that is being handled</typeparam>
    /// <typeparam name="TReturn">The expected return from the message</typeparam>
    public abstract class PipelineHandler<TMessage, TReturn> : IHandler<TMessage, TReturn>
    {
        /// <summary>
        /// The next handler in the pipeline
        /// </summary>
        protected readonly IHandler<TMessage, TReturn> Next;

        /// <summary>
        /// Default constructor initializing the next handler
        /// </summary>
        /// <param name="next"></param>
        protected PipelineHandler(IHandler<TMessage, TReturn> next)
        {
            Next = next;
        }
        
        /// <inheritdoc />
        public abstract Task<TReturn> Handle(TMessage message);
    }
}