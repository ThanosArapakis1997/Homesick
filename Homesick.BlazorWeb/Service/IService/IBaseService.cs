using Homesick.BlazorWeb.Models;

namespace Homesick.BlazorWeb.Service.IService
{
    public interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true);
    }
}
