using Homesick.UI.Models;
using Homesick.UI.Service.IService;
using Newtonsoft.Json;
using static Homesick.UI.Utility.SD;
using System.Net;
using System.Text;
using System.Net.Http;
using Blazored.SessionStorage;

namespace Homesick.UI.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private ISessionStorageService _sessionStorage;

        public BaseService(IHttpClientFactory httpClientFactory, ISessionStorageService sessionStorageService)
        {
            _httpClientFactory = httpClientFactory;
            _sessionStorage = sessionStorageService;
        }

        public async Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            if (_httpClientFactory == null)
                throw new Exception("HttpClientFactory is null. Ensure it is registered in DI.");

            if (string.IsNullOrEmpty(requestDto.Url))
                throw new Exception("RequestDto URL is null or empty.");

            try
            {
                if (_httpClientFactory == null)
                {
                    throw new Exception("HttpClientFactory is null. Ensure it is registered in the DI container.");
                }
                HttpClient client = _httpClientFactory.CreateClient("HomesickAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                //token
                if (withBearer)
                {
                    var token = await _sessionStorage.GetItemAsync<string>("jwt");
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }

                if (string.IsNullOrEmpty(requestDto.Url))
                {
                    throw new Exception("RequestDto URL is null or empty.");
                }
                message.RequestUri = new Uri(requestDto.Url);

                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;

                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                apiResponse = await client.SendAsync(message);
                
                if (apiResponse == null)
                {
                    throw new Exception("API response is null.");
                }
                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, Message = "Not Found" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, Message = "Access Denied" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, Message = "Unauthorized" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, Message = "Internal Server Error" };
                    case HttpStatusCode.UnsupportedMediaType:
                        return new() { IsSuccess = false, Message = "Unsupported Media Type" };
                    case HttpStatusCode.BadRequest:
                        return new() { IsSuccess = false, Message = "Bad Request" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        if (string.IsNullOrEmpty(apiContent))
                            throw new Exception("API returned an empty response.");

                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    Message = ex.Message.ToString(),
                    IsSuccess = false
                };
                return dto;
            }
        }
    }
}
