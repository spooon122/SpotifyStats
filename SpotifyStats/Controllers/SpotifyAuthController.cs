

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SpotifyStats.Models;

namespace SpotifyStats.Controllers;

public class SpotifyAuthController : Controller
{

    [HttpGet]
    [Route("auth")]
    public IActionResult Login()
    {
        var clientId = "clientId";
        var redirectUri = "http://localhost:5252/callback";
        var scopes = "user-read-private user-read-email user-top-read";
        var state = new GenerateRandomString().GenerateRandString(16);
        HttpContext.Session.SetString("authState", state);

        var authorizationUrl = $"https://accounts.spotify.com/authorize?response_type=code&client_id={clientId}&scope={scopes}&redirect_uri={redirectUri}&state={state}";

        return Redirect(authorizationUrl);
    }

    [Route("callback")]
    public async Task<IActionResult> Callback(string code)
    {
        var clientId = "clientId";
        var clientSecret = "clientSecret";
        var redirectUri = "http://localhost:5252/callback";

        var client = new RestClient("https://accounts.spotify.com");
        var request = new RestRequest("/api/token", Method.Post);
        request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
        request.AddParameter("grant_type", "authorization_code");
        request.AddParameter("code", code);
        request.AddParameter("redirect_uri", redirectUri);
        request.AddParameter("client_id", clientId);
        request.AddParameter("client_secret", clientSecret);
        request.AddParameter("state", HttpContext.Session.GetString("stateAuth"));

        var response = await client.ExecuteAsync(request);
        var content = JsonConvert.DeserializeObject<SpotifyTokenResponse>(response.Content);

        // Сохраните токены в сессии или базе данных
        HttpContext.Session.SetString("AccessToken", content?.AccessToken!);
        HttpContext.Session.SetString("RefreshToken", content?.RefreshToken!);

        return RedirectToAction("TopArtists", "Tracks");
    }
}

public class SpotifyTokenResponse
{
    [JsonProperty("access_token")]
    public string? AccessToken { get; set; }

    [JsonProperty("refresh_token")]
    public string RefreshToken { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }
}
