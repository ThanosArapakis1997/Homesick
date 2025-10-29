using System.Runtime.CompilerServices;

namespace Homesick.UI.Service.IService
{
    public interface IAgentService
    {
        Task<IAsyncEnumerable<string>> StreamRagResponseAsync(string prompt, [EnumeratorCancellation] CancellationToken cancellationToken = default);
    }
}
