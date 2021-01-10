# Delights

![](https://github.com/StardustDL/delights/workflows/CI/badge.svg) ![](https://github.com/StardustDL/delights/workflows/CD/badge.svg) ![](https://img.shields.io/github/license/StardustDL/delights.svg) [![](https://buildstats.info/nuget/Delights.Modules.Core)](https://www.nuget.org/packages/Delights.Modules.Core/)

[Delights](https://github.com/StardustDL/delights) is a light modular framework aimed to be low intrusive based on dependency injection for .NET 5, ASP.NET Core and Blazor.

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

## Project

- [Delights.Modules.Core](./src/Delights.Modules.Core/) Core types for modular framework.
- [Delights.Modules.Client](./src/Delights.Modules.Client/) Basic types for client modules.
- [Delights.Modules.Server.GraphQL](./src/Delights.Modules.Server.GraphQL/) Basic types for graphql server modules.
- [Delights.UI](./src/Delights.UI/) UI hosting for modules.
- [Delights.Client](./src/Delights.Client/) Blazor Server hosting.
- [Delights.Client.WebAssembly](./src/Delights.Client.WebAssembly/) Blazor WebAssembly hosting.
- [Hello module](./src/modules/hello/) A demo module.

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
- [Program.cs](./src/Delights.Client/Program.cs) & [Startup.cs](./src/Delights.Client/Startup.cs) Blazor Server hosting.
- [_Host.cshtml](./src/Delights.Client/Pages/_Host.cshtml) Server prerendering for JS/CSS resources.
- [Program.cs](./src/Delights.Client.WebAssembly/Program.cs) Blazor WebAssembly hosting.

### Use a GraphQL server module

- [Program.cs](./src/Delights.Api/Program.cs) & [Startup.cs](./src/Delights.Api/Startup.cs) GraphQL server integrating.
