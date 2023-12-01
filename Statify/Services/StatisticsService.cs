using Statify.Interfaces;
using Statify.Models;

namespace Statify.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUserService _userService;
        public TrackCollection LikedSongs { get; set; }
        public TrackCollection? TrackCollection { get; set; }
        public ArtistCollection? ArtistCollection { get; set; }
        public List<Artist> Artists { get; set; } = new();
        public List<Artist> Top99Artists { get; set; } = new();
        public Dictionary<string, int> Genres = new();
        public Track? Track { get; set; }

        public StatisticsService(IUserService userService)
        {
            _userService = userService;
        }
        private async Task GetData()
        {
            ArtistCollection = await _userService.GetTopItems<ArtistCollection>("artists", "long_term", 50);

            if (ArtistCollection is not null && ArtistCollection.Items is not null && ArtistCollection.Items.Count >= 50)
            {
                var secondBatch = await _userService.GetTopItems<ArtistCollection>("artists", "long_term", 50, 49);
                ArtistCollection.Items.AddRange(secondBatch.Items ?? Enumerable.Empty<Artist>());
            }

            TrackCollection = await _userService.GetTopItems<TrackCollection>("tracks", "long_term", 50);
        }
        public void ArrangesGenreFrequency()
        {
            if (ArtistCollection is null || ArtistCollection.Items is null)
            {
                return;
            }

            foreach (var artist in ArtistCollection!.Items!)
            {
                Artists.Add(artist);

                if (artist.Genres is null)
                {
                    continue;
                }

                foreach (var genre in artist.Genres!)
                {
                    Genres.TryGetValue(genre, out var count);
                    Genres[genre] = count + 1;
                }
            }
        }
        //public Task<Dictionary<string, int>> CalculateGenreScore()
        //{
            // Genres[0].Value * 1.3 == Score   #1 249 points, 350,

            // Loop trhough X amount of liked songs

            // .Where liked songs x => x.Artist == Genres.Artist == 1.5 extra points
        //}
        public async Task<Dictionary<string, int>> GetCalculatedGenreData()
        {
            await GetData();
            ArrangesGenreFrequency();

            return Genres.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
        }
        public async Task<TrackCollection> GetLikedSongs(int limit, int offset)
        {
            return await _userService.GetTracks<TrackCollection>($"me/tracks?limit={limit}&offset={offset}");
        }
        public async Task<Track> GetTrack(string trackId)
        {
            return await _userService.GetTracks<Track>($"tracks/{trackId}");
        }
        public async Task<TrackCollection> GetSeveralTracks(string trackIds)
        {
            return await _userService.GetTracks<TrackCollection>($"tracks?ids={trackIds}");
        }
    }
}
