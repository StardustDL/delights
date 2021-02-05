using Cake.Common.IO;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Build;
using Cake.Frosting;

namespace Build
{
    [TaskName("Build")]
    [IsDependentOn(typeof(RestoreTask))]
    public sealed class BuildTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            try
            {
                context.DotNetCoreBuild(Paths.Base.FullPath, new DotNetCoreBuildSettings
                {
                    MSBuildSettings = context.GetMSBuildSettings(),
                });
            }
            catch { }

            context.DotNetCoreBuild(Paths.Base.FullPath, new DotNetCoreBuildSettings
            {
                MSBuildSettings = context.GetMSBuildSettings(),
            });
        }
    }
}