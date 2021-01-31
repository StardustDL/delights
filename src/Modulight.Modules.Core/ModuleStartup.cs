using Microsoft.Extensions.DependencyInjection;

namespace Modulight.Modules
{
    /// <summary>
    /// Startup for module.
    /// </summary>
    public interface IModuleStartup
    {
        /// <summary>
        /// Configure the target services.
        /// </summary>
        /// <param name="services"></param>
        void ConfigureServices(IServiceCollection services);
    }

    /// <summary>
    /// Empty implementation for <see cref="IModuleStartup"/>.
    /// </summary>
    public abstract class ModuleStartup : IModuleStartup
    {
        /// <inheritdoc/>
        public virtual void ConfigureServices(IServiceCollection services) { }
    }
}
