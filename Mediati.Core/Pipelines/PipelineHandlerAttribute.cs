using System;

namespace Mediati.Core.Pipelines
{
    /// <summary>
    /// Attribute used as a baseline for pipeline handler attributes that decorate commands, queries and events
    /// </summary>
    public abstract class PipelineHandlerAttribute : Attribute
    {
        /// <summary>
        /// The order in which the pipeline runs
        /// </summary>
        public long Order { get; set; }
        /// <summary>
        /// The type of the pipeline handler related to the pipeline attribute
        /// </summary>
        public abstract Type GetDecoratorType { get; }
    }
}