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
        public PkceAuthorization? Authentication { get; set; }
        public User? User { get; set; }
        public PlayListCollection? PlaylistCollection { get; set; }

        public UserService(ISpotifyService spotifyService)
        {
            _spotifyService = spotifyService;
        }


        public async Task<User> GetUser()
        {
            return await _spotifyService.SendRequest<User>("me");
        }
        public async Task<PlayListCollection> GetPlaylists(int amount = 20)
        {
            return await _spotifyService.SendRequest<PlayListCollection>("me/playlists");
        }
        public Task<List<Track>> FindForgottenTracks()
        {
            // compare short term - 4 weeks // medium term - 6 months // long term - multiple years

            throw new NotImplementedException();
        }
        public async Task<T> GetTopItems<T>(string itemType, string term)
        {
            return await _spotifyService.SendRequest<T>($"me/top/{itemType}?time_range={term}");
        }
    }
}
