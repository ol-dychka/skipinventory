namespace API.Helpers;

public class Logger
{
    public static void LogHttpRequest(HttpRequestMessage request)
    {
        Console.WriteLine("=== REQUEST ===");
        Console.WriteLine($"{request.Method} {request.RequestUri}");

        foreach (var h in request.Headers)
        {
            Console.WriteLine($"{h.Key}: {string.Join(", ", h.Value)}");
        }

        if (request.Content?.Headers != null)
        {
            foreach (var h in request.Content.Headers)
            {
                Console.WriteLine($"{h.Key}: {string.Join(", ", h.Value)}");
            }
        }
    }

    public static void LogHttpResponse(HttpResponseMessage response)
    {
        Console.WriteLine("=== RESPONSE ===");
        Console.WriteLine($"Status: {(int)response.StatusCode} {response.ReasonPhrase}");

        foreach (var h in response.Headers)
        {
            Console.WriteLine($"{h.Key}: {string.Join(", ", h.Value)}");
        }

        foreach (var h in response.Content.Headers)
        {
            Console.WriteLine($"{h.Key}: {string.Join(", ", h.Value)}");
        }
    }
}
