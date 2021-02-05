using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Tool;
using Cake.Core.IO.Arguments;
using Cake.Frosting;

namespace Build
{
    [TaskName("Restore")]
    public class RestoreTask : FrostingTask<BuildContext>
    {
        public override void Run(BuildContext context)
        {
            context.DotNetCoreRestore();
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