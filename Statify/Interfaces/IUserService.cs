using Statify.Models;

namespace Statify.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUserFromSpotifyWithWebApi();

        public Task<PlayListCollection> Get50PlaylistsFromAuthorizedUser();

    }
}
