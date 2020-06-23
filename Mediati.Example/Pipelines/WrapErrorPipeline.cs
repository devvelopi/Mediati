using System;
using System.Threading.Tasks;
using Mediati.Core;
using Mediati.Core.Pipelines;

namespace Mediati.Example.Pipelines
{
    public class WrapErrorPipelineHandler<TMessage, TReturn> : PipelineHandler<TMessage, TReturn>,
        IAttributeInitialisedPipeline
    {
        private bool _rethrow;
        
        public WrapErrorPipelineHandler(IHandler<TMessage, TReturn> next) : base(next)
        {
        }

        public override async Task<TReturn> Handle(TMessage command)
        {
            try
            {
                return await Next.Handle(command);
            }
            catch (Exception e)
            {
                if (_rethrow) throw;
                return default;
                // Custom exception handling
            }
        }

        public void InitialiseFromAttribute(PipelineHandlerAttribute attribute)
        {
            var att = (WrapErrorAttribute)attribute;
            _rethrow = att.Rethrow;
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class WrapErrorAttribute : PipelineHandlerAttribute
    {
        public bool Rethrow { get; set; }
        public override Type GetDecoratorType => typeof(WrapErrorPipelineHandler<,>);
    }
}