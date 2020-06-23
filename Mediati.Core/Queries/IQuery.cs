namespace Mediati.Core.Queries
{
    /// <summary>
    /// Query tag used to designate the inheriting class is a query
    /// </summary>
    /// <typeparam name="TReturn">The expected response from handling the query</typeparam>
    public interface IQuery<TReturn>
    {
    }
}