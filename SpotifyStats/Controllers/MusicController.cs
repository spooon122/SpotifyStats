using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using SpotifyStats.Models;
using SpotifyAPI.Web;

namespace SpotifyStats.Controllers;

public class MusicController : Controller
{
    private readonly SpotifyClient _spotifyClient;

    public MusicController(SpotifyClient spotifyClient)
    {
        _spotifyClient = spotifyClient;
    }

    [Route("artist")]
    public async Task<IActionResult> TopArtists()
    {
        var user = _spotifyClient.UserProfile;
        var topArtistsResponse = await user.GetTopArtists(new UsersTopItemsRequest(TimeRange.LongTerm));

        var artists = topArtistsResponse.Items.Select(artist => new Artist
        {
            Name = artist.Name,
            ImageUrl = artist.Images.FirstOrDefault()?.Url
        }).ToList();

        return View(artists);
    }
}
