using System.Threading.Tasks;

namespace Delights.Modules.Services
{
    public interface IModuleService
    {
        public Task Initialize() => Task.CompletedTask;
    }

    public sealed class EmptyModuleService<T> : IModuleService
    {
    }
}
