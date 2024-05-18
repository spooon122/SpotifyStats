// using System.Text.Json;
// using Microsoft.AspNetCore.Mvc;
// using SpotifyAPI.Web;
// using SpotifyStats.Models;
// using ConfigurationManager = System.Configuration.ConfigurationManager;
//
//
// namespace SpotifyStats.Controllers;
//
// public class LoginController : Controller
// {
//     private readonly SpotifyClientFactory _spotifyClientFactory;
//     private readonly string? _clientId = ConfigurationManager.AppSettings["ClientId"];
//     private readonly string? _secretId = ConfigurationManager.AppSettings["SecretId"];
//
//     public LoginController(SpotifyClientFactory spotifyClientFactory)
//     {
//         _spotifyClientFactory = spotifyClientFactory;
//     }
//
//     [Route("auth")]
//     public async Task<RedirectResult> Auth()
//     {
//         // const string redirectUri = "http://localhost:5252/callback";
//         // var state = new GenerateRandomString().GenerateRandString(16);
//         // var scope = "user-read-private user-read-email";
//         // var queryString = HttpUtility.ParseQueryString(string.Empty);
//         //
//         // queryString["response_type"] = "code";
//         // queryString["client_id"] = _clientId;
//         // queryString["scope"] = scope;
//         // queryString["redirect_uri"] = redirectUri;
//         // queryString["state"] = state;
//         //
//         // var spotifyAuthUrl = "https://accounts.spotify.com/authorize?" + queryString;
//         // return Redirect(spotifyAuthUrl);
//         var loginRequest = new LoginRequest(
//             new Uri("http://localhost:5252/callback"),
//             _clientId,
//             LoginRequest.ResponseType.Code
//         )
//         {
//             Scope = new[] { Scopes.UserReadPrivate, Scopes.UserReadEmail, Scopes.UserTopRead, Scopes.UserReadRecentlyPlayed}
//         };
//         var uri = loginRequest.ToUri();
//         
//         return Redirect(uri.ToString());
//         
//     }
//     [Route("callback")]
//     public async Task Callback(string code)
//     {
//         var response = await new OAuthClient().RequestToken(
//             new AuthorizationCodeTokenRequest(_clientId, _secretId, code, new Uri("http://localhost:5252/artist/")));
//         _spotifyClientFactory.SetAccessToken(response.AccessToken);
//     }
// }

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
        var clientId = "d2499ddf4a7644b9b670dd31c929761b";
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
        var clientId = "d2499ddf4a7644b9b670dd31c929761b";
        var clientSecret = "e792ecb51ec5457bb0bb89d48d031660";
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