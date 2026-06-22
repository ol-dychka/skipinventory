using Application.Core;
using Application.Interfaces;
using Domain;
using MediatR;

namespace Application.SaleForecasts.Queries;

public class List
{
    public record Query(string OrganizationId) : IRequest<Result<List<SaleForecast>>>;

    public class Handler(ISaleForecastRepository repository)
        : IRequestHandler<Query, Result<List<SaleForecast>>>
    {
        public async Task<Result<List<SaleForecast>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            List<SaleForecast> forecasts;
            try
            {
                forecasts = await repository.GetLatest(request.OrganizationId, cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Result<List<SaleForecast>>.Failure("failed to fetch forecasts");
            }

            return Result<List<SaleForecast>>.Success(forecasts);
        }
    }
}
