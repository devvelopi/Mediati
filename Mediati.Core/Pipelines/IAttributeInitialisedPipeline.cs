namespace Mediati.Core.Pipelines
{
    /// <summary>
    /// Attaches to pipeline handlers that require configuration from attribute decorators
    /// </summary>
    public interface IAttributeInitialisedPipeline
    {
        /// <summary>
        /// Initialises the pipeline from attribute parameters
        /// </summary>
        /// <param name="attribute">The attribute to generate configuration from</param>
        void InitialiseFromAttribute(PipelineHandlerAttribute attribute);
    }
}