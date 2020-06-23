using System.Threading.Tasks;
using Mediati.Core.Commands;
using Mediati.Example.Pipelines;

namespace Mediati.Example.Commands
{
    public class Command : ICommand<string>
    {
        public string X { get; set; }
        
        [WrapError]
        internal class Handler : ICommandHandler<Command, string>
        {
            public async Task<string> Handle(Command message)
            {
                return await new Task<string>(() => $"The input for X was {message.X}");
            }
        }
    }
}