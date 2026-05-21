using System;
using Domain.StaticClasses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class MlServiceController(IHttpClientFactory factory) : BaseAPIController
{
    private readonly HttpClient _client = factory.CreateClient("mlservice");

    [HttpPost("train/generate")]
    public async Task<IActionResult> Generate()
    {
        if (Role == null || !UserRole.HasResolveJoinRights(Role))
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
        if (Role == null || !UserRole.HasResolveJoinRights(Role))
            return Unauthorized("unsufficient rights");

        var response = await _client.PostAsync("/train/data", null);

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode);
        }

        var responseBody = await response.Content.ReadAsStringAsync();

        return Content(responseBody, "application/json");
    }
}
