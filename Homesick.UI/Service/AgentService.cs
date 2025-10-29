using Homesick.UI.Models;
using Homesick.UI.Service.IService;
using Homesick.UI.Utility;
using System.Runtime.CompilerServices;

namespace Homesick.UI.Service
{
    public class AgentService : IAgentService
    {
        private readonly IBaseService _baseService;
        public AgentService(IBaseService baseService)
        {
            _baseService = baseService;
        }
        public Task<IAsyncEnumerable<string>> StreamRagResponseAsync(string prompt, [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            return null;
        }
    }
}
