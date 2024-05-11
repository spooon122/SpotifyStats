using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SpotifyStats.Models;
using SpotifyAPI.Web; // NuGet for SpotifyApi

namespace SpotifyStats.Controllers;

public class SpotifyController : Controller
{
    public async Task<ContentResult> GetTrackInfo(string id)
    {
        var spotify = new SpotifyClient("Access Token");
        var track = await spotify.Tracks.Get(id);
        return Content(track.Name);
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