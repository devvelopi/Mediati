# Mediati.AspNetCore
Extensions for configuring Mediati command, query and event handlers automatically with dependency injection.

## Installation

*Coming Soon*

## Usage

Usage is simple. In Startup:

```c#
using Microsoft.Extensions.DependencyInjection;
using Mediati.Core.Mediators;
using Mediati.AspNetCore.Extensions;

namespace SomeNamespace
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IMediator, Mediator>();
            services.AddHandlersDynamic();
        }
    }
}
```