using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlogTalks.Application.Abstractions;
using BlogTalks.EmailSenderApi.DTO;

namespace BlogTalks.Infrastructure.Messaging;

public class MessagingServiceHttp : IMessagingService
{
    private readonly HttpClient _httpClient;

    public MessagingServiceHttp(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    public async Task Send(EmailDTO emailDto)
    {
        var json = JsonSerializer.Serialize(emailDto);

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("/send", content);

        response.EnsureSuccessStatusCode();
    }
}