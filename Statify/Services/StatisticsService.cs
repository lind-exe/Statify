using Statify.Interfaces;
using Statify.Models;
using System.Linq;

namespace Statify.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUserService _userService;
        public TrackData.LikedTracks? LikedSongs { get; set; }
        public ArtistData.ArtistArtists? LikedArtists { get; set; } = new();
        public ArtistCollection? ArtistCollection { get; set; }
        public List<Artist> TopArtists { get; set; } = new();

        public Dictionary<string, int> GenresFromTopArtists = new();
        public Dictionary<string, int> GenresFromLikedSongs = new();


        public StatisticsService(IUserService userService)
        {
            _userService = userService;
            LikedArtists!.Artists = new();
        }
        public async Task<Dictionary<string, int>> GetCalculatedGenreData()
        {
            await GetData();

            // For GenresFromTopArtists
            ArrangesGenreFrequencyForTopList(ArtistCollection?.Artists, GenresFromTopArtists);

            await AddArtistsFromLikedSongs();
            await BuildsStringToGetSeveralArtists();
            // For GenresFromLikedSongs
            ArrangesGenreFrequencyForTopList(LikedArtists?.Artists, GenresFromLikedSongs);
            var genres = await CompareAndCalculateScore();

            return genres;
        }
        private async Task GetData()
        {
            LikedSongs = await GetLikedSongs(50, 0);

            if (LikedSongs is not null && LikedSongs.Tracks is not null && LikedSongs.Tracks.Count >= 50)
            {
                for (int i = 50; i < LikedSongs.Total; i += 50)
                {
                    var nextBatch = await GetLikedSongs(50, i);
                    LikedSongs.Tracks.AddRange(nextBatch.Tracks!);
                }
            }

            ArtistCollection = await _userService.GetTopItems<ArtistCollection>("artists", "long_term", 50);
            
            if (ArtistCollection is not null && ArtistCollection.Artists is not null && ArtistCollection.Artists.Count >= 50)
            {
                var secondBatch = await _userService.GetTopItems<ArtistCollection>("artists", "long_term", 50, 49);
                ArtistCollection.Artists.AddRange(secondBatch.Artists ?? Enumerable.Empty<Artist>());
            }
        }

        public async Task AddArtistsFromLikedSongs()
        {
            if (LikedSongs is null || LikedSongs.Tracks is null)
            {
                return;
            }
            // sorts data for unique artists
            TopArtists.AddRange(from trackItem in LikedSongs!.Tracks!
                                from artist in trackItem.Track!.Artists!
                                select artist);

        }

        public async Task BuildsStringToGetSeveralArtists()
        {
            List<Artist> uniqueArtists = TopArtists.GroupBy(x => x.Id)
                         .Select(group => group.First())
                         .ToList();

            const int batchSize = 50;

            for (int i = 0; i < uniqueArtists.Count; i += batchSize)
            {
                var currentBatch = uniqueArtists.Skip(i).Take(batchSize).ToList();

                string idsString = string.Join(",", currentBatch.Select(artist => artist.Id));

                var idk = await GetArtists(idsString);

                LikedArtists!.Artists!.AddRange(idk.Artists!);
            }
        }

        public void ArrangesGenreFrequencyForTopList(List<Artist>? artists, Dictionary<string, int> genreDictionary)
        {
            if (artists is null)
            {
                return;
            }

            foreach (var artist in artists)
            {
                if (artist.Genres is null)
                {
                    continue;
                }

                foreach (var genre in artist.Genres)
                {
                    genreDictionary.TryGetValue(genre, out var count);
                    genreDictionary[genre] = count + 1;
                }
            }
        }
        public async Task<Dictionary<string, int>> CompareAndCalculateScore()
        {
            // Compare each artist from each dictionary and multiply values where key matches. Example: Key: Rock, Value: 8 multiplied by: Key:Rock, Value: 37 == 8 X 37 == finalScore

            if (GenresFromTopArtists == null || GenresFromLikedSongs == null)
            {
                return null;
            }

            Dictionary<string, int> genreScores = new ();

            foreach (var topGenre in GenresFromTopArtists)
            {
                if (GenresFromLikedSongs.TryGetValue(topGenre.Key, out var likedSongsCount))
                {
                    genreScores[topGenre.Key] = topGenre.Value * likedSongsCount;
                }
                else
                {
                    genreScores[topGenre.Key] = topGenre.Value;
                }
            }

            foreach (var likedGenre in GenresFromLikedSongs.Keys.Except(GenresFromTopArtists.Keys))
            {
                genreScores[likedGenre] = GenresFromLikedSongs[likedGenre];
            }

            var sortedGenreScores = genreScores.OrderByDescending(x => x.Value).Take(10).ToDictionary(x => x.Key, x => x.Value);

            return sortedGenreScores;
        }

        public async Task<TrackData.LikedTracks> GetLikedSongs(int limit, int offset)
        {
            return await _userService.GetTracks<TrackData.LikedTracks>($"me/tracks?limit={limit}&offset={offset}");
        }
        public async Task<Track> GetTrack(string trackId)
        {
            return await _userService.GetTracks<Track>($"tracks/{trackId}");
        }
        public async Task<ArtistData.ArtistArtists> GetArtists(string ids)
        {
            return await _userService.GetArtists(ids);
        }
    }
}
