using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Mediati.Core;
using Mediati.Core.Commands;
using Mediati.Core.Events;
using Mediati.Core.Pipelines;
using Mediati.Core.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Mediati.AspNetCore.Extensions
{
    /// <summary>
    /// Extensions used to register command, query and event handlers dynamically from the assembly
    /// </summary>
    public static class MediatiServiceCollectionExtensions
    {
        /// <summary>
        /// Adds all public handlers in the assembly as services with dependencies
        /// </summary>
        /// <param name="services">Service collection to bind to</param>
        public static void AddHandlersDynamic(this IServiceCollection services)
        {
            // Gather all handlers 
            var handlerTypes = typeof(IHandler<,>).Assembly.GetTypes()
                .Where(x => x.GetInterfaces().Where(i => !x.BaseType?.GetInterfaces().Contains(i) ?? true)
                    .Any(IsHandlerInterface))
                .Where(x => !x.ContainsGenericParameters)
                .ToList();

            // Register all the handlers
            foreach (var handlerType in handlerTypes)
                AddHandler(services, handlerType);
        }

        private static void AddHandler(IServiceCollection services, Type type)
        {
            var interfaceType = type.GetInterfaces().Where(i => !type.BaseType?.GetInterfaces().Contains(i) ?? true)
                .Single(IsHandlerInterface);
            var factory = BuildPipeline(type, interfaceType);

            services.AddTransient(interfaceType, factory);
        }

        private static Func<IServiceProvider, object> BuildPipeline(Type handlerType, Type interfaceType)
        {
            // Gather all decorators attached to the handler
            var attributes = handlerType.GetCustomAttributes(false)
                .OfType<PipelineHandlerAttribute>()
                .OrderBy(a => a.Order)
                .ToList();
            var pipeline = attributes
                .Select(a => a.GetDecoratorType)
                .Concat(new[] {handlerType})
                .Reverse()
                .ToList();

            var constructors = pipeline.Select(t =>
            {
                var type = t.IsGenericType ? t.MakeGenericType(interfaceType.GenericTypeArguments) : t;
                return type.GetConstructors().Single();
            }).ToList();

            return provider =>
            {
                object current = null;
                // Populate required dependencies into the handler pipeline
                foreach (var c in constructors)
                {
                    var parameterInfos = c.GetParameters().ToList();
                    var parameters = GetParameters(parameterInfos, current, provider);
                    current = c.Invoke(parameters);

                    var relatedAttribute = attributes.FirstOrDefault(a =>
                        c.DeclaringType != null && c.DeclaringType.IsGenericType &&
                        a.GetDecoratorType == c.DeclaringType.GetGenericTypeDefinition());

                    if (relatedAttribute != null)
                        (current as IAttributeInitialisedPipeline)?.InitialiseFromAttribute(relatedAttribute);
                }

                return current;
            };
        }

        private static object[] GetParameters(IEnumerable<ParameterInfo> parameterInfos, object current,
            IServiceProvider provider)
            => parameterInfos.Select(p => GetParameter(p, current, provider)).ToArray();


        private static object GetParameter(ParameterInfo parameterInfo, object? current, IServiceProvider provider)
        {
            var parameterType = parameterInfo.ParameterType;
            if (parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() == typeof(IHandler<,>))
                return current;

            var service = provider.GetService(parameterType);
            if (service != null) return service;

            throw new ArgumentException($"Type {parameterType} not found");
        }

        private static bool IsHandlerInterface(Type type)
        {
            if (!type.IsGenericType) return false;
            var typeDefinition = type.GetGenericTypeDefinition();
            return typeDefinition == typeof(ICommandHandler<,>) || typeDefinition == typeof(IQueryHandler<,>) ||
                   typeDefinition == typeof(IDomainEventHandler<,>);
        }
    }
}