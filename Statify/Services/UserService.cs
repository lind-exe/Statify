using Statify.Models;
using Statify.Interfaces;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Razor.Infrastructure;

namespace Statify.Services
{
    public class UserService : IUserService
    {
        private readonly ISpotifyService _spotifyService;
        public UserService(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }


        public async Task<User> GetUser()
        {
            return await _spotifyService.SendRequest<User>("me");
        }
        public async Task<PlaylistResponse> GetPlaylists(int amount = 20)
        {
            return await _spotifyService.SendRequest<PlaylistResponse>("me/playlists");
        }
        public async Task<T> GetTopItems<T>(string itemType, string term, int amount = 20, int offset = 0)
        {
            return await _spotifyService.SendRequest<T>($"me/top/{itemType}?time_range={term}&limit={amount}&offset={offset}");
        }
        public async Task<T> GetTracks<T>(string endpoint)
        {
            return await _spotifyService.SendRequest<T>(endpoint);
        }
        public async Task<ArtistData.ArtistArtists> GetArtists(string ids)
        {
            return await _spotifyService.SendRequest<ArtistData.ArtistArtists>($"artists?ids={ids}");
        }
        public async Task<AudioFeatureCollection> GetAudioFeatures(string ids)
        {
            return await _spotifyService.SendRequest<AudioFeatureCollection>($"audio-features?ids={ids}");
        }
    }
}
