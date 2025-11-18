using System.Text.Json.Serialization;

namespace KRBlazor.Application.DTO;

public sealed class FleetResponseDto
{
    [JsonPropertyName("anchorageSize")]
    public DimensionDto AnchorageSize { get; set; } = default!;

    [JsonPropertyName("fleets")]
    public List<FleetSpecificationDto> Fleets { get; set; } = new();
}
