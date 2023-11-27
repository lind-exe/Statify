using Statify.Models;
using Statify.Interfaces;
using System.Text.Json;

namespace Statify.Services
{
    public class UserService : IUserService
    {
        public PKCEAuthorization? Authentication { get; set; }
        public User? User { get; set; }
        public PlayListCollection? PlaylistCollection { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetUserFromSpotifyWithWebApi()
        {
            Authentication = _httpContextAccessor.HttpContext!.Session.GetObjectFromJson<PKCEAuthorization>("User");

            if (Authentication is null)
            {
                throw new InvalidOperationException("User authentication is missing.");
            }

            string accessToken = Authentication.AccessToken;

            using HttpClient httpClient = new();
            httpClient.BaseAddress = new Uri("https://api.spotify.com/v1/");

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            string endpoint = "me";

            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                User = JsonSerializer.Deserialize<User>(content);
                return User ?? throw new NullReferenceException("User was null");
            }
            else
            {
                return User.Empty;
            }
        }
        public async Task<PlayListCollection> Get50PlaylistsFromAuthorizedUser()
        {
            Authentication = _httpContextAccessor.HttpContext!.Session.GetObjectFromJson<PKCEAuthorization>("User");

            if (Authentication is null)
            {
                throw new InvalidOperationException("User authentication is missing.");
            }

            string accessToken = Authentication.AccessToken;



            using HttpClient httpClient = new();
            httpClient.BaseAddress = new Uri("https://api.spotify.com/v1/");

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

            string endpoint = "me/playlists";

            HttpResponseMessage response = await httpClient.GetAsync(endpoint);

            if (response.IsSuccessStatusCode)
            {

                string content = await response.Content.ReadAsStringAsync();
                PlaylistCollection = JsonSerializer.Deserialize<PlayListCollection>(content);
                return PlaylistCollection ?? throw new NullReferenceException("Playlist was null after deserialization.");
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve user information. Status code: {response.StatusCode}");
            }
        }
    }
}
