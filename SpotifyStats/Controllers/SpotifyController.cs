using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SpotifyStats.Models;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Http; // NuGet for SpotifyApi

namespace SpotifyStats.Controllers;
public class SpotifyController : Controller
{
    private static readonly SpotifyClientConfig DefaultConfig = SpotifyClientConfig.CreateDefault();
    public async Task<ContentResult> GetTrackInfo(string id)
    {
        var config = DefaultConfig
                    .WithToken("BQA-IEanfLkLHcyh0uf-Cpb3Io-xPEQgmPIcjkuvM8si1scowxhEhmDV0-iYoormg07nUR_zWh0YkkefQb79MdZXWKRSv2FhlGu6OzQgwS_aSn_8q2g");
        var spotify = new SpotifyClient(config);
        // var track = await spotify.Tracks.Get("2TpxZ7JUBn3uw46aR7qd6V");
        var playlists = await spotify.Playlists.Get("37i9dQZF1DX0kbJZpiYdZl");
        var list = "";
        foreach (PlaylistTrack<IPlayableItem> item in playlists.Tracks!.Items!)
        {
            if (item.Track is FullTrack track)
            {
                list += track.Name + "\n";
            }
        }
        
        return Content(list);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}