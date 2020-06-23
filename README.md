# Mediati
Implementation of the mediator pattern. Supporting attribute based pipelines, commands, event and queries.

It is built with the assumption that all commands, queries and events are asynchronous. This will likely change in time. 


## Installation

*Coming Soon*

## Usage

### Mediator

At the center of the library is the idea of the mediator. A well established design pattern that you can learn more about [here](https://refactoring.guru/design-patterns/mediator/csharp/example).

The use of a mediator improves code cleanliness, testability, developer independence and most of all, minimises dependency hell.

By default, this library comes with an implementation of the `IMediator` interface that uses the built in IServiceCollection as the registration point of handlers. However this can be replaced by custom 'handler collection' registration and discovery.

The dispatching of commands, queries and events are as follows: 
```c#
// Assuming the service collection has already been initialised via dependency injection
IMediator _mediator = new Mediator(serviceCollection);
var result = await _mediator.Dispatch(new XYZCommand { X = "SomeString" });
```

### Messages
Commands, Queries and Events are all treated as a *message*.

All messages derive from two basic concepts. 
1. A basic object containing message details
2. A handler for the message

#### Commands, Queries and Events

Commands, queries and events all work in the same way.

The **ICommand / IQuery / IDomainEvent** interface:
```c#
using Mediati.Core.Commands;
...

// ICommand<T> is used simply as a tag to identify that the class is a command
public class XYZCommand : ICommand<string>
{
    // Properties that are used in the handler to dictate the logic of flow
    public string X {get; set;}
}

...
```

The **ICommandHandler / IQueryHandler / IDomainEventHandler** interface:
```c#
using Mediati.Core.Commands;

public class XYZCommandHandler : ICommandHandler<XYZCommand, string>
{
    private readonly ISomeDependancy _someDependancy;

    public XYZCommandHandler(ISomeDependancy someDependancy)
    {
        _someDependency = someDependency;
    }
    
    public async Task<string> Handle(XYZCommand command)
    {
        return await new Task<string>(() => $"The input for X was {command.X}");
    }
}
```

#### Pipelines
The second element of this library focuses on code re-use in the form of pipelines.

These pipelines work exactly the same way as the handlers do, but are run sequentially to 'wrap around' the 'logic' handler. This is very similar to that of how [ActionFilters work in ASP.Net Core](https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/filters?view=aspnetcore-3.1#how-filters-work)

Pipelines can be applied via decorating a command handler with an attribute or globally (*coming soon*).

This is useful in a number of situations:
- Separation from ASP.Net action filters, bringing the business logic out of controllers
- Global error handling
- Retries
- Audit logging
- And so on

The following example will display how a pipeline can be created and applied as a global exception guard.

##### Creating a Pipeline

On a basic level, pipeline handlers are just basic handlers but with the knowledge of a 'next' pipeline in the chain.
```c#
// Inherits from pipeline handler for the auto hookup of the next handler
public class WrapErrorPipelineHandler<TMessage, TReturn> : PipelineHandler<TMessage, TReturn>
    {
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
                // Custom exception handling
                return default;
            }
        }
    }
```

Additionally, it is possible to have a pipeline initialised from a custom attribute. To enable this auto-magically, the pipeline handler must implement the interface `IAttributeInitialisedPipeline` and define what should be extracted. An example is as follows (Using the previous example):
```c#
// Inherits from pipeline handler for the auto hookup of the next handler
public class WrapErrorPipelineHandler<TMessage, TReturn> : PipelineHandler<TMessage, TReturn>, IAttributeInitialisedPipeline
    {
        private bool _rethrow;
        // Constructor 

        public override async Task<TReturn> Handle(TMessage command)
        {
            try
            {
                return await Next.Handle(command);
            }
            catch (Exception e)
            {
                // Custom exception handling
                if (_rethrow) throw;
                return default;
            }
        }

        public void InitialiseFromAttribute(PipelineHandlerAttribute attribute)
        {
            // See definition of this attribute in the next section
            var att = (WrapErrorAttribute)attribute;
            _rethrow = att.Rethrow;
        }
    }
```

Where the definition of the `WrapErrorAttribute` is:
```c#
[AttributeUsage(AttributeTargets.Class)]
public class WrapErrorAttribute : PipelineHandlerAttribute
{
    public override Type GetDecoratorType => typeof(WrapErrorPipelineHandler<,>);
}
```

**Note**: The pipeline attribute needs to define what pipeline handler it 'belongs' to.

##### Consuming a Pipeline
Pipelines can be attached to command, query or event handler classes. I.e. The command example is simply changed slightly:

```c#
[WrapError]
public class Command : ICommand<string> 
{
    ...
}
```

**Note**: The order of the pipelines runs in the order of the decoration, unless explicitly changed by the "Order" property.

#### Configuring with Dependency Injection
See [Dependency Injection Configuration](./Mediati.AspNetCore/README.md)

## Tips and Tricks

### De-cluttering
As a personal rule of thumb, I like to keep the Handler and the Command together in one class to reduce clutter and improve testability / class bloat:

```c#
using Mediati.Core.Commands;

public class XYZCommand : ICommand<string>
{
    public string X {get; set;}    

    internal class Handler : ICommandHandler<XYZCommand, string>
    {
        // Handling code goes here
    }

}
```