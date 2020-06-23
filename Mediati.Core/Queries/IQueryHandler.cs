namespace Mediati.Core.Queries
{
    /// <summary>
    /// Query handler tag used to designate the inheriting class is a query handler
    /// </summary>
    /// <typeparam name="TQuery">The query type that the handler processes</typeparam>
    /// <typeparam name="TReturn">The expected response from handling the query</typeparam>
    public interface IQueryHandler<in TQuery, TReturn> : IHandler<TQuery, TReturn> where TQuery : IQuery<TReturn>
    {
    }
}