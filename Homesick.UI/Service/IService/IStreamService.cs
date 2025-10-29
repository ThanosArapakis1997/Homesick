using Homesick.UI.Models;

namespace Homesick.UI.Service.IService
{
    public interface IStreamService
    {
        IAsyncEnumerable<AgentResponseDto?> SendAsync(RequestDto requestDto, CancellationToken cancellationToken);
    }
}
