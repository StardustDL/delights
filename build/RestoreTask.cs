using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Tool;
using Cake.Core.IO.Arguments;
using Cake.Frosting;
using Cake.Common.Tools.Chocolatey;

namespace Build
{
    [TaskName("Restore")]
    public class RestoreTask : FrostingTask<BuildContext>
    {
        public const string CustomSourceName = "ownpkgs";

        public override void Run(BuildContext context)
        {
            if (!context.DotNetCoreNuGetHasSource(CustomSourceName))
            {
                try
                {
                    context.DotNetCoreNuGetAddSource(CustomSourceName, new Cake.Common.Tools.DotNetCore.NuGet.Source.DotNetCoreNuGetSourceSettings
                    {
                        Source = "https://sparkshine.pkgs.visualstudio.com/StardustDL/_packaging/feed/nuget/v3/index.json",
                    });
                }
                catch { }
            }

            context.DotNetCoreRestore(context.SolutionFile.FullPath);
            context.DotNetCoreTool("tool", new DotNetCoreToolSettings
            {
                ArgumentCustomization = builder =>
                {
                    builder.Append(new TextArgument("restore"));
                    return builder;
                }
            });
        }
    }
}