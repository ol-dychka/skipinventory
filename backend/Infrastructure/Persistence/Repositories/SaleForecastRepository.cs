using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

public class SaleForecastRepository(PsqlDbContext context) : ISaleForecastRepository
{
    private readonly PsqlDbContext _context = context;

    public void Add(SaleForecast saleForecast)
    {
        _context.SaleForecasts.Add(saleForecast);
    }

    public Task<List<SaleForecast>> GetLatest(
        string OrganizationId,
        CancellationToken cancellationToken
    )
    {
        return _context
            .SaleForecasts.Where(sf => sf.OrganizationId == OrganizationId)
            .GroupBy(sf => sf.ProductId)
            .Select(g => new { ProductId = g.Key, ForecastStart = g.Max(sf => sf.ForecastStart) })
            .Join(
                _context.SaleForecasts.Where(sf => sf.OrganizationId == OrganizationId),
                selector => selector,
                sf => new { sf.ProductId, sf.ForecastStart },
                (selector, sf) => sf
            )
            .ToListAsync(cancellationToken);
    }
}
