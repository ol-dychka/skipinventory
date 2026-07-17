namespace API.Middleware;

public class WakeUpHandler : DelegatingHandler
{
    private static DateTime _lastWakeUp = DateTime.MinValue;
    private static readonly SemaphoreSlim Gate = new(1, 1);
    private static readonly TimeSpan WakeUpInterval = TimeSpan.FromMinutes(10);

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        if (DateTime.UtcNow - _lastWakeUp > WakeUpInterval)
        {
            await Gate.WaitAsync(cancellationToken);
            try
            {
                if (DateTime.UtcNow - _lastWakeUp > WakeUpInterval)
                {
                    using var healthRequest = new HttpRequestMessage(HttpMethod.Get, "/health");
                    try
                    {
                        using var healthResponse = await base.SendAsync(
                            healthRequest,
                            cancellationToken
                        );
                        if (healthResponse.IsSuccessStatusCode)
                            _lastWakeUp = DateTime.UtcNow;
                    }
                    catch
                    {
                        // ignore - real request below will hit resilience/retry anyway
                    }
                }
            }
            finally
            {
                Gate.Release();
            }
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
