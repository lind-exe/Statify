using Statify.Models;

namespace Statify.Interfaces
{
    public interface IUserService // All api calls spotify receives originates from this interface
    {
        /// <summary>
        /// Returns a authorized user from Spotify
        /// </summary>
        /// <returns>A deserialized User from json</returns>
        public Task<User> GetUser();

        /// <summary>
        /// User selects # of playlists to receive from API
        /// User must be authorized
        /// </summary>
        /// <param name="amount">number of playlists to return, default 20, max 50</param>
        /// <returns>A deserialized PlayListCollection from json</returns>
        public Task<PlayListCollection> GetPlaylists(int amount = 20);

    }
}
