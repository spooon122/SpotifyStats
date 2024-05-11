using System.Text;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using ConfigurationManager = System.Configuration.ConfigurationManager;


namespace SpotifyStats.Controllers;

public class LoginController : Controller
{
    [HttpGet]
    public ActionResult Auth()
    {
        var clientId = ConfigurationManager.AppSettings["ClientId"];
        const string redirectUri = "http://localhost:5252/callback";
        var state = GenerateRandomString(16);
        var scope = "user-read-private user-read-email";
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        
        queryString["response_type"] = "code";
        queryString["client_id"] = clientId;
        queryString["scope"] = scope;
        queryString["redirect_uri"] = redirectUri;
        queryString["state"] = state;
        
        var spotifyAuthUrl = "https://accounts.spotify.com/authorize?" + queryString;
        return Redirect(spotifyAuthUrl);
        
    }
    private string GenerateRandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        var result = new StringBuilder(length);
        for (int i = 0; i < length; i++)
        {
            result.Append(chars[random.Next(chars.Length)]);
        }
        return result.ToString();
    }
}