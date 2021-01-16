using System.Threading.Tasks;

namespace Modulight.Modules.Services
{
    /// <summary>
    /// Specifies the contract for module services
    /// </summary>
    public interface IModuleService
    {
    }

    /// <summary>
    /// Provide empty module service implement
    /// </summary>
    /// <typeparam name="T">Module type</typeparam>
    public sealed class EmptyModuleService<T> : IModuleService
    {
    }
}
