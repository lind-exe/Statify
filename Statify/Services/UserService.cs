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
        /// <summary>
        /// Retrieves user from spotify
        /// </summary>
        /// <returns>neccessary data for authorization</returns>
        public async Task<User> GetUser()
        {
            return await _spotifyService.SendRequest<User>("me");
        }
        /// <summary>
        /// Sends playlist request to spotify, int amount default = 20, max = 50
        /// </summary>
        /// <param name="amount"></param>
        /// <returns>playlist object</returns>
        public async Task<PlaylistResponse> GetPlaylists(int amount = 20)
        {
            return await _spotifyService.SendRequest<PlaylistResponse>("me/playlists");
        }
        /// <summary>
        /// Sends top items request to spotify, either artists or tracks specified by type and paramaters. amount default = 20, max = 50. same with offset.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemType"></param>
        /// <param name="term"></param>
        /// <param name="amount"></param>
        /// <param name="offset"></param>
        /// <returns>object that contains a list of respective type</returns>
        public async Task<T> GetTopItems<T>(string itemType, string term, int amount = 20, int offset = 0)
        {
            return await _spotifyService.SendRequest<T>($"me/top/{itemType}?time_range={term}&limit={amount}&offset={offset}");
        }
        /// <summary>
        /// Sends track request to spotify
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint"></param>
        /// <returns>list of tracks</returns>
        public async Task<T> GetTracks<T>(string endpoint)
        {
            return await _spotifyService.SendRequest<T>(endpoint);
        }
        /// <summary>
        /// Sends artist request to spotify, specify each artist with and id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>array of artists</returns>
        public async Task<ArtistData.ArtistArtists> GetArtists(string ids)
        {
            return await _spotifyService.SendRequest<ArtistData.ArtistArtists>($"artists?ids={ids}");
        }
        /// <summary>
        /// Sends audio feature request to spotify, specify each track with an id
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>audio feature array</returns>
        public async Task<AudioFeatureCollection> GetAudioFeatures(string ids)
        {
            return await _spotifyService.SendRequest<AudioFeatureCollection>($"audio-features?ids={ids}");
        }
    }
}
