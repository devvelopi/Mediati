namespace Mediati.Core.Commands
{
    /// <summary>
    /// Command tag used to designate the inheriting class is a command
    /// </summary>
    /// <typeparam name="TReturn">The expected response from handling the command</typeparam>
    public interface ICommand<TReturn>
    {
    }
}