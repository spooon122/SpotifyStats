using Newtonsoft.Json;

namespace SpotifyStats.Models;

public record Track
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("artists")]
    public List<Artist> Artists { get; set; }

    [JsonProperty("album")]
    public Album Album { get; set; }
}