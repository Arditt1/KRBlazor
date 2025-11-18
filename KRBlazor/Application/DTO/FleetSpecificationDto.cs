using System.Text.Json.Serialization;

namespace KRBlazor.Application.DTO;

public sealed class FleetSpecificationDto
{
    [JsonPropertyName("singleShipDimensions")]
    public DimensionDto SingleShipDimensions { get; set; } = default!;

    [JsonPropertyName("shipDesignation")]
    public string ShipDesignation { get; set; } = string.Empty;

    [JsonPropertyName("shipCount")]
    public int ShipCount { get; set; }
}
