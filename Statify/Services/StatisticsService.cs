using Statify.Interfaces;
using Statify.Models;

namespace Statify.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUserService _userService;
        public TrackCollection? TrackCollection { get; set; }
        public ArtistCollection? ArtistCollection { get; set; }
        public List<Artist> Artists { get; set; } = new();
        public List<Artist> Top99Artists { get; set; } = new();
        public Dictionary<string, int> Genres = new();

        public StatisticsService(IUserService userService)
        {
            _userService = userService;
        }
        private async Task GetData()
        {
            if (ArtistCollection is null)
            {
                ArtistCollection = await _userService.GetTopItems<ArtistCollection>("artists", "long_term", 50);

            }
            else
            ArtistCollection = await _userService.GetTopItems<ArtistCollection>("artists", "long_term", 50, 49);

            TrackCollection = await _userService.GetTopItems<TrackCollection>("tracks", "long_term", 50);
        }
        public async Task<Dictionary<string, int>> ToBeDecided()
        {
            await GetData();

            foreach (var artist in ArtistCollection!.Items!)
            {
                Artists.Add(artist);
                foreach (var genre in artist.Genres!)
                {
                    if (Genres.ContainsKey(genre) is false)
                    {
                        Genres.Add(genre, 1);
                    }
                    else
                    {
                        Genres[genre] += 1;
                    }
                }
            }

            await GetData();
            foreach (var artist in ArtistCollection!.Items!)
            {
                Artists.Add(artist);
                foreach (var genre in artist.Genres!)
                {
                    if (Genres.ContainsKey(genre) is false)
                    {
                        Genres.Add(genre, 1);
                    }
                    else
                    {
                        Genres[genre] += 1;
                    }
                }
            }
            return Genres.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
