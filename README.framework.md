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
    .BridgeGraphQLServerModuleToAspNet(postMapEndpoint: (module, builder) =>
    {
        builder.RequireCors(cors =>
        {
            cors.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
    })
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

var aspnetModuleHost = app.ApplicationServices.GetAspNetServerModuleHost();
aspnetModuleHost.UseMiddlewares(app);

app.UseEndpoints(endpoints =>
{
    // other mapper, eg:
    endpoints.MapControllers();
    // modules mapper
    aspnetModuleHost.MapEndpoints(endpoints);
});
```

3. For razor components, add `ResourceDeclare` component to App.razor to load UI resources.

```razor
<Modulight.Modules.Client.RazorComponents.UI.ResourceDeclare />
```

This component will find all resources defined in modules, and render HTML tags for them.

This works for normal cases, but if you use WebAssembly target, no prerenderring, and the component library need the javascript files to be loaded initially. You can use the following codes to load resources manually.

```cs
// WebAssemblyHostBuilder builder;
await using(var provider = builder.Services.BuildServiceProvider())
{
    await provider.GetRazorComponentClientModuleHost().LoadResources();
}
await builder.Build().RunAsync();
```

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
- [App.razor](./src/Delights.UI/App.razor) Lazy loading for js/css/sassemblies when routing.
- [UIModule.cs](./src/Delights.UI/UIModule.cs) Definition of JS/CSS resources.
- [ModuleSetup.cs](./src/Delights.Client.Shared/ModuleSetup.cs) Use modules in client.
- [Startup.cs](./src/Delights.Client/Startup.cs) Blazor Server hosting.
- [Program.cs](./src/Delights.Client.WebAssembly/Program.cs) Blazor WebAssembly hosting.
- [index.html](./src/Delights.Client.WebAssembly/wwwroot/index.html) Clean index.html.

### Use a GraphQL server module

- [Startup.cs](./src/Delights.Api/Startup.cs) GraphQL server integrating.
