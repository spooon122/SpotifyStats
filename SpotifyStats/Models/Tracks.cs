using SpotifyAPI.Web;

namespace SpotifyStats.Models;

public record Tracks
{
    public string Name { get; set; }
    public List<SimpleArtist> Artist { get; set; }
    public string? ImageUrl { get; set; }
}