# Delights

![](https://github.com/StardustDL/delights/workflows/CI/badge.svg) ![](https://github.com/StardustDL/delights/workflows/CD/badge.svg) ![](https://img.shields.io/github/license/StardustDL/delights.svg)

Delights is my collection of useful tools in life.

> It's built on a light modular framework [Modulight](./README.framework.md).
> 
> Visit [here](https://github.com/StardustDL/delights/blob/master/README.framework.md) for details about the modular framework.

## Project guide

- [Modulight.Modules.Core](./src/Modulight.Modules.Core/) Core types for Modulight framework.
- [Modulight.Modules.Client.RazorComponents](./src/Modulight.Modules.Client.RazorComponents/) Basic types for razor component client modules.
- [Modulight.Modules.Server.AspNet](./src/Modulight.Modules.Server.AspNet/) Basic types for aspnet server modules.
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
- [ObjectStorage](./src/StardustDL.AspNet.ObjectStorage/) A module for Minio based object storage.
- [IdentityServer](./src/StardustDL.AspNet.IdentityServer/) A module for IdentityServer4 based authorization.
