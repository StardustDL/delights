﻿using Cake.Common;
using Cake.Common.Build;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;
using Cake.Frosting;
using System;
using System.Collections.Generic;

namespace Build
{
    public enum SolutionType
    {
        None,
        All,
        Modulight,
        Delights,
    }

    public class BuildContext : FrostingContext
    {
        const string Version = "0.0.4";

        const int BuildRunNumberOffset = 134;

        public string CommitMessage { get; set; }

        public bool EnableDocument { get; set; }

        public bool EnableNuGetPackage { get; set; }

        public bool EnableImage { get; set; }

        public string BuildConfiguration { get; set; }

        public string BuildVersion { get; set; }

        public bool Release { get; set; }

        public SolutionType Solution { get; set; }

        public IEnumerable<FilePath> SolutionFiles => Solution switch
        {
            SolutionType.Modulight => new FilePath[] { Paths.ModulightSolution },
            SolutionType.Delights => new FilePath[] { Paths.DelightsSolution },
            _ => Paths.Solutions,
        };

        public DotNetCoreMSBuildSettings GetMSBuildSettings()
        {
            return new DotNetCoreMSBuildSettings().SetVersion(BuildVersion)
                                                  .SetConfiguration(BuildConfiguration);
        }

        public BuildContext(ICakeContext context)
            : base(context)
        {
            Release = context.HasArgument("release");

            CommitMessage = context.Argument("commit", "");
            if (CommitMessage is "")
            {
                CommitMessage = context.EnvironmentVariable("COMMIT_MESSAGE", "");
            }

            if (CommitMessage.Contains("[modulight]", StringComparison.InvariantCultureIgnoreCase))
            {
                Solution = SolutionType.Modulight;
            }
            else
            {
                Solution = SolutionType.All;
            }
            Solution = context.Argument("solution", "").ToLowerInvariant() switch
            {
                "modulight" => SolutionType.Modulight,
                "delights" => SolutionType.Delights,
                "all" => SolutionType.All,
                _ => Solution,
            };

            foreach (var item in SolutionFiles)
            {
                context.Log.Information($"Selected solution: {item.FullPath}");
            }

            BuildConfiguration = context.Argument("configuration", "Release");
            EnableDocument = CommitMessage.Contains("/docs");
            EnableNuGetPackage = CommitMessage.Contains("/pkgs");
            EnableImage = CommitMessage.Contains("/imgs");

            BuildVersion = context.Argument("build-version", "");
            if (BuildVersion is "")
            {
                BuildVersion = context.EnvironmentVariable("BUILD_VERSION", Version);
            }
            {
                var actions = context.GitHubActions();
                if (actions.IsRunningOnGitHubActions)
                {
                    if (actions.Environment.Workflow.Workflow == "CI")
                    {
                        BuildVersion += $"-preview.{Math.Max(1, actions.Environment.Workflow.RunNumber - BuildRunNumberOffset)}";
                    }
                    else if (actions.Environment.Workflow.Workflow == "Release")
                    {
                        Release = true;
                    }
                }
            }

            EnableDocument = EnableDocument || Release;
            EnableNuGetPackage = EnableNuGetPackage || Release;
            EnableImage = EnableImage || Release;
        }
    }
}