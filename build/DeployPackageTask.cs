﻿using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Core.IO;
using Cake.Frosting;
using Cake.Common;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.Diagnostics;
using Cake.Common.Tools.DotNetCore.NuGet.Push;
using System;

namespace Build
{
    [TaskName("Deploy-Packages")]
    [IsDependentOn(typeof(PackTask))]
    public sealed class DeployPackageTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            var settings = new DotNetCoreNuGetPushSettings
            {
                SkipDuplicate = true,
            };

            if (context.EnableNuGetPackage)
            {
                context.Log.Information("Publish to NuGet.");

                string nugetToken = context.EnvironmentVariable("NUGET_AUTH_TOKEN", "");

                if (nugetToken is "")
                {
                    throw new Exception("No NUGET_AUTH_TOKEN environment variable setted.");
                }

                settings.ApiKey = nugetToken;
                settings.Source = "https://api.nuget.org/v3/index.json";

                DeployTo(context, settings);
            }
            else
            {
                context.Log.Information("Publish to Azure.");

                string nugetToken = context.EnvironmentVariable("AZ_AUTH_TOKEN", "");

                if (nugetToken is "")
                {
                    throw new Exception("No AZ_AUTH_TOKEN environment variable setted.");
                }

                try
                {
                    context.DotNetCoreNuGetAddSource(RestoreTask.CustomSourceName, new Cake.Common.Tools.DotNetCore.NuGet.Source.DotNetCoreNuGetSourceSettings
                    {
                        Source = "https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json",
                    });
                }
                catch (Exception ex)
                {
                    context.Log.Error(ex.Message);
                }

                context.DotNetCoreNuGetUpdateSource(RestoreTask.CustomSourceName, new Cake.Common.Tools.DotNetCore.NuGet.Source.DotNetCoreNuGetSourceSettings
                {
                    Source = "https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json",
                    UserName = "sparkshine",
                    StorePasswordInClearText = true,
                    Password = nugetToken,
                });

                settings.ApiKey = "az";
                settings.Source = RestoreTask.CustomSourceName;

                DeployTo(context, settings);
            }
        }

        public override void Finally(BuildContext context)
        {
        }

        void DeployTo(BuildContext context, DotNetCoreNuGetPushSettings settings)
        {
            List<string> packageList = new List<string>();

            switch (context.Solution)
            {
                case SolutionType.All:
                    break;
            }

            foreach (var pkgName in packageList)
            {
                var file = context.Globber.GetFiles(Paths.Dist.Packages.CombineWithFilePath(pkgName + context.BuildVersion).FullPath + "*.nupkg").FirstOrDefault();
                if (file is null)
                {
                    context.Log.Warning($"No package {pkgName} found.");
                    continue;
                }
                context.DotNetCoreNuGetPush(file.FullPath, settings);
            }
        }
    }
}