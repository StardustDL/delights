using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Modulight.Modules
{
    public interface IModuleStartup
    {
        Task ConfigureServices(IServiceCollection services);
    }

    public abstract class ModuleStartup : IModuleStartup
    {
        public virtual Task ConfigureServices(IServiceCollection services) => Task.CompletedTask;
    }
}
