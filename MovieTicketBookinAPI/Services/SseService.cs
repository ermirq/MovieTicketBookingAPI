public interface ISseService
{
    Task SendEventAsync(object data);
}

public class SseService : ISseService
{
    private readonly List<HttpResponse> _clients = new();

    public void AddClient(HttpResponse response)
    {
        _clients.Add(response);
    }

    public void RemoveClient(HttpResponse response)
    {
        _clients.Remove(response);
    }

    public async Task SendEventAsync(object data)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(data);

        foreach (var client in _clients.ToList())
        {
            try
            {
                await client.WriteAsync($"data: {json}\n\n");
                await client.Body.FlushAsync();
            }
            catch
            {
                _clients.Remove(client);
            }
        }
    }
}
