using Statify.Models;

namespace Statify.Interfaces
{
    public interface IStatisticsService
    {
        public Task<Dictionary<string, int>> GetCalculatedGenreData();

        public Task<TrackCollection> GetLikedSongs(int limit, int offset);

        public Task<Track> GetTrack(string trackId);

        public Task<TrackCollection> GetSeveralTracks(string trackIds);
    }
}
