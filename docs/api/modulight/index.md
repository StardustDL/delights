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
    .UseRazorComponentClientModules()
    .AddModule<FooModule>();
```

For GraphQL server modules:

```cs
var builder = ModuleHostBuilder.CreateDefaultBuilder()
    .UseAspNetServerModules().UseGraphQLServerModules()
    .AddModule<FooModule>();
```

2. Use the builder to register services.

```cs
// in Startup: void ConfigureServices(IServiceCollection services)

builder.Build(services);
```

Additional step for GraphQL server modules:

```cs
// in Startup: void Configure(IApplicationBuilder app, IWebHostEnvironment env)

app.UseAspNetServerModuleMiddlewares();

app.UseEndpoints(endpoints =>
{
    // modules mapper
    endpoints.MapAspNetServerModuleEndpoints();
    endpoints.MapGraphQLServerModuleEndpoints(postMapEndpoint: (module, builder) =>
    {
        builder.RequireCors(cors =>
        {
            cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    });
    // other mapper, eg:
    endpoints.MapControllers();
});
```

3. Configure the module initilizing & shutdown.

```cs
// in Program: Task Main(string[] args)

var host = CreateHostBuilder(args).Build();
await using var _ = await host.Services.UseModuleHost();
await host.RunAsync();
```

4. For razor components, add `ResourceDeclare` component to App.razor to load UI resources.

```razor
<Modulight.Modules.Client.RazorComponents.UI.ResourceDeclare />
```

This component will find all resources defined in modules, and render HTML tags for them.

This works for normal cases, but if you use WebAssembly target, no prerenderring, and the component library need the javascript files to be loaded initially. You can use the following codes to load resources manually.

```cs
// WebAssemblyHost host;
await using var _ = await host.Services.UseModuleHost();
await host.Services.GetRazorComponentClientModuleCollection().LoadResources();
await host.RunAsync();
```

## Example codes

They are based on nightly build package at: 

[NUGET source](https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json)

### Design a client (Blazor) module

- [HelloModule.cs](https://github.com/StardustDL/delights/blob/master/src/modules/hello/Delights.Modules.Hello/HelloModule.cs) Client module definition.
- [Index.razor](https://github.com/StardustDL/delights/blob/master/src/modules/hello/Delights.Modules.Hello.UI/Pages/Index.razor) Client module pages. It belongs to a different assembly from which Module belongs to because we want this assembly is lazy loading.

### Design a GraphQL server module

- [HelloServerModule.cs](https://github.com/StardustDL/delights/blob/master/src/modules/hello/Delights.Modules.Hello.Server/HelloServerModule.cs) GraphQL server module definition.

### Use a client module in Blazor websites

- [ModulePageLayout.razor](https://github.com/StardustDL/delights/blob/master/src/Delights.UI/Shared/ModulePageLayout.razor) Layout and container for module pages.
- [App.razor](https://github.com/StardustDL/delights/blob/master/src/Delights.UI/App.razor) Lazy loading for js/css/sassemblies when routing.
- [UIModule.cs](https://github.com/StardustDL/delights/blob/master/src/Delights.UI/UIModule.cs) Definition of JS/CSS resources.
- [ModuleSetup.cs](https://github.com/StardustDL/delights/blob/master/src/Delights.Client.Shared/ModuleSetup.cs) Use modules in client.
- [Startup.cs](https://github.com/StardustDL/delights/blob/master/src/Delights.Client/Startup.cs) Blazor Server hosting.
- [Program.cs](https://github.com/StardustDL/delights/blob/master/src/Delights.Client.WebAssembly/Program.cs) Blazor WebAssembly hosting.
- [index.html](https://github.com/StardustDL/delights/blob/master/src/Delights.Client.WebAssembly/wwwroot/index.html) Clean index.html.

### Use a GraphQL server module

- [Startup.cs](https://github.com/StardustDL/delights/blob/master/src/Delights.Api/Startup.cs) GraphQL server integrating.
