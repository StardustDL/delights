using Cake.Common;
using Cake.Common.Build;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Core;
using Cake.Core.Tooling;
using Cake.Frosting;
using System;

namespace Build
{
    public class BuildContext : FrostingContext
    {
        const string Version = "0.0.3";

        public string CommitMessage { get; set; }

        public bool EnableDocument { get; set; }

        public bool EnableNuGetPackage { get; set; }

        public bool EnableImage { get; set; }

        public string BuildConfiguration { get; set; }

        public string BuildVersion { get; set; }

        public DotNetCoreMSBuildSettings GetMSBuildSettings()
        {
            return new DotNetCoreMSBuildSettings().SetVersion(BuildVersion)
                                                  .SetConfiguration(BuildConfiguration);
        }

        public BuildContext(ICakeContext context)
            : base(context)
        {
            CommitMessage = context.Argument("commit", "");
            if (CommitMessage is "")
            {
                CommitMessage = context.EnvironmentVariable("COMMIT_MESSAGE", "");
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
                        BuildVersion += $"-preview.{actions.Environment.Workflow.RunNumber}";
                    }
                    else if (actions.Environment.Workflow.Workflow == "Release")
                    {
                        EnableDocument = true;
                        EnableNuGetPackage = true;
                        EnableImage = true;
                    }
                }
            }
        }
    }
}