using System.Diagnostics;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;
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
        var topArtistsResponse = await user.GetTopArtists(new UsersTopItemsRequest(TimeRange.LongTerm)
        {
            Limit = 50
        });

        var artists = topArtistsResponse.Items.Select(artist => new Artist
        {
            Name = artist.Name,
            Genres = string.Join(", ", artist.Genres),
            ImageUrl = artist.Images.FirstOrDefault()?.Url
        }).ToList();

        return View(artists);
    }

    [Route("tracks")]
    public async Task<IActionResult> TopTracks()
    {
        var user = _spotifyClient.UserProfile;
        var topTracksResponse = await user.GetTopTracks(new UsersTopItemsRequest(TimeRange.MediumTerm)
        {
            Limit = 50
        });

        var tracks = topTracksResponse.Items.Select(tracks => new Tracks
        {
            Name = tracks.Name,
            Artist = tracks.Artists,
            ImageUrl = tracks.Album.Images.FirstOrDefault()?.Url
        }).ToList();

        return View(tracks);
    }
    
    [Route("home")]
    public async Task<ViewResult> Index()
    {
        return View();
    }    
    
    [Route("about")]
    public async Task<ViewResult> About()
    {
        return View();
    }
}