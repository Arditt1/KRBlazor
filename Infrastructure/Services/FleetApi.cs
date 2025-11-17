using Domain.Interface;
using KRBlazor.Application.DTO;
using System.Net.Http.Json;

namespace Infrastructure.Services;

public class FleetApi : IFleetApi
{
    private readonly HttpClient _httpClient;

    public FleetApi(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<FleetResponseDto> GetRandomAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<FleetResponseDto>("fleets/random");
        return response ?? throw new InvalidOperationException("Failed to load fleet data from the API.");
    }
}

