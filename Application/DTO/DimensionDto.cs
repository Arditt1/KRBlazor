using System.Text.Json.Serialization;

namespace KRBlazor.Application.DTO;

public sealed class DimensionDto
{
    [JsonPropertyName("width")]
    public int Width { get; set; }

    [JsonPropertyName("height")]
    public int Height { get; set; }
    public DimensionDto() { }
}
