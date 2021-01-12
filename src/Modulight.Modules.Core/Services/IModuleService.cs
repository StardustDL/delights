using System.Threading.Tasks;

namespace Modulight.Modules.Services
{
    public interface IModuleService
    {
    }

    public sealed class EmptyModuleService<T> : IModuleService
    {
    }
}
