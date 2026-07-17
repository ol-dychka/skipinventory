using System;
using System.Net;
using System.Text;
using System.Text.Json;
using API.DTOs.MlService;
using API.DTOs.Responses;
using API.Helpers;
using Application.Core;
using Application.SaleForecasts.Commands;
using Application.SaleForecasts.Queries;
using Application.SaleRecords.Queries;
using Domain.StaticClasses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace API.Controllers;

public class MlServiceController(IHttpClientFactory factory) : BaseAPIController
{
    private readonly HttpClient _client = factory.CreateClient("mlservice");

    [HttpPost("train/generate")]
    public async Task<IActionResult> Generate()
    {
        if (Role == null || !UserRole.HasMLServiceRights(Role))
            return Unauthorized("unsufficient rights");

        var request = new HttpRequestMessage(HttpMethod.Post, "/train/generate")
        {
            Version = HttpVersion.Version11,
            VersionPolicy = HttpVersionPolicy.RequestVersionExact,
        };

        // var request = new HttpRequestMessage(HttpMethod.Get, "/health")
        // {
        //     Version = HttpVersion.Version11,
        //     VersionPolicy = HttpVersionPolicy.RequestVersionExact,
        // };

        Logger.LogHttpRequest(request);

        HttpResponseMessage response;

        try
        {
            response = await _client.SendAsync(request);
        }
        catch (Exception ex)
        {
            Console.WriteLine("HTTP CALL FAILED:");
            Console.WriteLine(ex.ToString());
            throw;
        }

        Logger.LogHttpResponse(response);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            return StatusCode((int)response.StatusCode, error);
        }

        var responseBody = await response.Content.ReadAsStringAsync();

        return Content(responseBody, "application/json");
    }

    [HttpPost("train/data")]
    public async Task<IActionResult> Train()
    {
        if (Role == null || !UserRole.HasMLServiceRights(Role))
            return Unauthorized("unsufficient rights");

        var response = await _client.PostAsync("/train/data", null);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode);
        }

        var responseBody = await response.Content.ReadAsStringAsync();

        return Content(responseBody, "application/json");
    }

    [HttpPost("forecast")]
    public async Task<IActionResult> Forecast()
    {
        // authorization
        if (Role == null || !UserRole.HasMLServiceRights(Role))
            return Unauthorized("unsufficient rights");
        if (OrganizationId == null)
            return Unauthorized("token does not exist");

        // get sales data
        var salesDataResponse = await Mediator.Send(new All.Query(OrganizationId));
        if (!salesDataResponse.IsSuccess || salesDataResponse.Value == null)
            return Unauthorized(salesDataResponse.Error);

        List<SaleRecordsDto> salesDataDto =
        [
            .. salesDataResponse
                .Value.GroupBy(sr => sr.ProductId)
                .Select(g => new SaleRecordsDto([.. g])),
        ];
        var salesDataJson = JsonSerializer.Serialize(salesDataDto);
        var salesDataContent = new StringContent(salesDataJson, Encoding.UTF8, "application/json");

        Console.WriteLine("SALES DATA:" + salesDataJson);

        // ml service
        var mlServiceResponse = await _client.PostAsync("/predict/batch", salesDataContent);

        if (!mlServiceResponse.IsSuccessStatusCode)
        {
            return StatusCode((int)mlServiceResponse.StatusCode);
        }

        // persist in db and return a list
        var response = await Mediator.Send(
            new Save.Command(mlServiceResponse.Content, OrganizationId)
        );
        if (!response.IsSuccess || response.Value == null)
        {
            return Unauthorized(response.Error);
        }

        var forecastsDto = response.Value.Select(f => new SaleForecastDto(f));

        return Ok(forecastsDto);
    }

    [HttpGet("list")]
    public async Task<IActionResult> List()
    {
        if (OrganizationId == null)
            return Unauthorized("token does not exist");

        var response = await Mediator.Send(new List.Query(OrganizationId));
        if (!response.IsSuccess || response.Value == null)
        {
            return Unauthorized(response.Error);
        }

        var forecastsDto = response.Value.Select(f => new SaleForecastDto(f));

        return Ok(forecastsDto);
    }
}
