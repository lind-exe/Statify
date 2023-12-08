using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Statify.Interfaces;
using Statify.Models;
using System.Linq;
using static Statify.Models.ArtistData;

namespace Statify.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IUserService _userService;
        public StatisticsService(IUserService userService)
        {
            _userService = userService;
            LikedArtists!.Artists = new();
        }
        public TrackData.LikedTracks? LikedSongs { get; set; }
        public ArtistData.ArtistArtists? LikedArtists { get; set; } = new();
        public ArtistData.ArtistItems? ArtistCollection { get; set; }
        public TrackData.TrackList? TrackList { get; set; }
        public List<Artist> TopArtists { get; set; } = new();

        public Dictionary<string, int> GenresFromTopArtists = new();
        public Dictionary<string, int> GenresFromLikedSongs = new();
        public async Task<Dictionary<string, int>?> GetCalculatedGenreData()
        {
            await GetData();

            // For GenresFromTopArtists
            ArrangesGenreFrequencyForTopList(ArtistCollection?.Artists, GenresFromTopArtists);

            AddArtistsFromLikedSongs();
            await BuildsStringToGetSeveralArtists();
            // For GenresFromLikedSongs
            ArrangesGenreFrequencyForTopList(LikedArtists?.Artists, GenresFromLikedSongs);
            var genres = CompareAndCalculateScore();

            return genres;
        }
        public TimeSpan GetTotalDurationOfTopSongs()
        {
            return CalculateTotalDurationOfTopSongs(TrackList!);
        }
        public async Task<AudioFeature> GetCalculatedAudioFeatures()
        {
            await GetTop50Tracks();
            await GetAudioFeatures();
            var audioFeatures = CalculateTypeOfListener();
            return audioFeatures;
        }
        public async Task<string> GetUserSoundProfileTitle()
        {
            await GetTop50Tracks();

            var audioFeatures = CalculateTypeOfListener();

            if (audioFeatures.Acousticness > 125)
            {
                return "instrumental enjoyer";
            }
            if (audioFeatures.Danceability > 300)
            {
                return "dance enjoyer";
            }
            if (audioFeatures.Valence > 300)
            {
                return "positive vibes enjoyer";
            }
            if (audioFeatures.Energy > 400)
            {
                return "red bull enjoyer";
            }

            return "versatile music taste enjoyer";
        }
        public async Task GetTop50Tracks()
        {
            TrackList = await _userService.GetTopItems<TrackData.TrackList>("tracks", "long_term", 50);
        }
        public async Task<AudioFeatureCollection> GetAudioFeatures()
        {
            string idsString = string.Join(",", TrackList!.Tracks!.Select(track => track.Id));

            return await _userService.GetAudioFeatures(idsString);
        }
        public AudioFeature CalculateTypeOfListener()
        {
            AudioFeature feature = new();

            var audioFeatures = GetAudioFeatures();

            var topDance = audioFeatures.Result.Features!.Where(x => x.Danceability > 0.6);

            for (int i = 0; i < audioFeatures.Result.Features!.Length; i++)
            {
                if (audioFeatures.Result.Features[i] is not null)
                {
                    feature.Acousticness += (float)Math.Round((audioFeatures.Result.Features[i].Acousticness * 10));
                    feature.Danceability += (float)Math.Round((audioFeatures.Result.Features[i].Danceability * 10));
                    feature.Energy += (float)Math.Round((audioFeatures.Result.Features[i].Energy * 10));
                    feature.Valence += (float)Math.Round((audioFeatures.Result.Features[i].Valence * 10));
                    feature.Speechiness += (float)Math.Round((audioFeatures.Result.Features[i].Speechiness * 10));
                }
            }
            return feature;
        }
        public TimeSpan CalculateTotalDurationOfTopSongs(TrackData.TrackList tracklist)
        {
            float durationInSeconds = 0;

            foreach (var track in tracklist.Tracks!)
            {
                durationInSeconds += (track.DurationMs / 1000);
            }

            float hours = durationInSeconds / 3600;

            float restHours = hours - (int)hours;

            float minutes = restHours * 60;

            float restMinutes = minutes - (int)minutes;

            float seconds = restMinutes * 60;

            string time = $"{(int)hours}:{(int)minutes}:{(int)seconds}";

            _ = TimeSpan.TryParse(time, out TimeSpan result);

            return result;
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

            ArtistCollection = await _userService.GetTopItems<ArtistItems>("artists", "long_term", 50);

            if (ArtistCollection is not null && ArtistCollection.Artists is not null && ArtistCollection.Artists.Count >= 50)
            {
                var secondBatch = await _userService.GetTopItems<ArtistItems>("artists", "long_term", 50, 49);
                ArtistCollection.Artists.AddRange(secondBatch.Artists ?? Enumerable.Empty<Artist>());
            }
        }

        public void AddArtistsFromLikedSongs()
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
        public Dictionary<string, int>? CompareAndCalculateScore()
        {
            // Compare each artist from each dictionary and multiply values where key matches. Example: Key: Rock, Value: 8 multiplied by: Key:Rock, Value: 37 == 8 X 37 == finalScore

            if (GenresFromTopArtists == null || GenresFromLikedSongs == null)
            {
                return null;
            }

            Dictionary<string, int> genreScores = new();

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