namespace Mediati.Core.Commands
{
    /// <summary>
    /// Command handler tag used to designate the inheriting class is a command handler
    /// </summary>
    /// <typeparam name="TCommand">The command type that the handler processes</typeparam>
    /// <typeparam name="TReturn">The expected response from handling the command</typeparam>
    public interface ICommandHandler<in TCommand, TReturn> : IHandler<TCommand, TReturn>
        where TCommand : ICommand<TReturn>
    {
    }
}