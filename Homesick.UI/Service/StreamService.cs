using Blazored.SessionStorage;
using Homesick.UI.Models;
using Homesick.UI.Service.IService;
using Newtonsoft.Json;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Homesick.UI.Service
{
    public class StreamService : IStreamService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private ISessionStorageService _sessionStorage;

        public StreamService(IHttpClientFactory httpClientFactory, ISessionStorageService sessionStorageService)
        {
            _httpClientFactory = httpClientFactory;
            _sessionStorage = sessionStorageService;
        }
        public async IAsyncEnumerable<AgentResponseDto?> SendAsync(RequestDto requestDto, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            if (_httpClientFactory == null)
                throw new Exception("HttpClientFactory is null. Ensure it is registered in DI.");

            if (string.IsNullOrEmpty(requestDto.Url))
                throw new Exception("RequestDto URL is null or empty.");

            HttpClient client = _httpClientFactory.CreateClient("HomesickAPI");
            HttpRequestMessage message = new();
            message.Headers.Add("Accept", "application/json");
            message.Method = HttpMethod.Post;

            message.RequestUri = new Uri(requestDto.Url);

            if (requestDto.Data != null)
            {
                message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
            }

            HttpResponseMessage? response = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
            response.EnsureSuccessStatusCode();

            var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var reader = new StreamReader(stream);

            while (!reader.EndOfStream && !cancellationToken.IsCancellationRequested)
            {
                var line = await reader.ReadLineAsync();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith(":"))
                    continue;

                if (line.StartsWith("data: "))
                {
                    var jsonData = line.Substring(6).Trim();

                    var data = System.Text.Json.JsonSerializer.Deserialize<AgentResponseDto>(jsonData);
                    if (data != null) yield return data;
                    // Force UI update
                    await Task.Delay(1, cancellationToken);

                    if (data?.Done == true) yield break;
                }
                else
                {
                    Console.WriteLine($"Unexpected line: {line}");
                }
            }          
        }
    }
}
