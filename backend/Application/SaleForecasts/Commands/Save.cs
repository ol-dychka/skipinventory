using System.Net.Http.Json;
using Application.Core;
using Application.Interfaces;
using Application.Models;
using Domain;
using MediatR;

namespace Application.SaleForecasts.Commands;

public class Save
{
    public record Command(HttpContent Content, string OrganizationId)
        : IRequest<Result<List<SaleForecast>>>;

    public class Handler(
        ISaleForecastRepository saleForecastRepository,
        // IProductRepository productRepository,
        IUnitOfWork unitOfWork
    ) : IRequestHandler<Command, Result<List<SaleForecast>>>
    {
        public async Task<Result<List<SaleForecast>>> Handle(
            Command request,
            CancellationToken cancellationToken
        )
        {
            List<SaleForecast> forecasts = [];
            try
            {
                var forecastsData = await request.Content.ReadFromJsonAsync<
                    List<MlServiceSaleForecast>
                >(cancellationToken);
                if (forecastsData == null)
                {
                    return Result<List<SaleForecast>>.Failure("Error processing the data");
                }

                foreach (var data in forecastsData)
                {
                    var forecast = new SaleForecast(
                        data.Sku,
                        request.OrganizationId,
                        data.Id,
                        DateTime.UtcNow,
                        DateTime.UtcNow.AddDays(7),
                        Convert.ToInt32(data.RecommendedOrderQuantity)
                    );

                    saleForecastRepository.Add(forecast);
                    forecasts.Add(forecast);
                }

                await unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Failed to save forecast data for org {request.OrganizationId}" + ex
                );
                return Result<List<SaleForecast>>.Failure("Failed to save forecasts");
            }

            return Result<List<SaleForecast>>.Success(forecasts);
        }
    }
}
