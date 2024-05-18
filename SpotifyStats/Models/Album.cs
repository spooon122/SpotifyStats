using Newtonsoft.Json;
namespace SpotifyStats.Models;

public record Album
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("images")]
    public List<ImagesUrl> Images { get; set; }
}