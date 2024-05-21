using Microsoft.AspNetCore.Mvc;
using SpotifyAPI.Web;
using SpotifyStats.Models;
using ConfigurationManager = System.Configuration.ConfigurationManager;
namespace SpotifyStats.Controllers;

public class LoginController : Controller
{
    private readonly SpotifyClientFactory _spotifyClientFactory;
    private readonly string? _clientId = ConfigurationManager.AppSettings["ClientId"];
    private readonly string? _secretId = ConfigurationManager.AppSettings["SecretId"];

    public LoginController(SpotifyClientFactory spotifyClientFactory)
    {
        _spotifyClientFactory = spotifyClientFactory;
    }

    [HttpGet("auth")]
    public async Task<IActionResult> Login()
    {
        var request = new LoginRequest(new Uri("http://localhost:5252/callback"), _clientId, LoginRequest.ResponseType.Code)
        {
            Scope = new List<string> { Scopes.UserReadEmail, Scopes.UserReadPrivate, Scopes.UserTopRead }
        };
        var uri = request.ToUri();

        return Redirect(uri.ToString());
    }

    [Route("callback")]
    public async Task<RedirectToActionResult> GetCallback(string code)
    {
        var response = await new OAuthClient().RequestToken(
            new AuthorizationCodeTokenRequest(_clientId, _secretId, code, new Uri("http://localhost:5252/callback"))
        );
        var config = SpotifyClientConfig
            .CreateDefault()
            .WithAuthenticator(new AuthorizationCodeAuthenticator(_clientId, _secretId, response));
        _spotifyClientFactory.SetAccessToken(response.AccessToken);
        return RedirectToAction("Index", "Music");
    }
}