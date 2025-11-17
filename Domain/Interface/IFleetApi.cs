using KRBlazor.Application.DTO;

namespace Domain.Interface;

public interface IFleetApi
{
    Task<FleetResponseDto> GetRandomAsync();
}
