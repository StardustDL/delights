using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Frosting;
using Cake.Common.Tools.DotNetCore.Pack;

namespace Build
{
    [TaskName("Pack")]
    [IsDependentOn(typeof(BuildTask))]
    public sealed class PackTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            context.CleanDirectory(Paths.Dist.Packages);
            context.DotNetCorePack(context.SolutionFile.FullPath, new DotNetCorePackSettings
            {
                OutputDirectory = Paths.Dist.Packages,
                MSBuildSettings = context.GetMSBuildSettings(),
            });
        }
    }
}