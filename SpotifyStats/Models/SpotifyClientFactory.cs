using SpotifyAPI.Web;

namespace SpotifyStats.Models;

public class SpotifyClientFactory
{
    private string accessToken;
    public void SetAccessToken(string token)
    {
        accessToken = token;
    }

    public SpotifyClient CreateClient()
    {
        return new SpotifyClient(accessToken);
    }
}