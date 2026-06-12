using System;
using System.Text;
using System.Text.Json;
using Application.SaleRecords.Queries;
using Domain.StaticClasses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MlServiceController(IHttpClientFactory factory) : BaseAPIController
{
    private readonly HttpClient _client = factory.CreateClient("mlservice");

    [HttpPost("train/generate")]
    public async Task<IActionResult> Generate()
    {
        if (Role == null || !UserRole.HasMLServiceRights(Role))
            return Unauthorized("unsufficient rights");

        var response = await _client.PostAsync("/train/generate", null);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode);
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
        if (Role == null || !UserRole.HasMLServiceRights(Role))
            return Unauthorized("unsufficient rights");
        if (OrganizationId == null)
            return Unauthorized("token does not exist");

        var salesDataResponse = await Mediator.Send(new All.Query(OrganizationId));
        var salesDataJson = JsonSerializer.Serialize(salesDataResponse.Value);
        var salesDataContent = new StringContent(salesDataJson, Encoding.UTF8, "application/json");

        var mlServiceResponse = await _client.PostAsync("/forecast", salesDataContent);

        if (!mlServiceResponse.IsSuccessStatusCode)
        {
            return StatusCode((int)mlServiceResponse.StatusCode);
        }

        var responseBody = await mlServiceResponse.Content.ReadAsStringAsync();

        return Content(responseBody, "application/json");
    }
}
