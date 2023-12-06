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
        public TrackData.TrackList? TrackList { get; set; }
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
        public async Task<AudioFeature> GetCalculatedAudioFeatures()
        {
            await GetTop50Tracks();
            await GetAudioFeatures();
            var audioFeatures = await CalculateTypeOfListener();
           
            return audioFeatures;
        }
        public async Task<string> GetUserSoundProfileTitle()
        {
            await GetTop50Tracks();
            await GetAudioFeatures();
            var audioFeatures = await CalculateTypeOfListener();
            var userSoundProfile = GetTop3Properties(audioFeatures);
            var title = SetUserSoundProfileTitle(userSoundProfile);

            return title;
        }
        private async Task GetTop50Tracks()
        {
            TrackList = await _userService.GetTopItems<TrackData.TrackList>("tracks", "long_term", 50);
        }

        public string SetUserSoundProfileTitle(Dictionary<string, float> userValues)
        {
            Dictionary<string, string> CombinationTitles = new Dictionary<string, string>
            {
                {"Acousticness Danceability Energy", "Melodic Dance Energy"},
                {"Acousticness Danceability Instrumentalness", "Acoustic Rhythmic Serenity"},
                {"Acousticness Danceability Speechiness", "Harmonic Dance Groove"},
                {"Acousticness Danceability Valence", "Serene Acoustic Flow"},
                {"Acousticness Energy Danceability", "Melodic Dance Energy"},
                {"Acousticness Energy Instrumentalness", "Energetic Speech Vibe"},
                {"Acousticness Energy Speechiness", "Instrumental Sonic Adventure"},
                {"Acousticness Energy Valence", "Acoustic Energy Pulse"},
                {"Acousticness Instrumentalness Danceability", "Acoustic Rhythmic Serenity"},
                {"Acousticness Instrumentalness Energy", "Energetic Speech Vibe"},
                {"Acousticness Instrumentalness Speechiness", "Rhythmic Dance Serenity"},
                {"Acousticness Instrumentalness Valence", "Instrumental Harmony Flow"},
                {"Acousticness Speechiness Danceability", "Harmonic Dance Groove"},
                {"Acousticness Speechiness Instrumentalness", "Rhythmic Dance Serenity"},
                {"Acousticness Speechiness Valence", "Speech Infused Groove"},
                {"Acousticness Valence Danceability", "Serene Acoustic Flow"},
                {"Acousticness Valence Instrumentalness", "Instrumental Harmony Flow"},
                {"Acousticness Valence Speechiness", "Speech Infused Groove"},

                {"Danceability Acousticness Energy", "Melodic Dance Energy"},
                {"Danceability Acousticness Instrumentalness", "Acoustic Rhythmic Serenity"},
                {"Danceability Acousticness Speechiness", "Harmonic Dance Groove"},
                {"Danceability Acousticness Valence", "Serene Acoustic Flow"},
                {"Danceability Energy Acousticness", "Melodic Dance Energy"},
                {"Danceability Energy Instrumentalness", "Energetic Speech Vibe"},
                {"Danceability Energy Speechiness", "Rhythmic Acoustic Serenity"},
                {"Danceability Energy Valence", "Sonic Dance Journey"},
                {"Danceability Instrumentalness Acousticness", "Acoustic Rhythmic Serenity"},
                {"Danceability Instrumentalness Energy", "Instrumental Sonic Bliss"},
                {"Danceability Instrumentalness Speechiness", "Speech Rhythmic Harmony"},
                {"Danceability Instrumentalness Valence", "Speech Rhythmic Harmony"},
                {"Danceability Speechiness Acousticness", "Harmonic Dance Groove"},
                {"Danceability Speechiness Energy", "Rhythmic Acoustic Serenity"},
                {"Danceability Speechiness Valence", "Acoustic Sonic Groove"},
                {"Danceability Valence Acousticness", "Serene Acoustic Flow"},
                {"Danceability Valence Instrumentalness", "Speech Rhythmic Harmony"},
                {"Danceability Valence Speechiness", "Acoustic Sonic Groove"},

                {"Energy Acousticness Danceability", "Melodic Dance Energy"},
                {"Energy Acousticness Instrumentalness", "Energetic Speech Vibe"},
                {"Energy Acousticness Speechiness", "Instrumental Sonic Adventure"},
                {"Energy Acousticness Valence", "Acoustic Energy Pulse"},
                {"Energy Danceability Acousticness", "Melodic Dance Energy"},
                {"Energy Danceability Instrumentalness", "Energetic Valence Flow"},
                {"Energy Danceability Speechiness", "Rhythmic Acoustic Serenity"},
                {"Energy Danceability Valence", "Sonic Dance Journey"},
                {"Energy Instrumentalness Acousticness", "Energetic Speech Vibe"},
                {"Energy Instrumentalness Danceability", "Energetic Valence Flow"},
                {"Energy Instrumentalness Speechiness", "Energetic Dance Vibe"},
                {"Energy Instrumentalness Valence", "Serene Acoustic Pulse"},
                {"Energy Speechiness Acousticness", "Instrumental Sonic Adventure"},
                {"Energy Speechiness Danceability", "Rhythmic Acoustic Serenity"},
                {"Energy Speechiness Valence", "Valence Sonic Adventure"},
                {"Energy Valence Acousticness", "Acoustic Energy Pulse"},
                {"Energy Valence Danceability", "Sonic Dance Journey"},
                {"Energy Valence Instrumentalness", "Serene Acoustic Pulse"},
                {"Energy Valence Speechiness", "Valence Sonic Adventure"},

                {"Instrumentalness Acousticness Danceability", "Acoustic Rhythmic Serenity"},
                {"Instrumentalness Acousticness Energy", "Energetic Speech Vibe"},
                {"Instrumentalness Acousticness Speechiness", "Rhythmic Dance Serenity"},
                {"Instrumentalness Acousticness Valence", "Instrumental Harmony Flow"},
                {"Instrumentalness Danceability Acousticness", "Acoustic Rhythmic Serenity"},
                {"Instrumentalness Danceability Energy", "Instrumental Sonic Bliss"},
                {"Instrumentalness Danceability Speechiness", "Speech Rhythmic Harmony"},
                {"Instrumentalness Danceability Valence", "Speech Rhythmic Harmony"},
                {"Instrumentalness Energy Acousticness", "Energetic Speech Vibe"},
                {"Instrumentalness Energy Danceability", "Energetic Valence Flow"},
                {"Instrumentalness Energy Speechiness", "Energetic Dance Vibe"},
                {"Instrumentalness Energy Valence", "Serene Acoustic Pulse"},
                {"Instrumentalness Speechiness Acousticness", "Rhythmic Dance Serenity"},
                {"Instrumentalness Speechiness Danceability", "Speech Rhythmic Harmony"},
                {"Instrumentalness Speechiness Valence", "Energetic Dance Pulse"},
                {"Instrumentalness Valence Acousticness", "Instrumental Harmony Flow"},
                {"Instrumentalness Valence Danceability", "Speech Rhythmic Harmony"},
                {"Instrumentalness Valence Speechiness", "Energetic Dance Pulse"},

                {"Speechiness Acousticness Danceability", "Harmonic Dance Groove"},
                {"Speechiness Acousticness Energy", "Instrumental Sonic Adventure"},
                {"Speechiness Acousticness Valence", "Speech Infused Groove"},
                {"Speechiness Danceability Acousticness", "Harmonic Dance Groove"},
                {"Speechiness Danceability Energy", "Rhythmic Acoustic Serenity"},
                {"Speechiness Danceability Valence", "Acoustic Sonic Groove"},
                {"Speechiness Energy Acousticness", "Instrumental Sonic Adventure"},
                {"Speechiness Energy Danceability", "Rhythmic Acoustic Serenity"},
                {"Speechiness Energy Valence", "Valence Sonic Adventure"},
                {"Speechiness Instrumentalness Acousticness", "Rhythmic Dance Serenity"},
                {"Speechiness Instrumentalness Danceability", "Speech Rhythmic Harmony"},
                {"Speechiness Instrumentalness Valence", "Energetic Dance Pulse"},
                {"Speechiness Valence Acousticness", "Speech Infused Groove"},
                {"Speechiness Valence Danceability", "Acoustic Sonic Groove"},
                {"Speechiness Valence Energy", "Valence Sonic Adventure"},

                {"Valence Acousticness Danceability", "Serene Acoustic Flow"},
                {"Valence Acousticness Energy", "Acoustic Energy Pulse"},
                {"Valence Acousticness Speechiness", "Speech Infused Groove"},
                {"Valence Danceability Acousticness", "Serene Acoustic Flow"},
                {"Valence Danceability Energy", "Sonic Dance Journey"},
                {"Valence Danceability Speechiness", "Acoustic Sonic Groove"},
                {"Valence Energy Acousticness", "Acoustic Energy Pulse"},
                {"Valence Energy Danceability", "Sonic Dance Journey"},
                {"Valence Energy Speechiness", "Valence Sonic Adventure"},
                {"Valence Instrumentalness Acousticness", "Instrumental Harmony Flow"},
                {"Valence Instrumentalness Danceability", "Speech Rhythmic Harmony"},
                {"Valence Instrumentalness Speechiness", "Energetic Dance Pulse"},
                {"Valence Speechiness Acousticness", "Speech Infused Groove"},
                {"Valence Speechiness Danceability", "Acoustic Sonic Groove"},
                {"Valence Speechiness Energy", "Valence Sonic Adventure"}
            };

            string combinationKey = GetCombinationKey(userValues);

            if (CombinationTitles.TryGetValue(combinationKey, out var title))
            {
                return title;
            }

            return "Unknown Profile";
        }

        private string GetCombinationKey(Dictionary<string, float> userValues)
        {
            var sortedUserValues = userValues.OrderByDescending(kv => kv.Value);
            return string.Join(" ", sortedUserValues.Select(kv => $"{kv.Key}"));
        }

        private async Task<AudioFeatureCollection> GetAudioFeatures()
        {
            string idsString = string.Join(",", TrackList!.Tracks!.Select(track => track.Id));

            return await _userService.GetAudioFeatures(idsString);
        }
        private async Task<AudioFeature> CalculateTypeOfListener()
        {
            // AudioFeatures
            // Accousticness    - speaks for itself
            // Danceability     - How danceable a song is
            // Energy           - Perceptual measure of intensity and activity
            // Valence          - How positive a song sounds
            // Instrumentalness - Predicts whether a track contains no vocals
            // Speechiness      - Speechiness detects the presence of spoken words in a track
            // Tempo            - Beats per minute

            // (Energy * 10) * (Energy * 10) =  

            // Feature = Process Track[0] and add to Feature, each value * something

            AudioFeature feature = new();

            var audioFeatures = GetAudioFeatures();

            for (int i = 0; i < audioFeatures.Result.Features!.Length; i++)
            {
                feature.Acousticness += (audioFeatures.Result.Features[i].Acousticness * 10);
                feature.Danceability += (audioFeatures.Result.Features[i].Danceability * 10);
                feature.Energy += (audioFeatures.Result.Features[i].Energy * 10);
                feature.Valence += (audioFeatures.Result.Features[i].Valence * 10);
                feature.Instrumentalness += (audioFeatures.Result.Features[i].Instrumentalness * 10);
                feature.Speechiness += (audioFeatures.Result.Features[i].Speechiness * 10);
            }
            return feature;
        }
        public static Dictionary<string, float> GetTop3Properties(AudioFeature audioFeature)
        {
            var properties = new Dictionary<string, float>
            {
                { "Acousticness", audioFeature.Acousticness },
                { "Danceability", audioFeature.Danceability },
                { "Energy", audioFeature.Energy },
                { "Instrumentalness", audioFeature.Instrumentalness },
                { "Speechiness", audioFeature.Speechiness },
                { "Valence", audioFeature.Valence }
            };


            var sortedProperties = properties.OrderByDescending(p => p.Value);


            var top3Properties = sortedProperties.Take(3).ToDictionary(pair => pair.Key, pair => pair.Value);

            return top3Properties;
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
