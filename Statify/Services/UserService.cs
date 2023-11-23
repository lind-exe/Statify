using Statify.Models;
using Statify.Interfaces;
using System.Text.Json;

namespace Statify.Services
{
    public class UserService : IUserService
    {
        public PKCEAuthorization? Authentication { get; set; }
        public User? User { get; set; }
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<User> GetUserFromSpotifyWithWebApi()
        {
            Authentication = _httpContextAccessor.HttpContext.Session.GetObjectFromJson<PKCEAuthorization>("User");
            string accessToken = string.Empty;
            if (Authentication is not null)
            {
                accessToken = Authentication.AccessToken;
            }

            using (HttpClient httpClient = new HttpClient())
            {
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
        }

    }
}
