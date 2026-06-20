using Domain;

namespace Application.Interfaces;

public interface ISaleForecastRepository
{
    public void Add(SaleForecast saleForecast);
    public Task<List<SaleForecast>> GetLatest(
        string OrganizationId,
        CancellationToken cancellationToken
    );
}
