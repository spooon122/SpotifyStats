namespace SpotifyStats.Models;

public record Artist
{
    public string Name { get; set; }
    public string Genres { get; set; }
    public string? ImageUrl { get; set; }
}