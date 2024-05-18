using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SpotifyStats.Models;
using SpotifyAPI.Web;

namespace SpotifyStats.Controllers;
public class TracksController : Controller
{
    private readonly IConfiguration _configuration;

    public TracksController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    [Route("artist")]
    public async Task<IActionResult> TopArtists()
    {
        var accessToken = HttpContext.Session.GetString("AccessToken");
        if (string.IsNullOrEmpty(accessToken))
        {
            return RedirectToAction("Login", "SpotifyAuth");
        }

        var topTracks = await GetTopTracks(accessToken);
        return View(topTracks);
    }

    private async Task<List<Track>> GetTopTracks(string accessToken)
    {
        var client = new RestClient("https://api.spotify.com/v1/me/top");
        var request = new RestRequest("/tracks", Method.Get); // Явно указываем метод GET
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddParameter("time_range", "medium_term"); // medium_term - последние 6 месяцев

        var response = await client.ExecuteAsync(request);

        if (!response.IsSuccessful)
        {
            // Обработка ошибок
            throw new HttpRequestException($"Ошибка при выполнении запроса к Spotify API: {response.StatusCode} - {response.Content}");
        }

        var content = JsonConvert.DeserializeObject<SpotifyTopTracksResponse>(response.Content);
        return content.Items;
    }
}
public class SpotifyTopTracksResponse
{
    [JsonProperty("items")]
    public List<Track> Items { get; set; }
}

