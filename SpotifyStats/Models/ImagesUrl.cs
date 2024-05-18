using Newtonsoft.Json;

namespace SpotifyStats.Models;

public record ImagesUrl
{
    [JsonProperty("url")]
    public string Url { get; set; }
}