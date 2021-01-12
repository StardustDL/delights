# Modulight

![](https://github.com/StardustDL/delights/workflows/CI/badge.svg) ![](https://github.com/StardustDL/delights/workflows/CD/badge.svg) ![](https://img.shields.io/github/license/StardustDL/delights.svg) [![](https://buildstats.info/nuget/Modulight.Modules.Core)](https://www.nuget.org/packages/Modulight.Modules.Core/) [![](https://buildstats.info/nuget/Modulight.Modules.Core?includePreReleases=true)](https://www.nuget.org/packages/Modulight.Modules.Core/)

[Modulight](https://github.com/StardustDL/delights) is a light modular framework aimed to be low intrusive based on dependency injection for .NET 5, ASP.NET Core and Blazor.

## Features

- Dependency injection
- Unified services registering
- Client (Blazor)
  - Unified CSS & JS lazy loading & prerendering. No need to append `<script>` and `<link>` repeatedly for every razor components, especially when use different hosting models.
  - Unified assembly lazy loading.
  - Interop between modules and host.
- Server (GraphQL cooperated with [ChilliCream GraphQL Platform](https://github.com/ChilliCream/hotchocolate))
  - Unified query/mutation/subscription definition
- Builtin module options support

It provides a place to unify resources, and it can be used to make Razor component library easy to use and manage. The user needn't to take care of related services and `<script>` or `<link>` tags in `index.html`.

## Usage

### Use modules

1. Create a ModuleHostBuilder and register modules.

For Razor component modules:

```cs
var builder = ModuleHostBuilder.CreateDefaultBuilder()
    .AddRazorComponentClientModules()
    .AddModule<FooModule>();
```

For GraphQL server modules:

```cs
var builder = ModuleHostBuilder.CreateDefaultBuilder()
    .AddGraphQLServerModules()
    .AddModule<FooModule>();
```

2. Use the builder to register services.

```cs
builder.Build(services);
```

Additional step for GraphQL server modules:

```cs
var graphQLModule = app.ApplicationServices.GetCoreGraphQLServerModule().GetService(app.ApplicationServices);
app.UseEndpoints(endpoints =>
{
    graphQLModule.MapEndpoints(endpoints);
});
```

3. Call module initializing functions.

```cs
// Blazor WebAssembly
await using (var provider = builder.Services.BuildServiceProvider())
{
    // This will load js/css resources into current DOM.
    await provider.GetModuleHost().Initialize();
}

// Others
var host = CreateHostBuilder(args).Build();
await host.Services.GetModuleHost().Initialize();
```

4. For Blazor server projects and prerendering projects, you need to add prerendering components for JS/CSS resouces.

```razor
<component type="typeof(Modulight.Modules.Client.RazorComponents.StyleSheetDeclare)" render-mode="Static" />

<component type="typeof(Modulight.Modules.Client.RazorComponents.ScriptDeclare)" render-mode="Static" />
```

These two components will find all resources defined in modules, and render HTML tags for them.

## Project guide

- [Modulight.Modules.Core](./src/Modulight.Modules.Core/) Core types for modular framework.
- [Modulight.Modules.Client.RazorComponents](./src/Modulight.Modules.Client.RazorComponents/) Basic types for client modules.
- [Modulight.Modules.Server.GraphQL](./src/Modulight.Modules.Server.GraphQL/) Basic types for graphql server modules.
- [Delights.UI](./src/Delights.UI/) UI hosting for modules.
- [Delights.Client](./src/Delights.Client/) Blazor Server hosting.
- [Delights.Client.WebAssembly](./src/Delights.Client.WebAssembly/) Blazor WebAssembly hosting.
- [Delights.Client.WebAssembly.Host](./src/Delights.Client.WebAssembly/) Blazor WebAssembly ASP.NET hosting.
- [Hello module](./src/modules/hello/) A demo module.
  - [Hello](./src/modules/hello/Delights.Modules.Hello) Client module.
  - [Hello.Core](./src/modules/hello/Delights.Modules.Hello.Core) Shared manifest between client & server module.
  - [Hello.UI](./src/modules/hello/Delights.Modules.Hello.UI) UI (pages) for client module.
  - [Hello.Server](./src/modules/hello/Delights.Modules.Hello.Server) Server module.

## Example codes

They are based on nightly build package at: 

https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json

### Design a client (Blazor) module

- [Module.cs](./src/modules/hello/Delights.Modules.Hello/Module.cs) Client module definition.
- [Index.razor](./src/modules/hello/Delights.Modules.Hello.UI/Pages/Index.razor) Client module pages. It belongs to a different assembly from which Module belongs to because we want this assembly is lazy loading.

### Design a GraphQL server module

- [Module.cs](./src/modules/hello/Delights.Modules.Hello.Server/Module.cs) GraphQL server module definition.

### Use a client module in Blazor websites

- [ModulePage.razor](./src/Delights.UI/Components/ModulePage.razor) Layout and container for module pages.
- [App.razor](./src/Delights.UI/App.razor) Lazy loading for assemblies when routing.
- [UIModule.cs](./src/Delights.UI/UIModule.cs) Definition of JS/CSS resources.
- [ModuleSetup.cs](./src/Delights.Client.Shared/ModuleSetup.cs) Use modules in client.
- [Program.cs](./src/Delights.Client/Program.cs) & [Startup.cs](./src/Delights.Client/Startup.cs) Blazor Server hosting.
- [Program.cs](./src/Delights.Client.WebAssembly/Program.cs) Blazor WebAssembly hosting.
- [index.html](./src/Delights.Client.WebAssembly/wwwroot/index.html) Clean index.html.
- [_Host.cshtml](./src/Delights.Client/Pages/_Host.cshtml) Server prerendering for JS/CSS resources.
- [_Host.cshtml](./src/Delights.Client.WebAssembly.Host/Pages/_Host.cshtml) WebAssembly prerendering.

### Use a GraphQL server module

- [Program.cs](./src/Delights.Api/Program.cs) & [Startup.cs](./src/Delights.Api/Startup.cs) GraphQL server integrating.
